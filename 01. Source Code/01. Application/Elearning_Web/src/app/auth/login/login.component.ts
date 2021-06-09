import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

import { AuthenticationService } from '../services';
import { Constants } from 'src/app/shared';
import { DeviceDetectorService } from 'ngx-device-detector';
import * as $ from 'jquery';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {


  message: string;
  returnUrl: string
  fieldTextType: boolean;

  captcha = '';

  isCaptcha = false;

  model: any = {
  }

  deviceInfo = null;

  constructor(private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private titleservice: Title,
    private constant: Constants,
    // private http: Http, 
    // private deviceService: DeviceDetectorService
  ) { }

  // epicFunction() {
  //   console.log('hello `Home` component');
  //   this.deviceInfo = this.deviceService.getDeviceInfo();
  //   const isMobile = this.deviceService.isMobile();
  //   const isTablet = this.deviceService.isTablet();
  //   const isDesktopDevice = this.deviceService.isDesktop();
  //   console.log(this.deviceInfo);
  //   console.log(isMobile);  // returns if the device is a mobile device (android / iPhone / windows-phone etc)
  //   console.log(isTablet);  // returns if the device us a tablet (iPad etc)
  //   console.log(isDesktopDevice); // returns if the app is running on a Desktop browser.
  // }

  ngOnInit() {
    this.isCaptcha = false;
    $(".toggle-password").click(function () {

      $(this).toggleClass("fa-eye fa-eye-slash");
      var input = $($(this).attr("toggle"));
      if (input.attr("type") == "password") {
        input.attr("type", "text");
        return;
      } else {
        input.attr("type", "password");
        return;
      }
    });
    // reset login status
    this.authenticationService.logout();

    this.titleservice.setTitle("PHẦN MỀM QUẢN LÝ ĐÀO TẠO TRỰC TUYẾN");
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    this.createCaptcha();
  }

  code: string;
  createCaptcha() {
    //clear the contents of captcha div first 
    document.getElementById('captcha').innerHTML = "";
    var charsArray =
      "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    var lengthOtp = 6;
    var captcha = [];
    for (var i = 0; i < lengthOtp; i++) {
      //below code will not allow Repetition of Characters
      var index = Math.floor(Math.random() * charsArray.length + 1); //get the next character from the array
      if (captcha.indexOf(charsArray[index]) == -1)
        captcha.push(charsArray[index]);
      else i--;
    }
    var canv = document.createElement("canvas");
    canv.id = "captcha";
    canv.width = 100;
    canv.height = 50;
    var ctx = canv.getContext("2d");
    ctx.font = "25px Georgia";
    ctx.strokeText(captcha.join(""), 0, 30);
    //storing captcha so that can validate you can save it somewhere else according to your specific requirements
    this.code = captcha.join("");
    document.getElementById("captcha").appendChild(canv); // adds the canvas to the body element
  }

  login() {
    if(this.model.captcha == ''||this.model.captcha == null){
      this.message = "Bạn chưa nhập capcha!";
    } else if (this.model.captcha == this.code) {
      this.authenticationService.login(this.model).subscribe(
        result => {
          if (result.statusCode == this.constant.StatusCode.Success) {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            result.data.LoginDate = new Date();
            if(result.data.permissions.indexOf('F000000')< 0) {
              this.router.navigate(['/dao-tao/khoa-hoc']);
            }
            else{
              this.router.navigate(['/dash-board/dash-board']);
            }
            localStorage.setItem('ElearningCurrentUser', JSON.stringify(result.data));
            
          }
          else {
            this.message = result.message;
          }
        },
        error => {
          if (error.status == 0 || error.status == 404) {
            this.message = "Không kết nối server";
          } else {
            this.message = error.error.error_description;
          }
        }
      );
    }
    else{
      this.message = "Mã xác nhận hình ảnh không đúng!";
      this.model.captcha = '';
      this.createCaptcha();
    }

  }

}
