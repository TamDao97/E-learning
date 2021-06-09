package nts.swipesafe.model;

public class AnswerModel {
    public String answerId;
    public String answerContent;
    public String answerLable;
    public int displayIndex;
    public Boolean isCorrect;

    /**
     * Đáp án người dùng
     */
    public String learnerAnswerContent;
    /**
     * Đáp án người dùng
     */
    public int learnerDisplayIndex;
    /**
     * Đáp án người dùng
     */
    public Boolean learnerIsCorrect;

    public int getDisplayIndex()
    {
        return displayIndex;
    }

    public int getLearnerDisplayIndex()
    {
        return learnerDisplayIndex;
    }
}
