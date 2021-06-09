package nts.swipesafe.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.model.AnswerModel;
import nts.swipesafe.model.SkillModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class AnswerTickMultipleAdapter extends RecyclerView.Adapter<AnswerTickMultipleAdapter.ViewHolder> {

    private final int mBackground;
    private List<AnswerModel> listAnswer = new ArrayList<>();
    private boolean mIsFinish = false;

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private AnswerTickMultipleAdapter.OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, AnswerModel model);
    }

    public void SetOnItemClickListener(final AnswerTickMultipleAdapter.OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout llAnswer;
        public TextView txtAnswerContent;
        public CheckBox ckbChoose;

        public ViewHolder(View v) {
            super(v);
            llAnswer = (LinearLayout) v.findViewById(R.id.llAnswer);
            txtAnswerContent = (TextView) v.findViewById(R.id.txtAnswerContent);
            ckbChoose = (CheckBox) v.findViewById(R.id.ckbChoose);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public AnswerTickMultipleAdapter(Context ctx, List<AnswerModel> items, boolean isFinish) {
        this.ctx = ctx;
        listAnswer = items;
        mIsFinish = isFinish;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public AnswerTickMultipleAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_answer_tick_multiple, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        AnswerTickMultipleAdapter.ViewHolder vh = new AnswerTickMultipleAdapter.ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(AnswerTickMultipleAdapter.ViewHolder holder, final int position) {
        final AnswerModel answerModel = listAnswer.get(position);
        holder.txtAnswerContent.setText(answerModel.answerContent);
        holder.ckbChoose.setChecked(answerModel.learnerIsCorrect);

        holder.llAnswer.setEnabled(!mIsFinish);
        // view detail message conversation
        holder.llAnswer.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    answerModel.learnerIsCorrect= !answerModel.learnerIsCorrect;
                    mOnItemClickListener.onItemClick(v, position, answerModel);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listAnswer.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
