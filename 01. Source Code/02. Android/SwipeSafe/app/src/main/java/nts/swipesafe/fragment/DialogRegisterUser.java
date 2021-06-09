package nts.swipesafe.fragment;

import android.app.Activity;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.widget.AppCompatEditText;
import androidx.fragment.app.DialogFragment;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.android.material.textfield.TextInputEditText;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import nts.swipesafe.R;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.RegisterUserModel;
import nts.swipesafe.model.ResultApiModel;
import nts.swipesafe.model.UserLoginModel;

public class DialogRegisterUser extends DialogFragment {
    private View root;
    private static Activity context;
    private Button btnRegisterUser;
    private DialogLoginELearning.CallbackResult callbackResult;
    private TextInputEditText edit_Email, edit_Name;
    private AppCompatEditText edit_Password, edit_ConfirmPassword;

    public interface CallbackResult {
        void sendResult(boolean requestCode);
    }

    public void setOnCallbackResult(final DialogLoginELearning.CallbackResult callbackResult) {
        this.callbackResult = callbackResult;
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        context = getActivity();
        root = inflater.inflate(R.layout.dialog_register_user, container, false);
        initToolbar();
        initcomponent();
        return root;
    }

    private void initToolbar() {
        TextView tvTitle = (TextView) root.findViewById(R.id.tvTitle);
        ImageButton imbClose = (ImageButton) root.findViewById(R.id.imbClose);
        tvTitle.setText("Đăng ký tài khoản");
        imbClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                callbackResult.sendResult(false);
                dismiss();
            }
        });
    }

    private void initcomponent() {
        btnRegisterUser = root.findViewById(R.id.btnRegisterUser);
        edit_Email = root.findViewById(R.id.edit_Email);
        edit_Name = root.findViewById(R.id.edit_Name);

        edit_Password = root.findViewById(R.id.edit_Password);
        edit_ConfirmPassword = root.findViewById(R.id.edit_ConfirmPassword);

        btnRegisterUser.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                submitRegister();
            }
        });
    }

    private void submitRegister() {
        String emailPattern = "[a-zA-Z0-9._-]+@[a-z]+\\.+[a-z]+";
        if (Utils.isEmpty(edit_Email.getText().toString())) {
            Toast.makeText(context, "Vui lòng nhập email đăng ký", Toast.LENGTH_SHORT).show();
            return;
        } else if (!edit_Email.getText().toString().matches(emailPattern)) {
            Toast.makeText(context, "E-mail không đúng định dạng", Toast.LENGTH_SHORT).show();
            return;
        }

        if (Utils.isEmpty(edit_Name.getText().toString())) {
            Toast.makeText(context, "Vui lòng nhập tên người dùng", Toast.LENGTH_SHORT).show();
            return;
        }

        if (Utils.isEmpty(edit_Password.getText().toString())) {
            Toast.makeText(context, "Vui lòng nhập mật khẩu", Toast.LENGTH_SHORT).show();
            return;
        } else if (Utils.isEmpty(edit_ConfirmPassword.getText().toString())) {
            Toast.makeText(context, "Vui lòng nhập xác nhận mật khẩu", Toast.LENGTH_SHORT).show();
            return;
        } else if (!edit_Password.getText().toString().equals(edit_ConfirmPassword.getText().toString())) {
            Toast.makeText(context, "Mật khẩu xác nhận không trùng khớp", Toast.LENGTH_SHORT).show();
            return;
        }


        final RegisterUserModel registerUserModel = new RegisterUserModel();
        registerUserModel.Email = edit_Email.getText().toString();
        registerUserModel.Name = edit_Name.getText().toString();
        registerUserModel.Password = edit_Password.getText().toString();


        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(registerUserModel));
        } catch (JSONException e) {
        }

        try {
            AndroidNetworking.post(Constants.ApiElearning + "api/mobile/user/create")
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
                                    Toast.makeText(context, "Đăng ký thành công", Toast.LENGTH_SHORT).show();
                                    dismiss();
                                } else {
                                    Toast.makeText(context, resultModel.message.get(0), Toast.LENGTH_SHORT).show();
                                }

                            } catch (Exception ex) {
                                Toast.makeText(context, "Đăng ký không thành công", Toast.LENGTH_SHORT).show();
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            Toast.makeText(context, "Đăng ký không thành công", Toast.LENGTH_SHORT).show();
                        }
                    });
        } catch (Exception ex) {
            Toast.makeText(context, "Đăng ký không thành công", Toast.LENGTH_SHORT).show();
        }

    }
}
