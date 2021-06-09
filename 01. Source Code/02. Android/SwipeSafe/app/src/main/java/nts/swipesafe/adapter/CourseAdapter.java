package nts.swipesafe.adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.HorizontalScrollView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import androidx.viewpager2.widget.CompositePageTransformer;
import androidx.viewpager2.widget.MarginPageTransformer;
import androidx.viewpager2.widget.ViewPager2;

import java.util.List;
import java.util.Locale;

import nts.swipesafe.ActivityTabCourse;
import nts.swipesafe.R;
import nts.swipesafe.model.CourseModel;
import nts.swipesafe.model.ProgramModel;

import static java.lang.Math.abs;
import static java.lang.Math.floor;

public class CourseAdapter extends BaseAdapter {
    LayoutInflater layoutInflater;
    public static Context context;
    private OnItemClickListener onItemClickListener;
    List<ProgramModel> listObject;

    public CourseAdapter(Context context, List<ProgramModel> listObject) {
        this.context = context;
        this.listObject = listObject;
        layoutInflater = LayoutInflater.from(context);
    }


    public interface OnItemClickListener {
        void onProgramClick(View view, ProgramModel obj, int pos);

        void onRegistrationClick(View view, CourseModel obj, int pos);

        void onStudynClick(View view, CourseModel obj, int pos);

        void onDetialClick(View view, CourseModel obj, int pos);
    }

    public void setOnItemClickListener(final OnItemClickListener onItemClickListener) {
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

    public class ViewHoder extends RecyclerView.ViewHolder {
        public TextView txtProgramName;
        public ViewPager2 view_pager;

        public ViewHoder(View v) {
            super(v);
            txtProgramName = v.findViewById(R.id.txtProgramName);
            view_pager = v.findViewById(R.id.view_pager);
        }

    }

    @Override
    public View getView(final int i, View view, ViewGroup viewGroup) {
        ViewHoder viewHoder;
        final ProgramModel programModel = listObject.get(i);
        if (view == null) {
            view = layoutInflater.inflate(R.layout.item_course, null);
            viewHoder = new ViewHoder(view);
            view.setTag(viewHoder);
        } else {
            viewHoder = (ViewHoder) view.getTag();
        }
        viewHoder.txtProgramName.setText(programModel.name);

        viewHoder.txtProgramName.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onItemClickListener.onProgramClick(view, programModel, i);
            }
        });

        CourseViewPagerAdapter courseViewPagerAdapter = new CourseViewPagerAdapter(programModel.courses, context);
        courseViewPagerAdapter.setOnItemClickListener(new CourseViewPagerAdapter.OnItemClickListener() {

            @Override
            public void onRegistrationClick(View view, CourseModel obj, int pos) {
                onItemClickListener.onRegistrationClick(view, obj, pos);
            }

            @Override
            public void onStudyClick(View view, CourseModel obj, int pos) {
                onItemClickListener.onStudynClick(view, obj, pos);
            }

            @Override
            public void onDetialClick(View view, CourseModel obj, int pos) {
                onItemClickListener.onDetialClick(view, obj, pos);
            }
        });
        viewHoder.view_pager.setAdapter(courseViewPagerAdapter);
        viewHoder.view_pager.setCurrentItem(0);
        viewHoder.view_pager.setOffscreenPageLimit(1);
        int nextItemVisiblePx = (int) context.getResources().getDimension(R.dimen.viewpager_next_item_visible);
        int currentItemHorizontalMarginPx = (int) context.getResources().getDimension(R.dimen.viewpager_current_item_horizontal_margin);
        final int pageTranslationX = nextItemVisiblePx + currentItemHorizontalMarginPx;

        ViewPager2.PageTransformer pageTransformer = new ViewPager2.PageTransformer() {
            @Override
            public void transformPage(@NonNull View page, float position) {
                page.setTranslationX(-pageTranslationX * position);
                // Next line scales the item's height. You can remove it if you don't want this effect
                page.setScaleY(1 - (0.15f * abs(position)));
                // If you want a fading effect uncomment the next line:
                page.setAlpha(0.75f + (1 - abs(position)));
            }
        };

        viewHoder.view_pager.setPageTransformer(pageTransformer);
        if (viewHoder.view_pager.getItemDecorationCount() == 0) {
            viewHoder.view_pager.addItemDecoration(new HorizontalMarginItemDecoration(context, R.dimen.viewpager_current_item_horizontal_margin_testing,
                    R.dimen.viewpager_next_item_visible_testing));
        }
        viewHoder.view_pager.registerOnPageChangeCallback(new ViewPager2.OnPageChangeCallback() {
            @Override
            public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {
                super.onPageScrolled(position, positionOffset, positionOffsetPixels);
            }
        });

        return view;
    }
}
