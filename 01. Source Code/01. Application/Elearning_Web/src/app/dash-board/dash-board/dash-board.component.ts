import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Constants, AppSetting, MessageService, Configuration } from 'src/app/shared';
import { DashBoardService } from '../service/dash-board.service';

@Component({
  selector: 'app-dash-board',
  templateUrl: './dash-board.component.html',
  styleUrls: ['./dash-board.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DashBoardComponent implements OnInit {

  constructor(
    public constant: Constants,
    private appSetting: AppSetting,
    private messageService: MessageService,
    private dashBoardService: DashBoardService,
    public config: Configuration,
  ) { }

  totalLearner = 0;
  totalLearnerStatus = 0;
  totalRegister = 0;
  totalHistoryLogin = 0;
  listCourse: any[] = [];
  listCourseTop: any[] = [];

  courseMode: any = {
    TimeType: "6",
    Month: 6,
    Year: new Date().getFullYear(),
    Quarter: 1,
    DateFromV: null,
    DateToV: null,
  }

  registerModel: any = {
    TimeType: "6",
    Month: 6,
    Year: new Date().getFullYear(),
    Quarter: 1,
    DateFromV: null,
    DateToV: null,
  }

  provinceModel: any = {
    TimeType: "6",
    Month: 6,
    Year: new Date().getFullYear(),
    Quarter: 1,
    DateFromV: null,
    DateToV: null,
  }

  public textViewChartCourse: string = 'Biểu đồ đăng ký khóa học';
  public barListViewCourse: string[];
  public barChartTypeCourse: string = 'line';
  public barChartLegendCourse: boolean = true;
  public barChartOptionsCourse: any = {
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
  public barChartDataCourse: any[] = [
    { data: [], label: 'Số người học' },
  ];
  public chartColorsCourse: Array<any> = [
    { backgroundColor: '#1ff70b' }
  ];

  public textViewChartRegister: string = 'Biểu đồ đăng ký tài khoản';
  public barListViewRegister: string[];
  public barChartTypeRegister: string = 'bar';
  public barChartLegendRegister: boolean = true;
  public barChartOptionsRegister: any = {
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
  public barChartDataRegister: any[] = [
    { data: [], label: 'Đăng ký Google' },
    { data: [], label: 'Đăng ký Facebook' },
    { data: [], label: 'Đăng ký Email' }
  ];
  public chartColorsRegister: Array<any> = [
    { backgroundColor: '#1ff70b' },
    { backgroundColor: '#ed1b24' }
  ];

  public textViewChartProvince: string = 'Biểu đồ Tỉnh, Thành phố có nhiều người học';
  public barListViewProvince: string[];
  public barChartTypeProvince: string = 'pie';
  public barChartLegendProvince: boolean = false;
  public barChartOptionsProvince: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    legend: {
      position: 'top',
    },
    plugins: {
      datalabels: {
        formatter: (value, ctx) => {
          const label = ctx.chart.data.labels[ctx.dataIndex];
          return label;
        },
      },
    }
  };
  public barChartDataProvince: any[] = [
    { data: [], label: 'Số người học' },
  ];
  public chartColorsProvince: Array<any> = [
    { backgroundColor: ['rgba(255, 99, 132, 1)', 'rgba(54, 162, 235,1)', 'rgba(255, 206, 86, 1)', 'rgba(75, 192, 192, 1)', 'rgba(153, 102, 255, 1)', 'rgba(255, 159, 64, 1)', 'rgba(153, 153, 255)', 'rgba(255, 153, 255)', 'rgb(255, 153, 153)', 'rgb(153, 255, 179)'] },
    { borderColor: ['rgba(255, 99, 132, 1)', 'rgba(54, 162, 235, 1)', 'rgba(255, 206, 86, 1)', 'rgba(75, 192, 192, 1)', 'rgba(153, 102, 255, 1)', 'rgba(255, 159, 64, 1)', 'rgba(153, 153, 255)', 'rgba(255, 153, 255)', 'rgb(255, 153, 153)', 'rgb(153, 255, 179)'] }
  ];

  ngOnInit(): void {
    this.appSetting.PageTitle = "Dashboard";
    this.getTotalAsync();
    this.getRegisterCourse(this.constant.TimeType_ThisYear);
    this.getRegister(this.constant.TimeType_ThisYear);
    this.getProvince(this.constant.TimeType_ThisYear);
  }

  getTotalAsync() {
    this.dashBoardService.getTotalAsync().subscribe((data: any) => {
      if (data.data) {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.totalLearner = data.data.totalLearner;
          this.totalLearnerStatus = data.data.totalLearnerStatus;
          this.totalRegister = data.data.totalRegister;
          this.totalHistoryLogin = data.data.totalHistoryLogin;
          this.listCourse = data.data.listCourse;
          this.listCourseTop = data.data.listCourseTop;
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

  getRegisterCourse(timeType: any) {
    this.courseMode.TimeType = timeType;
    this.dashBoardService.getRegisterCourse(this.courseMode).subscribe((data: any) => {
      if (data.data) {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.barChartDataCourse = [
            {
              data: data.data.listData,
              label: 'Số người học',
              borderColor: '#1ff70b',
              fill: false,
              //bezierCurve : false,
              lineTension: 0,
            }
          ];
          this.barListViewCourse = data.data.listLable;
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

  getRegister(timeType: any) {
    this.registerModel.TimeType = timeType;
    this.dashBoardService.getRegister(this.registerModel).subscribe((data: any) => {
      if (data.data) {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.barChartDataRegister = [
            {
              data: data.data.listGoogle,
              label: 'Đăng ký Google',
              borderColor: '#1ff70b',
              fill: false,
              //bezierCurve : false,
              lineTension: 0,
            },
            {
              data: data.data.listFacebook,
              label: 'Đăng ký Facebook',
              borderColor: '#ed1b24',
              fill: false,
              //bezierCurve : false,
              lineTension: 0,
            },

            {
              data: data.data.listEmail,
              label: 'Đăng ký Email',
              borderColor: '#F4FA58',
              fill: false,
              //bezierCurve : false,
              lineTension: 0,
            },
          ];
          this.barListViewRegister = data.data.listLable;
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

  getProvince(timeType: any) {
    this.provinceModel.TimeType = timeType;
    this.dashBoardService.getProvince(this.provinceModel).subscribe((data: any) => {
      if (data.data) {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.barChartDataProvince = [
            {
              data: data.data.listData,
              label: 'Số người học',
            }
          ];
          this.barListViewProvince = data.data.listLable;
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
}
