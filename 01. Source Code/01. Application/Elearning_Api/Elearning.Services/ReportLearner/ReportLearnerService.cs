using Elearning.Model.Models.ReportLearner;
using Elearning.Models.Entities;
using NTS.Common;
using NTS.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using System.IO;
using Syncfusion.XlsIORenderer;
using Syncfusion.Pdf;

namespace Elearning.Services.ReportLearner
{
    public class ReportLearnerService : IReportLearnerService
    {
        private readonly ElearningContext sqlContext;

        public ReportLearnerService(ElearningContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        /// <summary>
        /// Thống kê người học
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReportLearnerResultModel> ReportLearner(ReportLearnerSearchConditionModel model)
        {
            ReportLearnerResultModel resultModel = new ReportLearnerResultModel();
            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
            int quarterData = model.Quarter;

            if (!model.TimeType.Equals(Constants.TimeType_Between))
            {
                SearchHelper.GetDateFromDateToByTimeType(model.TimeType, model.Year, model.Month, model.Quarter, ref dateFrom, ref dateTo, ref quarterData);
            }
            else
            {

                if (model.DateTo == null)
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

            var list = await sqlContext.Learner.Where(i => i.CreateDate >= dateFrom && i.CreateDate <= dateTo).ToListAsync();

            // Tìm kiếm người học theo tỉnh
            if (!string.IsNullOrEmpty(model.ProvinceId))
            {
                list = list.Where(a => model.ProvinceId.Equals(a.ProvinceId)).ToList();
            }

            // Tìm kiếm người học theo huyện
            if (!string.IsNullOrEmpty(model.DistrictId))
            {
                list = list.Where(a => model.DistrictId.Equals(a.DistrictId)).ToList();
            }

            // Tìm kiếm người học theo xã
            if (!string.IsNullOrEmpty(model.WardId))
            {
                list = list.Where(a => model.WardId.Equals(a.WardId)).ToList();
            }
            List<ReportLearnerModel> listResult = new List<ReportLearnerModel>();
            List<string> listLable = new List<string>();
            List<double> listData = new List<double>();
            double total = 0;

            if (model.TimeType.Equals(Constants.TimeType_ThisWeek) || model.TimeType.Equals(Constants.TimeType_LastWeek) || model.TimeType.Equals(Constants.TimeType_SevenDay))
            {
                for (int a = 0; a < 7; a++)
                {
                    total = list.Where(i => i.CreateDate.Day == dateFrom.Day && i.CreateDate.Month == dateFrom.Month).Count();
                    listLable.Add($"{dateFrom.Day}/{dateFrom.Month}");
                    listData.Add(total);
                    listResult.Add(new ReportLearnerModel
                    {
                        Name = $"{dateFrom.Day}/{dateFrom.Month}",
                        Total = total
                    });
                    dateFrom = dateFrom.AddDays(1);
                }
            }
            else if (model.TimeType.Equals(Constants.TimeType_ThisMonth) || model.TimeType.Equals(Constants.TimeType_LastMonth) || model.TimeType.Equals(Constants.TimeType_Month))
            {
                int day = DateTime.DaysInMonth(dateFrom.Year, dateFrom.Month);
                for (int a = 1; a <= day; a++)
                {
                    total = list.Where(i => i.CreateDate.Day == a).Count();
                    listLable.Add(a.ToString());
                    listData.Add(total);
                    listResult.Add(new ReportLearnerModel
                    {
                        Name = a.ToString(),
                        Total = total
                    });
                }
            }
            else if (model.TimeType.Equals(Constants.TimeType_Quarter))
            {
                if (quarterData == 1)
                {
                    for (int a = 1; a < 4; a++)
                    {
                        total = list.Where(i => i.CreateDate.Month == a).Count();
                        listLable.Add(a.ToString());
                        listData.Add(total);
                        listResult.Add(new ReportLearnerModel
                        {
                            Name = a.ToString(),
                            Total = total
                        });
                    }
                }
                else if (quarterData == 2)
                {
                    for (int a = 4; a < 7; a++)
                    {
                        total = list.Where(i => i.CreateDate.Month == a).Count();
                        listLable.Add(a.ToString());
                        listData.Add(total);
                        listResult.Add(new ReportLearnerModel
                        {
                            Name = a.ToString(),
                            Total = total
                        });
                    }
                }
                else if (quarterData == 3)
                {
                    for (int a = 7; a < 10; a++)
                    {
                        total = list.Where(i => i.CreateDate.Month == a).Count();
                        listLable.Add(a.ToString());
                        listData.Add(total);
                        listResult.Add(new ReportLearnerModel
                        {
                            Name = a.ToString(),
                            Total = total
                        });
                    }
                }
                else if (quarterData == 4)
                {
                    for (int a = 10; a < 13; a++)
                    {
                        total = list.Where(i => i.CreateDate.Month == a).Count();
                        listLable.Add(a.ToString());
                        listData.Add(total);
                        listResult.Add(new ReportLearnerModel
                        {
                            Name = a.ToString(),
                            Total = total
                        });
                    }
                }
            }
            else if (model.TimeType.Equals(Constants.TimeType_ThisYear) || model.TimeType.Equals(Constants.TimeType_LastYear) || model.TimeType.Equals(Constants.TimeType_Year))
            {
                for (int a = 1; a <= 12; a++)
                {
                    total = list.Where(i => i.CreateDate.Month == a).Count();
                    listLable.Add(a.ToString());
                    listData.Add(total);
                    listResult.Add(new ReportLearnerModel
                    {
                        Name = a.ToString(),
                        Total = total
                    });
                }
            }
            else if (model.TimeType.Equals(Constants.TimeType_Between))
            {
                TimeSpan ts = dateTo - dateFrom;
                for (int a = 0; a < ts.Days + 1; a++)
                {
                    total = list.Where(i => i.CreateDate.Day == dateFrom.Day && i.CreateDate.Month == dateFrom.Month && i.CreateDate.Year == dateFrom.Year).Count();
                    listLable.Add($"{dateFrom.Day}/{dateFrom.Month}/{dateFrom.Year}");
                    listData.Add(total);
                    listResult.Add(new ReportLearnerModel
                    {
                        Name = $"{dateFrom.Day}/{dateFrom.Month}",
                        Total = total
                    });
                    dateFrom = dateFrom.AddDays(1);
                }
            }

            resultModel.ListResult = listResult;
            resultModel.ListLable = listLable;
            resultModel.ListData = listData;

            return resultModel;
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ExportFileModel> ExportExcelAsync(ReportLearnerSearchConditionModel model)
        {
            ExportFileModel exportFile = new ExportFileModel();
            var data = await ReportLearner(model);

            if (data.ListResult.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Template/ReportLearner.xlsx")));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = data.ListResult.Count();

            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
            int quarterData = model.Quarter;

            if (!model.TimeType.Equals(Constants.TimeType_Between))
            {
                SearchHelper.GetDateFromDateToByTimeType(model.TimeType, model.Year, model.Month, model.Quarter, ref dateFrom, ref dateTo, ref quarterData);
            }
            else
            {
                if (!model.DateFrom.HasValue || !model.DateTo.HasValue)
                {
                    return null;
                }

                dateFrom = model.DateFrom.Value.ToStartDate();
                dateTo = model.DateTo.Value.ToEndDate();
            }

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            string day = string.Empty;
            if (model.TimeType.Equals(Constants.TimeType_ThisWeek) || model.TimeType.Equals(Constants.TimeType_LastWeek) || model.TimeType.Equals(Constants.TimeType_SevenDay) || model.TimeType.Equals(Constants.TimeType_ThisMonth) || model.TimeType.Equals(Constants.TimeType_LastMonth) || model.TimeType.Equals(Constants.TimeType_Month))
            {
                day = "Ngày";
            }
            else
            {
                day = "Tháng";
            }

            IRange iRange = sheet.FindFirst("<day>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRange.Text = iRange.Text.Replace("<day>", day);
            string time = string.Empty;
            if (model.TimeType.Equals(Constants.TimeType_ThisWeek))
            {
                time = "Tuần này (" + dateFrom.ToString("dd/MM/yyyy") + " - " + dateTo.ToString("dd/MM/yyyy") + ")";
            }
            if (model.TimeType.Equals(Constants.TimeType_LastWeek))
            {
                time = "Tuần trước(" + dateFrom.ToString("dd/MM/yyyy") + " - " + dateTo.ToString("dd/MM/yyyy") + ")";
            }
            if (model.TimeType.Equals(Constants.TimeType_LastMonth) || model.TimeType.Equals(Constants.TimeType_Month) || model.TimeType.Equals(Constants.TimeType_ThisMonth))
            {
                time = "Tháng " + dateFrom.Month + " / " + dateTo.Year;
            }
            if (model.TimeType.Equals(Constants.TimeType_LastYear) || model.TimeType.Equals(Constants.TimeType_ThisYear) || model.TimeType.Equals(Constants.TimeType_Year))
            {
                time = "Năm " + dateFrom.Year;
            }
            if (model.TimeType.Equals(Constants.TimeType_ThisQuarter))
            {
                time = "Quý " + model.Quarter + " năm " + dateFrom.Year;
            }
            IRange iRangeTime = sheet.FindFirst("<time>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeTime.Text = iRangeTime.Text.Replace("<time>", time);
            var listExport = data.ListResult.Select((a, i) => new
            {
                a.Name,
                a.Total
            });

            if (listExport.Count() > 1)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            if (listExport.Count() > 25)
            {
                //Set Horizontal Page Breaks
                sheet.HPageBreaks.Add(sheet.Range[iRangeData.Row + total, 1]);
                //Set Vertical Page Breaks
                //sheet.VPageBreaks.Add(sheet.Range[iRangeData.Row + total, 3]);
            }
            IChartShape chart = sheet.Charts.Add();
            chart.TopRow = iRangeData.Row + total + 1;
            chart.LeftColumn = 1;
            chart.BottomRow = iRangeData.Row + total + 20;
            chart.RightColumn = 3;

            object[] yValues = listExport.Select(x => x.Total as object).ToArray();
            object[] xValues = listExport.Select(x => x.Name as object).ToArray();
            //Adding series and values
            IChartSerie serie = chart.Series.Add("Số người học", ExcelChartType.Line);

            //Enters the X and Y values directly
            serie.EnteredDirectlyValues = yValues;
            serie.EnteredDirectlyCategoryLabels = xValues;

            //Set Chart Title
            chart.ChartTitle = "Thống kê số lượng người học";

            var date = DateTime.Now.ToString("ddMMyyyyHHmmss");
            string pathExport = Path.Combine(Directory.GetCurrentDirectory(), "Export/Thống kê người học_" + date + ".xlsx");
            FileStream fileStream = new FileStream(pathExport, FileMode.Create, FileAccess.ReadWrite);
            workbook.SaveAs(fileStream);
            workbook.Close();
            excelEngine.Dispose();

            fileStream.Close();
            fileStream.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Export/Thống kê người học_" + date + ".xlsx";
            exportFile.Path = resultPathClient;
            exportFile.Name = "Thống kê người học_" + date + ".xlsx";

            return exportFile;
        }

        /// <summary>
        /// Xuất pdf
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ExportFileModel> ExportPdfAsync(ReportLearnerSearchConditionModel model)
        {
            ExportFileModel exportFile = new ExportFileModel();
            var data = await ExportExcelAsync(model);
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            FileStream excelStream = new FileStream(data.Path, FileMode.Open, FileAccess.Read);
            IWorkbook workbook = application.Workbooks.Open(excelStream);

            //Initialize XlsIO renderer.
            XlsIORenderer renderer = new XlsIORenderer();

            XlsIORendererSettings settings = new XlsIORendererSettings();
            settings.EmbedFonts = true;
            settings.ExportDocumentProperties = false;
            settings.ExportQualityImage = true;
            settings.ExportBookmarks = false;
            settings.ExportDocumentProperties = false;
            settings.LayoutOptions = LayoutOptions.NoScaling;

            //Convert Excel document into PDF document 
            PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);

            var date = DateTime.Now.ToString("ddMMyyyyHHmmss");
            string pathExport = Path.Combine(Directory.GetCurrentDirectory(), "Export/Thống kê người học_" + date + ".pdf");
            Stream stream = new FileStream(pathExport, FileMode.Create, FileAccess.ReadWrite);
            pdfDocument.Save(stream);

            excelStream.Close();
            excelStream.Dispose();
            stream.Close();
            stream.Dispose();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Export/Thống kê người học_" + date + ".pdf";
            exportFile.Path = resultPathClient;
            exportFile.Name = "Thống kê người học_" + date + ".pdf";

            return exportFile;
        }
    }
}
