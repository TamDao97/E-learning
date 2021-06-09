package nts.swipesafe.fragment;

import android.app.Activity;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.core.widget.NestedScrollView;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.adapter.GuiderAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.NonScrollListView;
import nts.swipesafe.model.CourseGeneralInfoModel;
import nts.swipesafe.model.EmployeeElearningModel;
import nts.swipesafe.model.MobileLearnerCourseCreateModel;
import nts.swipesafe.model.ResultApiModel;

public class CourseGeneralInfoTabFragment extends Fragment {
    private View root;
    private static Activity context;
    private String courseId;
    private WebView webview_CourseInfo;
    private Button btn_Registration, btn_Study;
    private NonScrollListView listViewGuider;
    private GuiderAdapter guiderAdapter;
    private List<EmployeeElearningModel> listObject;
    private LinearLayout lyProgress, lyProgress2;
    private RelativeLayout viewNoData, viewNoData2;
    private NestedScrollView nested_scroll_view;
    private SharedPreferences sharedPreferencesLogin;
    private String learnerId;

    public static CourseGeneralInfoTabFragment newInstance(String id) {
        CourseGeneralInfoTabFragment fragment = new CourseGeneralInfoTabFragment();
        fragment.courseId = id;
        return fragment;
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        root = inflater.inflate(R.layout.fragment_course_general_info, container, false);
        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        learnerId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, null);
        context = getActivity();
        initComponent();
        return root;
    }

    private void initComponent() {
        lyProgress = root.findViewById(R.id.lyProgress);
        lyProgress2 = root.findViewById(R.id.lyProgress2);
        viewNoData = root.findViewById(R.id.viewNoData);
        viewNoData2 = root.findViewById(R.id.viewNoData2);
        nested_scroll_view = root.findViewById(R.id.nested_scroll_view);

        webview_CourseInfo = root.findViewById(R.id.webview_CourseInfo);
        btn_Registration = root.findViewById(R.id.btn_Registration);
        getCourseGeneralInfo();

        listObject = new ArrayList<>();
        listViewGuider = root.findViewById(R.id.listViewGuider);
        guiderAdapter = new GuiderAdapter(context, listObject);
        guiderAdapter.notifyDataSetChanged();
        listViewGuider.setAdapter(guiderAdapter);

        btn_Study = root.findViewById(R.id.btn_Study);
        btn_Study.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String loginId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, "");
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                DialogFragment dialogFragment = LessonFragment.newInstance(courseId, loginId,"");
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }
        });

        btn_Registration.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                registerCourse();
            }
        });

    }

    private void registerCourse() {
        MobileLearnerCourseCreateModel mobileLearnerCourseCreateModel = new MobileLearnerCourseCreateModel();
        mobileLearnerCourseCreateModel.CourseId = courseId;
        mobileLearnerCourseCreateModel.LearnerId = learnerId;
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(mobileLearnerCourseCreateModel));
        } catch (JSONException e) {

        }
        try {
            AndroidNetworking.post(Constants.ApiElearning + "api/mobile/course/registerCourse")
                    .setPriority(Priority.MEDIUM)
                    .addJSONObjectBody(jsonModel)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {

                                String statusCode = response.getString(Constants.StatusCode);
                                if (statusCode.equals("1")) {
                                    Toast.makeText(context, "Đăng ký thành công", Toast.LENGTH_LONG).show();
                                    btn_Study.setVisibility(View.VISIBLE);
                                    btn_Registration.setVisibility(View.GONE);
                                } else {
                                    Toast.makeText(context, response.getString("message"), Toast.LENGTH_LONG).show();
                                }
                            } catch (Exception ex) {
                                Toast.makeText(context, "Đăng ký không thành công", Toast.LENGTH_LONG).show();
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            Toast.makeText(context, "Đăng ký không thành công", Toast.LENGTH_LONG).show();
                        }
                    });
        } catch (Exception ex) {
            Toast.makeText(context, "Đăng ký không thành công", Toast.LENGTH_LONG).show();
        }
    }

    private void getCourseGeneralInfo() {
        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/course/getcoursebyid?id=" + courseId + "&learnerId=" + learnerId)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<CourseGeneralInfoModel> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<CourseGeneralInfoModel>>() {
                                }.getType());
                                String statusCode = response.getString(Constants.StatusCode);
                                if (statusCode.equals("1")) {
                                    webview_CourseInfo.loadDataWithBaseURL(null, resultObject.data.content, "text/html", "utf-8", null);
                                    getListGuider();
                                    if (resultObject.data.isRegister) {
                                        btn_Registration.setVisibility(View.GONE);
                                        btn_Study.setVisibility(View.VISIBLE);
                                    } else {
                                        btn_Registration.setVisibility(View.VISIBLE);
                                        btn_Study.setVisibility(View.GONE);
                                    }
                                } else {
//                                    nested_scroll_view.setVisibility(View.GONE);
                                    viewNoData.setVisibility(View.VISIBLE);
                                    btn_Registration.setVisibility(View.GONE);
                                }
                                lyProgress.setVisibility(View.GONE);
                            } catch (Exception ex) {
                                lyProgress.setVisibility(View.GONE);
                                viewNoData.setVisibility(View.VISIBLE);
                                btn_Registration.setVisibility(View.GONE);
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            lyProgress.setVisibility(View.GONE);
                            viewNoData.setVisibility(View.VISIBLE);
                            btn_Registration.setVisibility(View.GONE);
                        }
                    });
        } catch (Exception ex) {
            lyProgress.setVisibility(View.GONE);
            viewNoData.setVisibility(View.VISIBLE);
            btn_Registration.setVisibility(View.GONE);
        }
    }

    private void getListGuider() {
        lyProgress2.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/course/getlistemployeecourse?id=" + courseId)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<List<EmployeeElearningModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<List<EmployeeElearningModel>>>() {
                                }.getType());
                                String statusCode = response.getString(Constants.StatusCode);
                                if (statusCode.equals("1")) {
                                    listObject.clear();
                                    if (resultObject.data.size() > 0) {
                                        listObject.addAll(resultObject.data);
                                        guiderAdapter.notifyDataSetChanged();
//                                        viewNoData2.setVisibility(View.GONE);
                                    } else {
                                        listViewGuider.setVisibility(View.GONE);
//                                        viewNoData2.setVisibility(View.VISIBLE);
                                    }
                                }
                                lyProgress2.setVisibility(View.GONE);
                            } catch (Exception ex) {
                                lyProgress2.setVisibility(View.GONE);
//                                viewNoData2.setVisibility(View.VISIBLE);
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            lyProgress2.setVisibility(View.GONE);
                            viewNoData2.setVisibility(View.VISIBLE);
                        }
                    });
        } catch (Exception ex) {
            lyProgress2.setVisibility(View.GONE);
//            viewNoData2.setVisibility(View.VISIBLE);
        }
    }
}
