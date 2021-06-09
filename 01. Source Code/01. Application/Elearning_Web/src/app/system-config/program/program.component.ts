import { Component, OnInit } from '@angular/core';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { SystemConfigService } from '../service/system-config.service';

@Component({
  selector: 'app-program',
  templateUrl: './program.component.html',
  styleUrls: ['./program.component.scss']
})
export class ProgramComponent implements OnInit {

  totalProgram = 0;
  totalCourse = 0;
  listEducationProgram: any = [];
  listCourse: any = [];
  isSelectAll = false;
  isDisable = true;
  isIndeterminate = false;
  programSelectedIndex = -1;

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    public config: Configuration,
    public messageService: MessageService,
    public service: SystemConfigService
  ) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Cấu hình khóa học hiển thị trên Trang chủ";
    this.getListEducationProgram();
  }

  getListEducationProgram() {
    this.service.getListEducationProgram().subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.listEducationProgram = data.data;
        this.programSelectedIndex = 0;
        this.totalProgram = this.listEducationProgram.length;

        for (let i = 0; i < this.listEducationProgram.length; i++) {
          let checkCount = this.listEducationProgram[i].checkCount;
          let length = this.listEducationProgram[i].listCourse.length;
          this.listCourse = this.listEducationProgram[i].listCourse;
          if (checkCount == 0) {
            this.listEducationProgram[i].isIndeterminate = false;
          }
          else {
            if (checkCount < length) {
              this.listEducationProgram[i].isIndeterminate = true;
            }
            else {
              this.listEducationProgram[i].isIndeterminate = false;
              this.isSelectAll = this.listEducationProgram[i].checkCount == this.listEducationProgram[i].listCourse.length;
              this.listEducationProgram[i].isChecked = this.isSelectAll;
            }
          }
        }
        this.resetIsDisable();
      }
      else {
        this.messageService.showListMessage(data.message);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  selectGroupFunction(program, index) {
    this.programSelectedIndex = index;
    this.isSelectAll = this.listEducationProgram[index].listCourse.length != 0 && this.listEducationProgram[index].checkCount == this.listEducationProgram[index].listCourse.length ? true : false;
  }

  changeGroupFunctionCheck(program, index) {
    program.listCourse.forEach(course => {
      if (course.isChecked && !program.isChecked) {
        program.checkCount--;
      }
      if (!course.isChecked && program.isChecked) {
        program.checkCount++;
      }
      course.isChecked = program.isChecked;
    });

    if (index == this.programSelectedIndex) {
      this.isSelectAll = program.isChecked;
    }
    this.listEducationProgram[index].isIndeterminate = false;
    this.resetIsDisable();
  }

  selectAllCourse() {
    this.listEducationProgram[this.programSelectedIndex].listCourse.forEach(course => {
      if (course.isChecked && !this.isSelectAll) {
        this.listEducationProgram[this.programSelectedIndex].checkCount--;
      }
      if (!course.isChecked && this.isSelectAll) {
        this.listEducationProgram[this.programSelectedIndex].checkCount++;
      }
      course.isChecked = this.isSelectAll;
    });
    this.listEducationProgram[this.programSelectedIndex].isChecked = this.isSelectAll;
    this.listEducationProgram[this.programSelectedIndex].isIndeterminate = false;
    this.resetIsDisable();
  }

  selectCourse(course) {
    if (!course.isChecked) {
      this.listEducationProgram[this.programSelectedIndex].checkCount--;
    }
    else {
      this.listEducationProgram[this.programSelectedIndex].checkCount++;
    }

    let checkCount = this.listEducationProgram[this.programSelectedIndex].checkCount;
    let length = this.listEducationProgram[this.programSelectedIndex].listCourse.length;
    if (checkCount == 0) {
      this.listEducationProgram[this.programSelectedIndex].isChecked = false;
      this.listEducationProgram[this.programSelectedIndex].isIndeterminate = false;
      this.isSelectAll = false;
    }
    else {
      if (checkCount < length) {
        this.listEducationProgram[this.programSelectedIndex].isIndeterminate = true;
        this.isSelectAll = false;
      }
      else {
        this.listEducationProgram[this.programSelectedIndex].isIndeterminate = false;
        this.isSelectAll = this.listEducationProgram[this.programSelectedIndex].checkCount == this.listEducationProgram[this.programSelectedIndex].listCourse.length;
        this.listEducationProgram[this.programSelectedIndex].isChecked = this.isSelectAll;
      }
    }

    this.resetIsDisable();
  }

  save() {
    var listCourseChecked = [];
    this.listEducationProgram.forEach(item => {
      item.listCourse.forEach(element => {
        if (element.isChecked)
          listCourseChecked.push(element);
      });
    });

    for (let i = 0; i < listCourseChecked.length - 1; i++) {
      for (let j = i + 1; j < listCourseChecked.length; j++) {
        if (listCourseChecked[i].displayIndex === listCourseChecked[j].displayIndex) {
          this.messageService.showMessage("Vị trí hiển thị của các khóa học phải theo thứ tự và không được trùng nhau.");
          return;
        }
      }
    }

    this.service.save(listCourseChecked).subscribe(data => {
      if (data.statusCode == this.constant.StatusCode.Success) {
        this.messageService.showSuccess('Lưu thành công!');
        this.getListEducationProgram();
      }
      else {
        this.messageService.showListMessage(data.Message);
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  refresh() {
    this.getListEducationProgram();
  }

  resetIsDisable() {
    let totalCheck = 0;
    this.isDisable = true;
    this.listEducationProgram.forEach(item => {
      totalCheck += item.checkCount;
    });
    if (totalCheck > 0)
      this.isDisable = false;
  }
}
