package nts.swipesafe.adapter;

import android.annotation.SuppressLint;
import android.content.Context;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import java.sql.Struct;
import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.model.AnswerModel;
import nts.swipesafe.model.ItemPage;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class PaginationAdapter extends RecyclerView.Adapter<PaginationAdapter.ViewHolder> {
    private List<ItemPage> listPage = new ArrayList<>();
    private int pageCurent = 1;
    private int pageSize = 5;
    private int totalPage = 0;
    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(int pageOld, int pageCurent);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout llItemPage, llPageNumber;
        public TextView tvPage;
        public View itemView;

        public ViewHolder(View v) {
            super(v);
            itemView = v;
            llItemPage = (LinearLayout) v.findViewById(R.id.llItemPage);
            llPageNumber = (LinearLayout) v.findViewById(R.id.llPageNumber);
            tvPage = (TextView) v.findViewById(R.id.tvPage);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public PaginationAdapter(Context ctx, int mTotalPage, int mPageCurent) {
        this.ctx = ctx;
        totalPage = mTotalPage;
        pageCurent = mPageCurent;
        if (mTotalPage == 0)
            return;
        ItemPage item;

        if (mTotalPage > pageSize) {
            item = new ItemPage();
            item.page = "<";
            item.visibility = false;
            listPage.add(item);
        }

        for (int i = 1; i <= mTotalPage; i++) {
            item = new ItemPage();
            item.page = String.valueOf(i);
            item.visibility = i <= pageSize ? false : true;
            listPage.add(item);
        }

        if (mTotalPage > pageSize) {
            item = new ItemPage();
            item.page = ">";
            item.visibility = false;
            listPage.add(item);
        }

        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
    }

    @Override
    public PaginationAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_pagination, parent, false);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @SuppressLint("ResourceAsColor")
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final ItemPage itemPage = listPage.get(position);
        holder.tvPage.setText(itemPage.page);
        RecyclerView.LayoutParams param = (RecyclerView.LayoutParams) holder.itemView.getLayoutParams();
        if (itemPage.visibility) {
            holder.itemView.setVisibility(View.GONE);
            param.height = 0;
            param.width = 0;
        } else {
            param.height = LinearLayout.LayoutParams.WRAP_CONTENT;
            param.width = LinearLayout.LayoutParams.WRAP_CONTENT;
            holder.itemView.setVisibility(View.VISIBLE);
        }

        holder.llPageNumber.setBackgroundResource((itemPage.page.equals(String.valueOf(pageCurent)) ? R.drawable.circle_shape_orange : R.drawable.circle_shape));

        holder.itemView.setLayoutParams(param);
        // view detail message conversation
        holder.llPageNumber.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                int pageOld = pageCurent;
                if (itemPage.page.equals("<")) {
                    if (pageCurent > 1) {
                        pageCurent--;
                    }
                } else if (itemPage.page.equals(">")) {
                    if (pageCurent < totalPage) {
                        pageCurent++;
                    }
                } else {
                    if (mOnItemClickListener != null) {
                        pageCurent = Integer.valueOf(itemPage.page);
                    }
                }
                updatePagination();
                mOnItemClickListener.onItemClick(pageOld, pageCurent);
            }
        });
    }

    public void setPageCurent(int mPageCurent) {
        int pageOld = pageCurent;
        pageCurent = mPageCurent;
        updatePagination();
        mOnItemClickListener.onItemClick(pageOld, pageCurent);
    }

    private void updatePagination() {
        if (totalPage > pageSize) {
            int count = 1;
            int indexCurent = pageCurent;
            ItemPage temp = listPage.get(indexCurent);
            temp.visibility = false;
            listPage.set(indexCurent, temp);
            for (int i = indexCurent - 1; i > 0; i--) {
                temp = listPage.get(i);
                if (count < 3 + (indexCurent + 2 > totalPage ? indexCurent + 2 - totalPage : 0)) {
                    temp.visibility = false;
                    count++;
                } else {
                    temp.visibility = true;
                }
                listPage.set(i, temp);
            }

            for (int i = indexCurent + 1; i <= totalPage; i++) {
                temp = listPage.get(i);
                if (count < 5) {
                    temp.visibility = false;
                    count++;
                } else {
                    temp.visibility = true;
                }
                listPage.set(i, temp);
            }
        }
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listPage.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
