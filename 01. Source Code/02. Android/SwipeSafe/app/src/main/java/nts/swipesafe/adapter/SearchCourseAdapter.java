package nts.swipesafe.adapter;

import android.app.Activity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import com.squareup.picasso.Picasso;

import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.model.CourseModel;
import nts.swipesafe.model.MyCourseModel;

public class SearchCourseAdapter extends BaseAdapter {
    LayoutInflater layoutInflater;
    public static Activity context;
    private OnItemClickListener onItemClickListener;
    List<CourseModel> listObject;

    public interface OnItemClickListener {
        void onRegisterClick(View view, CourseModel obj, int pos);

        void onStudyClick(View view, CourseModel obj, int pos);

        void onDetailsClick(View view, CourseModel obj, int pos);
    }

    public void setOnItemClickListener(final SearchCourseAdapter.OnItemClickListener onItemClickListener) {
        this.onItemClickListener = onItemClickListener;
    }

    public SearchCourseAdapter(Activity context, List<CourseModel> listObject) {
        this.context = context;
        this.listObject = listObject;
        layoutInflater = LayoutInflater.from(context);
    }

    public class ViewHoder extends RecyclerView.ViewHolder {
        public TextView tv_CourseName, tv_CourseDescription, tv_CreateDate, tv_Lecturer, tv_TotalComment, tv_StatusCourse, tv_Details;
        public Button btn_Study, btn_Registration;
        public ImageView image_Course;

        public ViewHoder(View view) {
            super(view);
            tv_CourseName = view.findViewById(R.id.tv_CourseName);
            tv_CourseDescription = view.findViewById(R.id.tv_CourseDescription);
            tv_CreateDate = view.findViewById(R.id.tv_CreateDate);
            tv_Lecturer = view.findViewById(R.id.tv_Lecturer);
            tv_TotalComment = view.findViewById(R.id.tv_TotalComment);
            tv_StatusCourse = view.findViewById(R.id.tv_StatusCourse);
            image_Course = view.findViewById(R.id.image_Course);
            btn_Study = view.findViewById(R.id.btn_Study);
            tv_Details = view.findViewById(R.id.tv_Details);
            btn_Registration = view.findViewById(R.id.btn_Registration);
        }
    }


    @Override
    public int getCount() {
        return listObject.size();
    }

    @Override
    public Object getItem(int i) {
        return listObject.get(i);
    }

    @Override
    public long getItemId(int i) {
        return listObject.size();
    }

    @Override
    public View getView(final int position, View view, ViewGroup viewGroup) {
        SearchCourseAdapter.ViewHoder holder;
        final CourseModel model = listObject.get(position);
        if (view == null) {
            view = layoutInflater.inflate(R.layout.item_my_course, null);
            holder = new SearchCourseAdapter.ViewHoder(view);
            view.setTag(holder);
        } else {
            holder = (SearchCourseAdapter.ViewHoder) view.getTag();
        }

        if (model.isRegister) {
            holder.btn_Study.setVisibility(View.VISIBLE);
            holder.btn_Registration.setVisibility(View.GONE);
        } else {
            holder.btn_Study.setVisibility(View.GONE);
            holder.btn_Registration.setVisibility(View.VISIBLE);
        }

        Picasso.with(context)
                .load(model.imagePath)
                .into(holder.image_Course);

        holder.tv_CourseName.setText(model.name);
        holder.tv_CourseDescription.setText(model.description);
        holder.tv_CreateDate.setText(DateUtils.ConvertYMDServerToDMY(model.startDate));
        if (model.employeeNames.size() > 0) {
            holder.tv_Lecturer.setText(model.employeeNames.get(0));
        } else {
            holder.tv_Lecturer.setText("");
        }
        holder.tv_TotalComment.setText(model.commentNumber);

        holder.tv_StatusCourse.setVisibility(View.GONE);

        holder.btn_Study.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onStudyClick(view, model, position);
            }
        });

        holder.btn_Registration.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onRegisterClick(view, model, position);
            }
        });

        holder.tv_Details.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onDetailsClick(view, model, position);
            }
        });
        return view;
    }
}
