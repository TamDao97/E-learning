package nts.swipesafe.fragment;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.cardview.widget.CardView;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONObject;

import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.Constants;
import nts.swipesafe.model.HomeSettingModel;
import nts.swipesafe.model.LessonModel;
import nts.swipesafe.model.ProgramModel;
import nts.swipesafe.model.ResultApiModel;

public class AppInfoFragment extends DialogFragment {
    private View root;
    private Activity thisActivity;
    private TextView txtAddress, txtNumberPhone, txtGmail, txtCopyRight;
    private LinearLayout llGoogle, llYouTube, llFacebook, llWebsite;
    private RelativeLayout viewNoData;

    public static AppInfoFragment newInstance() {
        AppInfoFragment fragment = new AppInfoFragment();
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        root = inflater.inflate(R.layout.fragment_app_info, container, false);
        initComponent();
        // Inflate the layout for this fragment
        return root;
    }

    private void initComponent() {
        thisActivity = getActivity();
        txtAddress = root.findViewById(R.id.txtAddress);
        txtGmail = root.findViewById(R.id.txtGmail);
        txtNumberPhone = root.findViewById(R.id.txtNumberPhone);
        txtCopyRight = root.findViewById(R.id.txtCopyRight);
        llFacebook = root.findViewById(R.id.cvFacebook);
        llGoogle = root.findViewById(R.id.cvGooglePlus);
        llYouTube = root.findViewById(R.id.cvYoutube);
        llWebsite = root.findViewById(R.id.cvWebsite);
        getLesson();
        initToolbar();
    }

    HomeSettingModel homeSettingModel;

    private void initToolbar() {
        TextView tvTitle = (TextView) root.findViewById(R.id.tvTitle);
        ImageButton imbClose = (ImageButton) root.findViewById(R.id.imbClose);
        tvTitle.setText("Thông tin ứng dụng");
        imbClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dismiss();
            }
        });

        llWebsite.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String theurl = homeSettingModel.website;
                Uri urlstr = Uri.parse(theurl);
                Intent urlintent = new Intent();
                urlintent.setData(urlstr);
                urlintent.setAction(Intent.ACTION_VIEW);
                startActivity(urlintent);
            }
        });
        llGoogle.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String theurl = homeSettingModel.linkGoogle;
                Uri urlstr = Uri.parse(theurl);
                Intent urlintent = new Intent();
                urlintent.setData(urlstr);
                urlintent.setAction(Intent.ACTION_VIEW);
                startActivity(urlintent);
            }
        });
        llYouTube.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String theurl = homeSettingModel.linkYouTube;
                Uri urlstr = Uri.parse(theurl);
                Intent urlintent = new Intent();
                urlintent.setData(urlstr);
                urlintent.setAction(Intent.ACTION_VIEW);
                startActivity(urlintent);
            }
        });
        llFacebook.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String theurl = homeSettingModel.linkFacebook;
                Uri urlstr = Uri.parse(theurl);
                Intent urlintent = new Intent();
                urlintent.setData(urlstr);
                urlintent.setAction(Intent.ACTION_VIEW);
                startActivity(urlintent);
            }
        });
    }

    private void getLesson() {
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/home-setting/get-home-setting")
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<HomeSettingModel> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<HomeSettingModel>>() {
                                }.getType());
                                String statusCode = response.getString(Constants.StatusCode);
                                if (statusCode.equals(Constants.Status_Success)) {
                                    homeSettingModel = resultObject.data;
                                    txtAddress.setText(homeSettingModel.address);
                                    txtGmail.setText(homeSettingModel.gmail);
                                    txtCopyRight.setText(homeSettingModel.copyRight);
                                    txtNumberPhone.setText(homeSettingModel.phone);
                                } else {
                                }
                            } catch (Exception ex) {
                            }
                        }

                        @Override
                        public void onError(ANError anError) {

                        }
                    });
        } catch (Exception ex) {
            viewNoData.setVisibility(View.VISIBLE);
        }
    }

}
