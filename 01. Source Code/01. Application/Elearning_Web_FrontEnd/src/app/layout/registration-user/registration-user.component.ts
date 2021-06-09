import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ReCaptcha2Component } from 'ngx-captcha';

@Component({
  selector: 'app-registration-user',
  templateUrl: './registration-user.component.html',
  styleUrls: ['./registration-user.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class RegistrationUserComponent implements OnInit {

  public aFormGroup: FormGroup;
  @ViewChild('captchaElem') captchaElem: ReCaptcha2Component;
  @ViewChild('langInput') langInput: ElementRef;
  constructor(private formBuilder: FormBuilder) { }

  public captchaIsLoaded = false;
  public captchaSuccess = false;
  public captchaIsExpired = false;
  public captchaResponse?: string;

  public theme: 'light' | 'dark' = 'light';
  public size: 'compact' | 'normal' = 'normal';
  public lang = 'en';
  public type: 'image' | 'audio';

  ngOnInit() {
    this.gencaptcha();
  }

  captcha: string;
  captchamodel: any;
  message: string;

  model: any = {
  }

  gencaptcha(){
     this.captcha = "";
    while(this.captcha.length<6&&6>0){
        var r = Math.random();
        this.captcha+= (r<0.1?Math.floor(r*100):String.fromCharCode(Math.floor(r*26) + (r>0.5?97:65)));
    }
    return this.captcha;
  }

  checkcaptcha(){
    if(this.captchamodel != this.captcha){
      
    }
  }

  registration(){

  }

  handleSuccess(data) {
    console.log(data);
  }

}
