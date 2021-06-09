package nts.swipesafe.model;

import java.util.List;

public class MobileCommentResultModel {
    /// <summary>
    /// Id phản hồi
    /// </summary>
    public long id ;

    /// <summary>
    /// Id phản hồi con
    /// </summary>
    public Long parentCommentId;

    /// <summary>
    /// Nội dung phản hồi
    /// </summary>
    public String content;

    /// <summary>
    /// Id người phản hồi
    /// </summary>
    public String objectId ;

    /// <summary>
    /// Tên người phản hồi
    /// </summary>
    public String userName;

    /// <summary>
    /// Đường dẫn ảnh
    /// </summary>
    public String imagePath;

    /// <summary>
    /// 0: Chưa phê duyệt
    /// 1: Phê duyệt hiển thị ra bên ngoài
    /// 2: Xóa bình luận
    /// </summary>
    public int status ;

    /// <summary>
    /// Ngày tạo phản hồi
    /// </summary>
    public String commentDate ;

    public List<MobileCommentResultModel> listReply ;
}
