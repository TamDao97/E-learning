import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants, Configuration, FileProcess, DateUtils } from '../../shared';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { CompleteStatisticalService } from '../service/complete-statistical.service';
import { from } from 'rxjs';
import { Label } from 'ng2-charts';
@Component({
  selector: 'app-statistical-complete-course',
  templateUrl: './statistical-complete-course.component.html',
  styleUrls: ['./statistical-complete-course.component.scss']
})
export class StatisticalCompleteCourseComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    public messageService: MessageService,
    public config: Configuration,
    public completeStatisticalService: CompleteStatisticalService,
    private fileProcess:FileProcess,
    public dateUtils: DateUtils
  ) { }
  listData: any[] = [];
  listYear: any[] = [];
  listMonth: any[] = [];
  year: null;
  lable: '';
  startDate: any;
  finishDate: any;
  model: any = {
    TimeType: "6",
    Month: new Date().getUTCMonth(),
    Year: new Date().getFullYear(),
    Quarter: 1,
    ProvinceId: '',
    DistrictId: '',
    dateFrom:'',
    dateTo:''
  }

  ngOnInit(): void {
    this.loadYear();
    this.loadMonth();
    this.search();
    this.appSetting.PageTitle = "Thống kê số lượng kết quả hoàn thành khóa học";
  }

  loadMonth() {
    for (var month = 1; month < 13; month++)
      this.listMonth.push(month);
  }

  clear(){
    this.model ={
      TimeType: "6",
      Month: new Date().getUTCMonth(),
      Year: new Date().getFullYear(),
      Quarter: 1,
      ProvinceId: '',
      DistrictId: '',
      dateFrom:'',
      dateTo:''
    }

    this.startDate =null;
    this.finishDate = null;
    this.search();
  }

  search() {
    if(this.startDate != null){
      this.model.dateFrom = this.dateUtils.convertObjectToDate(this.startDate);
    }
    else{
      this.model.dateFrom = null;
    }

    if(this.finishDate != null){
      this.model.dateTo = this.dateUtils.convertObjectToDate(this.finishDate);
    }
    else{
      this.model.dateTo = null;
    }
    var totalComplete: number[] = [];
    var totalIncomplete: number[] = [];
    var exisRequestResult: Label[] = [];
    this.completeStatisticalService.completeStatistical(this.model).subscribe((data: any) => {
      // console.log(data);


      for (let i = 0; i < data.data.length; i++) {
        totalComplete.push(data.data[i].totalComplete);
        totalIncomplete.push(data.data[i].totalIncomplete);
        exisRequestResult.push(data.data[i].courseName);
      }
      this.ChartCompleteResultData = [
        { data: totalComplete, label: 'Hoàn thành',backgroundColor:'rgb(252, 179, 210)' },
        { data: totalIncomplete, label: 'Đang học',backgroundColor:'rgb(140, 223, 254)'  },
      ];
      this.ChartCompleteResultLabel = exisRequestResult;

      this.listData = data.data;
    });
  }
  
  public ChartCompleteResultData: ChartDataSets[] = [{ data: [], label: 'Kết quả' }];
  public ChartCompleteResultLabel: Label[] = [];
  public ChartCompleteResultLegend = true;
  public ChartCompleteResultType = 'bar';
  public ChartCompleteResultPlugins = [];
  public chartOptionsComplete: ChartOptions = {
    responsive: true,
    scales: {
      xAxes: [{
        display: true,
        scaleLabel: {
          display: true,
          labelString: "Khóa học",
        },
      }],
      yAxes: [{
        display: true,
        scaleLabel: {
          display: true,
          labelString: 'Số người học hoàn thành'
        },
        ticks: {
          stepSize: 1,
          min:0
        }
      }]
    }
  };
 
  loadYear() {
    this.listYear.push('Tất cả');
    for (var year = 2019; year <= new Date().getFullYear(); year++) {
      this.listYear.push(year);
    }
  }

  exportExcel() {
    this.completeStatisticalService.export(this.model, 1).subscribe(d => {
      if(d.data!=null)
      {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerApi + d.data;
        this.fileProcess.downloadFileLink(link.href,'ket_qua_hoan_thanh_khoa_hoc.xlsx');
      }
      else {
        this.messageService.showListMessage(d.message);
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  exportPdf() {
    this.completeStatisticalService.export(this.model, 2).subscribe(d => {
      if(d.data!=null)
      {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerApi + d.data;
        this.fileProcess.downloadFile(d.data,'ket_qua_hoan_thanh_khoa_hoc.pdf');
      }
      else {
        this.messageService.showListMessage(d.message);
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

}
