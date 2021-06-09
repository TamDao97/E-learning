package nts.swipesafe.adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import androidx.viewpager2.widget.ViewPager2;

import com.squareup.picasso.Picasso;

import java.util.List;

import nts.swipesafe.ActivityTabCourse;
import nts.swipesafe.R;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.model.CourseModel;
import nts.swipesafe.model.CourseResultModel;
import nts.swipesafe.model.ProgramModel;

import static java.lang.Math.abs;

public class DataAdapter extends BaseAdapter {
    LayoutInflater layoutInflater;
    public static Context context;
    private DataAdapter.OnItemClickListener onItemClickListener;
    List<CourseResultModel> listObject;

    public DataAdapter(Context context, List<CourseResultModel> listObject) {
        this.context = context;
        this.listObject = listObject;
        layoutInflater = LayoutInflater.from(context);
    }

    public interface OnItemClickListener {
        //void onItemClick(View view, ProgramModel obj, int pos);
        void onRegistrationClick(View view, CourseResultModel obj, int pos);

        void onDetialClick(View view, CourseResultModel obj, int pos);

        void onStudyClick(View view, CourseResultModel obj, int pos);
    }

    public void setOnItemClickListener(final DataAdapter.OnItemClickListener onItemClickListener) {
        this.onItemClickListener = onItemClickListener;
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
        ViewHolder viewHoder;
        if (view == null) {
            view = layoutInflater.from(viewGroup.getContext()).inflate(R.layout.item_list_course, viewGroup, false);
            viewHoder = new ViewHolder(view);
            view.setTag(viewHoder);
        } else {
            viewHoder = (ViewHolder) view.getTag();
        }

        final CourseResultModel courseModel = listObject.get(position);
        viewHoder.tv_CourseName.setText(courseModel.title);
        viewHoder.tv_CourseDescription.setText(courseModel.description);
        Picasso.with(context)
                .load(courseModel.imagePath)
                .into(viewHoder.image_Course);
        viewHoder.tv_CreateDate.setText(DateUtils.ConvertYMDServerToDMY(courseModel.startDate));
        if (courseModel.listEmployees.size() > 0) {
            viewHoder.tv_Lecturer.setText(courseModel.listEmployees.get(0));
        }
        viewHoder.tv_TotalComment.setText(courseModel.totalComment);

        if (courseModel.isRegister) {
            viewHoder.btn_Study.setVisibility(View.VISIBLE);
            viewHoder.btn_Registration.setVisibility(View.GONE);
        } else {
            viewHoder.btn_Study.setVisibility(View.GONE);
            viewHoder.btn_Registration.setVisibility(View.VISIBLE);
        }

        viewHoder.btn_Registration.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onRegistrationClick(view, courseModel, position);
            }
        });

        viewHoder.tv_Details.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onDetialClick(view, courseModel, position);
            }
        });
        viewHoder.btn_Study.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onStudyClick(view, courseModel, position);
            }
        });
        return view;
    }

    static class ViewHolder extends RecyclerView.ViewHolder {
        public TextView tv_CourseName, tv_CourseDescription, tv_CreateDate, tv_Lecturer, tv_TotalComment, tv_Details;
        public ImageView image_Course;
        private Button btn_Registration, btn_Study;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            tv_CourseName = itemView.findViewById(R.id.tv_CourseName);
            tv_CourseDescription = itemView.findViewById(R.id.tv_CourseDescription);
            image_Course = itemView.findViewById(R.id.image_Course);
            btn_Registration = itemView.findViewById(R.id.btn_Registration);
            tv_CreateDate = itemView.findViewById(R.id.tv_CreateDate);
            tv_Lecturer = itemView.findViewById(R.id.tv_Lecturer);
            tv_TotalComment = itemView.findViewById(R.id.tv_TotalComment);
            tv_Details = itemView.findViewById(R.id.tv_Details);
            btn_Study = itemView.findViewById(R.id.btn_Study);
        }
    }
}
