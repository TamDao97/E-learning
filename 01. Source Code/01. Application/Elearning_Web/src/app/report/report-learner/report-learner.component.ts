import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Constants, AppSetting, MessageService, DateUtils, Configuration, FileProcess } from 'src/app/shared';
import { DownloadService } from 'src/app/shared/services/download.service';
import { ReportLearnerService } from '../service/report-learner.service';

@Component({
  selector: 'app-report-learner',
  templateUrl: './report-learner.component.html',
  styleUrls: ['./report-learner.component.scss']
})
export class ReportLearnerComponent implements OnInit, OnDestroy {
  @ViewChild('scrollHeaderOne') scrollHeaderOne: ElementRef;

  constructor(
    public constant: Constants,
    private appSetting: AppSetting,
    private messageService: MessageService,
    private reportLearnerService: ReportLearnerService,
    public dateUtils: DateUtils,
    private config: Configuration,
    private fileProcess: FileProcess,
  ) { }

  public textViewChart: string = 'Thống kê người học';
  public barListView: string[];
  public barChartType: string = 'line';
  public barChartLegend: boolean = true;
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    scales: {
      yAxes: [
        {
          ticks: {
            callback: function (label, index, labels) {
              return (label).toString().replace(/\B(?=(\d{0})+(?!\d))/g, ".");
            },
            stepSize: 1
          },
          scaleLabel: {
            display: true,
            //labelString: '1k = 100.000'
          }
        }
      ]
    },
    tooltips: {
      //enabled: true,
      mode: 'single',
      callbacks: {
        label: function (tooltipItem, data) {
          var datasetLabel = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toString().replace(/\B(?=(\d{0})+(?!\d))/g, ".");
          return data.datasets[tooltipItem.datasetIndex].label + ': ' + datasetLabel;
        }
      }
    }
  };
  public barChartData: any[] = [
    { data: [], label: 'Số lượng người học' },
  ];
  public chartColors: Array<any> = [
    { backgroundColor: '#1ff70b' }
  ];

  listYear: any[] = [];
  listMonth: any[] = [];
  listData: any[] = [];
  heightChart = 500;
  startDate: any;
  finishDate: any;
  model: any = {
    TimeType: "6",
    Month: new Date().getUTCMonth(),
    Year: 2020,
    Quarter: 1,
    ProvinceId: '',
    DistrictId: '',
    WardId: '',
    dateFrom: '',
    dateTo: ''
  }

  searchOptions: any = {
    Placeholder: 'Tìm kiếm theo tỉnh, huyện, xã',
    Items: [
      {
        Name: 'Tỉnh',
        FieldName: 'ProvinceId',
        Placeholder: 'Tỉnh',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Province,
        DisplayName: 'name',
        ValueName: 'id',
        IsRelation: true,
        RelationIndexTo: 1
      },
      {
        Name: 'Huyện',
        FieldName: 'DistrictId',
        Placeholder: 'Huyện',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Districti,
        DisplayName: 'name',
        ValueName: 'id',
        IsRelation: true,
        RelationIndexFrom: 0,
        RelationIndexTo: 2
      },
      {
        Name: 'Xã',
        FieldName: 'WardId',
        Placeholder: 'Xã',
        Type: 'ngselect',
        DataType: this.constant.SearchDataType.Ward,
        DisplayName: 'name',
        ValueName: 'id',
        RelationIndexFrom: 1
      },
    ]
  }

  ngOnInit(): void {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.appSetting.PageTitle = "Thống kê người học";
    this.loadMonth();
    this.loadYear();
    this.reportLearner();
  }

  ngOnDestroy() {
    window.removeEventListener('ps-scroll-x', null);
  }

  clear() {
    this.model = {
      TimeType: "6",
      Month: new Date().getUTCMonth(),
      Year: 2020,
      Quarter: 1,
      ProvinceId: '',
      DistrictId: '',
      WardId: '',
      dateFrom: '',
      dateTo: ''
    }
    this.startDate = null;
    this.finishDate = null;
    this.reportLearner();
  }

  showErrorDate(dateto, datefrom) {
    var day = ( new Date(dateto).getTime() - new Date(datefrom).getTime()) / (1000 * 3600 * 24);
    if (day > 45) {
      this.messageService.showWarning("Bạn không thể chọn quá 45 ngày!")
    }
  }

  reportLearner() {
    if ((this.startDate != null && this.startDate != undefined) && (this.finishDate == null || this.finishDate == undefined)) {
      this.finishDate = this.dateUtils.getDateNowToObject();
      this.showErrorDate(this.dateUtils.convertObjectToDate(this.finishDate), this.dateUtils.convertObjectToDate(this.startDate))
    }
    else if((this.startDate != null && this.startDate != undefined) && (this.finishDate != null || this.finishDate != undefined)){
      this.showErrorDate(this.dateUtils.convertObjectToDate(this.finishDate), this.dateUtils.convertObjectToDate(this.startDate))
    }


    if (this.startDate != null && this.startDate != undefined) {
      this.model.dateFrom = this.dateUtils.convertObjectToDate(this.startDate);
    }
    else {
      this.model.dateFrom = null;
    }

    if (this.finishDate != null &&this.finishDate != undefined) {
      this.model.dateTo = this.dateUtils.convertObjectToDate(this.finishDate);
    }
    else {
      this.model.dateTo = null;
    }
    this.reportLearnerService.reportLearner(this.model).subscribe((data: any) => {
      if (data.data) {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.heightChart = document.getElementById("myChart").clientHeight - 80;
          this.barChartData = [
            {
              data: data.data.listData,
              label: 'Số lượng người học',
              borderColor: '#1ff70b',
              fill: false,
              //bezierCurve : false,
              lineTension: 0,
            }
          ];
          this.barListView = data.data.listLable;
          this.listData = data.data.listResult;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }
      else {
        this.messageService.showError(data.message);
      }
    });
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

  exportExcel() {
    this.reportLearnerService.exportExcel(this.model).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.fileProcess.downloadFile(data.data.path, data.data.name);
        // var link = document.createElement('a');
        // link.setAttribute("type", "hidden");
        // link.href = this.config.ServerApi + data.data;
        // link.download = 'Download.docx';
        // document.body.appendChild(link);
        // link.focus();
        // link.click();
      }
      else {
        this.messageService.showListMessage(data.message);
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  exportPdf() {
    this.reportLearnerService.exportPdf(this.model).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.fileProcess.downloadFile(data.data.path, data.data.name);
        // var link = document.createElement('a');
        // link.setAttribute("type", "hidden");
        // link.href = this.config.ServerApi + data.data;
        // link.download = 'Download.docx';
        // document.body.appendChild(link);
        // link.focus();
        // link.click();
      }
      else {
        this.messageService.showListMessage(data.message);
      }
    }, e => {
      this.messageService.showError(e);
    });
  }
}
