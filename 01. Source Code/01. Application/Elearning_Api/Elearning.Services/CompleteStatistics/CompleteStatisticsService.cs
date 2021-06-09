using Elearning.Model.Models.CompleteStatistics;
using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using Syncfusion.XlsIO;
using System.IO;
using Syncfusion.XlsIORenderer;
using Syncfusion.Pdf;
using Syncfusion.Drawing;
using NTS.Common.Helpers;

namespace Elearning.Services.CompleteStatistics
{
    public class CompleteStatisticsService : ICompleteStatisticsService
    {
        private readonly ElearningContext _sqlContext;
        public CompleteStatisticsService(ElearningContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        public async Task<string> Export(SearchCompleteModel model, int type)
        {
            var data = await StatisticalCompleteCourse(model);
            if (data.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Template/Ket_qua_hoan_thanh_khoa_hoc.xlsx")));
                IWorksheet sheet = workbook.Worksheets[0];

                var total = data.Count;

                IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

                int index = 1;
                List<int[]> colors = new List<int[]>
                {
                    new int[]{252, 179, 210},
                    new int[]{140, 223, 254},
                };
                List<ResultCompleteExportModel> listExport = new List<ResultCompleteExportModel>();
                foreach (var item in data)
                {

                    listExport.Add(new ResultCompleteExportModel()
                    {
                        Index = index++,
                        CourseName = item.CourseName,
                        TotalComplete = item.TotalComplete,
                        TotalIncomplete = item.TotalIncomplete,
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
                chart.RightColumn = 5;
                //Set chart type
                chart.ChartType = ExcelChartType.Column_Clustered;

                //Set Chart Title
                chart.ChartTitle = "Thống kê kết quả hoàn thành khóa học của người học";

                //Set first serie
                IChartSerie complete = chart.Series.Add("Hoàn thành");
                complete.Values = sheet.Range[iRangeData.Row, iRangeData.Column + 2, iRangeData.Row + total - 1, iRangeData.Column + 2];
                complete.CategoryLabels = sheet.Range[iRangeData.Row, iRangeData.Column + 1, iRangeData.Row + total - 1, iRangeData.Column + 1];
                
                //SetColor
                for (int i = 0; i < listExport.Count(); i++)
                {
                    complete.DataPoints[i].DataFormat.Fill.FillType = ExcelFillType.SolidColor;
                    complete.DataPoints[i].DataFormat.Fill.ForeColor = Color.FromArgb(252, 179, 210);
                }
                //Set second serie
                IChartSerie inComplete = chart.Series.Add("Đang học");
                inComplete.Values = sheet.Range[iRangeData.Row, iRangeData.Column + 3, iRangeData.Row + total - 1, iRangeData.Column + 3];
                inComplete.CategoryLabels = sheet.Range[iRangeData.Row, iRangeData.Column + 1, iRangeData.Row + total - 1, iRangeData.Column + 1];
                
                for (int i = 0; i < listExport.Count(); i++)
                {
                    inComplete.DataPoints[i].DataFormat.Fill.FillType = ExcelFillType.SolidColor;
                    inComplete.DataPoints[i].DataFormat.Fill.ForeColor = Color.FromArgb(140, 223, 254);
                }
                complete.SerieFormat.Fill.ForeColor = Color.FromArgb(252, 179, 210);
                inComplete.SerieFormat.Fill.ForeColor = Color.FromArgb(140, 223, 254);
                string pathExport = Path.Combine(Directory.GetCurrentDirectory(), "Export/" + Constants.Type_Export_Complete_Course + ".xlsx");
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
                    string resultPathClient = "Export/" + Constants.Type_Export_Complete_Course + ".xlsx";

                    return resultPathClient;
                }
                else
                {
                    XlsIORenderer renderer = new XlsIORenderer();

                    //Convert Excel document with charts into PDF document 
                    PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);

                    Stream stream = new FileStream("Export/" + Constants.Type_Export_Complete_Course + ".pdf", FileMode.Create, FileAccess.ReadWrite);
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
                    string resultPathClient = "Export/" + Constants.Type_Export_Complete_Course + ".pdf";

                    return resultPathClient;
                }
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw NTSException.CreateInstance(ex.Message);
            }
        }

        public async Task<List<ResultCompleteModel>> StatisticalCompleteCourse(SearchCompleteModel model)
        {
            var data = new List<ResultCompleteModel>();

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

            data = (from a in _sqlContext.Course.AsNoTracking()
                    where a.StartDate >= dateFrom && a.StartDate <= dateTo && a.Status == Constants.Course_Status_Show
                    select new ResultCompleteModel
                    {
                        CourseId = a.Id,
                        CourseName = a.Name,
                    }).ToList();

            foreach (var item in data)
            {
                int complete = 0, incomplete = 0;

                var totalLesson = _sqlContext.LessonCourse.Where(s => s.CourseId == item.CourseId).Count();
                var totalLessonLearned = _sqlContext.LessonHistory.Where(s => s.CourseId == item.CourseId)
                    .GroupBy(s => s.LearnerId).Select(n => new
                    {
                        LearnerId = n.Key,
                        Count = n.Count()
                    }).ToList();
                foreach (var i in totalLessonLearned)
                {
                    if (i.Count == totalLesson)
                    {
                        complete++;
                    }
                    else
                    {
                        incomplete++;
                    }
                }
                item.TotalComplete = complete;
                item.TotalIncomplete = incomplete;
            }
            return data.ToList();

        }
    }
}
