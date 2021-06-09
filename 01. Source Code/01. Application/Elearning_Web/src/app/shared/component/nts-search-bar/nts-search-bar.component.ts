import {
  Component,
  ViewEncapsulation,
  OnInit,
  Input,
  Output,
  EventEmitter,
  forwardRef,
  ChangeDetectorRef,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { NtsSearchService } from '../../services/nts-search.service';
import { Constants } from '../../common/Constants';
import { DateUtils } from '../../common/date-utils';
import { ComboboxService } from '../../services/combobox.service';
@Component({
  selector: 'nts-search-bar',
  templateUrl: './nts-search-bar.component.html',
  styleUrls: ['./nts-search-bar.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NTSSearchBarComponent),
      multi: true,
    },
  ],
  encapsulation: ViewEncapsulation.None,
})
export class NTSSearchBarComponent implements ControlValueAccessor {
  constructor(
    private _cd: ChangeDetectorRef,
    private ntsSearchService: NtsSearchService,
    private comboboxService: ComboboxService,
    public constants: Constants,
    private dateUtils: DateUtils
  ) { }

  count = 0;
  @Input()
  get options() {
    return this._options;
  }
  set options(value: any) {
    this._options = value;
  }

  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  public _options: any = {};
  public _searchModel: any = {};
  public _searchModelView: any = {};
  private _searchItemId = 0;
  public _searchItems: any[] = [];
  public _searchValues: any[] = [];

  @Output('change') changeEvent = new EventEmitter();
  disabled = false;

  ngOnInit() {
    for (let i = 0; i < this._options.Items.length; i++) {
      this._options.Items[i].Index = i;
      this._searchItems.push({
        Id: i,
        Name: this._options.Items[i].Name,
        Value: null,
        FieldName: this._options.Items[i].FieldName,
        FieldNameTo: this._options.Items[i].FieldNameTo,
        FieldNameFrom: this._options.Items[i].FieldNameFrom,
        Type: this._options.Items[i].Type,
        Permission: this._options.Items[i].Permission,
      });

      if (this._options.Items[i].DataType) {
        //this.getDataByDataType(this._options.Items[i], true);
      }
    }
  }

  writeValue(value: any | any[]): void {
    if (value) {
      this._searchModel = value;
      this._searchModelView = Object.assign({}, value);
      this._searchValues = [];
      this.count = 0;
      for (let i = 0; i < this._searchItems.length; i++) {
        this._searchItems[i].Checked = false;
        this._searchItems[i].Value = null;
        if (!this._options.Items[i].DataType) {
          if (this._searchItems[i].Type != 'date') {
            if (
              this._searchModel[this._searchItems[i].FieldName] != null &&
              this._searchModel[this._searchItems[i].FieldName] != '' &&
              this._searchModel[this._searchItems[i].FieldName] != undefined
            ) {
              if (this._options.Items[i].Type === 'select') {
                this.setValueDefaut(this._options.Items[i]);
              } else {
                this._searchItems[i].Checked = true;
                this._searchItems[i].Value = this._searchModel[
                  this._searchItems[i].FieldName
                ];
              }
            }
            else if (this._searchModel[this._searchItems[i].FieldName] == 0 && this._options.Items[i].Type === 'select') {
              this.setValueDefaut(this._options.Items[i]);
            }
          } else {
            if (
              this._searchModel[this._searchItems[i].FieldNameFrom] != null &&
              this._searchModel[this._searchItems[i].FieldNameFrom] != '' &&
              this._searchModel[this._searchItems[i].FieldNameFrom] != undefined
            ) {
              this._searchItems[i].Checked = true;
              this._searchItems[i].Value =
                (this._searchModel[this._searchItems[i].FieldNameFrom].day < 10
                  ? '0' +
                  this._searchModel[this._searchItems[i].FieldNameFrom].day
                  : this._searchModel[this._searchItems[i].FieldNameFrom].day) +
                '/' +
                (this._searchModel[this._searchItems[i].FieldNameFrom].month <
                  10
                  ? '0' +
                  this._searchModel[this._searchItems[i].FieldNameFrom].month
                  : this._searchModel[this._searchItems[i].FieldNameFrom]
                    .month) +
                '/' +
                this._searchModel[this._searchItems[i].FieldNameFrom].year;
            }

            if (
              this._searchModel[this._searchItems[i].FieldNameTo] != null &&
              this._searchModel[this._searchItems[i].FieldNameTo] != '' &&
              this._searchModel[this._searchItems[i].FieldNameTo] != undefined
            ) {
              this._searchItems[i].Checked = true;
              this._searchItems[i].Value +=
                ' - ' +
                (this._searchModel[this._searchItems[i].FieldNameTo].day < 10
                  ? '0' +
                  this._searchModel[this._searchItems[i].FieldNameTo].day
                  : this._searchModel[this._searchItems[i].FieldNameTo].day) +
                '/' +
                (this._searchModel[this._searchItems[i].FieldNameTo].month < 10
                  ? '0' +
                  this._searchModel[this._searchItems[i].FieldNameTo].month
                  : this._searchModel[this._searchItems[i].FieldNameTo].month) +
                '/' +
                this._searchModel[this._searchItems[i].FieldNameTo].year;
            }
          }

          if (
            this._searchItems[i].Checked &&
            this._options.Items[i].Type != 'select'
          ) {
            this._searchValues.push({
              Name: this._searchItems[i].Name,
              Value: this._searchItems[i].Value,
              Index: i,
            });
          }
        }
      }

      this.getData();
    }
    this._cd.markForCheck();
  }

