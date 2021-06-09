package nts.swipesafe.model;

public class MobileCommentSearchModel {
    /// <summary>
    /// id khóa học
    /// </summary>
    public String courseId;
    /// <summary>
    /// id bài giảng
    /// </summary>
    public String lessonId;

    /// <summary>
    /// id đăng nhập
    /// </summary>
    public String userId;

    /// <summary>
    /// Loại
    /// 1: Khóa học
    /// 2: Bài giảng
    /// </summary>
    public int type;
}
