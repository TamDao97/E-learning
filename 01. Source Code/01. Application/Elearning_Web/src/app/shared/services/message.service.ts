import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

import { MessageComponent } from '../component/message/message.component';
import { MessageconfirmComponent } from '../component/messageconfirm/messageconfirm.component';
import { NTSModalService } from '../services/ntsmodal.service';
import { MessageconfirmcodeComponent } from '../component/messageconfirmcode/messageconfirmcode.component';

@Injectable({
    providedIn: 'root',
})
export class MessageService {

    constructor(private modalService: NgbModal, private ntsModalService: NTSModalService,
        public router: Router,
        private toastr: ToastrService) {

    }

    /*
    * type:
    * 0: Bình thường
    * 1: Lỗi
    */
    showMessage(message: string) {
        const activeModal = this.modalService.open(MessageComponent, { container: 'body' })
        activeModal.componentInstance.message = message;
        activeModal.result.then((result) => {
            this.ntsModalService.closeMultiModal();
        }, (reason) => {
            this.ntsModalService.closeMultiModal();
        });
    }

    showListMessage(message: string[]) {
        const activeModal = this.modalService.open(MessageComponent, { container: 'body' })
        activeModal.componentInstance.messages = message;
        activeModal.result.then((result) => {
            this.ntsModalService.closeMultiModal();
        }, (reason) => {
            this.ntsModalService.closeMultiModal();
        });
    }

    showError(error: any) {

        if (error.status == 0) {
            this.showMessage('Không thể kết nối được đến server, vui lòng kiểm tra lại');
        } else
            if (error.status == 401) {
                localStorage.removeItem('ElearningCurrentUser');
                let message = 'Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.';
                this.router.navigate(['/auth/dang-nhap']);
                this.showMessage(message);
            } else {
                let message: string;
                if (error.error.error_description) {
                    message = error.error.error_description;
                }
                else {
                    message = error.error;
                }
                this.showMessage(message);
            }

        // let message: string;
        // if (error.status == 500 || error.status == 405) {
        //     if (error._body) {
        //         message = error._body;
        //     } else
        //         if (error.error.error_description) {
        //             message = error.error.error_description;
        //         }
        //         else {
        //             message = error.error;
        //         }
        // } else if (error.status == 401) {
        //     localStorage.removeItem('qltkcurrentUser');
        //     //message = 'Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.';
        //     this.router.navigate(['/login']);
        // }
        // else {
        //     message = 'Không kết nối được đến server';
        // }

        // this.showMessage(message);
    }

    showMessageErrorBlob(err) {
        var arrayBuffer;
        var fileReader = new FileReader();
        fileReader.onload = event => {
            arrayBuffer = event.target;
            let str = new TextDecoder().decode(arrayBuffer.result);
            this.showMessage(str);
        };
        fileReader.readAsArrayBuffer(err.error);
    }

    showConfirm(message: string): Promise<any> {
        return new Promise((resolve, reject) => {
            const activeModalConfirm = this.modalService.open(MessageconfirmComponent, { container: 'body' })
            activeModalConfirm.componentInstance.message = message;
            activeModalConfirm.result.then((result) => {
                this.ntsModalService.closeMultiModal();
                if (result) {
                    resolve(result);
                }
                // else{
                //     reject(false);
                // }
            }, (reason) => {
                reject('');
                this.ntsModalService.closeMultiModal();
            });
        });
    }

    showSuccess(message) {
        this.toastr.success(message);
    }

    showInfo(message) {
        this.toastr.info(message);
    }

    showWarning(message) {
        this.toastr.warning(message);
    }

    showConfirmCode(message: string): Promise<any> {
        return new Promise((resolve, reject) => {
            const activeModalConfirm = this.modalService.open(MessageconfirmcodeComponent, { container: 'body' })
            activeModalConfirm.componentInstance.message = message;
            activeModalConfirm.result.then((result) => {
                this.ntsModalService.closeMultiModal();
                if (result) {
                    resolve(result);
                }
                else {
                    reject(false);
                }
            }, (reason) => {
                reject('');
                this.ntsModalService.closeMultiModal();
            });
        });
    }
}
