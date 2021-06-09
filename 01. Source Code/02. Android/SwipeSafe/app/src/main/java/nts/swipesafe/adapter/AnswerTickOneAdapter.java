package nts.swipesafe.adapter;

import android.content.Context;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.model.AnswerModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class AnswerTickOneAdapter extends RecyclerView.Adapter<AnswerTickOneAdapter.ViewHolder> {

    private final int mBackground;
    private List<AnswerModel> listAnswer = new ArrayList<>();
    private boolean mIsFinish = false;

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, List<AnswerModel> listAnswerChoose);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout llAnswer;
        public TextView txtAnswerContent;
        public RadioButton rdgChoose;

        public ViewHolder(View v) {
            super(v);
            llAnswer = (LinearLayout) v.findViewById(R.id.llAnswer);
            txtAnswerContent = (TextView) v.findViewById(R.id.txtAnswerContent);
            rdgChoose = (RadioButton) v.findViewById(R.id.rdgChoose);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public AnswerTickOneAdapter(Context ctx, List<AnswerModel> items, boolean isFinish) {
        this.ctx = ctx;
        listAnswer = items;
        mIsFinish = isFinish;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public AnswerTickOneAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_answer_tick_one, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final AnswerModel answerModel = listAnswer.get(position);
        holder.txtAnswerContent.setText(answerModel.answerContent);
        holder.rdgChoose.setChecked(answerModel.learnerIsCorrect);

        holder.llAnswer.setEnabled(!mIsFinish);
        // view detail message conversation
        holder.llAnswer.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    for (AnswerModel item : listAnswer) {
                        if (item.answerId.equals((answerModel.answerId))) {
                            item.learnerIsCorrect = true;
                        } else {
                            item.learnerIsCorrect = false;
                        }
                    }
                    mOnItemClickListener.onItemClick(v, position, listAnswer);
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
