package nts.swipesafe.adapter;

import android.content.Context;
import android.content.res.Resources;

import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.model.InternetModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class InternetAdapter extends RecyclerView.Adapter<InternetAdapter.ViewHolder> {

    private final int mBackground;
    private List<InternetModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, InternetModel model);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout lySkill;
        public TextView txtTitle, txtDateTime, txtSubTitle;
        public ImageView imageThumbnail;

        public ViewHolder(View v) {
            super(v);
            lySkill = (LinearLayout) v.findViewById(R.id.lySkill);
            txtTitle = (TextView) v.findViewById(R.id.txtTitle);
            txtDateTime = (TextView) v.findViewById(R.id.txtDateTime);
            txtSubTitle = (TextView) v.findViewById(R.id.txtSubTitle);
            imageThumbnail = (ImageView) v.findViewById(R.id.imageThumbnail);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public InternetAdapter(Context ctx, List<InternetModel> items) {
        this.ctx = ctx;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public InternetAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_skill, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final InternetModel model = filtered_items.get(position);
        holder.txtTitle.setText(model.Title);
        holder.txtSubTitle.setText(model.SubTitle);
        if(model.IsVideo) {
            holder.txtDateTime.setText(DateUtils.ConvertYMDHHmmServerToDMYHHmm(model.DateTime));
            Picasso.with(ctx)
                    .load(model.ImageThumbnail)
                    .resize(120, 90)
                    .placeholder(R.drawable.ic_image)
                    .error(R.drawable.ic_image)
                    .centerCrop().into(holder.imageThumbnail);
        }else {
            holder.txtDateTime.setText(model.DateTime);

            int imageId = ctx.getResources().getIdentifier(model.ImageThumbnail, "drawable", ctx.getPackageName());
            Resources resources = ctx.getResources();
            holder.imageThumbnail.setImageDrawable(resources.getDrawable(imageId));
        }
        // view detail message conversation
        holder.lySkill.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onItemClick(v, position, model);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return filtered_items.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
