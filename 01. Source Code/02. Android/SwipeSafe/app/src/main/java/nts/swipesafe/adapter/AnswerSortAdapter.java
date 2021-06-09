package nts.swipesafe.adapter;

import android.content.Context;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.model.AnswerModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class AnswerSortAdapter extends RecyclerView.Adapter<AnswerSortAdapter.ViewHolder> {

    private final int mBackground;
    private List<AnswerModel> listAnswer = new ArrayList<>();
    private boolean mIsFinish = false;

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private AnswerSortAdapter.OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemChange(List<AnswerModel> listAnswer);
    }

    public void SetOnItemClickListener(final AnswerSortAdapter.OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtAnswerContent;

        public ViewHolder(View v) {
            super(v);
            txtAnswerContent = (TextView) v.findViewById(R.id.txtAnswerContent);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public AnswerSortAdapter(Context ctx, List<AnswerModel> items, boolean isFinish) {
        this.ctx = ctx;
        listAnswer = items;
        mIsFinish = isFinish;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    public void onMove(int fromPosition, int toPosition) {
        if (fromPosition < toPosition) {
            for (int i = fromPosition; i < toPosition; i++) {
                Collections.swap(listAnswer, i, i + 1);
            }
        } else {
            for (int i = fromPosition; i > toPosition; i--) {
                Collections.swap(listAnswer, i, i - 1);
            }
        }
        notifyItemMoved(fromPosition, toPosition);
        mOnItemClickListener.onItemChange(listAnswer);
    }

    @Override
    public AnswerSortAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_answer_sort, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        AnswerSortAdapter.ViewHolder vh = new AnswerSortAdapter.ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(AnswerSortAdapter.ViewHolder holder, final int position) {
        final AnswerModel answerModel = listAnswer.get(position);
        holder.txtAnswerContent.setText(answerModel.answerContent);
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
