package nts.swipesafe.adapter;

import android.annotation.SuppressLint;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.squareup.picasso.Picasso;

import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.model.CourseModel;
import nts.swipesafe.model.ProgramModel;

public class CourseViewPagerAdapter extends RecyclerView.Adapter<CourseViewPagerAdapter.ViewHolder> {
    private LayoutInflater mInflater;
    private List<CourseModel> listObject;
    private Context context;
    private OnItemClickListener onItemClickListener;

    public CourseViewPagerAdapter(List<CourseModel> listObject, Context mcontext) {
        context = mcontext;
        mInflater = LayoutInflater.from(context);
        this.listObject = listObject;
    }

    public interface OnItemClickListener {
        void onRegistrationClick(View view, CourseModel obj, int pos);

        void onStudyClick(View view, CourseModel obj, int pos);

        void onDetialClick(View view, CourseModel obj, int pos);
    }

    public void setOnItemClickListener(final CourseViewPagerAdapter.OnItemClickListener onItemClickListener) {
        this.onItemClickListener = onItemClickListener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_pager_card, parent, false);
        return new ViewHolder(view);
    }

    @SuppressLint("RestrictedApi")
    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, final int position) {
        final CourseModel courseModel = listObject.get(position);
        holder.tv_CourseName.setText(courseModel.name);
        holder.tv_CourseDescription.setText(courseModel.description);
        Picasso.with(context)
                .load(courseModel.imagePath)
                .into(holder.image_Course);
        holder.tv_CreateDate.setText(DateUtils.ConvertYMDServerToDMY(courseModel.startDate));
        if (courseModel.employeeNames.size() > 0) {
            holder.tv_Lecturer.setText(courseModel.employeeNames.get(0));
        } else {
            holder.tv_Lecturer.setText("");
        }

        if (courseModel.isRegister) {
            holder.btn_Registration.setVisibility(View.GONE);
            holder.btn_Study.setVisibility(View.VISIBLE);
        } else {
            holder.btn_Registration.setVisibility(View.VISIBLE);
            holder.btn_Study.setVisibility(View.GONE);
        }

        if(courseModel.isNew){
            holder.floatButtonNew.setVisibility(View.VISIBLE);
        }else {
            holder.floatButtonNew.setVisibility(View.GONE);
        }

        holder.tv_TotalComment.setText(courseModel.commentNumber);

        holder.btn_Registration.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onRegistrationClick(view, courseModel, position);
            }
        });

        holder.tv_Details.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onDetialClick(view, courseModel, position);
            }
        });

        holder.btn_Study.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onStudyClick(view, courseModel, position);
            }
        });
    }

    @Override
    public int getItemCount() {
        return listObject.size();
    }

    static class ViewHolder extends RecyclerView.ViewHolder {
        public TextView tv_CourseName, tv_CourseDescription, tv_CreateDate, tv_Lecturer, tv_TotalComment, tv_Details;
        public ImageView image_Course;
        private Button btn_Registration, btn_Study;
        private FloatingActionButton floatButtonNew;

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
            floatButtonNew = itemView.findViewById(R.id.floatButtonNew);
        }
    }
}
