package nts.swipesafe.adapter;

import android.app.Activity;
import android.content.Context;
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
import nts.swipesafe.model.MyCourseModel;

public class MyCourseAdapter extends BaseAdapter {

    LayoutInflater layoutInflater;
    public static Activity context;
    private OnItemClickListener onItemClickListener;
    List<MyCourseModel.Course> listObject;

    public interface OnItemClickListener {
        void onItemClick(View view, MyCourseModel.Course obj, int pos);

        void onStudyClick(View view, MyCourseModel.Course obj, int pos);

        void onDetailsClick(View view, MyCourseModel.Course obj, int pos);
    }

    public void setOnItemClickListener(final OnItemClickListener onItemClickListener) {
        this.onItemClickListener = onItemClickListener;
    }

    public MyCourseAdapter(Activity context, List<MyCourseModel.Course> listObject) {
        this.context = context;
        this.listObject = listObject;
        layoutInflater = LayoutInflater.from(context);
    }

    public class ViewHoder extends RecyclerView.ViewHolder {
        public TextView tv_CourseName, tv_CourseDescription, tv_CreateDate, tv_Lecturer, tv_TotalComment, tv_StatusCourse, tv_Details;
        public Button btn_Study;
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
        ViewHoder holder;
        final MyCourseModel.Course model = listObject.get(position);
        if (view == null) {
            view = layoutInflater.inflate(R.layout.item_my_course, null);
            holder = new ViewHoder(view);
            view.setTag(holder);
        } else {
            holder = (ViewHoder) view.getTag();
        }

        Picasso.with(context)
                .load(model.imagePath)
                .into(holder.image_Course);

        holder.tv_CourseName.setText(model.name);
        holder.tv_CourseDescription.setText(model.description);
        holder.tv_CreateDate.setText(DateUtils.ConvertYMDServerToDMY(model.createDate));
        if (model.employeeNames.size() > 0) {
            holder.tv_Lecturer.setText(model.employeeNames.get(0));
        } else {
            holder.tv_Lecturer.setText("");
        }
        holder.tv_TotalComment.setText(model.commentNumber);
        if (Integer.valueOf(model.percent) == 100) {
            holder.tv_StatusCourse.setText("Đã hoàn thành");
            holder.tv_StatusCourse.setTextColor(context.getResources().getColor(R.color.colorAccentDark));
        } else {
            int percent = (Integer.valueOf(model.completed) / Integer.valueOf(model.totalUnits)) * 100;
            holder.tv_StatusCourse.setText("Đã hoàn thành " + String.valueOf(model.percent) + "%");
            holder.tv_StatusCourse.setTextColor(context.getResources().getColor(R.color.amber_800));
        }

        holder.btn_Study.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onStudyClick(view, model, position);
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
