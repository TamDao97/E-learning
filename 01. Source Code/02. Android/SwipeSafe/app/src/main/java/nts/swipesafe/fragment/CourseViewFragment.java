package nts.swipesafe.fragment;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentActivity;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.JsonSyntaxException;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.ActivityTabCourse;
import nts.swipesafe.R;
import nts.swipesafe.adapter.CourseAdapter;
import nts.swipesafe.adapter.DataAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.model.CourseModel;
import nts.swipesafe.model.CourseResultModel;
import nts.swipesafe.model.MobileLearnerCourseCreateModel;
import nts.swipesafe.model.ResultApiModel;
import nts.swipesafe.model.SearchModel;

public class CourseViewFragment extends DialogFragment {
    private View root;
    public static Activity context;
    private ListView listViewCourse;
    private List<CourseResultModel> listCourse;
    private DataAdapter adapter;
    private LinearLayout lyProgress;
    private RelativeLayout viewNoData;
    private String id;
    private SharedPreferences sharedPreferencesLogin;
    private String learnerId;

    public static CourseViewFragment newInstance(String id) {
        CourseViewFragment fragment = new CourseViewFragment();
        fragment.id = id;
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        root = inflater.inflate(R.layout.fragment_course_view, container, false);
        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        learnerId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, null);
        initToolbar();
        initComponent();
        // Inflate the layout for this fragment
        return root;
    }

    public CourseViewFragment.CallbackResult callbackResult;

    public void setOnCallbackResult(final CourseViewFragment.CallbackResult callbackResult) {
        this.callbackResult = callbackResult;
    }

    public interface CallbackResult {
        void sendResult(boolean requestCode);
    }

    private void initToolbar() {
        TextView tvTitle = (TextView) root.findViewById(R.id.tvTitle);
        ImageButton imbClose = (ImageButton) root.findViewById(R.id.imbClose);
        tvTitle.setText("Danh sách khoá học");
        imbClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                callbackResult.sendResult(true);
                dismiss();
            }
        });
    }

    private void initComponent() {
        context = getActivity();
        listViewCourse = root.findViewById(R.id.listViewCourse);
        lyProgress = root.findViewById(R.id.lyProgress);
        viewNoData = root.findViewById(R.id.viewNoData);

        listCourse = new ArrayList<>();
        adapter = new DataAdapter(context, listCourse);
        listViewCourse.setAdapter(adapter);
        adapter.notifyDataSetChanged();
        adapter.setOnItemClickListener(new DataAdapter.OnItemClickListener() {
            @Override
            public void onRegistrationClick(View view, CourseResultModel obj, int pos) {
                boolean isLogin = sharedPreferencesLogin.getBoolean(Constants.IsLogin, false);

                if (!isLogin) {
                    final FragmentManager fragmentManager = ((FragmentActivity) getActivity()).getSupportFragmentManager();
                    DialogLoginELearning dialogLoginELearning = new DialogLoginELearning();
                    FragmentTransaction transaction = fragmentManager.beginTransaction();
                    transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                    transaction.add(R.id.drawer_layout, dialogLoginELearning).addToBackStack(null).commit();
                    dialogLoginELearning.setOnCallbackResult(new DialogLoginELearning.CallbackResult() {
                        @Override
                        public void sendResult(boolean requestCode) {
                            learnerId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, null);
                            searchCourseByProgramId();
                        }
                    });
                } else {
                    registerCourse(obj.id);
                }
            }

            @Override
            public void onDetialClick(View view, CourseResultModel obj, int pos) {
                Intent intent = new Intent(context, ActivityTabCourse.class);
                intent.putExtra("courseName", obj.title);
                intent.putExtra("courseId", obj.id);
                context.startActivity(intent);
            }

            @Override
            public void onStudyClick(View view, CourseResultModel obj, int pos) {
                String loginId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, "");
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                DialogFragment dialogFragment = LessonFragment.newInstance(obj.id, loginId,"");
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }
        });
        searchCourseByProgramId();
    }

    private void registerCourse(String id) {
        MobileLearnerCourseCreateModel mobileLearnerCourseCreateModel = new MobileLearnerCourseCreateModel();
        mobileLearnerCourseCreateModel.CourseId = id;
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
                                } else {
                                    Toast.makeText(context, response.getString("message"), Toast.LENGTH_LONG).show();
                                }
                                searchCourseByProgramId();
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

    private void searchCourseByProgramId() {
        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/course/search?id=" + id + "&learnerId=" + learnerId)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<List<CourseResultModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<List<CourseResultModel>>>() {
                                }.getType());
                                lyProgress.setVisibility(View.GONE);
                                String statusCode = response.getString(Constants.StatusCode);
                                if (statusCode.equals(Constants.Status_Success)) {
                                    listCourse.clear();
                                    listCourse.addAll(resultObject.data);
                                    adapter.notifyDataSetChanged();
                                    if (listCourse.size() > 0) {
                                        viewNoData.setVisibility(View.GONE);
                                    } else {
                                        viewNoData.setVisibility(View.VISIBLE);
                                    }
                                } else {
                                    viewNoData.setVisibility(View.VISIBLE);
                                }

                            } catch (JsonSyntaxException ex) {
                                viewNoData.setVisibility(View.VISIBLE);
                            } catch (JSONException ex) {
                                viewNoData.setVisibility(View.VISIBLE);
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            viewNoData.setVisibility(View.VISIBLE);
                        }
                    });
        } catch (Exception ex) {
            viewNoData.setVisibility(View.VISIBLE);
        }
    }
}
