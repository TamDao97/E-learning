package nts.swipesafe.adapter;

import android.content.Context;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import com.squareup.picasso.Callback;
import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.MobileCommentResultModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class CommentReplyAdapter extends RecyclerView.Adapter<CommentReplyAdapter.ViewHolder> {
    private List<MobileCommentResultModel> listComment = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, List<MobileCommentResultModel> listAnswerChoose);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        private ImageView imgAvatar;
        public TextView tvLetter, tvName, tvContent, tvDate;

        public ViewHolder(View v) {
            super(v);
            imgAvatar = (ImageView) v.findViewById(R.id.imgAvatar);
            tvLetter = (TextView) v.findViewById(R.id.tvLetter);
            tvName = (TextView) v.findViewById(R.id.tvName);
            tvContent = (TextView) v.findViewById(R.id.tvContent);
            tvDate = (TextView) v.findViewById(R.id.tvDate);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public CommentReplyAdapter(Context ctx, List<MobileCommentResultModel> items) {
        this.ctx = ctx;
        listComment = items;
        //ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
    }

    @Override
    public CommentReplyAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_comment_reply, parent, false);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(final ViewHolder holder, final int position) {
        final MobileCommentResultModel comment = listComment.get(position);

        if (Utils.isEmpty(comment.imagePath)) {
            holder.imgAvatar.setImageResource(R.drawable.shape_circle_grey);
            if (!Utils.isEmpty(comment.userName)) {
                String[] name = comment.userName.toUpperCase().split(" ");
                holder.tvLetter.setText((name[0].substring(0, 1) + name[name.length - 1].substring(0, 1)));
            }
        } else {
            Picasso.with(ctx)
                    .load(comment.imagePath)
                    .resize(40, 40)
                    .error(R.drawable.shape_circle_grey)
                    .centerCrop().into(holder.imgAvatar, new Callback() {
                @Override
                public void onSuccess() {

                }

                @Override
                public void onError() {
                    if (!Utils.isEmpty(comment.userName)) {
                        String[] name = comment.userName.toUpperCase().split(" ");
                        holder.tvLetter.setText((name[0].substring(0, 1) + name[name.length - 1].substring(0, 1)));
                    }
                }
            });
        }

        //holder.tvLetter.setText();
        holder.tvName.setText(comment.userName);
        holder.tvContent.setText(comment.content);
        holder.tvDate.setText(DateUtils.ConvertYMDServerToDMYHHMM(comment.commentDate));
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listComment.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
