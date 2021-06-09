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

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.baoyz.widget.PullRefreshLayout;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.ActivityTabCourse;
import nts.swipesafe.R;
import nts.swipesafe.adapter.CourseAdapter;
import nts.swipesafe.adapter.MyCourseAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.model.MyCourseModel;
import nts.swipesafe.model.ProgramModel;
import nts.swipesafe.model.ResultApiModel;


public class MyCourseFragment extends DialogFragment {
    private View root;
    private static Activity context;
    private ListView listViewCourse;
    private List<MyCourseModel.Course> listCourse;
    private MyCourseAdapter myCourseAdapter;
    private LinearLayout lyProgress;
    private RelativeLayout viewNoData;
    private SharedPreferences sharedPreferencesLogin;
    private String learnerId;
    private TextView tv_totalCourse, tv_FinishCourse, tv_Test;
    private PullRefreshLayout swipeRefreshLayout;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        root = inflater.inflate(R.layout.fragment_my_course, container, false);
        context = getActivity();
        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        learnerId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, null);
        initToolbar();
        initComponent();
        // Inflate the layout for this fragment
        return root;
    }

    private void initToolbar() {
        TextView tvTitle = (TextView) root.findViewById(R.id.tvTitle);
        ImageButton imbClose = (ImageButton) root.findViewById(R.id.imbClose);
        tvTitle.setText("Khoá học của tôi");
        imbClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dismiss();
            }
        });
    }

    private void initComponent() {
        listViewCourse = root.findViewById(R.id.listViewCourse);
        lyProgress = root.findViewById(R.id.lyProgress);
        viewNoData = root.findViewById(R.id.viewNoData);
        listCourse = new ArrayList<>();
        myCourseAdapter = new MyCourseAdapter(context, listCourse);
        myCourseAdapter.notifyDataSetChanged();
        listViewCourse.setAdapter(myCourseAdapter);
        tv_totalCourse = root.findViewById(R.id.tv_totalCourse);
        tv_FinishCourse = root.findViewById(R.id.tv_FinishCourse);
        tv_Test = root.findViewById(R.id.tv_Test);
        myCourseAdapter.setOnItemClickListener(new MyCourseAdapter.OnItemClickListener() {
            @Override
            public void onItemClick(View view, MyCourseModel.Course obj, int pos) {

            }

            @Override
            public void onStudyClick(View view, MyCourseModel.Course obj, int pos) {
                String loginId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, "");
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                DialogFragment dialogFragment = LessonFragment.newInstance(obj.courseId, loginId,"");
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }

            @Override
            public void onDetailsClick(View view, MyCourseModel.Course obj, int pos) {
                Intent intent = new Intent(context, ActivityTabCourse.class);
                intent.putExtra("courseName", obj.name);
                intent.putExtra("courseId", obj.courseId);
                context.startActivity(intent);
            }
        });
        swipeRefreshLayout = root.findViewById(R.id.swipeRefreshLayout);
        swipeRefreshLayout.setOnRefreshListener(new PullRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
                getMyCourse();
                swipeRefreshLayout.setRefreshing(false);
            }
        });
        getMyCourse();
    }

    private void getMyCourse() {
        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/course/getMyCourse?learnerId=" + learnerId)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<MyCourseModel> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<MyCourseModel>>() {
                                }.getType());
                                String statusCode = response.getString(Constants.StatusCode);
                                if (statusCode.equals(Constants.Status_Success)) {
                                    viewNoData.setVisibility(View.GONE);
                                    tv_totalCourse.setText(resultObject.data.totalCourse);
                                    tv_FinishCourse.setText(resultObject.data.completed);
                                    tv_Test.setText(resultObject.data.testComplete);
                                    listCourse.clear();
                                    listCourse.addAll(resultObject.data.courses);
                                    myCourseAdapter.notifyDataSetChanged();
                                } else {
                                    viewNoData.setVisibility(View.VISIBLE);
                                }
                            } catch (Exception ex) {
                                viewNoData.setVisibility(View.VISIBLE);
                            }
                            lyProgress.setVisibility(View.GONE);
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
}
