import { Component, OnInit, Input, ViewEncapsulation, forwardRef, ChangeDetectorRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { AppSetting, Configuration, Constants, FileProcess, MessageService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LearnerCreateComponent } from '../learner-create/learner-create.component';
import { CourseService } from 'src/app/education-programs/services/course.service';

@Component({
  selector: 'app-learner-manage',
  templateUrl: './learner-manage.component.html',
  styleUrls: ['./learner-manage.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => LearnerManageComponent),
    multi: true
  }
  ],
  encapsulation: ViewEncapsulation.None
})
export class LearnerManageComponent implements OnInit {

  @Input() status: number;
  public _listLeaner;
  get listLeaner(): any {
    return this._listLeaner;
  }
  @Input()
  set listLeaner(val: any) {
    this._listLeaner = val;
  }

  public _course;
  get course(): any {
    return this._course;
  }
  @Input()
  set course(val: any) {
    this._course = val;
  }

  public _templateId;
  get templateId(): any {
    return this._templateId;
  }
  @Input()
  set templateId(val: any) {
    this._templateId = val;
  }

  constructor(
    public constant: Constants,
    public config: Configuration,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private messageService: MessageService,
    private _cd: ChangeDetectorRef,
    private courseService: CourseService,
    private fileProcess:FileProcess
  ) { }

  @Input()
  get items() { return this._items };
  set items(value: any[]) {
    this.listLeaner = value;
  };

  _items = [];
  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any | any[]): void {
    if (value != null) {
      this.listLeaner = value;
    } else {
      this.listLeaner = [];
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

  modelPrint : any = {
    TemplateId: '',
    CourseName: '',
    CourseId: '',
    LearnerIds: []
  }

  listTemplate: any = [];
  dataSelect: any = [];

  ngOnInit(): void {
    this.getFileTemplates();
  }

  ngOnChanges() {
    this.view();
  }

  getFileTemplates() {
    this.courseService.getFileTemplates().subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listTemplate = data.data;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  view() {
    this.dataSelected = [];
    this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
    this.model.TotalItems = this.listLeaner.length;
    this.listLeaner.forEach(element => {
      this.dataSelected.push(element);
    });
  }

  showCreateUpdate() {
    let activeModal = this.modalService.open(LearnerCreateComponent, { container: 'body', windowClass: 'learner-create-model', backdrop: 'static' })
    activeModal.componentInstance.dataSelected = this.dataSelected;
    activeModal.result.then((result) => {
      if (result !== true) {
        this.dataSelected = [];
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listLeaner = result.data;
        this.model.TotalItems = result.totalItems;
        this.listLeaner.forEach(element => {
          this.dataSelected.push(element);
        });
        this._onChange(this.listLeaner ? this.listLeaner : []);
      }
    }, (reason) => {
    });
  }

  confirmDelete(row) {
    this.messageService.showConfirm("Bạn chắc chắn muốn xóa người học này?").then(
      data => {
        this.dataSelected = [];
        const index = this.listLeaner.indexOf(row);
        this.listLeaner.splice(index, 1);
        const indexDataSelect = this.dataSelect.indexOf(row.id);
        if(indexDataSelect !== -1){
          this.dataSelect.splice(indexDataSelect,1);
        }
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.model.TotalItems = this.listLeaner.length;
        this.listLeaner.forEach(element => {
          this.dataSelected.push(element);
        });
        this._onChange(this.listLeaner ? this.listLeaner : []);
      }
    );
  }

  printCertificate(){
    this.modelPrint.CourseId = this.course.courseId;
    this.modelPrint.CourseName = this.course.courseName;
    this.modelPrint.LearnerIds = this.dataSelect;
    this.modelPrint.TemplateId = this.templateId;
    if(this.modelPrint.LearnerIds.length === 0){
      this.messageService.showMessage("Hãy chọn học viên cần cấp chứng chỉ!");
    }
    else{
      this.courseService.printCertificate(this.modelPrint).subscribe(
        (data: any) => {
          if (data.statusCode == this.constant.StatusCode.Success) {
            if(data.data !== null){
              var link = document.createElement('a');
              link.setAttribute("type", "hidden");
              link.href = this.config.ServerApi + data.data;
              this.fileProcess.downloadFileLink(link.href,'Download.xlsx');
              this.messageService.showSuccess("In thành công");
            }
            else{
              this.messageService.showMessage("Template không phù hợp");
            }
          }
          else {
            this.messageService.showListMessage(data.message);
          }
        }, error => {
          this.messageService.showError(error);
        }
      );
    }
  }
 
  selectChange($event, row) {
    const index = this.dataSelect.indexOf(row.id);
    if (row.isChecked) {
      if(index === -1){
        this.dataSelect.push(row.id);
      }
    }
    else {
      this.dataSelect.splice(index, 1);
    }
  }
}
