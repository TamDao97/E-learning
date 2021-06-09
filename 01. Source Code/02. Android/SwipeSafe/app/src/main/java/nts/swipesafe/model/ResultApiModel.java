package nts.swipesafe.model;

import java.util.List;

public class ResultApiModel<T> {
    public String statusCode;
    public List<String> message;
    public T data;
}
