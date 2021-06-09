import { Component, OnInit, Input, ViewEncapsulation, forwardRef, ChangeDetectorRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MentorCreateComponent } from '../mentor-create/mentor-create.component';
@Component({
  selector: 'app-mentor-manage',
  templateUrl: './mentor-manage.component.html',
  styleUrls: ['./mentor-manage.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => MentorManageComponent),
    multi: true
  }
  ],
  encapsulation: ViewEncapsulation.None
})
export class MentorManageComponent implements OnInit, ControlValueAccessor {

  @Input() status: number;
  public _listMentor;
  get listMentor(): any {
    return this._listMentor;
  }
  @Input()
  set listMentor(val: any) {
    this._listMentor = val;
  }

  output = [];
  constructor(
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private messageService: MessageService,
    private _cd: ChangeDetectorRef
  ) { }

  @Input()
  get items() { return this._items };
  set items(value: any[]) {
    this.listMentor = value;
  };

  _items = [];
  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any | any[]): void {
    if (value != null) {
      this.listMentor = value;
    } else {
      this.listMentor = [];
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
    this._cd.markForCheck();
  }

  startIndex = 1;
  dataSelected = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
  }

  ngOnInit(): void {
  }

  ngOnChanges() {
    this.view();
  }

  view() {
    this.dataSelected = [];
    this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
    this.model.TotalItems = this.listMentor.length;
    this.listMentor.forEach(element => {
      this.dataSelected.push(element.id);
    });
  }

  showCreateUpdate() {
    let activeModal = this.modalService.open(MentorCreateComponent, { container: 'body', windowClass: 'mentor-create-model', backdrop: 'static' })
    activeModal.componentInstance.dataSelected = this.dataSelected;
    activeModal.result.then((result) => {
      if (result !== true) {
        this.dataSelected = [];
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listMentor = result.data;
        this.model.TotalItems = result.totalItems;
        this.listMentor.forEach(element => {
          this.dataSelected.push(element.id);
        });
        this._onChange(this.listMentor ? this.listMentor : []);
      }
    }, (reason) => {
    });
  }

  confirmDelete(row) {
    this.messageService.showConfirm("Bạn chắc chắn muốn xóa giảng viên này?").then(
      data => {
        this.dataSelected = [];
        const index = this.listMentor.indexOf(row);
        this.listMentor.splice(index, 1);
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.model.TotalItems = this.listMentor.length;
        this.listMentor.forEach(element => {
          this.dataSelected.push(element.id);
        });
        this._onChange(this.listMentor ? this.listMentor : []);
      }
    );
  }
}
