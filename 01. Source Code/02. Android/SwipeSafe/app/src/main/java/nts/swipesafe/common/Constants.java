package nts.swipesafe.common;

/**
 * Created by NTS-VANVV on 26/12/2018.
 */

public class Constants {
//    ////Api test
//    public static String ApiUrlBac = "http://27.72.31.133:6868/";
//    public static String ApiUrlTrung = "http://27.72.31.133:6868/";
//    public static String ApiUrlNam = "http://27.72.31.133:6868/";


    ////API tổng đài
    public static String ApiUrlBac = "http://hanoi.tongdai111.vn:6868/";
    public static String ApiUrlTrung = "http://danang.tongdai111.vn:6868/";
    public static String ApiUrlNam = "http://angiang.tongdai111.vn:6868/";
    public static String ApiElearning = "https://nhantinsoft.vn:9507/";

    public static final String AutoUpdateAPI = "http://gsatgt.vn/swipesafe/update.xml";

    public static final String Status_Success = "1";
    public static final String Status_Error = "2";

    public static final String KeyInfoLogin = "InfoLogin";
    public static final String KeyInfoLoginId = "KeyInfoLoginId";
    public static final String KeyInfoLoginName = "KeyInfoLoginName";
    public static final String KeyInfoLoginPhoneNumber = "KeyInfoLoginPhoneNumber";
    public static final String KeyInfoLoginDateOfBirthday = "KeyInfoLoginDateOfBirthday";
    public static final String KeyInfoLoginProvinceId = "KeyInfoLoginProvinceId";
    public static final String KeyInfoLoginDistrictId = "KeyInfoLoginDistrictId";
    public static final String KeyInfoLoginProvinceName = "KeyInfoLoginProvinceName";
    public static final String KeyInfoLoginDistrictName = "KeyInfoLoginDistrictName";
    public static final String KeyInfoLoginWardId = "KeyInfoLoginWardId";
    public static final String KeyInfoLoginWardName = "KeyInfoLoginWardName";
    public static final String KeyInfoLoginNationId = "KeyInfoLoginNationId";
    public static final String KeyInfoLoginNationName = "KeyInfoLoginNationName";
    public static final String KeyInfoLoginPicture = "KeyInfoLoginPicture";
    public static final String KeyInfoLoginAvatar = "KeyInfoLoginAvatar";
    public static final String KeyInfoLoginGender = "KeyInfoLoginGender";
    public static final String KeyInfoLoginIsDisable = "KeyInfoLoginIsDisable";
    public static final String KeyInfoLoginEmail = "KeyInfoLoginEmail";
    public static final String KeyInfoLoginIdToken = "KeyInfoLoginIdToken";
    public static final String KeyInfoLoginAccess_token = "KeyInfoLoginAccess_token";
    public static final String KeyInfoLoginProvider = "KeyInfoLoginProvider";
    public static final String KeyInfoLoginAddress = "KeyInfoLoginAddress";
    public static String IsLogin = "IsLogin";
    public static final String StatusCode = "statusCode";

    public static String KeyTypeLogin = "typeLogin";//Loại đăng nhập
    public static String TypeLoginGoogle = "google";//Loại đăng nhập gg
    public static String TypeLoginFacebook = "facebook";//Loại đăng nhập fb
    public static String TypeLoginByApp = "email";//Loại đăng nhập email
    /**
     * Mã chọn ảnh
     */
    public static final int REQUEST_CHOOSE_IMAGE = 200;

    /**
     * Yêu cầu chụp ảnh
     */
    public static final int REQUEST_IMAGE_CAPTURE = 100;


    //gioi tinh
    public static int Male = 1;
    public static int FeMale = 0;

    /**
     * Tiêu đề dialog nút DELETE
     */
    public static final String DIALOG_CONTROL_VALUE_DELETE = "XÓA";

    public static final String Swipe_Safe_Data_Fix = "childprofile_data_fix";
    public static final String Key_Data_Fix_Abuse = "Fix_Abuse";
    public static final String Key_Data_Fix_Province = "Fix_Province";
    public static final String Key_Data_Fix_District = "Fix_District";
    public static final String Key_Data_Fix_Ward = "Fix_Ward";
    public static final String Key_Data_Fix_Relationship = "Fix_Relationship";
    public static final String DataReportOffline = "ReportOffline";
    public static final String KeyReportOffline = "ListReportOffline";

    //Key
    public static final String KeyYoutube = "AIzaSyB9MO9X7_TfHklQ2TCI0NFEe0NuTp0Vl5w";
    //Id kênh video tổng đài 111
    public static final String KeyPlaylistId111 = "UUXFEQMmydLLSwM3_RSCxhIw";
    //Id an toàn mạng
    public static final String KeyPlaylistIdATM = "PLmUATB_TTWXx3Li7056KiUKUIsHLo2x4Q";

    /***
     * Câu chọn một đáp án
     */
    public static final int QuestionOneCorrect = 1;
    /***
     * Câu chọn nhiều đáp án
     */
    public static final int QuestionMutileCorrect = 2;
    /***
     * Câu đúng sai
     */
    public static final int QuestionYesNo = 3;
    /***
     * Câu điền từ vào chỗ trống
     */
    public static final int QuestionFillText = 4;
    /***
     * Câu sắp xếp theo thứ tự đúng
     */
    public static final int QuestionSort = 5;

    /// <summary>
    /// Loại bài giảng: lý thuyết
    /// </summary>
    public static int Lesson_Type_Theory = 1;

    /// <summary>
    /// Loại bài giảng: trắc nghiệm
    /// </summary>
    public static int Lesson_Type_Study = 2;
    /// <summary>
    /// Bài thi
    /// </summary>
    public static int Lesson_Type_Exam = 3;

    /// <summary>
    /// Bình luận khóa học
    /// </summary>
    public static int Comment_Course_Type = 1;
    /// <summary>
    /// Bình luận bài giảng
    /// </summary>
    public static int Comment_Lesson_Type = 2;
}
