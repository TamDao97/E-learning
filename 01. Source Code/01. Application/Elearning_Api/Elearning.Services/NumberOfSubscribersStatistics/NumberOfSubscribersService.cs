using Elearning.Model.Models.NumberSubscribersStatistics;
using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using Syncfusion.XlsIO;
using System.IO;
using Syncfusion.XlsIORenderer;
using Syncfusion.Pdf;
using NTS.Common.Helpers;

namespace Elearning.Services.NumberOfSubscribersStatistics
{
    public class NumberOfSubscribersService : INumberOfSubscribersService
    {
        private readonly ElearningContext _sqlContext;
        public NumberOfSubscribersService(ElearningContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }
        public async Task<string> Export(SearchNumberSubscribersModel model, int type)
        {
            var data = await StatisticalNumberSubscribers(model);
            if (data.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Template/so_luot_dang_ky_khoa_hoc.xlsx")));
                IWorksheet sheet = workbook.Worksheets[0];

                var total = data.Count;

                IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);


                int index = 1;

                List<NumberSubscribersExportModel> listExport = new List<NumberSubscribersExportModel>();
                foreach (var item in data)
                {

                    listExport.Add(new NumberSubscribersExportModel()
                    {
                        Index = index++,
                        CourseName = item.CourseName,
                        NumberSubscribers = item.NumberSubscribers
                    });
                }
                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);

                IChartShape chart = sheet.Charts.Add();
                chart.TopRow = 2;
                chart.LeftColumn = 1;
                chart.BottomRow = 16;
                chart.RightColumn = 4;

                object[] yValues = listExport.Select(x => x.NumberSubscribers as object).ToArray();
                object[] xValues = listExport.Select(x => x.CourseName as object).ToArray();
                //Adding series and values
                IChartSerie serie = chart.Series.Add("Số đăng ký", ExcelChartType.Column_Clustered);

                //Enters the X and Y values directly
                serie.EnteredDirectlyValues = yValues;
                serie.EnteredDirectlyCategoryLabels = xValues;

                //Set Chart Title
                chart.ChartTitle = "Thống kê số lượng đăng ký khóa học của người học";

                string pathExport = Path.Combine(Directory.GetCurrentDirectory(), "Export/" + Constants.Type_Export_Subcribe + ".xlsx");
                FileStream fileStream = new FileStream(pathExport, FileMode.Create, FileAccess.ReadWrite);
                workbook.SaveAs(fileStream);
                sheet.UsedRange.AutofitRows();
                if (type == Constants.Type_Export_Excel)
                {
                    workbook.SaveAs(fileStream);
                    workbook.Close();
                    excelEngine.Dispose();

                    fileStream.Close();
                    fileStream.Dispose();

                    //Đường dẫn file lưu trong web client
                    string resultPathClient = "Export/" + Constants.Type_Export_Subcribe + ".xlsx";

                    return resultPathClient;
                }
                else
                {
                    XlsIORenderer renderer = new XlsIORenderer();

                    //Convert Excel document with charts into PDF document 
                    PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);

                    Stream stream = new FileStream("Export/" + Constants.Type_Export_Subcribe + ".pdf", FileMode.Create, FileAccess.ReadWrite);
                    pdfDocument.Save(stream);
                    workbook.Close();
                    excelEngine.Dispose();
                    pdfDocument.Close();
                    pdfDocument.Dispose();
                    stream.Close();
                    stream.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();

                    //Đường dẫn file lưu trong web client
                    string resultPathClient = "Export/" + Constants.Type_Export_Subcribe + ".pdf";

                    return resultPathClient;
                }
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw NTSException.CreateInstance(ex.Message);
            }
        }

        public async Task<List<ResultNumberSubscribersModel>> StatisticalNumberSubscribers(SearchNumberSubscribersModel model)
        {
            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
            int quarterData = model.Quarter;

            if (!model.TimeType.Equals(Constants.TimeType_Between))
            {
                SearchHelper.GetDateFromDateToByTimeType(model.TimeType, model.Year, model.Month, model.Quarter, ref dateFrom, ref dateTo, ref quarterData);
            }
            else
            {

                if (!model.DateTo.HasValue)
                {
                    model.DateTo = DateTime.Now;
                }
                if (!model.DateFrom.HasValue || !model.DateTo.HasValue)
                {
                    return null;
                }

                dateFrom = model.DateFrom.Value.ToStartDate();
                dateTo = model.DateTo.Value.ToEndDate();
            }

            var data = (from a in _sqlContext.LearnerCourse.AsNoTracking()
                       
                        select new ResultNumberSubscribersModel
                        {
                            CourseId = a.CourseId,
                            RegistrationDate = a.RegistrationDate
                        }).AsQueryable();

            data = data.Where(a => a.RegistrationDate.Value >= dateFrom && a.RegistrationDate.Value <= dateTo);
            var listCourse = (from a in data
                              group new { a } by new {a.CourseId } into g
                              select new ResultNumberSubscribersModel
                              {
                                  CourseId=g.Key.CourseId,
                                  NumberSubscribers = g.Count(),
                              }).AsQueryable();
            var listResult=(from a in _sqlContext.Course.AsNoTracking()
                           join b in listCourse on a.Id equals b.CourseId
                           into ab
                           from ba in ab.DefaultIfEmpty()
                            where a.Status == Constants.Course_Status_Show && a.ApprovalStatus == Constants.Course_Approval_Approved
                            select new ResultNumberSubscribersModel
                           {
                               ProgramId = a.ProgramId,
                               CourseName = a.Name,
                               CourseId = a.Id,
                               NumberSubscribers=ba.NumberSubscribers,
                           }).AsQueryable();

            if (!string.IsNullOrEmpty(model.ProgramId))
            {
                listResult = listResult.Where(s => s.ProgramId == model.ProgramId);
            }


            return listResult.OrderBy(a => a.CourseName).ToList();
        }
    }
}
