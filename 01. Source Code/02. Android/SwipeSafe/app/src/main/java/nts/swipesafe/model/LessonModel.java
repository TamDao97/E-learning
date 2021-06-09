package nts.swipesafe.model;

import java.util.List;

public class LessonModel {
    public String lessonId;
    public String name;
    public String content;
    public int typeLesson;
    //Thời gian thi
    public Integer examTime;
    public String startDate;
    public String finishDate;
    public String description;
    public List<QuestionModel> listQuestion;
    //Kết thúc bài thi
    public boolean isFinish;

    //THời gian còn làm bài còn lại của bài thi
    public double remainingTime;

    /// <summary>
    /// Tổng câu trả lời đúng
    /// </summary>
    public int totalCorrect;

    /// <summary>
    /// Tổng số câu hỏi
    /// </summary>
    public int totalQuestion;
}
