package nts.swipesafe.fragment;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.inputmethod.EditorInfo;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
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
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.ActivityTabCourse;
import nts.swipesafe.R;
import nts.swipesafe.adapter.CourseViewPagerAdapter;
import nts.swipesafe.adapter.SearchCourseAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.model.CourseModel;
import nts.swipesafe.model.MobileLearnerCourseCreateModel;
import nts.swipesafe.model.ProgramModel;
import nts.swipesafe.model.ResultApiModel;
import nts.swipesafe.model.SearchCourseModel;

public class FragmentSearchCourse extends Fragment {
    private View root;
    public static Activity context;
    private ListView listViewSearchCourse;
    private List<CourseModel> listCourse;
    private SearchCourseAdapter adapter;
    private SearchCourseModel searchCourseModel = new SearchCourseModel();
    private SharedPreferences sharedPreferencesLogin;
    private String learnerId;
    private TextView editText_search;
    private LinearLayout lyProgress;
    private RelativeLayout viewNoData;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        root = inflater.inflate(R.layout.fragment_search_course, container, false);
        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        learnerId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, null);
        context = getActivity();
        initComponent();
        return root;
    }

    private void initComponent() {
        editText_search = root.findViewById(R.id.editText_search);
        listViewSearchCourse = root.findViewById(R.id.listViewSearchCourse);
        lyProgress = root.findViewById(R.id.lyProgress);
        viewNoData = root.findViewById(R.id.viewNoData);
        editText_search.setOnEditorActionListener(action);
        listCourse = new ArrayList<>();
        adapter = new SearchCourseAdapter(context, listCourse);
        listViewSearchCourse.setAdapter(adapter);
        adapter.notifyDataSetChanged();
        adapter.setOnItemClickListener(new SearchCourseAdapter.OnItemClickListener() {
            @Override
            public void onRegisterClick(View view, CourseModel obj, int pos) {
                SharedPreferences sharedPreferences = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
                boolean isLogin = sharedPreferences.getBoolean(Constants.IsLogin, false);

                if (!isLogin) {
                    final FragmentManager fragmentManager = ((FragmentActivity) getActivity()).getSupportFragmentManager();
                    DialogLoginELearning dialogLoginELearning = new DialogLoginELearning();
                    FragmentTransaction transaction = fragmentManager.beginTransaction();
                    transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                    transaction.add(R.id.drawer_layout, dialogLoginELearning).addToBackStack(null).commit();
                    dialogLoginELearning.setOnCallbackResult(new DialogLoginELearning.CallbackResult() {
                        @Override
                        public void sendResult(boolean requestCode) {
                            searchCourse();
                        }
                    });
                } else {
                    registerCourse(obj.courseId);
                }
            }


            @Override
            public void onStudyClick(View view, CourseModel obj, int pos) {
                String loginId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, "");
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                DialogFragment dialogFragment = LessonFragment.newInstance(obj.courseId, loginId,"");
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }

            @Override
            public void onDetailsClick(View view, CourseModel obj, int pos) {
                Intent intent = new Intent(context, ActivityTabCourse.class);
                intent.putExtra("courseName", obj.name);
                intent.putExtra("courseId", obj.courseId);
                context.startActivity(intent);
            }
        });
        searchCourse();
    }

    public TextView.OnEditorActionListener action = new TextView.OnEditorActionListener() {
        @Override
        public boolean onEditorAction(TextView textView, int i, KeyEvent keyEvent) {
            switch (i) {
                case EditorInfo.IME_ACTION_SEARCH:
                    searchCourse();
                    break;
            }

            return false;
        }
    };

    public void searchCourse() {
        searchCourseModel.searchCondition = editText_search.getText().toString();
        searchCourseModel.learnerId = learnerId;
        lyProgress.setVisibility(View.VISIBLE);
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(searchCourseModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        try {
            AndroidNetworking.post(Constants.ApiElearning + "api/mobile/course/searchCourse")
                    .setPriority(Priority.MEDIUM)
                    .addJSONObjectBody(jsonModel)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<List<CourseModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<List<CourseModel>>>() {
                                }.getType());
                                lyProgress.setVisibility(View.GONE);
                                String statusCode = response.getString(Constants.StatusCode);
                                if (statusCode.equals(Constants.Status_Success)) {
                                    listCourse.clear();
                                    listCourse.addAll(resultObject.data);
                                    adapter.notifyDataSetChanged();
                                    if (listCourse.size() > 0) {
                                        viewNoData.setVisibility(View.GONE);
                                        listViewSearchCourse.setVisibility(View.VISIBLE);
                                    } else {
                                        viewNoData.setVisibility(View.VISIBLE);
                                        listViewSearchCourse.setVisibility(View.GONE);
                                    }
                                } else {
                                    viewNoData.setVisibility(View.VISIBLE);
                                }

                                lyProgress.setVisibility(View.GONE);
                            } catch (Exception ex) {
                                lyProgress.setVisibility(View.GONE);
                                viewNoData.setVisibility(View.VISIBLE);
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            lyProgress.setVisibility(View.GONE);
                            viewNoData.setVisibility(View.VISIBLE);
                        }
                    });
        } catch (Exception ex) {
            lyProgress.setVisibility(View.GONE);
            viewNoData.setVisibility(View.VISIBLE);
        }
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
                                searchCourse();
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

}