  registerOnChange(fn: any): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this._onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
    this._cd.markForCheck();
  }

  searchItemChange(event) {
    let value = event.currentTarget.selectedOptions[0].value;
    if (value >= 0) {
      this._searchItems[value].Checked = true;
      this.count++;
    }
  }

  removeItem(index) {
    this._searchItems[index].Checked = false;
    this.count--;
  }

  removeSearch(index, itemIndex) {
    this._searchItems[itemIndex].Checked = false;
    this._searchItems[itemIndex].Value = null;
    if (this._searchItems[itemIndex].Type == 'date') {
      this._searchModelView[this._searchItems[itemIndex].FieldNameFrom] = null;
      this._searchModelView[this._searchItems[itemIndex].FieldNameTo] = null;
    } else {
      this._searchModelView[this._searchItems[itemIndex].FieldName] = null;
    }

    this._searchValues.splice(index, 1);
    this.count--;

    this._searchModel = Object.assign({}, this._searchModelView);

    this._onChange(this._searchModel);
    this.changeEvent.emit(this._searchModel);

    if (this._options.Items[index].IsRelation) {
      this.getDataByDataType(
        this._options.Items[this._options.Items[index].RelationIndexTo],
        false
      );
    }
  }

  search = () => {
    this._searchValues = [];
    this.count = 0;
    for (let i = 0; i < this._searchItems.length; i++) {
      if (this._searchItems[i].Checked && this._searchItems[i].Value) {
        this._searchValues.push({
          Name: this._searchItems[i].Name,
          Value: this._searchItems[i].Value,
          Index: i,
        });
        this.count++;
      } else {
        this._searchItems[i].Checked = false;
        if (this._searchItems[i].Type == 'date') {
          this._searchModelView[this._searchItems[i].FieldNameFrom] = null;
          this._searchModelView[this._searchItems[i].FieldNameTo] = null;
        } else {
          this._searchModelView[this._searchItems[i].FieldName] = null;
        }
      }
    }

    this._searchModel = Object.assign({}, this._searchModelView);

    this._onChange(this._searchModel);
    this.changeEvent.emit(this._searchModel);
  };

  selectChange(event, index, name) {
    let value = event.currentTarget.selectedOptions[0].value;
    if (value) {
      this._searchItems[index].Value =
        event.currentTarget.selectedOptions[0].text;
    } else {
      this._searchItems[index].Value = '';
    }

    if (this._options.Items[index].IsRelation) {
      this.getDataByDataType(
        this._options.Items[this._options.Items[index].RelationIndexTo],
        false
      );
    }
  }

  ngSelectChange(event, index, valueName, displayName) {
    let value = null;
    if (event) {
      value = event[valueName];
    }

    if (value != '' || value != null) {
      this._searchItems[index].Value = event[displayName];
    } else {
      this._searchItems[index].Value = '';
    }

    if (this._options.Items[index].IsRelation) {
      var a = this._options.Items[this._options.Items[index].RelationIndexTo];
      this.getDataByDataType(
        this._options.Items[this._options.Items[index].RelationIndexTo],
        false
      );
    }
  }

  object: any;
  getListChildren(key, datas) {
    if (datas.children.length > 0) {
      datas.children.forEach(element => {
        if (element.key == key) {
          this.object = element;
          return this.object;
        }
        this.getListChildren(key, element);
      })
    }
  }

  ngNztreeSelectChange(key, datas, index) {
    if (key) {
      datas.forEach(element => {
        if (element.key != key && element.children.length > 0) {
          this.getListChildren(key, element);
        }
        else if (element.key == key) {
          this.object = element;
          return this.object;
        }
      });
      this._searchItems[index].Value = this.object.title;
    } else {
      this._searchItems[index].Value = '';
    }

  }

  selectChangeExpressionType(event, index, name) {
    let value = event.currentTarget.selectedOptions[0].value;
    if (
      value &&
      this._searchModelView[name] != null &&
      this._searchModelView[name] != ''
    ) {
      this._searchItems[index].Value =
        event.currentTarget.selectedOptions[0].text +
        ' ' +
        this._searchModelView[name];
    } else {
      this._searchItems[index].Value = '';
    }
  }

  searchContentChange(contentName) {
    this._searchModel[contentName] = this._searchModelView[contentName];
    this._onChange(this._searchModel);
  }

  textChange(index, name) {
    this._searchItems[index].Value = this._searchModelView[name];
  }

  numberChange(index, name, nameType) {
    if (this._searchModelView[nameType] > 0) {
      this._searchItems[index].Value =
        this.constants.SearchExpressionTypes[
          this._searchModelView[nameType] - 1
        ].Name +
        ' ' +
        this._searchModelView[name];
    } else {
      this._searchItems[index].Value = '';
    }
  }

  numberChangeYear(index, name) {
    this._searchItems[index].Value = this._searchModelView[name];
  }

  showPopover(popover) {
    if (popover.isOpen()) {
      popover.close();
    } else {
      this.count = 0;

      this._searchModelView = Object.assign({}, this._searchModel);

      for (let i = 0; i < this._searchItems.length; i++) {
        this._searchItems[i].Checked = false;
        this._searchItems[i].Value = null;

        for (let j = 0; j < this._searchValues.length; j++) {
          if (this._searchValues[j].Index == i) {
            this._searchItems[i].Checked = true;
            this._searchItems[i].Value = this._searchValues[j].Value;
          }
        }

        if (!this._searchItems[i].Checked) {
          if (this._searchItems[i].Type == 'date') {
            this._searchModelView[this._searchItems[i].FieldNameFrom] = null;
            this._searchModelView[this._searchItems[i].FieldNameTo] = null;
          } else {
            this._searchModelView[this._searchItems[i].FieldName] = null;
          }
        }
      }

      popover.open();
    }
  }

  dateChange(index, nameFrom, nameTo) {
    this._searchItems[index].Value = '';
    if (this._searchModelView[nameFrom]) {
      this._searchItems[index].Value =
        (this._searchModelView[nameFrom].day < 10
          ? '0' + this._searchModelView[nameFrom].day
          : this._searchModelView[nameFrom].day) +
        '/' +
        (this._searchModelView[nameFrom].month < 10
          ? '0' + this._searchModelView[nameFrom].month
          : this._searchModelView[nameFrom].month) +
        '/' +
        this._searchModelView[nameFrom].year;
    }

    if (this._searchModelView[nameTo]) {
      this._searchItems[index].Value +=
        ' - ' +
        (this._searchModelView[nameTo].day < 10
          ? '0' + this._searchModelView[nameTo].day
          : this._searchModelView[nameTo].day) +
        '/' +
        (this._searchModelView[nameTo].month < 10
          ? '0' + this._searchModelView[nameTo].month
          : this._searchModelView[nameTo].month) +
        '/' +
        this._searchModelView[nameTo].year;
    }

    if (this._options.Items[index].IsRelation) {
      this.getDataByDataType(
        this._options.Items[this._options.Items[index].RelationIndexTo],
        false
      );
    }
  }

  dropdownChange(index) {
    let displayName = this._options.Items[index].DisplayName;
    let valueName = this._options.Items[index].ValueName;
    let valueSelect = this._searchModelView[
      this._options.Items[index].FieldName
    ];
    let selected = this._options.Items[index].Data.filter(function (data) {
      if (data[valueName] == valueSelect) {
        return data;
      }
    });

    if (selected && selected.length > 0) {
      this._searchItems[index].Value = selected[0][displayName];
    } else {
      this._searchItems[index].Value = null;
    }

    if (this._options.Items[index].IsRelation) {
      this.getDataByDataType(
        this._options.Items[this._options.Items[index].RelationIndexTo],
        false
      );
    }
  }

  getData() {
    for (let i = 0; i < this._options.Items.length; i++) {
      if (
        this._options.Items[i].DataType &&
        (!this._options.Items[i].RelationIndexFrom ||
          this._options.Items[i].RelationIndexFrom == 0)
      ) {
        this.getDataByDataType(this._options.Items[i], true);
      }
    }
  }

  setValueDefaut(item) {
    var isExist = false;
    for (let i = 0; i < this._searchItems.length; i++) {
      if (item.Index == this._searchItems[i].Id) {
        if (
          this._searchModel[this._searchItems[i].FieldName] != null &&
          this._searchModel[this._searchItems[i].FieldName] != '' &&
          this._searchModel[this._searchItems[i].FieldName] != undefined
        ) {
          let valueName = item.ValueName;
          let valueSelect = this._searchModelView[item.FieldName];
          let selected = item.Data.filter(function (data) {
            if (data[valueName] == valueSelect) {
              return data;
            }
          });

          if (selected && selected.length > 0) {
            this._searchItems[i].Checked = true;
            this._searchItems[i].Value = selected[0][item.DisplayName];

            isExist = false;
            for (let index = 0; index < this._searchValues.length; index++) {
              if (this._searchValues[index].Index == i) {
                isExist = true;
                this._searchValues[index].Name = this._searchItems[i].Name;
                this._searchValues[index].Value = this._searchItems[i].Value;
              }
            }

            if (!isExist) {
              this._searchValues.push({
                Name: this._searchItems[i].Name,
                Value: this._searchItems[i].Value,
                Index: i,
              });
            }
          }
        } else if (this._searchModel[this._searchItems[i].FieldName] === 0 && this._options.Items[i].Type === 'select') {
          let valueName = item.ValueName;
          let valueSelect = this._searchModelView[item.FieldName];
          let selected = item.Data.filter(function (data) {
            if (data[valueName] == valueSelect) {
              return data;
            }
          });

          if (selected && selected.length > 0) {
            this._searchItems[i].Checked = true;
            this._searchItems[i].Value = selected[0][item.DisplayName];

            isExist = false;
            for (let index = 0; index < this._searchValues.length; index++) {
              if (this._searchValues[index].Index == i) {
                isExist = true;
                this._searchValues[index].Name = this._searchItems[i].Name;
                this._searchValues[index].Value = this._searchItems[i].Value;
              }
            }

            if (!isExist) {
              this._searchValues.push({
                Name: this._searchItems[i].Name,
                Value: this._searchItems[i].Value,
                Index: i,
              });
            }
          }
        }
      }
    }
  }

  getDataByDataType(item, isLoad: boolean) {
    switch (item.DataType) {
      // Ví dụ
      case this.constants.SearchDataType.Sample:
        this.getSample(item, isLoad);
        break;
      case this.constants.SearchDataType.Category:
        this.getCategory(item, isLoad);
        break;
      case this.constants.SearchDataType.Program:
        this.getProgram(item, isLoad);
        break;
      case this.constants.SearchDataType.Topic:
        this.getTopic(item, isLoad);
        break;
      case this.constants.SearchDataType.Nation:
        this.getListNation(item, isLoad);
        break;
      case this.constants.SearchDataType.Province:
        this.getListProvince(item, isLoad);
        break;
      case this.constants.SearchDataType.Districti:
        this.getListDistrictByProvinceId(item, isLoad);
        break;
      case this.constants.SearchDataType.Ward:
        this.getListWardByDistrictId(item, isLoad);
        break;
      case this.constants.SearchDataType.User:
        this.getListUser(item, isLoad);
        break;
      case this.constants.SearchDataType.Learner:
        this.getListLearner(item, isLoad);
      case this.constants.SearchDataType.ManageUnit:
        this.getListManageUnit(item, isLoad);
        break;
      default:
        break;
    }
  }

  getSample(item, isLoad: boolean) {
    let data: any[] = [
      {
        Id: 1,
        Name: 'Name 1',
      },
      {
        Id: 2,
        Name: 'Name 2',
      },
    ];

    item.Data = data;

    // this.ntsSearchService.getCbbCustomer().subscribe((data: any) => {
    //   item.Data = data;
    //   if (isLoad) {
    //     this.setValueDefaut(item);
    //     if (item.IsRelation) {
    //       this.getDataByDataType(this._options.Items[item.RelationIndexTo], isLoad);
    //     }
    //   }
    // });
  }

  getCategory(item: any, isLoad: boolean) {
    this.comboboxService.getCategoryParent().subscribe((data: any) => {
      item.Data = data.data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(
            this._options.Items[item.RelationIndexTo],
            isLoad
          );
        }
      }
    });
  }

  getProgram(item: any, isLoad: boolean) {
    this.comboboxService.getProgram().subscribe((data: any) => {
      item.Data = data.data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(
            this._options.Items[item.RelationIndexTo],
            isLoad
          );
        }
      }
    });
  }

  getTopic(item: any, isLoad: boolean) {
    this.comboboxService.getTopicFull().subscribe((data: any) => {
      item.Data = data.data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(
            this._options.Items[item.RelationIndexTo],
            isLoad
          );
        }
      }
    });
  }

  getListNation(item: any, isLoad: boolean) {
    this.comboboxService.getListNation().subscribe((data: any) => {
      item.Data = data.data.dataResults;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(
            this._options.Items[item.RelationIndexTo],
            isLoad
          );
        }
      }
    });
  }

  getListProvince(item: any, isLoad: boolean) {
    this.comboboxService.getListProvince().subscribe((data: any) => {
      item.Data = data.data.dataResults;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(
            this._options.Items[item.RelationIndexTo],
            isLoad
          );
        }
      }
    });
  }

  getListDistrictByProvinceId(item: any, isLoad: boolean) {
    var provinceId = '';

    if (item.RelationIndexFrom >= 0 && this._searchModelView) {
      provinceId = this._searchModelView[
        this._options.Items[item.RelationIndexFrom].FieldName
      ];
    }

    this.comboboxService
      .getListDistrictByProvinceId(provinceId)
      .subscribe((data: any) => {
        item.Data = data.data.dataResults;
        if (isLoad) {
          this.setValueDefaut(item);
          if (item.IsRelation) {
            this.getDataByDataType(
              this._options.Items[item.RelationIndexTo],
              isLoad
            );
          }
        }
      });
  }

  getListWardByDistrictId(item: any, isLoad: boolean) {
    var districtId = '';

    if (item.RelationIndexFrom >= 0 && this._searchModelView) {
      districtId = this._searchModelView[
        this._options.Items[item.RelationIndexFrom].FieldName
      ];
    }

    this.comboboxService
      .getListWardByDistrictId(districtId)
      .subscribe((data: any) => {
        item.Data = data.data.dataResults;
        if (isLoad) {
          this.setValueDefaut(item);
          if (item.IsRelation) {
            this.getDataByDataType(
              this._options.Items[item.RelationIndexTo],
              isLoad
            );
          }
        }
      });
  }

  getListUser(item: any, isLoad: boolean) {
    this.comboboxService.getUser().subscribe((data: any) => {
      item.Data = data.data.dataResults;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(
            this._options.Items[item.RelationIndexTo],
            isLoad
          );
        }
      }
    });
  }

  getListLearner(item: any, isLoad: boolean) {
    this.comboboxService.getLearner().subscribe((data: any) => {
      item.Data = data.data.dataResults;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(
            this._options.Items[item.RelationIndexTo],
            isLoad
          );
        }
      }
    });
  }
  getListManageUnit(item: any, isLoad: boolean) {
    this.comboboxService.getListManageUnit().subscribe((data: any) => {
      item.Data = data.data;
      if (isLoad) {
        this.setValueDefaut(item);
        if (item.IsRelation) {
          this.getDataByDataType(
            this._options.Items[item.RelationIndexTo],
            isLoad
          );
        }
      }
    });
  }
}
