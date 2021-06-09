package nts.swipesafe.fragment;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.widget.AppCompatEditText;
import androidx.cardview.widget.CardView;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentActivity;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.facebook.login.LoginManager;
import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.android.material.textfield.TextInputEditText;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import nts.swipesafe.R;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.Tools;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ResultApiModel;
import nts.swipesafe.model.UserLoginModel;

public class UserFragment extends Fragment {
    private View root;
    private LinearLayout cvLogin;
    private LinearLayout cvUpdateInfo;
    private LinearLayout cvMyCourse;
    private LinearLayout cvAppInfo;
    private LinearLayout cvLogOut;
    private LinearLayout cvChangePassword;
    private static Activity thisActivity;
    private SharedPreferences sharedPreferencesLogin;
    private Dialog dialogChangePassword;
    private String learnerId;

    public static UserFragment newInstance() {
        UserFragment fragment = new UserFragment();
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        root = inflater.inflate(R.layout.fragment_user, container, false);
        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        learnerId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, null);
        initComponent();
        // Inflate the layout for this fragment
        return root;
    }

    private void initComponent() {
        this.thisActivity = getActivity();
        cvLogin = root.findViewById(R.id.cvLogin);
        cvUpdateInfo = root.findViewById(R.id.cvUpdateInfo);
        cvAppInfo = root.findViewById(R.id.cvAppInfo);
        cvMyCourse = root.findViewById(R.id.cvMyCourse);
        cvLogOut = root.findViewById(R.id.cvLogOut);
        cvChangePassword = root.findViewById(R.id.cvChangePassword);

        boolean isLogin = sharedPreferencesLogin.getBoolean(Constants.IsLogin, false);
        if (isLogin) {
            cvLogin.setVisibility(View.GONE);
            cvChangePassword.setVisibility(View.VISIBLE);
            cvLogOut.setVisibility(View.VISIBLE);
            cvMyCourse.setVisibility(View.VISIBLE);
            cvUpdateInfo.setVisibility(View.VISIBLE);
        } else {
            cvLogin.setVisibility(View.VISIBLE);
            cvChangePassword.setVisibility(View.GONE);
            cvLogOut.setVisibility(View.GONE);
            cvMyCourse.setVisibility(View.GONE);
            cvUpdateInfo.setVisibility(View.GONE);
        }

        cvLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                final FragmentManager fragmentManager = ((FragmentActivity) getActivity()).getSupportFragmentManager();
                DialogLoginELearning dialogLoginELearning = new DialogLoginELearning();
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogLoginELearning).addToBackStack(null).commit();
                dialogLoginELearning.setOnCallbackResult(new DialogLoginELearning.CallbackResult() {
                    @Override
                    public void sendResult(boolean requestCode) {
                        boolean isLogin = sharedPreferencesLogin.getBoolean(Constants.IsLogin, false);
                        if (isLogin) {
                            cvLogin.setVisibility(View.GONE);
                            cvChangePassword.setVisibility(View.VISIBLE);
                            cvLogOut.setVisibility(View.VISIBLE);
                            cvMyCourse.setVisibility(View.VISIBLE);
                            cvUpdateInfo.setVisibility(View.VISIBLE);
                        } else {
                            cvLogin.setVisibility(View.VISIBLE);
                            cvChangePassword.setVisibility(View.GONE);
                            cvLogOut.setVisibility(View.GONE);
                            cvMyCourse.setVisibility(View.GONE);
                            cvUpdateInfo.setVisibility(View.GONE);
                        }
                    }
                });
            }
        });
        cvUpdateInfo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                DetailAccountFragment dialogFragment = new DetailAccountFragment();
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }
        });
        cvMyCourse.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                MyCourseFragment dialogFragment = new MyCourseFragment();
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }
        });
        cvAppInfo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                AppInfoFragment dialogFragment = new AppInfoFragment();
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }
        });
        cvLogOut.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                AlertDialog.Builder builder = new AlertDialog.Builder(getActivity(), R.style.MyAlertDialogStyle);
                builder.setCancelable(true);
                builder.setTitle("Xác nhận");
                builder.setMessage("Bạn có chắc chắn muốn đăng xuất?");
                builder.setPositiveButton("Đồng ý",
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                logout();
                            }
                        });
                builder.setNegativeButton("Bỏ qua", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                    }
                });
                AlertDialog dialog = builder.create();
                dialog.show();
            }
        });

        cvChangePassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showDialogChangePassword();
            }
        });

    }

    private void showDialogChangePassword() {
        dialogChangePassword = new Dialog(getContext());
        dialogChangePassword.requestWindowFeature(Window.FEATURE_NO_TITLE); // before
        dialogChangePassword.setContentView(R.layout.dialog_change_password);
        dialogChangePassword.setCancelable(false);
        WindowManager.LayoutParams lp = new WindowManager.LayoutParams();
        lp.copyFrom(dialogChangePassword.getWindow().getAttributes());
        lp.width = WindowManager.LayoutParams.MATCH_PARENT;
        lp.height = WindowManager.LayoutParams.WRAP_CONTENT;

        TextView tv_Close = dialogChangePassword.findViewById(R.id.tv_Close);
        tv_Close.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialogChangePassword.dismiss();
            }
        });

        final AppCompatEditText edit_OldPassword = dialogChangePassword.findViewById(R.id.edit_OldPassword);
        final AppCompatEditText edit_NewPassword = dialogChangePassword.findViewById(R.id.edit_NewPassword);
        final AppCompatEditText edit_ConfirmNewPassword = dialogChangePassword.findViewById(R.id.edit_ConfirmNewPassword);

        Button btnSignIn = dialogChangePassword.findViewById(R.id.btnSignIn);
        btnSignIn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (Utils.isEmpty(edit_OldPassword.getText().toString())) {
                    Toast.makeText(thisActivity, "Vui lòng nhập mật khẩu cũ", Toast.LENGTH_SHORT).show();
                    return;
                }
                if (Utils.isEmpty(edit_NewPassword.getText().toString())) {
                    Toast.makeText(thisActivity, "Vui lòng nhập mật khẩu mới", Toast.LENGTH_SHORT).show();
                    return;
                }
                if (Utils.isEmpty(edit_ConfirmNewPassword.getText().toString())) {
                    Toast.makeText(thisActivity, "Vui lòng nhập xác nhận mật khẩu mới", Toast.LENGTH_SHORT).show();
                    return;
                }
                if (!edit_NewPassword.getText().toString().equals(edit_ConfirmNewPassword.getText().toString())) {
                    Toast.makeText(thisActivity, "Xác nhận mật khẩu mới không trùng khớp", Toast.LENGTH_SHORT).show();
                    return;
                }
                ChangePasswordModel changePasswordModel = new ChangePasswordModel();
                changePasswordModel.PasswordNew = edit_NewPassword.getText().toString();
                changePasswordModel.PasswordOld = edit_OldPassword.getText().toString();
                JSONObject jsonModel = new JSONObject();
                try {
                    jsonModel = new JSONObject(new Gson().toJson(changePasswordModel));
                } catch (JSONException e) {
                }

                try {
                    AndroidNetworking.post(Constants.ApiElearning + "api/mobile/user/change-pass/" + learnerId)
                            .addJSONObjectBody(jsonModel)
                            .setPriority(Priority.MEDIUM)
                            .build()
                            .getAsJSONObject(new JSONObjectRequestListener() {
                                @Override
                                public void onResponse(JSONObject response) {
                                    try {
                                        ResultApiModel<String> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<String>>() {
                                        }.getType());
                                        String statusCode = response.getString("statusCode");
                                        if (statusCode.equals(Constants.Status_Success)) {
                                            Toast.makeText(thisActivity, "Đổi mật khẩu thành công", Toast.LENGTH_SHORT).show();
                                            dialogChangePassword.dismiss();
                                        } else {
                                            Toast.makeText(thisActivity, resultModel.message.get(0), Toast.LENGTH_SHORT).show();
                                        }

                                    } catch (Exception ex) {
                                        Toast.makeText(thisActivity, "Đổi mật khẩu không thành công", Toast.LENGTH_SHORT).show();
                                    }
                                }

                                @Override
                                public void onError(ANError anError) {
                                    Toast.makeText(thisActivity, "Đổi mật khẩu không thành công", Toast.LENGTH_SHORT).show();
                                }
                            });
                } catch (Exception ex) {
                    Toast.makeText(thisActivity, "Đổi mật khẩu không thành công", Toast.LENGTH_SHORT).show();
                }

            }
        });

        dialogChangePassword.show();
        dialogChangePassword.getWindow().setAttributes(lp);
    }

    private void logout() {
        try {
            SharedPreferences prefsLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
            String typeLogin = prefsLogin.getString(Constants.KeyTypeLogin, "");
            if (typeLogin.equals(Constants.TypeLoginGoogle)) {
                GoogleSignInAccount acct = GoogleSignIn.getLastSignedInAccount(getActivity());
                if (acct != null) {
                    GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                            .requestEmail()
                            .requestIdToken("565371125610-6dfm6gccu83aevmi3tpn4ko1ob6rss8n.apps.googleusercontent.com")
                            .requestServerAuthCode("565371125610-6dfm6gccu83aevmi3tpn4ko1ob6rss8n.apps.googleusercontent.com", false)
                            .build();
                    GoogleSignInClient mGoogleSignInClient = GoogleSignIn.getClient(getActivity(), gso);
                    mGoogleSignInClient.signOut()
                            .addOnCompleteListener(getActivity(), new OnCompleteListener<Void>() {
                                @Override
                                public void onComplete(@NonNull Task<Void> task) {
                                }
                            });
                }
            } else if (typeLogin.equals(Constants.TypeLoginFacebook)) {
                LoginManager.getInstance().logOut();
            } else {
                AndroidNetworking.post(Constants.ApiElearning + "api/mobile/user/logout/" + learnerId)
                        .setPriority(Priority.MEDIUM)
                        .build()
                        .getAsJSONObject(new JSONObjectRequestListener() {
                            @Override
                            public void onResponse(JSONObject response) {
                            }

                            @Override
                            public void onError(ANError anError) {
                            }
                        });

            }

            SharedPreferences.Editor editor = prefsLogin.edit();
            editor.clear().commit();
            editor.apply();

            cvLogin.setVisibility(View.VISIBLE);
            cvChangePassword.setVisibility(View.GONE);
            cvLogOut.setVisibility(View.GONE);
            cvMyCourse.setVisibility(View.GONE);
            cvUpdateInfo.setVisibility(View.GONE);

            Toast.makeText(getActivity(), "Đăng xuất thành công", Toast.LENGTH_SHORT).show();
        } catch (Exception ex) {
            Toast.makeText(getActivity(), "Đăng xuất không thành công", Toast.LENGTH_SHORT).show();
        }
    }


}
