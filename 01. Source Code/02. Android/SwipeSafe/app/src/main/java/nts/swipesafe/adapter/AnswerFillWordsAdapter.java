package nts.swipesafe.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
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
public class AnswerFillWordsAdapter extends RecyclerView.Adapter<AnswerFillWordsAdapter.ViewHolder> {

    private final int mBackground;
    private List<AnswerModel> listAnswer = new ArrayList<>();
    private boolean mIsFinish = false;

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private AnswerFillWordsAdapter.OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(int position, AnswerModel model);
    }

    public void SetOnItemClickListener(final AnswerFillWordsAdapter.OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public EditText txtAnswerFill;
        public  TextView tvAnswerLabel;

        public ViewHolder(View v) {
            super(v);
            txtAnswerFill = (EditText) v.findViewById(R.id.txtAnswerFill);
            tvAnswerLabel = (TextView) v.findViewById(R.id.tvAnswerLabel);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public AnswerFillWordsAdapter(Context ctx, List<AnswerModel> items, boolean isFinish) {
        this.ctx = ctx;
        listAnswer = items;
        mIsFinish = isFinish;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public AnswerFillWordsAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_answer_fill_words, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        AnswerFillWordsAdapter.ViewHolder vh = new AnswerFillWordsAdapter.ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(AnswerFillWordsAdapter.ViewHolder holder, final int position) {
        final AnswerModel answerModel = listAnswer.get(position);
        holder.txtAnswerFill.setText(answerModel.learnerAnswerContent);
        holder.tvAnswerLabel.setText(answerModel.answerLable + ")");

        holder.txtAnswerFill.setEnabled(!mIsFinish);
        // view detail message conversation
        holder.txtAnswerFill.addTextChangedListener(new TextWatcher() {

            @Override
            public void afterTextChanged(Editable s) {
                if (s.length() != 0) {
                    answerModel.learnerAnswerContent = s.toString();
                    mOnItemClickListener.onItemClick(position, answerModel);
                }
            }

            @Override
            public void beforeTextChanged(CharSequence s, int start,
                                          int count, int after) {
            }

            @Override
            public void onTextChanged(CharSequence s, int start,
                                      int before, int count) {
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
