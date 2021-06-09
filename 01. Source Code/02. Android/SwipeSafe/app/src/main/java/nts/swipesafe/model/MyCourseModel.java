package nts.swipesafe.model;

import java.util.List;

public class MyCourseModel {
    public String totalCourse;
    public String completed;
    public String testComplete;
    public List<Course> courses;

    public class Course {
        public String courseId;
        public String name;
        public String description;
        public String createDate;
        public String imagePath;
        public String commentNumber;
        public List<String> employeeNames;
        public String completed;
        public String totalUnits;
        public String percent;
    }
}
