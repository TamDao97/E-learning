package nts.swipesafe.adapter;

import android.content.Context;
import android.os.Bundle;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.TextView;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.ItemTouchHelper;
import androidx.recyclerview.widget.RecyclerView;

import com.google.gson.Gson;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.ItemTouchListenner;
import nts.swipesafe.common.SimpleItemTouchHelperCallback;
import nts.swipesafe.common.Utils;
import nts.swipesafe.fragment.LibrarySkillDetailFragment;
import nts.swipesafe.model.AnswerModel;
import nts.swipesafe.model.QuestionModel;
import nts.swipesafe.model.SkillModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class QuestionAdapter extends RecyclerView.Adapter<QuestionAdapter.ViewHolder> {

    private final int mBackground;
    private List<QuestionModel> listQuestion = new ArrayList<>();
    private boolean mIsFinish = false;

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, List<QuestionModel> listQuestionChoose);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout llQuestion, llAnswer;
        public WebView wvContent;
        public RecyclerView rvAnswerLearner, rvAnswer;
        public TextView lbNote;
        public ImageView imgIsResult;

        public ViewHolder(View v) {
            super(v);
            llQuestion = (LinearLayout) v.findViewById(R.id.llQuestion);
            wvContent = (WebView) v.findViewById(R.id.wvContent);
            llAnswer = (LinearLayout) v.findViewById(R.id.llAnswer);
            wvContent.setBackgroundColor(0);
            wvContent.setLayerType(View.LAYER_TYPE_SOFTWARE, null);
            rvAnswerLearner = (RecyclerView) v.findViewById(R.id.rvAnswerLearner);
            rvAnswerLearner.setLayoutManager(new GridLayoutManager(ctx, 1));
            rvAnswerLearner.setHasFixedSize(true);
            lbNote = (TextView) v.findViewById(R.id.lbNote);
            imgIsResult = (ImageView) v.findViewById(R.id.imgIsResult);
            rvAnswer = (RecyclerView) v.findViewById(R.id.rvAnswer);
            rvAnswer.setLayoutManager(new GridLayoutManager(ctx, 1));
            rvAnswer.setHasFixedSize(true);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public QuestionAdapter(Context ctx, List<QuestionModel> items, boolean isFinish) {
        this.ctx = ctx;
        listQuestion = items;
        mIsFinish = isFinish;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public QuestionAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_question, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(final ViewHolder holder, final int position) {
        final QuestionModel questionModel = listQuestion.get(position);
        String content = "<body style=\"padding: 10px; background-color:transparent\">" + questionModel.content + "</body>";
        holder.wvContent.loadData(content, "text/html; charset=utf-8", "UTF-8");
        if (mIsFinish) {
            holder.llQuestion.setClickable(false);
            if (questionModel.isResultLearner)
                holder.imgIsResult.setImageResource(R.drawable.ic_success);
            else
                holder.imgIsResult.setImageResource(R.drawable.ic_wrong);

            holder.llAnswer.setVisibility(View.VISIBLE);
        } else {
            holder.llAnswer.setVisibility(View.GONE);
            holder.llQuestion.setClickable(true);
        }

        switch (questionModel.type) {
            case Constants.QuestionOneCorrect: {
                final AnswerTickOneAdapter answerTickOneAdapter = new AnswerTickOneAdapter(ctx, questionModel.listAnswer, mIsFinish);
                holder.rvAnswerLearner.setAdapter(answerTickOneAdapter);
                answerTickOneAdapter.SetOnItemClickListener(new AnswerTickOneAdapter.OnItemClickListener() {
                    @Override
                    public void onItemClick(View view, int position, List<AnswerModel> listAnswer) {
                        for (AnswerModel itemView : questionModel.listAnswer) {
                            for (AnswerModel itemChoose : listAnswer) {
                                if (itemView.answerId.equals(itemChoose.answerId)) {
                                    itemView.learnerIsCorrect = itemChoose.learnerIsCorrect;
                                    break;
                                }
                            }
                        }
                        answerTickOneAdapter.notifyDataSetChanged();
                    }
                });

                if (mIsFinish) {
                    List<AnswerModel> listAnswer = new ArrayList<>();
                    AnswerModel answerModelTemp;
                    for (AnswerModel answer : questionModel.listAnswer) {
                        if (answer.isCorrect) {
                            answerModelTemp = new AnswerModel();
                            answerModelTemp.answerId = answer.answerId;
                            answerModelTemp.learnerIsCorrect = answer.isCorrect;
                            answerModelTemp.learnerDisplayIndex = answer.displayIndex;
                            answerModelTemp.answerContent = answer.answerContent;
                            listAnswer.add(answerModelTemp);
                        }
                    }
                    AnswerTickOneAdapter answerAdapter = new AnswerTickOneAdapter(ctx, listAnswer, mIsFinish);
                    holder.rvAnswer.setAdapter(answerAdapter);
                }
                break;
            }
            case Constants.QuestionMutileCorrect: {
                final AnswerTickMultipleAdapter answerTickMultipleAdapter = new AnswerTickMultipleAdapter(ctx, questionModel.listAnswer, mIsFinish);
                holder.rvAnswerLearner.setAdapter(answerTickMultipleAdapter);
                answerTickMultipleAdapter.SetOnItemClickListener(new AnswerTickMultipleAdapter.OnItemClickListener() {
                    @Override
                    public void onItemClick(View view, int position, AnswerModel model) {
                        for (AnswerModel item : questionModel.listAnswer) {
                            if (item.answerId.equals(model.answerId)) {
                                item.learnerIsCorrect = model.learnerIsCorrect;
                            }
                        }
                        answerTickMultipleAdapter.notifyDataSetChanged();
                    }
                });

                if (mIsFinish) {
                    List<AnswerModel> listAnswer = new ArrayList<>();
                    AnswerModel answerModelTemp;
                    for (AnswerModel answer : questionModel.listAnswer) {
                        if (answer.isCorrect) {
                            answerModelTemp = new AnswerModel();
                            answerModelTemp.answerId = answer.answerId;
                            answerModelTemp.learnerIsCorrect = answer.isCorrect;
                            answerModelTemp.learnerDisplayIndex = answer.displayIndex;
                            answerModelTemp.answerContent = answer.answerContent;
                            listAnswer.add(answerModelTemp);
                        }
                    }
                    AnswerTickMultipleAdapter answerAdapter = new AnswerTickMultipleAdapter(ctx, listAnswer, mIsFinish);
                    holder.rvAnswer.setAdapter(answerAdapter);
                }
                break;
            }
            case Constants.QuestionYesNo: {
                final AnswerTickOneAdapter answerTickYesNoAdapter = new AnswerTickOneAdapter(ctx, questionModel.listAnswer, mIsFinish);
                holder.rvAnswerLearner.setAdapter(answerTickYesNoAdapter);
                answerTickYesNoAdapter.SetOnItemClickListener(new AnswerTickOneAdapter.OnItemClickListener() {
                    @Override
                    public void onItemClick(View view, int position, List<AnswerModel> listAnswer) {
                        for (AnswerModel itemView : questionModel.listAnswer) {
                            for (AnswerModel itemChoose : listAnswer) {
                                if (itemView.answerId.equals(itemChoose.answerId)) {
                                    itemView.learnerIsCorrect = itemChoose.learnerIsCorrect;
                                    break;
                                }
                            }
                        }
                        answerTickYesNoAdapter.notifyDataSetChanged();
                    }
                });

                if (mIsFinish) {
                    List<AnswerModel> listAnswer = new ArrayList<>();
                    AnswerModel answerModelTemp;
                    for (AnswerModel answer : questionModel.listAnswer) {
                        if (answer.isCorrect) {
                            answerModelTemp = new AnswerModel();
                            answerModelTemp.answerId = answer.answerId;
                            answerModelTemp.learnerIsCorrect = answer.isCorrect;
                            answerModelTemp.learnerDisplayIndex = answer.displayIndex;
                            answerModelTemp.answerContent = answer.answerContent;
                            listAnswer.add(answerModelTemp);
                        }
                    }
                    AnswerTickOneAdapter answerAdapter = new AnswerTickOneAdapter(ctx, listAnswer, mIsFinish);
                    holder.rvAnswer.setAdapter(answerAdapter);
                }
                break;
            }
            case Constants.QuestionFillText: {
                final AnswerFillWordsAdapter answerFillWordsAdapter = new AnswerFillWordsAdapter(ctx, questionModel.listAnswer, mIsFinish);
                holder.rvAnswerLearner.setAdapter(answerFillWordsAdapter);
                answerFillWordsAdapter.SetOnItemClickListener(new AnswerFillWordsAdapter.OnItemClickListener() {
                    @Override
                    public void onItemClick(int position, AnswerModel model) {
                        for (AnswerModel item : questionModel.listAnswer) {
                            if (item.answerId.equals(model.answerId)) {
                                item.learnerAnswerContent = model.learnerAnswerContent;
                                item.learnerIsCorrect = true;
                            }
                        }
                    }
                });

                if (mIsFinish) {
                    List<AnswerModel> listAnswer = new ArrayList<>();
                    AnswerModel answerModelTemp;
                    for (AnswerModel answer : questionModel.listAnswer) {
                        answerModelTemp = new AnswerModel();
                        answerModelTemp.answerId = answer.answerId;
                        answerModelTemp.learnerIsCorrect = answer.isCorrect;
                        answerModelTemp.learnerDisplayIndex = answer.displayIndex;
                        answerModelTemp.learnerAnswerContent = answer.answerContent;
                        listAnswer.add(answerModelTemp);
                    }
                    AnswerFillWordsAdapter answerAdapter = new AnswerFillWordsAdapter(ctx, listAnswer, mIsFinish);
                    holder.rvAnswer.setAdapter(answerAdapter);
                }
                break;
            }
            case Constants.QuestionSort: {
                holder.lbNote.setVisibility(mIsFinish ? View.GONE : View.VISIBLE);
                Collections.sort(questionModel.listAnswer, new Comparator<AnswerModel>() {
                    @Override
                    public int compare(AnswerModel lhs, AnswerModel rhs) {
                        return lhs.getLearnerDisplayIndex() > rhs.getLearnerDisplayIndex() ? 1 : -1;
                    }
                });
                AnswerSortAdapter answerSortAdapter = new AnswerSortAdapter(ctx, questionModel.listAnswer, mIsFinish);
                holder.rvAnswerLearner.setAdapter(answerSortAdapter);
                if (!mIsFinish) {
                    addItemTouchCallback(holder.rvAnswerLearner, answerSortAdapter);
                    answerSortAdapter.SetOnItemClickListener(new AnswerSortAdapter.OnItemClickListener() {
                        @Override
                        public void onItemChange(List<AnswerModel> listAnswer) {
                            int index = 1;
                            for (AnswerModel itemChoose : listAnswer) {
                                itemChoose.learnerDisplayIndex = index;
                                itemChoose.learnerIsCorrect = true;
                                index++;
                            }
                            questionModel.listAnswer = listAnswer;
                        }
                    });
                }

                if (mIsFinish) {
                    List<AnswerModel> listAnswer = new ArrayList<>();
                    AnswerModel answerModelTemp;
                    for (AnswerModel answer : questionModel.listAnswer) {
                        answerModelTemp = new AnswerModel();
                        answerModelTemp.answerId = answer.answerId;
                        answerModelTemp.learnerIsCorrect = answer.isCorrect;
                        answerModelTemp.learnerDisplayIndex = answer.displayIndex;
                        answerModelTemp.answerContent = answer.answerContent;
                        listAnswer.add(answerModelTemp);
                    }
                    Collections.sort(listAnswer, new Comparator<AnswerModel>() {
                        @Override
                        public int compare(AnswerModel lhs, AnswerModel rhs) {
                            return lhs.getLearnerDisplayIndex() > rhs.getLearnerDisplayIndex() ? 1 : -1;
                        }
                    });
                    AnswerSortAdapter answerAdapter = new AnswerSortAdapter(ctx, listAnswer, mIsFinish);
                    holder.rvAnswer.setAdapter(answerAdapter);
                }
                break;
            }
        }
    }

    private void addItemTouchCallback(RecyclerView recyclerView, final AnswerSortAdapter answerSortAdapter) {
        ItemTouchHelper.Callback callback = new SimpleItemTouchHelperCallback(new ItemTouchListenner() {
            @Override
            public void onMove(int oldPosition, int newPosition) {
                answerSortAdapter.onMove(oldPosition, newPosition);
            }
        });
        ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
        itemTouchHelper.attachToRecyclerView(recyclerView);
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listQuestion.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
