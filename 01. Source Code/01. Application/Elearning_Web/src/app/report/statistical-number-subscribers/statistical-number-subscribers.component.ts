import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants, Configuration, FileProcess } from '../../shared';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { NumberSubscribersService } from '../service/number-subscribers.service';
import { ComboboxService } from '../../shared/services/combobox.service';
import { from } from 'rxjs';
import { Label } from 'ng2-charts';
@Component({
  selector: 'app-statistical-number-subscribers',
  templateUrl: './statistical-number-subscribers.component.html',
  styleUrls: ['./statistical-number-subscribers.component.scss']
})
export class StatisticalNumberSubscribersComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    public messageService: MessageService,
    public config: Configuration,
    public numberSubscribersService: NumberSubscribersService,
    private fileProcess: FileProcess,
    public comboboxService: ComboboxService
  ) { }
  listProgram: any[] = [];
  listData: any[] = [];
  listMonth: any[] = [];
  listYear: any[] = [];
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
    dateTo:'',
    programId: null,
  }
  programId: null;
  lable: '';

  ngOnInit(): void {
    this.loadYear();
    this.loadMonth();
    this.search();
    this.getProgram();
    this.appSetting.PageTitle = "Thống kê số người đăng ký học";
  }

  loadMonth() {
    for (var month = 1; month < 13; month++)
      this.listMonth.push(month);
  }

  loadYear() {
    for (var year = 2019; year <= new Date().getFullYear(); year++) {
      this.listYear.push(year);
    }
  }

  getProgram() {
    this.comboboxService.getProgram().subscribe((data: any) => {
      this.listProgram = data.data;
    });
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
      dateTo:'',
      programId: null,

    }

    this.startDate =null;
    this.finishDate = null;
    this.search();
  }

  search() {
    var numberSubscribers: number[] = [];
    var exisRequestResult: Label[] = [];
    this.numberSubscribersService.numberSubscribers(this.model).subscribe((data: any) => {
      console.log(data);


      for (let i = 0; i < data.data.length; i++) {
        numberSubscribers.push(data.data[i].numberSubscribers);
        exisRequestResult.push(data.data[i].courseName);
      }
      this.ChartCompleteResultData = [
        { data: numberSubscribers, label: 'Số người đăng ký' },
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
          labelString: 'Số người đăng ký'
        },
        ticks: {
          stepSize: 1,
          min: 0
        }
      }]
    }
  };

  exportExcel() {
    this.numberSubscribersService.export(this.model, 1).subscribe(d => {
      if (d.data != null) {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerApi + d.data;
        this.fileProcess.downloadFileLink(link.href, 'thong_ke_so_dang_ky.xlsx');
      }
      else {
        this.messageService.showListMessage(d.message);
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  exportPdf() {
    this.numberSubscribersService.export(this.model, 2).subscribe(d => {
      if (d.data != null) {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerApi + d.data;
        this.fileProcess.downloadFile(d.data, 'thong_ke_so_dang_ky.pdf');
      }
      else {
        this.messageService.showListMessage(d.message);
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

}
