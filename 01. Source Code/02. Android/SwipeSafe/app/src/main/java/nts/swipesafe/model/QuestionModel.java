package nts.swipesafe.model;

import java.util.List;

public class QuestionModel {
    public String questionId;
    public String content;
    public int type;
    public List<AnswerModel> listAnswer;
    public boolean isResultLearner;
}
