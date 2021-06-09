package nts.swipesafe.fragment;

import android.annotation.SuppressLint;
import android.app.Activity;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.ActivityInfo;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.TextView;
import android.widget.Toast;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.ANRequest;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.androidnetworking.widget.ANImageView;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.squareup.picasso.Picasso;
import com.zhihu.matisse.Matisse;
import com.zhihu.matisse.MimeType;
import com.zhihu.matisse.engine.impl.GlideEngine;
import com.zhihu.matisse.filter.Filter;
import com.zhihu.matisse.internal.entity.CaptureStrategy;
import com.zhihu.matisse.listener.OnCheckedListener;
import com.zhihu.matisse.listener.OnSelectedListener;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.BuildConfig;
import nts.swipesafe.R;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.GifSizeFilter;
import nts.swipesafe.common.Tools;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ComboboxResult;
import nts.swipesafe.model.ComboboxResultData;
import nts.swipesafe.model.ReportInfoModel;
import nts.swipesafe.model.ResultApiModel;
import nts.swipesafe.model.ResultModel;
import nts.swipesafe.model.UploadFileResultELearningModel;
import nts.swipesafe.model.UserLoginModel;
import nts.swipesafe.model.UserUpdateModel;

public class DetailAccountFragment extends DialogFragment {

    // Khởi tạo giá trị
    private ImageView imageView;
    private EditText txtNgaysinh, txtSodienthoai, txtEmail, txtProvinceId, txtDistricId, txtWardId, txtNationId, txtAddress, editName;
    private Button btnUpdate;
    private UserLoginModel userInfoModel;
    private RadioButton radioGenderNam, radioGenderNu;
    public Activity context;
    private AutoCompleteTextView edProvince, edDistric, edWard, edNation;
    private List<ComboboxResult> listProvince, listDistric, listWrad, listNation;
    private String[] arrayProvinceName, arrayDistricName, arrayWardName, arrayNavitionName;
    private View root;
    private SharedPreferences sharedPreferencesLogin;
    private String learnerId;
    private static final int REQUEST_CODE_CHOOSE = 23;
    private String imageLink;

    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        root = inflater.inflate(R.layout.activity_detail_account, container, false);
        context = getActivity();
        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        learnerId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, null);
        initToolbar();
        initComponent();
        getListProvince();
        getNavition();
        getInfoUser();
        return root;
    }

    private void initToolbar() {
        TextView tvTitle = (TextView) root.findViewById(R.id.tvTitle);
        ImageButton imbClose = (ImageButton) root.findViewById(R.id.imbClose);
        tvTitle.setText("Cập nhật thông tin người dùng");
        imbClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dismiss();
            }
        });
    }

    /***
     * Khởi tạo components
     */
    private void initComponent() {
        txtAddress = root.findViewById(R.id.address);
        imageView = root.findViewById(R.id.avatar);
        edProvince = root.findViewById(R.id.province);
        edProvince.setSelectAllOnFocus(true);
        edDistric = root.findViewById(R.id.district);
        edWard = root.findViewById(R.id.wrap);
        edNation = root.findViewById(R.id.dantoc);
        editName = root.findViewById(R.id.editName);
        txtEmail = root.findViewById(R.id.email);
        txtNationId = root.findViewById(R.id.dantoc);
        txtProvinceId = root.findViewById(R.id.province);
        txtDistricId = root.findViewById(R.id.district);
        txtWardId = root.findViewById(R.id.wrap);
        txtNgaysinh = root.findViewById(R.id.ngaysinh);
        txtSodienthoai = root.findViewById(R.id.PhoneNumber);
        radioGenderNam = root.findViewById(R.id.checkboxNam);
        radioGenderNu = root.findViewById(R.id.checkboxNu);
        btnUpdate = root.findViewById(R.id.btnUpdateInfo);
        btnUpdate.setText("Lưu");

        btnUpdate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                getInfoModel();
            }
        });

        // Show popup ngày tháng
        txtNgaysinh.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DateUtils.dialogDatePicker(getContext(), txtNgaysinh);
            }
        });

        // Hiển thị danh sách tỉnh
        edProvince.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                int index = Utils.getIndexByName(arrayProvinceName, adapterView.getItemAtPosition(i).toString());
                getDistrictByProvinceId(listProvince.get(index).id);
                edProvince.setTag(listProvince.get(index).id);
                edDistric.setTag("");
                edDistric.setText("");
                edWard.setTag("");
                edWard.setText("");
                txtAddress.setText("");
            }
        });

        edProvince.setOnTouchListener(new View.OnTouchListener() {

            @SuppressLint("ClickableViewAccessibility")
            @Override
            public boolean onTouch(View paramView, MotionEvent paramMotionEvent) {
                if (arrayProvinceName != null && arrayProvinceName.length > 0) {
                    // show all suggestions
                    if (edProvince.getText().toString().equals(""))
                        edProvince.showDropDown();
                }
                return false;
            }
        });

        // Danh sách huyện
        edDistric.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                int index = Utils.getIndexByName(arrayDistricName, adapterView.getItemAtPosition(i).toString());
                getWardByDistricId(listDistric.get(index).id);
                edDistric.setTag(listDistric.get(index).id);
                edWard.setTag("");
                edWard.setText("");
                txtAddress.setText("");

            }
        });

        edDistric.setOnTouchListener(new View.OnTouchListener() {

            @SuppressLint("ClickableViewAccessibility")
            @Override
            public boolean onTouch(View paramView, MotionEvent paramMotionEvent) {
                if (arrayDistricName != null && arrayDistricName.length > 0) {
                    // show all suggestions
                    if (edDistric.getText().toString().equals(""))
                        edDistric.showDropDown();
                }
                return false;
            }
        });

        // hiển thị danh sách xã
        edWard.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                int index = Utils.getIndexByName(arrayWardName, adapterView.getItemAtPosition(i).toString());
                edWard.setTag(listWrad.get(index).id);
                txtAddress.setText("");

            }
        });

        edWard.setOnTouchListener(new View.OnTouchListener() {

            @SuppressLint("ClickableViewAccessibility")
            @Override
            public boolean onTouch(View paramView, MotionEvent paramMotionEvent) {
                if (arrayWardName != null && arrayWardName.length > 0) {
                    // show all suggestions
                    if (edWard.getText().toString().equals(""))
                        edWard.showDropDown();
                }
                return false;
            }
        });

        edNation.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                int index = Utils.getIndexByName(arrayNavitionName, adapterView.getItemAtPosition(i).toString());
                edNation.setTag(listNation.get(index).id);
                txtAddress.setText("");

            }
        });

        // hiển thị danh sách dân tộc
        edNation.setOnTouchListener(new View.OnTouchListener() {

            @SuppressLint("ClickableViewAccessibility")
            @Override
            public boolean onTouch(View paramView, MotionEvent paramMotionEvent) {
                if (arrayNavitionName != null && arrayNavitionName.length > 0) {
                    // show all suggestions
                    if (edNation.getText().toString().equals(""))
                        edNation.showDropDown();
                }
                return false;
            }
        });

        imageView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Matisse.from(context)
                        .choose(MimeType.ofImage(), false)
                        .countable(true)
                        .capture(true)
                        .captureStrategy(
                                new CaptureStrategy(true, BuildConfig.APPLICATION_ID + ".nts.swipesafe.provider", "elearning"))
                        .maxSelectable(1)
                        .addFilter(new GifSizeFilter(320, 320, 5 * Filter.K * Filter.K))
                        .gridExpectedSize(
                                getResources().getDimensionPixelSize(R.dimen.grid_expected_size))
                        .restrictOrientation(ActivityInfo.SCREEN_ORIENTATION_PORTRAIT)
                        .thumbnailScale(0.85f)
                        .imageEngine(new GlideEngine())
                        .setOnSelectedListener(new OnSelectedListener() {
                            @Override
                            public void onSelected(@NonNull List<Uri> uriList, @NonNull List<String> pathList) {
                            }
                        })
                        .showSingleMediaType(true)
                        .originalEnable(false)
                        .maxOriginalSize(10)
                        .autoHideToolbarOnSingleTap(false)
                        .setOnCheckedListener(new OnCheckedListener() {
                            @Override
                            public void onCheck(boolean isChecked) {
                            }
                        })
                        .forResult(REQUEST_CODE_CHOOSE);
            }
        });

    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == REQUEST_CODE_CHOOSE && resultCode == context.RESULT_OK) {
            List<Uri> uris = new ArrayList<>();
            List<String> paths = new ArrayList<>();
            uris = Matisse.obtainResult(data);
            paths = Matisse.obtainPathResult(data);

            if (paths.size() > 0) {
                {
                    imageLink = paths.get(0);
                }
            }

            if (!Utils.isEmpty(imageLink)) {
                uploadAvatar();
            }
        }
    }

    private void uploadAvatar() {
        ANRequest.MultiPartBuilder anRequest = AndroidNetworking.upload(Constants.ApiElearning + "api/uploads/upload-file");
        if (!imageLink.isEmpty()) {
            File file = new File(imageLink);
            anRequest.addMultipartFile("file", file);
        }

        anRequest.addMultipartParameter("folderName", "UserFontend")
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultApiModel<UploadFileResultELearningModel> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<UploadFileResultELearningModel>>() {
                            }.getType());
                            String statusCode = response.getString(Constants.StatusCode);
                            if (statusCode.equals(Constants.Status_Success)) {
                                userInfoModel.avatar = resultObject.data.fileUrl;
                                modelUpdate.avatar = resultObject.data.fileUrl;
                                Tools.displayImageRound(context, imageView, Constants.ApiElearning + resultObject.data.fileUrl);
                            }
                        } catch (Exception ex) {
                            Toast.makeText(context, "Tải ảnh lên không thành công", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Toast.makeText(context, "Tải ảnh lên không thành công", Toast.LENGTH_SHORT).show();
                    }
                });
    }

    // Validate thông tin user
    private boolean validate() {
        String emailPattern = "[a-zA-Z0-9._-]+@[a-z]+\\.+[a-z]+";
        if (Utils.isEmpty(editName.getText().toString())) {
            Toast.makeText(context, "Vui lòng nhập tên người dùng", Toast.LENGTH_SHORT).show();
            return false;
        }

        if (!Utils.isEmpty(edNation.getText().toString())) {
            boolean isNationalOk = false;
            for (ComboboxResult item : listNation) {
                if (item.name.equals(edNation.getText().toString())) {
                    isNationalOk = true;
                }
            }
            if (!isNationalOk) {
                Toast.makeText(context, "Dân tộc không có trong danh sách", Toast.LENGTH_SHORT).show();
                return false;
            }
        }

        if (!Utils.isEmpty(txtSodienthoai.getText().toString())) {
            if (txtSodienthoai.getText().toString().length() != 10) {
                Toast.makeText(context, "Số điện thoại phải có 10 số", Toast.LENGTH_SHORT).show();
                return false;
            }
        }

        if (Utils.isEmpty(txtEmail.getText().toString())) {
            Toast.makeText(context, "Vui lòng nhập email", Toast.LENGTH_SHORT).show();
            return false;
        } else if (!txtEmail.getText().toString().matches(emailPattern)) {
            Toast.makeText(context, "E-mail không đúng định dạng", Toast.LENGTH_SHORT).show();
            return false;
        }

        if (!Utils.isEmpty(edProvince.getText().toString())) {
            boolean isProvinceOk = false;
            for (ComboboxResult item : listProvince) {
                if (item.name.equals(edProvince.getText().toString())) {
                    isProvinceOk = true;
                }
            }
            if (!isProvinceOk) {
                Toast.makeText(context, "Tỉnh/ Thành phố không có trong danh sách", Toast.LENGTH_SHORT).show();
                return false;
            }
        }

        if (!Utils.isEmpty(edDistric.getText().toString())) {
            boolean isDistricOk = false;
            for (ComboboxResult item : listDistric) {
                if (item.name.equals(edDistric.getText().toString())) {
                    isDistricOk = true;
                }
            }
            if (!isDistricOk) {
                Toast.makeText(context, "Quận/ Huyện không có trong danh sách", Toast.LENGTH_SHORT).show();
                return false;
            }
        }

        if (!Utils.isEmpty(edWard.getText().toString())) {
            boolean isWardOk = false;
            for (ComboboxResult item : listWrad) {
                if (item.name.equals(edWard.getText().toString())) {
                    isWardOk = true;
                }
            }
            if (!isWardOk) {
                Toast.makeText(context, "Xã/ Phường không có trong danh sách", Toast.LENGTH_SHORT).show();
                return false;
            }
        }

        return true;
    }

    // update tài khoản
    private void updateUserInfo(UserUpdateModel model) {
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(model));
        } catch (JSONException e) {
        }

        AndroidNetworking.put(Constants.ApiElearning + "api/mobile/user/" + learnerId)
                .addJSONObjectBody(jsonModel)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            String statusCode = response.getString("statusCode");
                            if (statusCode.equals(Constants.Status_Success)) {
                                Toast.makeText(context, "Cập nhật thông tin thành công", Toast.LENGTH_SHORT).show();
                                dismiss();
                            }
                        } catch (Exception ex) {
                            Toast.makeText(context, "Cập nhật thông tin thất bại", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Toast.makeText(context, "Cập nhật thông tin thất bại", Toast.LENGTH_SHORT).show();
                    }
                });
    }

    // Lấy thông tin tài khoản
    private void getInfoUser() {
        AndroidNetworking.get(Constants.ApiElearning + "api/mobile/user/" + learnerId)
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
                                userInfoModel = resultModel.data;
                                displayUserInfo();
                            }
                        } catch (Exception ex) {
                            Toast.makeText(context, ex.toString(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Toast.makeText(context, "Có lỗi", Toast.LENGTH_SHORT).show();
                    }

                });
    }

    private UserUpdateModel modelUpdate = new UserUpdateModel();

    // Gán giá trị cho model
    private void getInfoModel() {
        if (validate()) {
            modelUpdate.nationId = edNation.getTag() != null ? edNation.getTag().toString() : "";
            modelUpdate.name = editName.getText().toString();
            modelUpdate.phoneNumber = txtSodienthoai.getText().toString();
            modelUpdate.email = txtEmail.getText().toString();
            modelUpdate.provinceId = edProvince.getTag() != null ? edProvince.getTag().toString() : "";
            modelUpdate.districtId = edDistric.getTag() != null ? edDistric.getTag().toString() : "";
            modelUpdate.wardId = edWard.getTag() != null ? edWard.getTag().toString() : "";
            modelUpdate.dateOfBirthday = DateUtils.ConvertDMYToYMD(txtNgaysinh.getText().toString());
            if (!Utils.isEmpty(txtAddress.getText().toString())) {
                modelUpdate.address = txtAddress.getText().toString();
            } else {
                modelUpdate.address = userInfoModel.address;
            }
            modelUpdate.avatar = userInfoModel.avatar;
            if (radioGenderNam.isChecked()) {
                modelUpdate.gender = false;
            } else if (radioGenderNu.isChecked()) {
                modelUpdate.gender = true;
            }
            if (Utils.isEmpty(imageLink)) {
                modelUpdate.avatar = userInfoModel.avatar.replace(Constants.ApiElearning, "");
            }
            this.updateUserInfo(modelUpdate);
        }
    }

    // Gán giá trị lấy từ db view ra màn hình
    private void displayUserInfo() {
        editName.setText(userInfoModel.name);
        if (userInfoModel.dateOfBirthday != null && userInfoModel.dateOfBirthday != "") {
            txtNgaysinh.setText(DateUtils.ConvertYMDServerToDMY(userInfoModel.dateOfBirthday));
        }
        txtEmail.setText(userInfoModel.email);
        if (userInfoModel.provider.equals("Email")) {
            txtEmail.setEnabled(false);
        }
        edProvince.setText(userInfoModel.provinceName);
        edProvince.setTag(userInfoModel.provinceId);
        edDistric.setText(userInfoModel.districtName);
        edDistric.setTag(userInfoModel.districtId);
        edWard.setText(userInfoModel.wardName);
        edWard.setTag(userInfoModel.wardId);
        edNation.setText(userInfoModel.nationName);
        edNation.setTag(userInfoModel.nationId);
        // set nam nữ
        if (!userInfoModel.gender) {
            radioGenderNam.setChecked(true);
        } else {
            radioGenderNu.setChecked(true);
        }
        // Ngày sinh
        if (!Utils.isEmpty(userInfoModel.dateOfBirthday)) {
            txtNgaysinh.setText(DateUtils.ConvertYMDServerToDMY(userInfoModel.dateOfBirthday));
        }
        txtAddress.setText(userInfoModel.address);
        txtSodienthoai.setText(userInfoModel.phoneNumber);
        if (!userInfoModel.avatar.equals(Constants.ApiElearning)) {
            Tools.displayImageRound(context, imageView, userInfoModel.avatar);
        }
        if (!Utils.isEmpty(userInfoModel.provinceId)) {
            getDistrictByProvinceId(userInfoModel.provinceId);
        }
        if (!Utils.isEmpty(userInfoModel.districtId)) {
            getWardByDistricId(userInfoModel.districtId);
        }
    }

    // Lấy danh sách tỉnh
    private void getListProvince() {
        AndroidNetworking.get(Constants.ApiElearning + "api/combobox/search-province")
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultApiModel<ComboboxResultData> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<ComboboxResultData>>() {
                            }.getType());

                            listProvince = resultModel.data.dataResults;
                            if (listProvince != null && listProvince.size() > 0) {
                                arrayProvinceName = new String[listProvince.size()];
                                int index = 0;
                                for (ComboboxResult item : listProvince) {
                                    arrayProvinceName[index] = item.name;
                                    index++;
                                }

                                ArrayAdapter adapterProvince = new ArrayAdapter(getContext(), R.layout.item_filter, arrayProvinceName);
                                edProvince.setAdapter(adapterProvince);
                            }
                        } catch (Exception ex) {
                            Toast.makeText(context, "Lỗi lấy dữ liệu Tỉnh/Thành phố", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Toast.makeText(context, "Lỗi lấy dữ liệu Tỉnh/Thành phố", Toast.LENGTH_SHORT).show();
                    }

                });
    }

    // Lấy danh sách huyện theo tỉnh
    private void getDistrictByProvinceId(String id) {
        AndroidNetworking.get(Constants.ApiElearning + "api/combobox/search-district/" + id)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultApiModel<ComboboxResultData> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<ComboboxResultData>>() {
                            }.getType());
                            listDistric = resultModel.data.dataResults;
                            if (listDistric != null && listDistric.size() > 0) {
                                arrayDistricName = new String[listDistric.size()];
                                int index = 0;
                                for (ComboboxResult item : listDistric) {
                                    arrayDistricName[index] = item.name;
                                    index++;
                                }
                                ArrayAdapter adapterWard = new ArrayAdapter(getContext(), R.layout.item_filter, arrayDistricName);
                                edDistric.setAdapter(adapterWard);
                            }
                        } catch (Exception ex) {
                            Toast.makeText(context, "Lỗi lấy dữ liệu Quận/Huyện", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Toast.makeText(context, "Lỗi lấy dữ liệu Quận/Huyện", Toast.LENGTH_SHORT).show();
                    }

                });
    }

    // Lấy danh sách xã theo huyện
    private void getWardByDistricId(String id) {
        AndroidNetworking.get(Constants.ApiElearning + "api/combobox/search-ward/" + id)
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultApiModel<ComboboxResultData> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<ComboboxResultData>>() {
                            }.getType());
                            listWrad = resultModel.data.dataResults;
                            if (listWrad != null && listWrad.size() > 0) {
                                arrayWardName = new String[listWrad.size()];
                                int index = 0;
                                for (ComboboxResult item : listWrad) {
                                    arrayWardName[index] = item.name;
                                    index++;
                                }
                                ArrayAdapter adapterWard = new ArrayAdapter(getContext(), R.layout.item_filter, arrayWardName);
                                edWard.setAdapter(adapterWard);
                            }
                        } catch (Exception ex) {
                            Toast.makeText(context, "Lỗi lấy dữ liệu Phường/Xã", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Toast.makeText(context, "Lỗi lấy dữ liệu Phường/Xã", Toast.LENGTH_SHORT).show();
                    }

                });
    }

    // Lấy danh sách dân tộc
    private void getNavition() {
        AndroidNetworking.get(Constants.ApiElearning + "api/combobox/search-nation")
                .setPriority(Priority.MEDIUM)
                .build()
                .getAsJSONObject(new JSONObjectRequestListener() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            ResultApiModel<ComboboxResultData> resultModel = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<ComboboxResultData>>() {
                            }.getType());
                            listNation = resultModel.data.dataResults;
                            if (listNation != null && listNation.size() > 0) {
                                arrayNavitionName = new String[listNation.size()];
                                int index = 0;
                                for (ComboboxResult item : listNation) {
                                    arrayNavitionName[index] = item.name;
                                    index++;
                                }
                                ArrayAdapter adapterWard = new ArrayAdapter(getContext(), R.layout.item_filter, arrayNavitionName);
                                edNation.setAdapter(adapterWard);
                            }
                        } catch (Exception ex) {
                            Toast.makeText(context, "Lỗi lấy dữ liệu dân tộc", Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onError(ANError anError) {
                        Toast.makeText(context, "Lỗi lấy dữ liệu dân tộc", Toast.LENGTH_SHORT).show();
                    }
                });
    }


}
