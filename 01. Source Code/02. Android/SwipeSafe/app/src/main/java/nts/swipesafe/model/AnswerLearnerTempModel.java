package nts.swipesafe.model;

import java.util.List;

public class AnswerLearnerTempModel {
    /// <summary>
    /// Id khóa học
    /// </summary>
    public String courseId ;


    /// <summary>
    /// Id người học
    /// </summary>
    public String learnerId ;

    /// <summary>
    /// Id bài giảng
    /// </summary>
    public String lessonId;

    /// <summary>
    /// Id câu hỏi
    /// </summary>
    public String questionId ;

    public List<AnswerModel> listAnswer;
}
