package nts.swipesafe.common;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.DrawableRes;
import androidx.appcompat.widget.AppCompatButton;
import androidx.core.graphics.drawable.RoundedBitmapDrawable;
import androidx.core.graphics.drawable.RoundedBitmapDrawableFactory;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentActivity;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.target.BitmapImageViewTarget;
import com.google.android.material.snackbar.Snackbar;

import nts.swipesafe.R;
import nts.swipesafe.fragment.DialogLoginELearning;
import nts.swipesafe.fragment.ProgramInfoFragment;

public class Tools {

    public static void setSystemBarColor(Activity act) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            Window window = act.getWindow();
            window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS);
            window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
            window.setStatusBarColor(act.getResources().getColor(R.color.colorAccentDark));
        }
    }

    public static void snackBarMessage(View parent_view, String content) {
        Snackbar snackbar = Snackbar.make(parent_view, content, Snackbar.LENGTH_LONG)
                .setAction("Xóa", new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                    }
                });
        snackbar.show();
    }

    public static void displayFragment(Activity activity, Fragment fragment, Bundle bundle) {
        FragmentManager fragmentManager = ((FragmentActivity) activity).getSupportFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();

        if (fragment == null) return;
        if (bundle != null) {
            fragment.setArguments(bundle);
        }
        fragmentTransaction.replace(R.id.frame_content, fragment);
        fragmentTransaction.commit();
    }

    public static boolean checkLogin(Activity activity) {
        SharedPreferences sharedPreferences = activity.getSharedPreferences(Constants.KeyInfoLogin, activity.MODE_PRIVATE);
        boolean isLogin = sharedPreferences.getBoolean(Constants.IsLogin, false);
        if (!isLogin) {
            final FragmentManager fragmentManager = ((FragmentActivity) activity).getSupportFragmentManager();
            DialogLoginELearning dialogLoginELearning = new DialogLoginELearning();
            FragmentTransaction transaction = fragmentManager.beginTransaction();
            transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
            transaction.add(R.id.drawer_layout, dialogLoginELearning).addToBackStack(null).commit();
            return false;
        } else {
            return true;
        }
    }

    /***
     * Hiển thị thông báo
     * @param context
     * @param title
     * @param content
     */
    public static void showCustomDialog(Context context, String title, String content) {
        final Dialog dialog = new Dialog(context);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE); // before
        dialog.setContentView(R.layout.dialog_info);
        dialog.setCancelable(true);

        WindowManager.LayoutParams lp = new WindowManager.LayoutParams();
        lp.copyFrom(dialog.getWindow().getAttributes());
        lp.width = WindowManager.LayoutParams.WRAP_CONTENT;
        lp.height = WindowManager.LayoutParams.WRAP_CONTENT;

        TextView tvTitle = dialog.findViewById(R.id.tvTitle);
        tvTitle.setText(title);

        TextView tvContent = dialog.findViewById(R.id.tvContent);
        tvContent.setText(content);


        ((AppCompatButton) dialog.findViewById(R.id.btClose)).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialog.dismiss();
            }
        });

        dialog.show();
        dialog.getWindow().setAttributes(lp);
    }

    public static void displayImageRound(final Context ctx, final ImageView img, String url) {
        try {
            Glide.with(ctx).asBitmap().load(url).centerCrop().into(new BitmapImageViewTarget(img) {
                @Override
                protected void setResource(Bitmap resource) {
                    RoundedBitmapDrawable circularBitmapDrawable = RoundedBitmapDrawableFactory.create(ctx.getResources(), resource);
                    circularBitmapDrawable.setCircular(true);
                    img.setImageDrawable(circularBitmapDrawable);
                }
            });
        } catch (Exception e) {
        }
    }

    /***
     * Hiển thị thông báo
     * @param context
     * @param title
     * @param content
     */
    public static Dialog showCustomDialog(Context context, String title, String content, View.OnClickListener onClickListener) {
        final Dialog dialog = new Dialog(context);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE); // before
        dialog.setContentView(R.layout.dialog_info);
        dialog.setCancelable(true);

        WindowManager.LayoutParams lp = new WindowManager.LayoutParams();
        lp.copyFrom(dialog.getWindow().getAttributes());
        lp.width = WindowManager.LayoutParams.WRAP_CONTENT;
        lp.height = WindowManager.LayoutParams.WRAP_CONTENT;

        TextView tvTitle = dialog.findViewById(R.id.tvTitle);
        tvTitle.setText(title);

        TextView tvContent = dialog.findViewById(R.id.tvContent);
        tvContent.setText(content);


        ((AppCompatButton) dialog.findViewById(R.id.btClose)).setOnClickListener(onClickListener);

        dialog.show();
        dialog.getWindow().setAttributes(lp);
        return dialog;
    }

    /***
     * Hiển thị thông báo
     * @param context
     * @param title
     */
    public static Dialog showConfirm(Context context, String title, View.OnClickListener onClickClose,View.OnClickListener onClickOk, String textNo, String textYes) {
        final Dialog dialog = new Dialog(context);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE); // before
        dialog.setContentView(R.layout.dialog_confirm);
        dialog.setCancelable(true);

        WindowManager.LayoutParams lp = new WindowManager.LayoutParams();
        lp.copyFrom(dialog.getWindow().getAttributes());
        lp.width = WindowManager.LayoutParams.WRAP_CONTENT;
        lp.height = WindowManager.LayoutParams.WRAP_CONTENT;

        TextView tvTitle = dialog.findViewById(R.id.tvTitle);
        tvTitle.setText(title);

        AppCompatButton btClose = dialog.findViewById(R.id.btClose);
        if(!Utils.isEmpty(textNo))
        {
            btClose.setText(textNo);
        }

        AppCompatButton btOk = dialog.findViewById(R.id.btOk);
        if(!Utils.isEmpty(textYes))
        {
            btOk.setText(textYes);
        }

        btClose.setOnClickListener(onClickClose);
        btOk.setOnClickListener(onClickOk);

        dialog.show();
        dialog.getWindow().setAttributes(lp);
        return dialog;
    }
}
