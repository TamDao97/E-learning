using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common
{
    public class Constants
    {
        /// <summary>
        /// Loại tạo OTP: Số điện thoại
        /// </summary>
        public const int Type_CreateOTP_Phone = 1;

        /// <summary>
        /// Loại tạo OTP: Gmail
        /// </summary>
        public const int Type_CreateOTP_Email = 2;

        /// <summary>
        /// IPN 
        /// </summary>
        public const string SUCCESS = "SUCCESS";

        /// <summary>
        /// Trạng thái chưa kiểm duyệt đăng ký xe
        /// </summary>
        public const int Approve_Status_UnCheck = 0;

        /// <summary>
        /// Trạng thái đã duyệt đăng ký xe
        /// </summary>
        public const int Approve_Status_Check = 1;

        /// <summary>
        /// Trạng thái không duyệt đăng ký xe
        /// </summary>
        public const int Approve_Status_NoCheck = 2;

        /// <summary>
        /// Content Notify lưu vào queue Order
        /// </summary>
        public const string Order_Notify_Content = "Bạn đã thanh toán thành công";

        public const string UserPayment_Supplier = "NAPAS";

        #region Users
        /// <summary>
        /// Loại ngườ dùng: Admin
        /// </summary>
        public const int User_UserType_Admin = 1;
        /// <summary>
        /// Loại ngườ dùng: Chuyên gia
        /// </summary>
        public const int User_UserType_Expert = 2;
        /// <summary>
        /// Loại ngườ dùng: Hướng dẫn
        /// </summary>
        public const int User_UserType_Instructor = 3;
        /// <summary>
        /// Loại ngườ dùng: Người học
        /// </summary>
        public const int User_UserType_Student = 4;

        /// <summary>
        /// Loại ngườ dùng: khách
        /// </summary>
        public const int User_UserType_Customer = 2;

        /// <summary>
        /// Trạng thái mở khóa
        /// </summary>
        public const bool User_UnDisable = false;

        /// <summary>
        /// Trạng thái khóa
        /// </summary>
        public const bool User_Disable = true;
        #endregion

        #region Vehicle
        /// <summary>
        /// Xác nhận yêu cầu chia sẻ
        /// </summary>
        public const bool VehicleShare_Status_Confirm = true;

        /// <summary>
        /// Chưa xác nhận yêu cầu chia sẻ
        /// </summary>
        public const bool VehicleShare_Status_UnConfirm = false;

        /// <summary>
        /// Trạng thái đăng ký xe chưa được chia sẻ
        /// </summary>
        public const bool Vehicle_IsShareNotify_NotShare = false;

        /// <summary>
        /// Trạng thái đăng ký xe đã được chia sẻ
        /// </summary>
        public const bool Vehicle_IsShareNotify_Shared = false;
        #endregion


        /// <summary>
        /// Loại đối tượng: Vi phạm
        /// </summary>
        public const int Notify_ObjectType_Violation = 1;

        /// <summary>
        ///  Loại đối tượng: Thanh toán
        /// </summary>
        public const int Notify_ObjectType_Order = 2;

        /// <summary>
        ///  Loại đối tượng: Chia sẻ đăng ký xe
        /// </summary>
        public const int Notify_ObjectType_VehicleShare = 3;

        #region Reports

        /// <summary>
        /// Ngày hôm nay
        /// </summary>
        public const string TimeType_Today = "1";

        /// <summary>
        /// Ngày qua
        /// </summary>
        public const string TimeType_Yesterday = "2";

        /// <summary>
        /// Tuần này
        /// </summary>
        public const string TimeType_ThisWeek = "3";

        /// <summary>
        /// Tuần trước
        /// </summary>
        public const string TimeType_LastWeek = "4";

        /// <summary>
        /// 7 ngày gần đây
        /// </summary>
        public const string TimeType_SevenDay = "5";

        /// <summary>
        /// Tháng này
        /// </summary>
        public const string TimeType_ThisMonth = "6";

        /// <summary>
        /// Tháng trước
        /// </summary>
        public const string TimeType_LastMonth = "7";

        /// <summary>
        /// Tháng
        /// </summary>
        public const string TimeType_Month = "8";

        /// <summary>
        /// Quý
        /// </summary>
        public const string TimeType_Quarter = "9";

        /// <summary>
        /// Năm nay
        /// </summary>
        public const string TimeType_ThisYear = "10";

        /// <summary>
        /// Năm trước
        /// </summary>
        public const string TimeType_LastYear = "11";

        /// <summary>
        /// Năm 
        /// </summary>
        public const string TimeType_Year = "12";

        /// <summary>
        /// Khoảng thời gian
        /// </summary>
        public const string TimeType_Between = "13";

        /// <summary>
        /// Quý này
        /// </summary>
        public const string TimeType_ThisQuarter = "14";

        /// <summary>
        /// Quý trước
        /// </summary>
        public const string TimeType_LastQuarter = "15";

        #endregion

        #region UserInfor

        /// <summary>
        /// Giới tính Nam
        /// </summary>
        public const string Man = "1";

        /// <summary>
        /// Giới tính nữ
        /// </summary>
        public const string Woman = "2";

        /// <summary>
        /// Giới tính khác
        /// </summary>
        public const string Other = "3";
        #endregion

        #region
        /// <summary>
        /// Loại queue của tuyến: Chấp nhận vi phạm
        /// </summary>
        public const int AvenueQueue_Type_Accept = 1;
        #endregion

        #region convert giới tính
        public static string GetGender(bool value)
        {
            if (value == false)
            {
                return "Nam";
            }
            else if (value == true)
            {
                return "Nữ";
            }
            else
                return string.Empty;
        }

        #endregion

        #region convert tình trạng khóa tài khoản
        public static string GetIsDisable(bool value)
        {
            if (value == true)
            {
                return "Đang hoạt động";
            }
            else if (value == false)
            {
                return "Khóa tài khoản";
            }
            else
                return string.Empty;
        }
        #endregion

        #region Lesson_Type
        /// <summary>
        /// Loại bài giảng: lý thuyết
        /// </summary>
        public const int Lesson_Type_Theory = 1;

        /// <summary>
        /// Loại bài giảng: trắc nghiệm
        /// </summary>
        public const int Lesson_Type_Study = 2;
        /// <summary>
        /// Bài thi
        /// </summary>
        public const int Lesson_Type_Exam = 3;
        #endregion

        #region Question
        /// <summary>
        /// Loại câu hỏi: Một lựa chọn
        /// </summary>
        public const int Question_Type_OneOption = 1;

        /// <summary>
        /// Loại câu hỏi: Nhiều lựa chọn
        /// </summary>
        public const int Question_Type_MultiOption = 2;

        /// <summary>
        /// Loại câu hỏi: Đúng sai
        /// </summary>
        public const int Question_Type_Boolean = 3;

        /// <summary>
        /// Loại câu hỏi: Điền từ vào chỗ trống
        /// </summary>
        public const int Question_Type_FillWords = 4;

        /// <summary>
        /// Loại câu hỏi: Sắp xếp thành câu đúng
        /// </summary>
        public const int Question_Type_OrderWords = 5;

        /// <summary>
        /// Trạng thái câu hói: Điền từ vào chỗ trống
        /// </summary>
        public const bool Question_Status_Active = true;

        /// <summary>
        /// Trạng thái câu hói: Điền từ vào chỗ trống
        /// </summary>
        public const bool Question_Status_NoActive = false;
        #endregion

        #region   
        /// <summary>
        /// Khóa học được phép hiển thị
        /// </summary>
        public const bool Course_Status_Show = true;

        /// <summary>
        /// Khóa học không được hiển thị
        /// </summary>
        public const bool Course_Status_Hide = false;
        #endregion

        #region Comment_Status
        /// <summary>
        /// Trạng thái: chưa phê duyệt
        /// </summary>
        public const int Comment_Status_Pending = 0;
        /// <summary>
        /// Trạng thái: hiển thị
        /// </summary>
        public const int Comment_Status_Approved = 1;
        /// <summary>
        /// Trạng thái: ẩn
        /// </summary>
        public const int Comment_Status_Delete = 2;
        #endregion

        public const bool Program_Status_Show = true;

        public const bool Program_Status_Hidden = false;

        /// <summary>
        /// Loại export: excel
        /// </summary>
        public const int Type_Export_Excel = 1;

        /// <summary>
        /// Loại export: pdf
        /// </summary>
        public const int Type_Export_Pdf = 2;

        #region
        /// <summary>
        /// Thống kê người học theo tỉnh
        /// </summary>
        public const string Type_Export_Provin = "Thống kê người học theo tỉnh";

        /// <summary>
        /// Thống kê người học theo giới tính
        /// </summary>
        public const string Type_Export_Gender = "Thống kê người học theo giới tính";

        /// <summary>
        /// Thống kê người hocj theo độ tuổi
        /// </summary>
        public const string Type_Export_YearOld = "Thống kê người học theo độ tuổi";

        /// <summary>
        /// Thống kê người học theo tỉnh
        /// </summary>
        public const int Type_Export_Provin_Int = 1;

        /// <summary>
        /// Thống kê người học theo giới tính
        /// </summary>
        public const int Type_Export_Gender_Int = 2;

        /// <summary>
        /// Thống kê người hocj theo độ tuổi
        /// </summary>
        public const int Type_Export_YearOld_Int = 3;

        /// <summary>
        /// Thống kê kết quả hoàn thành khóa học
        /// </summary>
        public const string Type_Export_Complete_Course = "Thống kê kết quả hoàn thành khóa học";

        /// <summary>
        /// Thống kê số lượt đăng ký khóa học
        /// </summary>
        public const string Type_Export_Subcribe = "Thống kê số lượt đăng ký khóa học";
        #endregion

        #region
        public const string Learner_Provider_Google = "GOOGLE";
        public const string Learner_Provider_Facebook = "FACEBOOK";
        public const string Learner_Provider_Email = "Email";
        #endregion

        #region Comment_Type
        /// <summary>
        /// Loại bình luận: Khóa học
        /// </summary>
        public const int Comment_Type_Course = 1;

        /// <summary>
        /// Loại bình luận: Bài giảng
        /// </summary>
        public const int Comment_Type_Lesson = 2;

        #endregion

        #region systemparam
        /// <summary>
        /// Không check email
        /// </summary>
        public const string EmailNoCheck = "1";

        /// <summary>
        /// Check email
        /// </summary>
        public const string EmailCheck = "2";

        #endregion

        #region Course_ApprovalStatus
        /// <summary>
        /// Tình trạng duyệt: Đang tạo
        /// </summary>
        public const int Course_Approval_Creating = 0;
        /// <summary>
        /// Tình trạng duyệt: Yêu cầu duyệt
        /// </summary>
        public const int Course_Approval_Request = 1;
        /// <summary>
        /// Tình trạng duyệt: Đã duyệt
        /// </summary>
        public const int Course_Approval_Approved = 2;
        /// <summary>
        /// Tình trạng duyệt: Hủy duyệt
        /// </summary>
        public const int Course_Approval_NotApproved = 3;
        /// <summary>
        /// Tình trạng duyệt: Không duyệt duyệt
        /// </summary>
        public const int Course_Approval_NotBrowse = 4;
        #endregion

        #region SystemParam
        /// <summary>
        /// Địa chỉ email gửi thông tin đăng nhập
        /// </summary>
        public const string SystemParam_SP01 = "SP01";

        /// <summary>
        /// Mật khẩu email gửi thông tin đăng nhập
        /// </summary>
        public const string SystemParam_SP02 = "SP02";

        /// <summary>
        /// Nội dung gửi email thông tin đăng nhập
        /// </summary>
        public const string SystemParam_SP03 = "SP03";

        /// <summary>
        /// Nội dung tài khoản bị khóa
        /// </summary>
        public const string SystemParam_SP04 = "SP04";

        /// <summary>
        /// Email liên hệ
        /// </summary>
        public const string SystemParam_SP05 = "SP05";

        /// <summary>
        /// Nội dung gửi email thông tin đăng nhập fontend
        /// </summary>
        public const string SystemParam_SP06 = "SP06";
        #endregion

        public const string File_Upload_Zip = ".zip";
    }
}
