using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Elearning.Model.Models.ReportLearnerProvince;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.XlsIORenderer;
using Elearning.Model.Models.ReportLearner;
using Syncfusion.XlsIO;
using NTS.Common;
using Syncfusion.Drawing;
using NTS.Common.Helpers;

namespace Elearning.Services.ReportLearnerProvince
{
    public class ReportLearnerProvinceService : IReportLearnerProvinceService
    {
        private readonly ElearningContext sqlContext;

        public ReportLearnerProvinceService (ElearningContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        /// <summary>
        /// Thống kê thông tin người học
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<ReportLearnerProvinceResultModel> ReportLearnerProvince (SearchReportLearnerModel model)
        {
            ReportLearnerProvinceResultModel resultModel = new ReportLearnerProvinceResultModel();
            List<ReportProvinceModel> reportProvinces = new List<ReportProvinceModel>();
            List<ReportProvinceModel> reportGender = new List<ReportProvinceModel>();
            List<ReportProvinceModel> reportYearOld = new List<ReportProvinceModel>();

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


            var list = await (from a in sqlContext.Learner.AsNoTracking()
                              where a.CreateDate >= dateFrom && a.CreateDate <= dateTo
                              select new ReportDataModel
                              {
                                  LearnerId = a.Id,
                                  ProvinceId = a.ProvinceId,
                                  DistrictId = a.DistrictId,
                                  WardId = a.WardId,
                                  Gender = a.Gender,
                                  DateOfBirthday = a.DateOfBirthday
                              }).ToListAsync();

            // Tỉnh thành
            var listProvince = sqlContext.Province.OrderBy(i => i.Name).ToList();
            double totalProvince = 0;
            foreach (var item in listProvince)
            {
                totalProvince = list.Where(i => !string.IsNullOrEmpty(i.ProvinceId) && i.ProvinceId.Equals(item.ProvinceId)).GroupBy(i => i.LearnerId).Count();
                //if (totalProvince > 0)
                //{
                reportProvinces.Add(new ReportProvinceModel
                {
                    Name = item.Name,
                    Total = totalProvince
                });
                //}
            }

            if (!string.IsNullOrEmpty(model.ProvinceId))
            {
                var listDistrict = sqlContext.District.Where(a => a.ProvinceId.Equals(model.ProvinceId)).OrderBy(a => a.Name).ToList();
                reportProvinces = new List<ReportProvinceModel>();
                foreach (var item in listDistrict)
                {
                    totalProvince = list.Where(i => item.DistrictId.Equals(i.DistrictId)).GroupBy(i => i.LearnerId).Count();
                    //if (totalProvince > 0)
                    //{
                    reportProvinces.Add(new ReportProvinceModel
                    {
                        Name = item.Name,
                        Total = totalProvince
                    });
                }

            }


            // Giới tính
            double totalGender = 0;
            totalGender = list.Where(i => i.Gender.HasValue && i.Gender.Value).GroupBy(i => i.LearnerId).Count();
            reportGender.Add(new ReportProvinceModel
            {
                Name = "Nữ",
                Total = totalGender
            });

            totalGender = list.Where(i => i.Gender.HasValue && !i.Gender.Value).GroupBy(i => i.LearnerId).Count();
            reportGender.Add(new ReportProvinceModel
            {
                Name = "Nam",
                Total = totalGender
            });

            // Độ tuổi
            ReportYearOldModel reportYear = new ReportYearOldModel();
            double totalYearOld = 0;
            int yearOld = DateTime.Now.Year;
            foreach (var item in reportYear.ListYearOld)
            {
                totalYearOld = list.Where(i => i.DateOfBirthday.HasValue && (yearOld - i.DateOfBirthday.Value.Year) >= item.Min && (yearOld - i.DateOfBirthday.Value.Year) <= item.Max).GroupBy(i => i.LearnerId).Count();
                reportYearOld.Add(new ReportProvinceModel
                {
                    Name = item.Name,
                    Total = totalYearOld
                });
            }

            reportProvinces = reportProvinces.OrderByDescending(i => i.Total).ToList();
            resultModel.ReportProvinces = reportProvinces.ToList();
            resultModel.ReportProvincesLable = reportProvinces.Where(s => s.Total != 0).Select(i => i.Name).Take(10).ToList();
            resultModel.ReportProvincesData = reportProvinces.Where(s => s.Total != 0).Select(i => i.Total).Take(10).ToList();
            resultModel.ReportProvincesLable.Add("Còn lại");
            resultModel.ReportProvincesData.Add(reportProvinces.Skip(10).Sum(i => i.Total));

            resultModel.ReportGender = reportGender;
            resultModel.ReportGenderLable = reportGender.Select(i => i.Name).ToList();
            resultModel.ReportGenderData = reportGender.Select(i => i.Total).ToList();

            resultModel.ReportYearOld = reportYearOld;
            resultModel.ReportYearOldLable = reportYearOld.Select(i => i.Name).ToList();
            resultModel.ReportYearOldData = reportYearOld.Select(i => i.Total).ToList();

            return resultModel;
        }

        /// <summary>
        /// Xuất file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> ExportFileAsync (ReportLearnerProvinceModel model)
        {
            if (model.ListResult.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Template/ReportLearnerProvin.xlsx")));

            if (model.Type == Constants.Type_Export_Gender_Int)
            {
                workbook = application.Workbooks.Open(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Template/ReportLearnerGender.xlsx")));
            }
            else if (model.Type == Constants.Type_Export_YearOld_Int)
            {
                workbook = application.Workbooks.Open(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Template/ReportLearnerAge.xlsx")));
            }
            else
            {
                workbook = application.Workbooks.Open(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Template/ReportLearnerProvin.xlsx")));
            }

            IWorksheet sheet = workbook.Worksheets[0];

            var total = model.ListResult.Count();

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var listExport = model.ListResult.Select((a, i) => new
            {
                index = i + 1,
                a.Name,
                a.Total
            });
            List<int[]> colors = new List<int[]>
                {
                    new int[]{127,255,212},
                    new int[]{255,228,196},
                    new int[]{0,0,255},
                    new int[]{138,43,226},
                    new int[]{165,42,42},
                    new int[]{95,158,160},
                    new int[]{127,255,0},
                    new int[]{255,127,802},
                    new int[]{184,134,11},
                    new int[]{0,139,139},
                    new int[]{169,169,169},
                };
            List<int[]> colorsGender = new List<int[]>
                {
                     new int[]{127,255,212},
                    new int[]{255,228,196},
                };
            if (listExport.Count() > 1)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            string nameExcel = string.Empty;
            if (model.Type == Constants.Type_Export_Provin_Int)
            {
                // Add biểu đồ
                IChartShape chart = sheet.Charts.Add();
                chart.TopRow = 4;
                chart.LeftColumn = 1;
                chart.BottomRow = 16;
                chart.RightColumn = 4;
                //Set chart type
                chart.ChartType = ExcelChartType.Pie;
                chart.Legend.Position = ExcelLegendPosition.Bottom;

                //Set Chart Title
                chart.ChartTitle = "Thống kê người học theo Tỉnh, Thành phố";
                chart.ChartTitleArea.FontName = "Times New Roman";
                chart.ChartTitleArea.Size = 14;
                chart.ChartTitleArea.Bold = false;

                var listExportChart = listExport.Take(10).ToList();
                listExportChart.Add(new
                {
                    index = 11,
                    Name = "Còn lại",
                    Total = listExport.Skip(10).Sum(s => s.Total)
                });
                object[] yValues = listExportChart.Where(s => s.Total != 0).Select(x => x.Total as object).ToArray();
                object[] xValues = listExportChart.Where(s => s.Total != 0).Select(x => x.Name as object).ToArray();
                //Set first serie
                IChartSerie complete = chart.Series.Add("Thông tin người học", ExcelChartType.Pie);
                complete.EnteredDirectlyValues = yValues;
                complete.EnteredDirectlyCategoryLabels = xValues;
                //for (int i = 0; i < listExportChart.Where(s => s.Total != 0).Count(); i++)
                //{
                //    complete.DataPoints[i].DataFormat.Fill.FillType = ExcelFillType.SolidColor;
                //    complete.DataPoints[i].DataFormat.Fill.ForeColor = Color.FromArgb(colors[i][0], colors[i][1], colors[i][2]);
                //}
                nameExcel = "Export/" + Constants.Type_Export_Provin + ".xlsx";

            }
            else if (model.Type == Constants.Type_Export_Gender_Int)
            {
                nameExcel = "Export/" + Constants.Type_Export_Gender + ".xlsx";

                IChartShape chart = sheet.Charts.Add();
                chart.TopRow = 4;
                chart.LeftColumn = 1;
                chart.BottomRow = 16;
                chart.RightColumn = 4;
                //Set chart type
                chart.ChartType = ExcelChartType.Pie;
                chart.Legend.Position = ExcelLegendPosition.Bottom;
                object[] yValues = listExport.Select(x => x.Total as object).ToArray();
                object[] xValues = listExport.Select(x => x.Name as object).ToArray();
                IChartSerie complete = chart.Series.Add("Thông tin người học", ExcelChartType.Pie);
                complete.EnteredDirectlyValues = yValues;
                complete.EnteredDirectlyCategoryLabels = xValues;
                //for (int i = 0; i < listExport.Count(); i++)
                //{
                //    complete.DataPoints[i].DataFormat.Fill.FillType = ExcelFillType.SolidColor;
                //    complete.DataPoints[i].DataFormat.Fill.ForeColor = Color.FromArgb(colorsGender[i][0], colorsGender[i][1], colorsGender[i][2]);
                //}
            }
            else if (model.Type == Constants.Type_Export_YearOld_Int)
            {
                nameExcel = "Export/" + Constants.Type_Export_YearOld + ".xlsx";
            }
           
            string pathExport = Path.Combine(Directory.GetCurrentDirectory(), nameExcel);
            FileStream fileStream = new FileStream(pathExport, FileMode.Create, FileAccess.ReadWrite);
            string resultPathClient = string.Empty;
            try
            {
                if (model.ExportType == Constants.Type_Export_Excel)
                {
                    workbook.SaveAs(fileStream);
                    workbook.Close();
                    excelEngine.Dispose();

                    fileStream.Close();
                    fileStream.Dispose();

                    //Đường dẫn file lưu trong web client
                    resultPathClient = nameExcel;

                    return resultPathClient;
                }
                else
                {
                    XlsIORenderer renderer = new XlsIORenderer();

                    //Convert Excel document with charts into PDF document 
                    PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);

                    string namePdf = string.Empty;
                    if (model.Type == Constants.Type_Export_Provin_Int)
                    {
                        namePdf = "Export/" + Constants.Type_Export_Provin + ".pdf";
                    }
                    else if (model.Type == Constants.Type_Export_Gender_Int)
                    {
                        namePdf = "Export/" + Constants.Type_Export_Gender + ".pdf";
                    }
                    else if (model.Type == Constants.Type_Export_YearOld_Int)
                    {
                        namePdf = "Export/" + Constants.Type_Export_YearOld + ".pdf";
                    }

                    Stream stream = new FileStream(namePdf, FileMode.Create, FileAccess.ReadWrite);

                    pdfDocument.Save(stream);
                    workbook.Close();
                    excelEngine.Dispose();
                    pdfDocument.Close();
                    pdfDocument.Dispose();
                    stream.Close();
                    stream.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();

                    return namePdf;
                }
            }
            catch (Exception ex)
            {
                throw NTSException.CreateInstance(ex.Message);
            }
            finally
            {
                workbook.Close();
                excelEngine.Dispose();
                fileStream.Close();
                fileStream.Dispose();
            }
        }
    }
}
