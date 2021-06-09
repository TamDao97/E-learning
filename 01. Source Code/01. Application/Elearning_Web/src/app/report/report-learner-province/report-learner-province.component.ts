import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Constants, AppSetting, MessageService, DateUtils, Configuration, FileProcess } from 'src/app/shared';
import { ReportLearnerProvinceService } from '../service/report-learner-province.service';

@Component({
  selector: 'app-report-learner-province',
  templateUrl: './report-learner-province.component.html',
  styleUrls: ['./report-learner-province.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReportLearnerProvinceComponent implements OnInit {
  @ViewChild('scrollHeaderOne') scrollHeaderOne: ElementRef;
  constructor(
    public constant: Constants,
    private appSetting: AppSetting,
    private messageService: MessageService,
    private reportLearnerProvinceService: ReportLearnerProvinceService,
    public dateUtils: DateUtils,
    private config: Configuration,
    private fileProcess: FileProcess,
  ) { }

  year: number = new Date().getFullYear();
  listYear: any[] = [];
  listProvin: any[] = [];
  listGender: any[] = [];
  listAge: any[] = [];
  listMonth = [];
  startDate: any;
  finishDate: any;
  model: any = {
    TimeType: "6",
    Month: new Date().getUTCMonth(),
    Year: 2020,
    Quarter: 1,
    ProvinceId: '',
    DistrictId: '',
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
      // {
      //   Name: 'Huyện',
      //   FieldName: 'DistrictId',
      //   Placeholder: 'Huyện',
      //   Type: 'ngselect',
      //   DataType: this.constant.SearchDataType.Districti,
      //   DisplayName: 'name',
      //   ValueName: 'id',
      //   IsRelation: true,
      //   RelationIndexFrom: 0,
      //   RelationIndexTo: 2
      // },
    ]
  }

  heightProvin: number = 300;
  heightGender: number = 300;
  heightAge: number = 300;
  modelExport: any = {
    Type: null,
    ExportType: null,
    ListResult: []
  }

  public textViewChartProvin: string = 'Thống kê người học theo Tỉnh, Thành phố';
  public textViewChartDistrict: string = 'Thống kê người học theo Huyện, Quận';
  public barListViewProvin: string[];
  public barChartTypeProvin: string = 'pie';
  public barChartLegendProvin: boolean = false;
  public barChartOptionsProvin: any = {
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
  public barChartDataProvin: any[] = [
    { data: [], label: 'Tỉnh, Thành phố' }
  ];

  public barChartDataDistrict: any[] = [
    { data: [], label: 'Huyện, Quận' }
  ];

  public doughnutChartColors: any[] =
    [
      {
        backgroundColor:
          [
            'rgba(127,255,212)',
            'rgba(255,228,196)',
            'rgba(0,0,255)',
            'rgba(138,43,226)',
            'rgba(165,42,42)',
            'rgba(95,158,160)',
            'rgba(127,255,0)',
            'rgba(255,127,80)',
            'rgba(184,134,11)',
            'rgba(0,139,139)',
            'rgba(169,169,169)',
          ]
      }
    ]

  public textViewChartGender: string = 'Thống kê người học theo giới tính';
  public barListViewGender: string[];
  public barChartTypeGender: string = 'pie';
  public barChartLegendGender: boolean = true;
  public barChartOptionsGender: any = {
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
  
  public chartGenderColors: any[] =
    [
      {
        backgroundColor:
          [
            'rgba(127,255,212)',
            'rgba(255,228,196)',
          ]
      }
    ]
  public barChartDataGender: any[] = [
    { data: [], label: 'Giới tính' }
  ];

  public textViewChartAge: string = 'Thống kê người học theo độ tuổi';
  public barListViewAge: string[];
  public barChartTypeAge: string = 'bar';
  public barChartLegendAge: boolean = true;
  public barChartOptionsAge: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    scales: {
      yAxes: [
        {
          ticks: {
            callback: function (label, index, labels) {
              return label.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
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
          var datasetLabel = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
          return data.datasets[tooltipItem.datasetIndex].label + ': ' + datasetLabel;
        }
      }
    }
  };
  public barChartDataAge: any[] = [
    { data: [], label: 'Độ tuổi' },
  ];
  public chartColors: Array<any> = [
    { backgroundColor: ['rgba(255, 99, 132, 1)', 'rgba(54, 162, 235,1)', 'rgba(255, 206, 86, 1)', 'rgba(75, 192, 192, 1)', 'rgba(153, 102, 255, 1)', 'rgba(255, 159, 64, 1)', '#CCCCCC'] },
    { borderColor: ['rgba(255, 99, 132, 1)', 'rgba(54, 162, 235, 1)', 'rgba(255, 206, 86, 1)', 'rgba(75, 192, 192, 1)', 'rgba(153, 102, 255, 1)', 'rgba(255, 159, 64, 1)'] }
  ];

  ngOnInit(): void {

    document.forms['search']['Code'].disabled = true;
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.appSetting.PageTitle = "Thống kê thông tin người học";
    this.loadYear();
    this.loadMonth();
    this.reportLearnerProvince();
  }

  loadMonth() {
    for (var month = 1; month < 13; month++)
      this.listMonth.push(month);
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
      dateFrom: '',
      dateTo: ''
    }
    this.startDate = null;
    this.finishDate = null;
    this.reportLearnerProvince();
  }

  reportLearnerProvince() {
    if (this.startDate != null) {
      this.model.dateFrom = this.dateUtils.convertObjectToDate(this.startDate);
    }
    else {
      this.model.dateFrom = null;
    }

    if (this.finishDate != null) {
      this.model.dateTo = this.dateUtils.convertObjectToDate(this.finishDate);
    }
    else {
      this.model.dateTo = null;
    }
    this.reportLearnerProvinceService.reportLearnerProvince(this.model).subscribe((data: any) => {
      if (data.data) {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.heightProvin = document.getElementById("myProvince").clientHeight - 40;
          this.heightGender = document.getElementById("myGeder").clientHeight;
          this.heightAge = document.getElementById("myAge").clientHeight;
          this.listProvin = data.data.reportProvinces;
          this.listGender = data.data.reportGender;
          this.listAge = data.data.reportYearOld;

          if (this.model.ProvinceId == null || this.model.ProvinceId == '') {
            this.barChartDataProvin = [
              {
                data: data.data.reportProvincesData,
                label: 'Số người học',
              }
            ];
          }
          else {
            this.barChartDataDistrict = [
              {
                data: data.data.reportProvincesData,
                label: 'Số người học',
              }
            ];
          }

          this.barListViewProvin = data.data.reportProvincesLable;

          this.barChartDataGender = [
            {
              data: data.data.reportGenderData,
              label: 'Thống kê người học theo giới tính',
            }
          ];
          this.barListViewGender = data.data.reportGenderLable;

          this.barChartDataAge = [
            {
              data: data.data.reportYearOldData,
              label: 'Số người học',
            }
          ];

          this.barListViewAge = data.data.reportYearOldLable;
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

  loadYear() {
    for (var year = 2020; year <= new Date().getFullYear(); year++) {
      this.listYear.push(year);
    }
    this.listYear.unshift("Tất cả");
  }

  // type: loại thống kê
  // exportType: loại file
  exportExcel(type: number, exportType: number) {
    this.modelExport = {
      Type: type,
      ExportType: exportType,
      ListResult: []
    }

    var nameFile = "";
    if (type == 1) {
      nameFile = "Thống kê người học theo tỉnh.xlsx";
      this.modelExport.ListResult = this.listProvin;
    } else if (type == 2) {
      nameFile = "Thống kê người học theo giới tính.xlsx";
      this.modelExport.ListResult = this.listGender;
    } else if (type == 3) {
      nameFile = "Thống kê người học theo độ tuổi.xlsx";
      this.modelExport.ListResult = this.listAge;
    }

    this.reportLearnerProvinceService.exportFile(this.modelExport).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerApi + data.data;
        this.fileProcess.downloadFileLink(link.href, nameFile);
      }
      else {
        this.messageService.showListMessage(data.message);
      }
    }, e => {
      this.messageService.showError(e);
    });
  }

  exportPdf(type: number, exportType: number) {
    this.modelExport = {
      Type: type,
      ExportType: exportType,
      ListResult: []
    }

    var nameFile = "";
    if (type == 1) {
      nameFile = "Thống kê người học theo tỉnh.pdf";
      this.modelExport.ListResult = this.listProvin;
    } else if (type == 2) {
      nameFile = "Thống kê người học theo giới tính.pdf";
      this.modelExport.ListResult = this.listGender;
    } else if (type == 3) {
      nameFile = "Thống kê người học theo độ tuổi.pdf";
      this.modelExport.ListResult = this.listAge;
    }

    this.reportLearnerProvinceService.exportFile(this.modelExport).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.fileProcess.downloadFile(data.data, nameFile);
      }
      else {
        this.messageService.showListMessage(data.message);
      }
    }, e => {
      this.messageService.showError(e);
    });
  }
}
