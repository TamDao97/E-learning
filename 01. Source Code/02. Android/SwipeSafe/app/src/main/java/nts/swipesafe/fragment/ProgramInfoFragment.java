package nts.swipesafe.fragment;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.Display;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.webkit.WebView;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.core.widget.NestedScrollView;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;
import androidx.viewpager2.widget.ViewPager2;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;

import nts.swipesafe.ActivityTabCourse;
import nts.swipesafe.R;
import nts.swipesafe.adapter.CourseViewPagerAdapter;
import nts.swipesafe.adapter.HorizontalMarginItemDecoration;
import nts.swipesafe.common.Constants;
import nts.swipesafe.model.CourseGeneralInfoModel;
import nts.swipesafe.model.CourseModel;
import nts.swipesafe.model.MobileLearnerCourseCreateModel;
import nts.swipesafe.model.ResultApiModel;

import static java.lang.Math.abs;

public class ProgramInfoFragment extends DialogFragment {
    private View root;
    private static Activity context;
    private WebView webview_ProgramInfo;
    private String programId, learnerId, programName;
    private TextView tv_AllCourse;
    private ViewPager2 view_pager_course_program;
    private CourseViewPagerAdapter courseViewPagerAdapter;
    private List<CourseModel> listCourse;
    private LinearLayout lyProgress;
    private RelativeLayout viewNoData;
    private NestedScrollView nested_scroll_view;
    private SharedPreferences sharedPreferencesLogin;


    public static ProgramInfoFragment newInstance(String id, String mlearnerId, String mprogramName, List<CourseModel> mlistCourse) {
        ProgramInfoFragment fragment = new ProgramInfoFragment();
        fragment.programId = id;
        fragment.learnerId = mlearnerId;
        fragment.programName = mprogramName;
        fragment.listCourse = mlistCourse;
        return fragment;
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        root = inflater.inflate(R.layout.fragment_program_info, container, false);
        context = getActivity();
        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        initComponent();
        initToolbar();
        return root;
    }

    private void initComponent() {
        lyProgress = root.findViewById(R.id.lyProgress);
        viewNoData = root.findViewById(R.id.viewNoData);
        nested_scroll_view = root.findViewById(R.id.nested_scroll_view);
        webview_ProgramInfo = root.findViewById(R.id.webview_ProgramInfo);
        getProgramInfo();
        tv_AllCourse = root.findViewById(R.id.tv_AllCourse);
        tv_AllCourse.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                CourseViewFragment dialogFragment = CourseViewFragment.newInstance(programId);
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
                dialogFragment.setOnCallbackResult(new CourseViewFragment.CallbackResult() {
                    @Override
                    public void sendResult(boolean requestCode) {
                        getProgramInfo();
                    }
                });
            }
        });
        view_pager_course_program = root.findViewById(R.id.view_pager_course_program);
        setupViewPager();

    }

    private void setupViewPager() {
        courseViewPagerAdapter = new CourseViewPagerAdapter(listCourse, context);
        courseViewPagerAdapter.setOnItemClickListener(new CourseViewPagerAdapter.OnItemClickListener() {
            @Override
            public void onRegistrationClick(View view, CourseModel obj, int pos) {
                registerCourse(obj.courseId);
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
            public void onDetialClick(View view, CourseModel obj, int pos) {
                Intent intent = new Intent(context, ActivityTabCourse.class);
                intent.putExtra("courseName", obj.name);
                intent.putExtra("courseId", obj.courseId);
                context.startActivity(intent);
            }
        });

        view_pager_course_program.setAdapter(courseViewPagerAdapter);
        view_pager_course_program.setCurrentItem(0);
        view_pager_course_program.setOffscreenPageLimit(1);
        int nextItemVisiblePx = (int) context.getResources().getDimension(R.dimen.viewpager_next_item_visible);
        int currentItemHorizontalMarginPx = (int) context.getResources().getDimension(R.dimen.viewpager_current_item_horizontal_margin);
        final int pageTranslationX = nextItemVisiblePx + currentItemHorizontalMarginPx;

        ViewPager2.PageTransformer pageTransformer = new ViewPager2.PageTransformer() {
            @Override
            public void transformPage(@NonNull View page, float position) {
                page.setTranslationX(-pageTranslationX * position);
                // Next line scales the item's height. You can remove it if you don't want this effect
                page.setScaleY(1 - (0.15f * abs(position)));
                // If you want a fading effect uncomment the next line:
                page.setAlpha(0.25f + (1 - abs(position)));
            }
        };

        view_pager_course_program.setPageTransformer(pageTransformer);
        if (view_pager_course_program.getItemDecorationCount() == 0) {
            view_pager_course_program.addItemDecoration(new HorizontalMarginItemDecoration(context, R.dimen.viewpager_current_item_horizontal_margin_testing,
                    R.dimen.viewpager_next_item_visible_testing));
        }
        view_pager_course_program.registerOnPageChangeCallback(new ViewPager2.OnPageChangeCallback() {
            @Override
            public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {
                super.onPageScrolled(position, positionOffset, positionOffsetPixels);
            }
        });
    }

    private void registerCourse(final String id) {
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
                                setListCourse(id);
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

    private void setListCourse(String id) {
        if (listCourse.size() > 0) {
            for (CourseModel item : listCourse) {
                if (item.courseId.equals(id)) {
                    item.isRegister = true;
                    break;
                }
            }
        }
        setupViewPager();
    }

    private void initToolbar() {
        TextView tvTitle = (TextView) root.findViewById(R.id.tvTitle);
        ImageButton imbClose = (ImageButton) root.findViewById(R.id.imbClose);
        tvTitle.setText(programName);
        imbClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                callbackResult.sendResult(true);
                dismiss();
            }
        });
    }

    private void getProgramInfo() {
        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/program/getProgramDetailById?id=" + programId+"&learnerId="+learnerId)
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
                                    webview_ProgramInfo.loadDataWithBaseURL(null, resultObject.data.content, "text/html", "utf-8", null);
                                    if (resultObject.data.courses.size() > 0) {
                                        listCourse.clear();
                                        listCourse.addAll(resultObject.data.courses);
                                        courseViewPagerAdapter.notifyDataSetChanged();
                                    }
                                } else {
                                    Toast.makeText(context, resultObject.message.get(0), Toast.LENGTH_SHORT).show();
                                    nested_scroll_view.setVisibility(View.GONE);
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

    public CallbackResult callbackResult;

    public void setOnCallbackResult(final ProgramInfoFragment.CallbackResult callbackResult) {
        this.callbackResult = callbackResult;
    }

    public interface CallbackResult {
        void sendResult(boolean requestCode);
    }

    @Override
    public void onDestroyView() {
        callbackResult.sendResult(true);
        dismiss();
        super.onDestroyView();
    }
}
