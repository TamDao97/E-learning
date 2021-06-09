import { Component, forwardRef, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, Constants, MessageService } from 'src/app/shared';
import { ChooseTabContentComponent } from '../choose-tab-content/choose-tab-content.component';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { CourseService } from '../services/course.service';
import { ChangeDetectorRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { CourseCreateComponent } from '../course-create/course-create.component';

@Component({
  selector: 'app-tab-content',
  templateUrl: './tab-content.component.html',
  styleUrls: ['./tab-content.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => TabContentComponent),
    multi: true
  }
  ],
  encapsulation: ViewEncapsulation.None
})
export class TabContentComponent implements OnInit {
  @Input() courseId: string;
  @Input() status: number;
  constructor(
    public constant: Constants,
    private modalService: NgbModal,
    private messageService: MessageService,
    public dragDropModule: DragDropModule,
    public config: Configuration,
    public courseService: CourseService,
    private _cd: ChangeDetectorRef
  ) { }
  listData = [];

  @Input()
  get items() { return this._items };
  set items(value: any[]) {
    this.listData = value;
  };

  _items = [];
  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any | any[]): void {
    if (value != null) {
      this.modelLesson = value;
    } else {
      this.modelLesson = null;
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

  modelLesson = {
    Listid: [],
    isDelete: false,
  }

  model: any = {
    LessonId: '',
    CourseId: '',
    DisplayIndex: ''
  }

  ngOnInit(): void {
    if (this.courseId) {
      this.searchLessonByCourseId();
    }
  }

  searchLessonByCourseId() {
    this.courseService.searchLessonByCourseId(this.courseId).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listData = data.data.dataResults;
        }
        else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  drop(event: CdkDragDrop<string[]>) {
    this.listData = [];
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }

    this.listData = event.container.data;
    var i = 1;
    this.listData.forEach(element => {
      element.displayIndex = i++;
    })
    this.modelLesson.Listid = this.listData;


    this._onChange(this.modelLesson ? this.modelLesson : null);
  }

  selectLesson() {
    let activeModal = this.modalService.open(ChooseTabContentComponent, { container: 'body', windowClass: 'choose-tab-content', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];

    this.listData.forEach(element => {
      ListIdSelect.push(element.lessonId);
    });

    activeModal.componentInstance.listIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
        });

        this.modelLesson.Listid = this.listData;
        this._onChange(this.modelLesson ? this.modelLesson : null);
      }
    }, (reason) => {

    });
  }

  showConfirmDelete(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bài giảng này này không?").then(
      data => {
        this.removeLesson(row);
      }
    );
  }

  removeLesson(row) {
    var index = this.listData.indexOf(row);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
    this.modelLesson.Listid = this.listData;
    this.modelLesson.isDelete = true;
    this._onChange(this.modelLesson ? this.modelLesson : null);
  }

}
