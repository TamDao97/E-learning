package nts.swipesafe.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;
import androidx.viewpager2.widget.ViewPager2;

import com.squareup.picasso.Picasso;

import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.CircleTransform;
import nts.swipesafe.model.CourseGeneralInfoModel;
import nts.swipesafe.model.EmployeeElearningModel;
import nts.swipesafe.model.ProgramModel;

public class GuiderAdapter extends BaseAdapter {
    LayoutInflater layoutInflater;
    public static Context context;
    private CourseAdapter.OnItemClickListener onItemClickListener;
    private List<EmployeeElearningModel> listObject;

    public GuiderAdapter(Context context, List<EmployeeElearningModel> listObject) {
        this.context = context;
        this.listObject = listObject;
        layoutInflater = LayoutInflater.from(this.context);
    }

    public interface OnItemClickListener {
        void onItemClick(View view, ProgramModel obj, int pos);
    }

    public void setOnItemClickListener(final CourseAdapter.OnItemClickListener onItemClickListener) {
        this.onItemClickListener = onItemClickListener;
    }


    public class ViewHoder extends RecyclerView.ViewHolder {
        public TextView tv_NameGuide, tv_NumberCourseGuide;
        public ImageView image_Guider;

        public ViewHoder(View v) {
            super(v);
            tv_NameGuide = v.findViewById(R.id.tv_NameGuide);
            tv_NumberCourseGuide = v.findViewById(R.id.tv_NumberCourseGuide);
            image_Guider = v.findViewById(R.id.image_Guider);
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
    public View getView(int i, View view, ViewGroup viewGroup) {
        GuiderAdapter.ViewHoder viewHoder;
        EmployeeElearningModel employeeElearningModel = listObject.get(i);

        if (view == null) {
            view = layoutInflater.inflate(R.layout.item_list_guider, null);
            viewHoder = new GuiderAdapter.ViewHoder(view);
            view.setTag(viewHoder);
        } else {
            viewHoder = (GuiderAdapter.ViewHoder) view.getTag();
        }

        viewHoder.tv_NameGuide.setText(employeeElearningModel.employeeName);
        viewHoder.tv_NumberCourseGuide.setText("Hướng dẫn " + employeeElearningModel.totalCourse + " khoá học");
        Picasso.with(context)
                .load(employeeElearningModel.imagePath)
                .transform(new CircleTransform())
                .into(viewHoder.image_Guider);
        return view;
    }

}
