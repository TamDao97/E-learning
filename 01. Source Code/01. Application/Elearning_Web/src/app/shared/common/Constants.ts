import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';

import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Injectable({
    providedIn: 'root'
})
export class Constants {
    QLNV: number = 1;
    KTNVS: number = 0;

    ScrollConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: false,
        suppressScrollY: false,
        minScrollbarLength: 20,
        wheelPropagation: true
    };

    ScrollXConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: false,
        suppressScrollY: true,
        minScrollbarLength: 20,
        wheelPropagation: true
    };
    ScrollYConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: true,
        suppressScrollY: false,
        minScrollbarLength: 20,
        wheelPropagation: true
    };

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
        { Id: true, Name: 'Không hoạt động', BadgeClass: 'badge-danger' },
    ]

    provinceStudent = [
        { Id: 1, Name: 'Google' },
        { Id: 2, Name: 'Facebook' },
        { Id: 3, Name: 'Email' },
    ]

    approvalStatus = [
        { Id: 0, Name: 'Đang tạo', BadgeClass: 'badge-secondary' },
        { Id: 1, Name: 'Yêu cầu duyệt', BadgeClass: 'badge-info' },
        { Id: 2, Name: 'Đã duyệt', BadgeClass: 'badge-success' },
        { Id: 3, Name: 'Hủy duyệt', BadgeClass: 'badge-danger' },
        { Id: 4, Name: 'Không duyệt', BadgeClass: 'badge-warning' },
    ]

    approval_status = {
        Course_Approval_Creating: 0,
        Course_Approval_Request: 1,
        Course_Approval_Approved: 2,
        Course_Approval_NotApproved: 3,
        Course_Approval_NotBrowse: 4,
    }

    // Giới tính
    Gender = [
        { Id: false, Name: 'Nam', Checked: false },
        { Id: true, Name: 'Nữ', Checked: false },
    ];

    userType = [
        { Id: 1, Name: 'Tài khoản quản trị' },
        { Id: 2, Name: 'Tài khoản chuyên gia' },
        { Id: 3, Name: 'Tài khoản giảng viên' },
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

    SearchTimeType: any[] = [
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
    ];

    SearchTimeType1: any[] = [
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
        { Id: '13', Name: 'Từ ngày - đến ngày' },
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
        { Id: true, Name: 'Kích hoạt', Checked: false, BadgeClass: 'badge-success', },
        { Id: false, Name: 'Không kích hoạt', Checked: false, BadgeClass: 'badge-danger', },
    ];
    StatusCourse = [
        { Id: true, Name: 'Hiển thị', Checked: false, BadgeClass: 'badge-success', },
        { Id: false, Name: 'Không hiển thị', Checked: false, BadgeClass: 'badge-danger', },
    ];
    StatusHomeService = [
        { Id: true, Name: 'Hiển thị', Checked: false, BadgeClass: 'badge-success', },
        { Id: false, Name: 'Không hiển thị', Checked: false, BadgeClass: 'badge-danger', },
    ];

    LessonStatus = [
        { Id: false, Name: 'Không hiển thị', Checked: false, BadgeClass: 'badge-danger' },
        { Id: true, Name: 'Hiển thị', Checked: false, BadgeClass: 'badge-success' },
    ];

    LessonType: any[] = [
        { Id: 1, Name: 'Bài giảng lý thuyết' },
        { Id: 2, Name: 'Bài thi cuối khóa' },
        //{ Id: 3, Name: 'Bài thi' }
        { Id: 4, Name: 'Scorm Package' },
    ];

    LessonFrame: any[] = [
        { Id: 1, Name: 'Bài giảng lý thuyết' },
        { Id: 2, Name: 'Bài giảng câu hỏi trắc nghiệm' },
    ]

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
        { Id: true, Name: 'Đã áp dụng' },
        { Id: false, Name: 'Chưa áp dụng' }
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
        Province: 8,
        Districti: 9,
        Ward: 10,
        Nation: 11,
        User: 12,
        Learner: 13,
        ManageUnit: 14,
    };

    Lesson_Type = {
        Lesson_Type_Theory: 1,
        Lesson_Type_Study: 2,
        Lesson_Type_Exam: 3
    };

    Question_Type = {
        Question_Type_1Answer: 1,
        Question_Type_MoreAnswer: 2,
        Question_Type_YesNo: 3,
        Question_Type_Fill: 4,
        Question_Type_Sort: 5,
    }

    ListPageSize = [5, 10, 15, 20, 25, 30];

    validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    TimeType_Today = "1";

    /// <summary>
    /// Ngày qua
    /// </summary>
    TimeType_Yesterday = "2";

    /// <summary>
    /// Tuần này
    /// </summary>
    TimeType_ThisWeek = "3";

    /// <summary>
    /// Tuần trước
    /// </summary>
    TimeType_LastWeek = "4";

    /// <summary>
    /// 7 ngày gần đây
    /// </summary>
    TimeType_SevenDay = "5";

    /// <summary>
    /// Tháng này
    /// </summary>
    TimeType_ThisMonth = "6";

    /// <summary>
    /// Tháng trước
    /// </summary>
    TimeType_LastMonth = "7";

    /// <summary>
    /// Tháng
    /// </summary>
    TimeType_Month = "8";

    /// <summary>
    /// Quý
    /// </summary>
    TimeType_Quarter = "9";

    /// <summary>
    /// Năm nay
    /// </summary>
    TimeType_ThisYear = "10";

    /// <summary>
    /// Năm trước
    /// </summary>
    TimeType_LastYear = "11";

    /// <summary>
    /// Năm 
    /// </summary>
    TimeType_Year = "12";

    /// <summary>
    /// Khoảng thời gian
    /// </summary>
    TimeType_Between = "13";

    /// <summary>
    /// Quý này
    /// </summary>
    TimeType_ThisQuarter = "14";

    /// <summary>
    /// Quý trước
    /// </summary>
    TimeType_LastQuarter = "15";
}