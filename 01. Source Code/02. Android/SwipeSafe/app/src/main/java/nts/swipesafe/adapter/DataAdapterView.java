package nts.swipesafe.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.squareup.picasso.Picasso;

import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.model.CourseModel;
import nts.swipesafe.model.CourseResultModel;

public class DataAdapterView extends RecyclerView.Adapter<DataAdapterView.ViewHolder> {
    private LayoutInflater mInflater;
    private List<CourseResultModel> listObject;
    private Context context;
    private DataAdapterView.OnItemClickListener onItemClickListener;

    public DataAdapterView(List<CourseResultModel> listObject, Context mcontext) {
        context = mcontext;
        mInflater = LayoutInflater.from(context);
        this.listObject = listObject;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_pager_card, parent, false);
        return new ViewHolder(view);
    }

    public interface OnItemClickListener {
        void onRegistrationClick(View view, CourseResultModel obj, int pos);

        void onDetialClick(View view, CourseResultModel obj, int pos);
    }

    public void setOnItemClickListener(final DataAdapterView.OnItemClickListener onItemClickListener) {
        this.onItemClickListener = onItemClickListener;
    }

    @Override
    public void onBindViewHolder(@NonNull DataAdapterView.ViewHolder holder, final int position) {
        final CourseResultModel courseModel = listObject.get(position);
        holder.tv_CourseName.setText(courseModel.title);
        holder.tv_CourseDescription.setText(courseModel.description);
        Picasso.with(context)
                .load(courseModel.imagePath)
                .into(holder.image_Course);
        holder.tv_CreateDate.setText(DateUtils.ConvertYMDServerToDMY(courseModel.startDate));
        holder.tv_Lecturer.setText(courseModel.listEmployees.get(0));
        holder.tv_TotalComment.setText(courseModel.totalComment);

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
    }

    @Override
    public int getItemCount() {
        return listObject.size();
    }

    static class ViewHolder extends RecyclerView.ViewHolder {
        public TextView tv_CourseName, tv_CourseDescription, tv_CreateDate, tv_Lecturer, tv_TotalComment, tv_Details;
        public ImageView image_Course;
        private Button btn_Registration;

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
        }
    }
}
