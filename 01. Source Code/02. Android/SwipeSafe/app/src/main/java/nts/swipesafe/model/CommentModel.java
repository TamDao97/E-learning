package nts.swipesafe.model;

public class CommentModel {
    /// <summary>
    ///  Id khóa học
    /// </summary>
    public String courseId;

    /// <summary>
    /// Id bài giảng
    /// </summary>
    public String lessonId;

    /// <summary>
    /// Id người dùng đang đăng nhập.
    /// </summary>
    public String learnerId ;

    /// <summary>
    /// Id hỏi đáp con
    /// </summary>
    public Long parentCommentId;

    /// <summary>
    /// Loại commet
    /// 1: Khóa học
    /// 2: Bài giảng
    /// </summary>
    public int type ;

    /// <summary>
    /// Nội dung phản hồi
    /// </summary>
    public String content ;
}
