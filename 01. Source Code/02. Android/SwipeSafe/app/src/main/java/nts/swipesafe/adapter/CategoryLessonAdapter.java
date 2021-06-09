package nts.swipesafe.adapter;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.res.ColorStateList;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.TextView;

import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.RecyclerView;

import com.google.android.material.floatingactionbutton.FloatingActionButton;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.model.AnswerModel;
import nts.swipesafe.model.LessonMenuModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class CategoryLessonAdapter extends RecyclerView.Adapter<CategoryLessonAdapter.ViewHolder> {

    private final int mBackground;
    private List<LessonMenuModel> listLesson = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, LessonMenuModel item);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout llItemLesson;
        public TextView txtLessonTitle;
        public FloatingActionButton fabStatus;

        public ViewHolder(View v) {
            super(v);
            llItemLesson = (LinearLayout) v.findViewById(R.id.llItemLesson);
            txtLessonTitle = (TextView) v.findViewById(R.id.txtLessonTitle);
            fabStatus = (FloatingActionButton) v.findViewById(R.id.fabMenuLesson);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public CategoryLessonAdapter(Context ctx, List<LessonMenuModel> items) {
        this.ctx = ctx;
        listLesson = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public CategoryLessonAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_menu_lesson, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final LessonMenuModel lesson = listLesson.get(position);
        holder.txtLessonTitle.setText(lesson.name);
        holder.fabStatus.setBackgroundTintList(ContextCompat.getColorStateList(ctx,(lesson.status? R.color.green_700 : R.color.grey_20)));
        // view detail message conversation
        holder.llItemLesson.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onItemClick(v, position, lesson);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listLesson.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
