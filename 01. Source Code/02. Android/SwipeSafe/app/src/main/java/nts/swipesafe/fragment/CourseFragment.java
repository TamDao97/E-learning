package nts.swipesafe.fragment;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.RelativeLayout;
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
import com.baoyz.widget.PullRefreshLayout;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.ActivityTabCourse;
import nts.swipesafe.MainActivity;
import nts.swipesafe.R;
import nts.swipesafe.adapter.CourseAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.Tools;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.CourseModel;
import nts.swipesafe.model.MobileLearnerCourseCreateModel;
import nts.swipesafe.model.ProgramModel;
import nts.swipesafe.model.ResultApiModel;

public class CourseFragment extends Fragment {
    private View root;
    public static Activity context;
    private ListView listViewCourse;
    private List<ProgramModel> listCourse;
    private CourseAdapter adapter;
    private LinearLayout lyProgress;
    private RelativeLayout viewNoData;
    private SharedPreferences sharedPreferencesLogin;
    private String learnerId;
    private PullRefreshLayout swipeRefreshLayout;
    public static CourseFragment newInstance() {
        CourseFragment fragment = new CourseFragment();
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        root = inflater.inflate(R.layout.fragment_course, container, false);

        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        learnerId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, null);
        initComponent();
        // Inflate the layout for this fragment
        return root;
    }

    private void initComponent() {
        context = getActivity();
        listViewCourse = root.findViewById(R.id.listViewCourse);
        lyProgress = root.findViewById(R.id.lyProgress);
        viewNoData = root.findViewById(R.id.viewNoData);

        listCourse = new ArrayList<>();
        adapter = new CourseAdapter(context, listCourse);
        adapter.notifyDataSetChanged();
        listViewCourse.setAdapter(adapter);

        adapter.setOnItemClickListener(new CourseAdapter.OnItemClickListener() {
            @Override
            public void onProgramClick(View view, ProgramModel obj, int pos) {
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                ProgramInfoFragment dialogFragment = ProgramInfoFragment.newInstance(obj.programId, learnerId, obj.name, obj.courses);
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
                dialogFragment.setOnCallbackResult(new ProgramInfoFragment.CallbackResult() {
                    @Override
                    public void sendResult(boolean requestCode) {
                        searchProgram();
                    }
                });
            }

            @Override
            public void onRegistrationClick(View view, CourseModel obj, int pos) {
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
                            learnerId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, null);
                            searchProgram();
                        }
                    });
                } else {
//                    AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
//                    builder.setCancelable(true);
//                    builder.setTitle("Xác nhận");
//                    builder.setMessage("Bạn có chắc chắn muốn đăng xuất?");
//                    builder.setPositiveButton("Đồng ý",
//                            new DialogInterface.OnClickListener() {
//                                @Override
//                                public void onClick(DialogInterface dialog, int which) {
//                                    logout();
//                                }
//                            });
//                    builder.setNegativeButton("Bỏ qua", new DialogInterface.OnClickListener() {
//                        @Override
//                        public void onClick(DialogInterface dialog, int which) {
//                        }
//                    });
//                    AlertDialog dialog = builder.create();
//                    dialog.show();

                    registerCourse(obj.courseId);
                }
            }

            @Override
            public void onStudynClick(View view, CourseModel obj, int pos) {
                String loginId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, "");
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                DialogFragment dialogFragment = LessonFragment.newInstance(obj.courseId, loginId,"");
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }

            @Override
            public void onDetialClick(View view, CourseModel obj, int pos) {
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
                searchProgram();
                swipeRefreshLayout.setRefreshing(false);
            }
        });

        searchProgram();
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
                                searchProgram();
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

    private void searchProgram() {
        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/program/getAllProgram?learnerId=" + learnerId)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<List<ProgramModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<List<ProgramModel>>>() {
                                }.getType());
                                lyProgress.setVisibility(View.GONE);
                                String statusCode = response.getString(Constants.StatusCode);
                                if (statusCode.equals("1")) {
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

}
