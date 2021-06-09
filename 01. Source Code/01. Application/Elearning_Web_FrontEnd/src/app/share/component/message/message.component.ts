import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  message: string;
  messages: string[];
  isList:boolean = false;

  constructor(private activeModal: NgbActiveModal) { }

  ngOnInit() {
    this.isList = this.messages && this.messages.length>0 ?true: false;
  }

  closeModal() {
    this.activeModal.close();
  }
}
