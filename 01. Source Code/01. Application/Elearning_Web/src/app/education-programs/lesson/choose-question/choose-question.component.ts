import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants } from 'src/app/shared';
import { LessonService } from '../../services/lesson.service';
import { ShowQuestionComponent } from '../show-question/show-question.component';

@Component({
  selector: 'app-choose-question',
  templateUrl: './choose-question.component.html',
  styleUrls: ['./choose-question.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseQuestionComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private lessonService: LessonService,
    private modalService: NgbModal,
    public constant: Constants,
  ) { }

  modalInfo = {
    Title: 'Chọn câu hỏi',
  };
  listQuestId: [];
  listData: any[] = [];
  listSelect: any[] = [];
  selectAll = false;

  model: any = {
    TopicId: null,
    Name: '',
    Type: null,
    ListId: []
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Chủ đề',
    Items: [
      {
        Name: 'Chủ đề',
        FieldName: 'TopicId',
        Placeholder: 'Chọn',
        Type: 'treeSelect',
        DataType: this.constant.SearchDataType.Topic,
        DisplayName: 'title',
        ValueName: 'key'
      },
      {
        Name: 'Loại câu hỏi',
        FieldName: 'Type',
        Placeholder: 'Chọn',
        Type: 'select',
        Data: this.constant.QuestionType,
        DisplayName: 'Name',
        ValueName: 'Id'
      }
    ]
  };

  ngOnInit() {
    this.searchQuestion();
  }

  searchQuestion() {
    this.model.ListId = this.listQuestId;
    this.lessonService.searchQuestion(this.model).subscribe(
      (data: any) => {
        if (data.statusCode == this.constant.StatusCode.Success) {
          this.listData = data.data;
        } else {
          this.messageService.showListMessage(data.message);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  chooseQuestion() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.activeModal.close(this.listSelect);
  }

  closeModal() {
    this.activeModal.close(false);
  }

  clear() {
    this.model = {
      TopicId: null,
      Name: '',
      Type: null,
      ListId: []
    }
    this.searchQuestion();
  }

  selectAlls() {
    this.listData.forEach(element => {
      element.Checked = this.selectAll;
    });
  }

  showQuestion(id: string) {
    let activeModal = this.modalService.open(ShowQuestionComponent, { container: 'body', windowClass: 'show-question-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.type = 1;
    activeModal.result.then((result: any) => {
      if (result) {

      }
    });
  }
}
