package nts.swipesafe.fragment;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.Signature;
import android.os.Bundle;
import android.util.Base64;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.AppCompatEditText;
import androidx.appcompat.widget.Toolbar;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.FragmentActivity;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.facebook.CallbackManager;
import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.login.LoginManager;
import com.facebook.login.LoginResult;
import com.facebook.login.widget.LoginButton;
import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.common.api.ApiException;
import com.google.android.gms.tasks.Task;
import com.google.android.material.textfield.TextInputEditText;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Arrays;
import java.util.Date;

import nts.swipesafe.ActivityMainELearning;
import nts.swipesafe.R;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.Tools;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.FacebookModel;
import nts.swipesafe.model.GoogleModel;
import nts.swipesafe.model.LoginModel;
import nts.swipesafe.model.ResultApiModel;
import nts.swipesafe.model.ResultModel;
import nts.swipesafe.model.UserLoginModel;
import okhttp3.OkHttpClient;

public class DialogLoginELearning extends DialogFragment {
    private View root;
    private static Activity context;
    private Button btnSignInFacebook, btnSignInGoogle, btnSignIn;
    private LoginButton loginButtonFBDefaul;
    CallbackManager callbackManager;
    private GoogleSignInClient mGoogleSignInClient;
    private int RC_SIGN_IN = 0;
    private TextView tv_ForgotPassword;
    private LinearLayout layout_Registration;
    private Dialog dialogForgotPassword;
    private ProgressBar progressBarGmail, progressBarGoogle, progressBarFb;

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        context = getActivity();
        root = inflater.inflate(R.layout.activity_login_elearning, container, false);
        GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestEmail()
                .requestIdToken("565371125610-6dfm6gccu83aevmi3tpn4ko1ob6rss8n.apps.googleusercontent.com")
                .requestServerAuthCode("565371125610-6dfm6gccu83aevmi3tpn4ko1ob6rss8n.apps.googleusercontent.com", false)
                .build();
        mGoogleSignInClient = GoogleSignIn.getClient(context, gso);
        initToolbar();
        initcomponent();
        return root;
    }

    private void initToolbar() {
        TextView tvTitle = (TextView) root.findViewById(R.id.tvTitle);
        ImageButton imbClose = (ImageButton) root.findViewById(R.id.imbClose);
        tvTitle.setText("Đăng nhập");
        imbClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                callbackResult.sendResult(false);
                dismiss();
            }
        });
    }

    private void initcomponent() {
        progressBarFb = root.findViewById(R.id.progressBarFb);
        btnSignInFacebook = root.findViewById(R.id.btnSignInFacebook);
        btnSignInFacebook.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                progressBarFb.setVisibility(View.VISIBLE);
                btnSignInFacebook.setVisibility(View.GONE);
                loginButtonFBDefaul.performClick();
            }
        });
        progressBarGmail = root.findViewById(R.id.progressBarGmail);
        progressBarGoogle = root.findViewById(R.id.progressBarGoogle);

        loginButtonFBDefaul = root.findViewById(R.id.login_button_FB_default);
        callbackManager = CallbackManager.Factory.create();
        loginButtonFBDefaul.setReadPermissions(Arrays.asList("email"));
        loginButtonFBDefaul.setFragment(this);
        loginButtonFBDefaul.registerCallback(callbackManager, new FacebookCallback<LoginResult>() {
            @Override
            public void onSuccess(LoginResult loginResult) {
                String b = loginResult.getAccessToken().toString();
                FacebookModel facebookModel = new FacebookModel();
                facebookModel.Id_Token = loginResult.getAccessToken().getToken();
                JSONObject jsonModel = new JSONObject();
                try {
                    jsonModel = new JSONObject(new Gson().toJson(facebookModel));
                } catch (JSONException e) {
                    // MessageUtils.Show(getActivity(), e.getMessage());
                }
                AndroidNetworking.post(Constants.ApiElearning + "api/mobile/user/login-facebook")
                        .addJSONObjectBody(jsonModel)
                        .setPriority(Priority.MEDIUM)
                        .build()
                        .getAsJSONObject(new JSONObjectRequestListener() {
                            @Override
                            public void onResponse(JSONObject response) {
                                try {
                                    String statusCode = response.getString(Constants.Status_Success);
                                    if (statusCode.equals(Constants.Status_Success)) {
                                        ResultApiModel<UserLoginModel> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<UserLoginModel>>() {
                                        }.getType());
                                        SharedPreferences.Editor editor = context.getSharedPreferences(Constants.KeyInfoLogin, context.MODE_PRIVATE).edit();
                                        editor.putString(Constants.KeyInfoLoginId, resultModel.data.id);
                                        editor.putString(Constants.KeyInfoLoginName, resultModel.data.name);
                                        editor.putString(Constants.KeyInfoLoginPhoneNumber, resultModel.data.phoneNumber);
                                        editor.putString(Constants.KeyInfoLoginDateOfBirthday, resultModel.data.dateOfBirthday);
                                        editor.putString(Constants.KeyInfoLoginProvinceId, resultModel.data.provinceId);
                                        editor.putString(Constants.KeyInfoLoginDistrictId, resultModel.data.districtId);
                                        editor.putString(Constants.KeyInfoLoginProvinceName, resultModel.data.provinceName);
                                        editor.putString(Constants.KeyInfoLoginDistrictName, resultModel.data.districtName);
                                        editor.putString(Constants.KeyInfoLoginWardId, resultModel.data.wardId);
                                        editor.putString(Constants.KeyInfoLoginWardName, resultModel.data.wardName);
                                        editor.putString(Constants.KeyInfoLoginNationId, resultModel.data.nationId);
                                        editor.putString(Constants.KeyInfoLoginNationName, resultModel.data.nationName);
                                        editor.putString(Constants.KeyInfoLoginPicture, resultModel.data.picture);
                                        editor.putString(Constants.KeyInfoLoginAvatar, resultModel.data.avatar);
                                        editor.putBoolean(Constants.KeyInfoLoginGender, resultModel.data.gender);
                                        editor.putBoolean(Constants.KeyInfoLoginIsDisable, resultModel.data.isDisable);
                                        editor.putString(Constants.KeyInfoLoginEmail, resultModel.data.email);
                                        editor.putString(Constants.KeyInfoLoginIdToken, resultModel.data.idToken);
                                        editor.putString(Constants.KeyInfoLoginAccess_token, resultModel.data.access_token);
                                        editor.putString(Constants.KeyInfoLoginProvider, resultModel.data.provider);
                                        editor.putString(Constants.KeyInfoLoginAddress, resultModel.data.address);
                                        editor.putBoolean(Constants.IsLogin, true);
                                        editor.putString(Constants.KeyTypeLogin, Constants.TypeLoginFacebook);
                                        editor.apply();
                                        dismiss();
                                        callbackResult.sendResult(true);
                                        Toast.makeText(context, "Đăng nhập thành công", Toast.LENGTH_SHORT).show();
                                        progressBarFb.setVisibility(View.GONE);
                                        btnSignInFacebook.setVisibility(View.VISIBLE);
                                    } else {
                                        Toast.makeText(context, response.getString("message"), Toast.LENGTH_SHORT).show();
                                        progressBarFb.setVisibility(View.GONE);
                                        btnSignInFacebook.setVisibility(View.VISIBLE);
                                    }
                                } catch (Exception ex) {
                                    Toast.makeText(context, "Đăng nhập không thành công", Toast.LENGTH_SHORT).show();
                                    progressBarFb.setVisibility(View.GONE);
                                    btnSignInFacebook.setVisibility(View.VISIBLE);
                                }
                            }

                            @Override
                            public void onError(ANError anError) {
                                Toast.makeText(context, "Đăng nhập không thành công", Toast.LENGTH_SHORT).show();
                                progressBarFb.setVisibility(View.GONE);
                                btnSignInFacebook.setVisibility(View.VISIBLE);
                            }
                        });
            }

            @Override
            public void onCancel() {
                String a = "";
            }

            @Override
            public void onError(FacebookException error) {
                String a = "";
            }
        });

        btnSignInGoogle = root.findViewById(R.id.btnSignInGoogle);
        btnSignInGoogle.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                signInGoogle();
            }
        });

        btnSignIn = root.findViewById(R.id.btnSignIn);
        btnSignIn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                signInAccount();
            }
        });

        tv_ForgotPassword = root.findViewById(R.id.tv_ForgotPassword);
        layout_Registration = root.findViewById(R.id.layout_Registration);
        layout_Registration.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final FragmentManager fragmentManager = ((FragmentActivity) getActivity()).getSupportFragmentManager();
                DialogRegisterUser dialogRegisterUser = new DialogRegisterUser();
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogRegisterUser).addToBackStack(null).commit();
                dialogRegisterUser.setOnCallbackResult(new DialogLoginELearning.CallbackResult() {
                    @Override
                    public void sendResult(boolean requestCode) {

                    }
                });
            }
        });

        tv_ForgotPassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showDialogForgotPassword();
            }
        });
    }

    private void showDialogForgotPassword() {
        dialogForgotPassword = new Dialog(getContext());
        dialogForgotPassword.requestWindowFeature(Window.FEATURE_NO_TITLE); // before
        dialogForgotPassword.setContentView(R.layout.dialog_forgot_password);
        dialogForgotPassword.setCancelable(false);
        WindowManager.LayoutParams lp = new WindowManager.LayoutParams();
        lp.copyFrom(dialogForgotPassword.getWindow().getAttributes());
        lp.width = WindowManager.LayoutParams.MATCH_PARENT;
        lp.height = WindowManager.LayoutParams.WRAP_CONTENT;

        TextView tv_Close = dialogForgotPassword.findViewById(R.id.tv_Close);
        tv_Close.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialogForgotPassword.dismiss();
            }
        });

        final TextInputEditText edit_EmialForgotPassword = dialogForgotPassword.findViewById(R.id.edit_EmialForgotPassword);
        Button btnForgotPassword = dialogForgotPassword.findViewById(R.id.btnForgotPassword);
        btnForgotPassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String emailPattern = "[a-zA-Z0-9._-]+@[a-z]+\\.+[a-z]+";
                if (Utils.isEmpty(edit_EmialForgotPassword.getText().toString())) {
                    Toast.makeText(context, "Vui lòng nhập địa chỉ e-mail", Toast.LENGTH_SHORT).show();
                    return;
                } else if (!edit_EmialForgotPassword.getText().toString().matches(emailPattern)) {
                    Toast.makeText(context, "E-mail không đúng định dạng", Toast.LENGTH_SHORT).show();
                    return;
                }

                try {
                    AndroidNetworking.post(Constants.ApiElearning + "api/mobile/user/forgot-pass/" + edit_EmialForgotPassword.getText().toString())
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
                                            Toast.makeText(context, "Mật khẩu mới đã được gửi đến e-mail", Toast.LENGTH_SHORT).show();
                                            dialogForgotPassword.dismiss();
                                        } else {
                                            Toast.makeText(context, resultModel.message.get(0), Toast.LENGTH_SHORT).show();
                                        }

                                    } catch (Exception ex) {
                                        Toast.makeText(context, "Đã có lỗi phát sinh trong quá trình xử lý", Toast.LENGTH_SHORT).show();
                                    }
                                }

                                @Override
                                public void onError(ANError anError) {
                                    Toast.makeText(context, "Đã có lỗi phát sinh trong quá trình xử lý", Toast.LENGTH_SHORT).show();
                                }
                            });
                } catch (Exception ex) {
                    Toast.makeText(context, "Đã có lỗi phát sinh trong quá trình xử lý", Toast.LENGTH_SHORT).show();
                }
            }
        });

        dialogForgotPassword.show();
        dialogForgotPassword.getWindow().setAttributes(lp);
    }

    private void signInAccount() {
        AppCompatEditText edit_Username = root.findViewById(R.id.edit_Username);
        AppCompatEditText edit_Password = root.findViewById(R.id.edit_Password);
        if (Utils.isEmpty(edit_Username.getText().toString())) {
            Toast.makeText(context, "Bạn chưa nhập tài khoản", Toast.LENGTH_SHORT).show();
            return;
        }
        if (Utils.isEmpty(edit_Password.getText().toString())) {
            Toast.makeText(context, "Bạn chưa nhập mật khẩu", Toast.LENGTH_SHORT).show();
            return;
        }

        LoginModel loginModel = new LoginModel();
        loginModel.Email = edit_Username.getText().toString();
        loginModel.Password = edit_Password.getText().toString();
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(loginModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }

        progressBarGmail.setVisibility(View.VISIBLE);
        btnSignIn.setVisibility(View.GONE);
        AndroidNetworking.post(Constants.ApiElearning + "api/mobile/user/login-email")
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultApiModel<UserLoginModel> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<UserLoginModel>>() {
                            }.getType());
                            String statusCode = response.getString("statusCode");
                            if (statusCode.equals(Constants.Status_Success)) {
                                SharedPreferences.Editor editor = context.getSharedPreferences(Constants.KeyInfoLogin, context.MODE_PRIVATE).edit();
                                editor.putString(Constants.KeyInfoLoginId, resultModel.data.id);
                                editor.putString(Constants.KeyInfoLoginName, resultModel.data.name);
                                editor.putString(Constants.KeyInfoLoginPhoneNumber, resultModel.data.phoneNumber);
                                editor.putString(Constants.KeyInfoLoginDateOfBirthday, resultModel.data.dateOfBirthday);
                                editor.putString(Constants.KeyInfoLoginProvinceId, resultModel.data.provinceId);
                                editor.putString(Constants.KeyInfoLoginDistrictId, resultModel.data.districtId);
                                editor.putString(Constants.KeyInfoLoginProvinceName, resultModel.data.provinceName);
                                editor.putString(Constants.KeyInfoLoginDistrictName, resultModel.data.districtName);
                                editor.putString(Constants.KeyInfoLoginWardId, resultModel.data.wardId);
                                editor.putString(Constants.KeyInfoLoginWardName, resultModel.data.wardName);
                                editor.putString(Constants.KeyInfoLoginNationId, resultModel.data.nationId);
                                editor.putString(Constants.KeyInfoLoginNationName, resultModel.data.nationName);
                                editor.putString(Constants.KeyInfoLoginPicture, resultModel.data.picture);
                                editor.putString(Constants.KeyInfoLoginAvatar, resultModel.data.avatar);
                                editor.putBoolean(Constants.KeyInfoLoginGender, resultModel.data.gender);
                                editor.putBoolean(Constants.KeyInfoLoginIsDisable, resultModel.data.isDisable);
                                editor.putString(Constants.KeyInfoLoginEmail, resultModel.data.email);
                                editor.putString(Constants.KeyInfoLoginIdToken, resultModel.data.idToken);
                                editor.putString(Constants.KeyInfoLoginAccess_token, resultModel.data.access_token);
                                editor.putString(Constants.KeyInfoLoginProvider, resultModel.data.provider);
                                editor.putString(Constants.KeyInfoLoginAddress, resultModel.data.address);
                                editor.putBoolean(Constants.IsLogin, true);
                                editor.putString(Constants.KeyTypeLogin, Constants.TypeLoginByApp);
                                editor.apply();
                                dismiss();
                                callbackResult.sendResult(true);
                                Toast.makeText(context, "Đăng nhập thành công", Toast.LENGTH_SHORT).show();
                            } else {
                                Toast.makeText(context, resultModel.message.get(0), Toast.LENGTH_SHORT).show();
                            }

                            progressBarGmail.setVisibility(View.GONE);
                            btnSignIn.setVisibility(View.VISIBLE);

                        } catch (Exception ex) {
                            Toast.makeText(context, "Đăng nhập không thành công", Toast.LENGTH_SHORT).show();
                            progressBarGmail.setVisibility(View.GONE);
                            btnSignIn.setVisibility(View.VISIBLE);
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Toast.makeText(context, "Đăng nhập không thành công", Toast.LENGTH_SHORT).show();
                        progressBarGmail.setVisibility(View.GONE);
                        btnSignIn.setVisibility(View.VISIBLE);
                    }
                });

    }

    private void signInGoogle() {
        progressBarGoogle.setVisibility(View.VISIBLE);
        btnSignInGoogle.setVisibility(View.GONE);
        Intent signInIntent = mGoogleSignInClient.getSignInIntent();
        startActivityForResult(signInIntent, RC_SIGN_IN);
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        // Result returned from launching the Intent from GoogleSignInClient.getSignInIntent(...);
        if (requestCode == RC_SIGN_IN) {
            // The Task returned from this call is always completed, no need to attach
            // a listener.
            Task<GoogleSignInAccount> task = GoogleSignIn.getSignedInAccountFromIntent(data);
            handleSignInResult(task);
        }
        callbackManager.onActivityResult(requestCode, resultCode, data);
        super.onActivityResult(requestCode, resultCode, data);
    }

    private void handleSignInResult(Task<GoogleSignInAccount> completedTask) {
        try {
            GoogleModel googleModel = new GoogleModel();
            GoogleSignInAccount account = completedTask.getResult(ApiException.class);
            googleModel.Id = account.getId();
            googleModel.Id_Token = account.getIdToken();

            JSONObject jsonModel = new JSONObject();
            try {
                jsonModel = new JSONObject(new Gson().toJson(googleModel));
            } catch (JSONException e) {
                // MessageUtils.Show(getActivity(), e.getMessage());
            }
            // Signed in successfully, show authenticated UI.
            AndroidNetworking.post(Constants.ApiElearning + "api/mobile/user/login-google")
                    .addJSONObjectBody(jsonModel)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                String statusCode = response.getString("statusCode");
                                ResultApiModel<UserLoginModel> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<UserLoginModel>>() {
                                }.getType());
                                if (statusCode.equals(Constants.Status_Success)) {
                                    SharedPreferences.Editor editor = context.getSharedPreferences(Constants.KeyInfoLogin, context.MODE_PRIVATE).edit();
                                    editor.putString(Constants.KeyInfoLoginId, resultModel.data.id);
                                    editor.putString(Constants.KeyInfoLoginName, resultModel.data.name);
                                    editor.putString(Constants.KeyInfoLoginPhoneNumber, resultModel.data.phoneNumber);
                                    editor.putString(Constants.KeyInfoLoginDateOfBirthday, resultModel.data.dateOfBirthday);
                                    editor.putString(Constants.KeyInfoLoginProvinceId, resultModel.data.provinceId);
                                    editor.putString(Constants.KeyInfoLoginDistrictId, resultModel.data.districtId);
                                    editor.putString(Constants.KeyInfoLoginProvinceName, resultModel.data.provinceName);
                                    editor.putString(Constants.KeyInfoLoginDistrictName, resultModel.data.districtName);
                                    editor.putString(Constants.KeyInfoLoginWardId, resultModel.data.wardId);
                                    editor.putString(Constants.KeyInfoLoginWardName, resultModel.data.wardName);
                                    editor.putString(Constants.KeyInfoLoginNationId, resultModel.data.nationId);
                                    editor.putString(Constants.KeyInfoLoginNationName, resultModel.data.nationName);
                                    editor.putString(Constants.KeyInfoLoginPicture, resultModel.data.picture);
                                    editor.putString(Constants.KeyInfoLoginAvatar, resultModel.data.avatar);
                                    editor.putBoolean(Constants.KeyInfoLoginGender, resultModel.data.gender);
                                    editor.putBoolean(Constants.KeyInfoLoginIsDisable, resultModel.data.isDisable);
                                    editor.putString(Constants.KeyInfoLoginEmail, resultModel.data.email);
                                    editor.putString(Constants.KeyInfoLoginIdToken, resultModel.data.idToken);
                                    editor.putString(Constants.KeyInfoLoginAccess_token, resultModel.data.access_token);
                                    editor.putString(Constants.KeyInfoLoginProvider, resultModel.data.provider);
                                    editor.putString(Constants.KeyInfoLoginAddress, resultModel.data.address);
                                    editor.putBoolean(Constants.IsLogin, true);
                                    editor.putString(Constants.KeyTypeLogin, Constants.TypeLoginGoogle);
                                    editor.apply();
                                    dismiss();
                                    callbackResult.sendResult(true);
                                    Toast.makeText(context, "Đăng nhập thành công", Toast.LENGTH_SHORT).show();
                                    progressBarGoogle.setVisibility(View.GONE);
                                    btnSignInGoogle.setVisibility(View.VISIBLE);
                                } else {
                                    Toast.makeText(context, resultModel.message.get(0), Toast.LENGTH_SHORT).show();
                                    progressBarGoogle.setVisibility(View.GONE);
                                    btnSignInGoogle.setVisibility(View.VISIBLE);
                                }

                            } catch (Exception ex) {
                                Toast.makeText(context, "Đăng nhập không thành công", Toast.LENGTH_SHORT).show();
                                progressBarGoogle.setVisibility(View.GONE);
                                btnSignInGoogle.setVisibility(View.VISIBLE);
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            Toast.makeText(context, "Đăng nhập không thành công", Toast.LENGTH_SHORT).show();
                            progressBarGoogle.setVisibility(View.GONE);
                            btnSignInGoogle.setVisibility(View.VISIBLE);
                        }
                    });
        } catch (ApiException e) {
            // The ApiException status code indicates the detailed failure reason.
            // Please refer to the GoogleSignInStatusCodes class reference for more information.
            Log.w("Login GG failed", "signInResult:failed code=" + e.getStatusCode());
            progressBarGoogle.setVisibility(View.GONE);
            btnSignInGoogle.setVisibility(View.VISIBLE);
        }
    }

    private CallbackResult callbackResult;

    public interface CallbackResult {
        void sendResult(boolean requestCode);
    }

    public void setOnCallbackResult(final DialogLoginELearning.CallbackResult callbackResult) {
        this.callbackResult = callbackResult;
    }
}
