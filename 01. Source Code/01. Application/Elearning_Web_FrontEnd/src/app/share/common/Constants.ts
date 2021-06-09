import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
//import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Injectable({
    providedIn: 'root'
})
export class Constants {

    // ScrollXConfig: PerfectScrollbarConfigInterface = {
    //     suppressScrollX: false,
    //     suppressScrollY: true,
    //     minScrollbarLength: 20,
    //     wheelPropagation: true
    // };

    QLNV: number = 1;
    KTNVS: number = 0;

    HttpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    FileHttpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'multipart/form-data' })
    };

    StatusCode = {
        Success: 1,
        Error: 2,
        Validate: 3
    };

    statusUserAdmin = [
        { Id: false, Name: 'Đang hoạt động', BadgeClass: 'badge-success' },
        { Id: true, Name: 'Khóa tài khoản', BadgeClass: 'badge-danger' },
    ]

    // Giới tính
    Gender = [
        { Id: 1, Name: 'Nam', Checked: false },
        { Id: 2, Name: 'Nữ', Checked: false },
    ];

    userType = [
        { Id: 1, Name: 'Tài khoản quản trị' },
        { Id: 2, Name: 'Tài khoản chuyên gia' },
        { Id: 3, Name: 'Tài khoản hướng dẫn viên' },
    ]

    SearchTimeTypes: any[] = [
        { Id: '1', Name: 'Hôm nay' },
        { Id: '2', Name: 'Hôm qua' },
        { Id: '3', Name: 'Tuần này' },
        { Id: '4', Name: 'Tuần trước' },
        { Id: '5', Name: '7 ngày gần đây' },
        { Id: '6', Name: 'Tháng này' },
        { Id: '7', Name: 'Tháng trước' },
        { Id: '8', Name: 'Tháng' },
        { Id: '9', Name: 'Quý' },
        { Id: '10', Name: 'Năm nay' },
        { Id: '11', Name: 'Năm trước' },
        { Id: '12', Name: 'Năm' },
        { Id: '13', Name: 'Khoảng thời gian' }
    ];

    TimeTypesDashboad: any[] = [

        { Id: '6', Name: 'Tháng này' },
        { Id: '7', Name: 'Tháng trước' },
        { Id: '8', Name: 'Tháng' },
        { Id: '9', Name: 'Quý' },
        { Id: '10', Name: 'Năm nay' },
        { Id: '11', Name: 'Năm trước' },
        { Id: '12', Name: 'Năm' },
    ];

    SearchExpressionTypes: any[] = [
        { Id: 1, Name: '=' },
        { Id: 2, Name: '>' },
        { Id: 3, Name: '>=' },
        { Id: 4, Name: '<' },
        { Id: 5, Name: '<=' }
    ];
    Disable = [
        { Id: true, Name: 'Đang sử dụng', Checked: false, BadgeClass: 'badge-success', },
        { Id: false, Name: 'Không sử dụng', Checked: false, BadgeClass: 'badge-danger', },
    ];
    EmployeeStatus = [
        { Id: 1, Name: 'Đang làm việc', Checked: false, BadgeClass: 'badge-success', },
        { Id: 2, Name: 'Đã nghỉ việc', Checked: false, BadgeClass: 'badge-danger', },
    ];
    ProductStatus = [
        { Id: 0, Name: 'Chưa kiểm tra', Checked: false, BadgeClass: 'badge-warning' },
        { Id: 1, Name: 'Đạt', Checked: false, BadgeClass: 'badge-success' },
        { Id: 2, Name: 'Không đạt', Checked: false, BadgeClass: 'badge-danger' },
    ];
    TestRequestStatus = [
        { Id: 0, Name: 'Chờ xác nhận', Checked: false, BadgeClass: 'badge-danger' },
        { Id: 1, Name: 'Đang chờ mẫu', Checked: false, BadgeClass: 'badge-warning' },
        { Id: 2, Name: 'Đang chờ kiểm tra', Checked: false, BadgeClass: 'badge-warning' },
        { Id: 3, Name: 'Đang kiểm tra', Checked: false, BadgeClass: 'badge-primary' },
        { Id: 4, Name: 'Đã kiểm tra', Checked: false, BadgeClass: 'badge-success' },
    ];
    TestRequestProductResult = [
        { Id: 0, Name: 'Chưa kiểm tra', Checked: false, BadgeClass: 'badge-warning' },
        { Id: 1, Name: 'Đạt', Checked: false, BadgeClass: 'badge-success' },
        { Id: 2, Name: 'Không đạt', Checked: false, BadgeClass: 'badge-danger' },
    ];
    StatusProgram = [
        { Id: true, Name: 'Hiển thị', Checked: false, BadgeClass: 'badge-success', },
        { Id: false, Name: 'Không hiển thị', Checked: false, BadgeClass: 'badge-danger', },
    ];
    StatusCourse = [
        { Id: true, Name: 'Hiển thị', Checked: false, BadgeClass: 'badge-success', },
        { Id: false, Name: 'Không hiển thị', Checked: false, BadgeClass: 'badge-danger', },
    ];

    LessonStatus = [
        { Id: false, Name: 'Không hiển thị', Checked: false, BadgeClass: 'badge-danger' },
        { Id: true, Name: 'Hiển thị', Checked: false, BadgeClass: 'badge-success' },
    ];

    LessonType: any[] = [
        { Id: 1, Name: 'Bài giảng lý thuyết' },
        { Id: 2, Name: 'Bài giảng câu hỏi trắc nghiệm' }
    ];

    QuestionType: any[] = [
        { Id: 1, Name: 'Câu chọn một đáp án' },
        { Id: 2, Name: 'Câu chọn nhiều đáp án' },
        { Id: 3, Name: 'Câu đúng sai' },
        { Id: 4, Name: 'Câu điền từ vào chỗ trống' },
        { Id: 5, Name: 'Câu sắp xếp theo thứ tự đúng' }
    ];

    CommentStatus: any[] = [
        { Id: 0, Name: 'Chưa phê duyệt' },
        { Id: 1, Name: 'Phê duyệt' },
        { Id: 2, Name: 'Ẩn' },
    ]

    QuestionStatus: any[] = [
        { Id: 0, Name: 'Chưa áp dụng' },
        { Id: 1, Name: 'Đã áp dụng' }
    ]

    TopicType: any[] = [
        { Id: '2', Name: 'Phát triển toàn diện trẻ thơ toàn diện' },
        { Id: '3', Name: 'KIẾN THỨC CHĂM SÓC TRẺ SƠ SINH' },
        { Id: '4', Name: 'Kỹ năng làm cha mẹ' },
        { Id: '5', Name: 'Chung tay bảo vệ trẻ em, phòng, chống xâm hại trẻ em' }
    ];

    SearchDataType = {
        Sample: 1,
        Customer: 2,
        ProductType: 3,
        Product: 4,
        Category: 5,
        Program: 6,
        Topic: 7,
        EmployeeCourse: 8
    };

    Lesson_Type = {
        Lesson_Type_Theory: 1,
        Lesson_Type_Study: 2
    }

    ListPageSize = [5, 10, 15, 20, 25, 30];

    validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

}