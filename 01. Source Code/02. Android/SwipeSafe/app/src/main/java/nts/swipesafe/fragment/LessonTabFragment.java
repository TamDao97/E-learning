package nts.swipesafe.fragment;

import android.app.Activity;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.RelativeLayout;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

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

import nts.swipesafe.R;
import nts.swipesafe.adapter.CategoryLessonAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.Tools;
import nts.swipesafe.model.LessonMenuModel;
import nts.swipesafe.model.ResultApiModel;

public class LessonTabFragment extends Fragment {
    private View root;
    private static Activity context;
    private String courseId;
    private RecyclerView listViewLesson;
    private LinearLayout lyProgress;
    private RelativeLayout viewNoData;
    private PullRefreshLayout refreshLayout;
    private boolean userScrolled = false;
    private List<LessonMenuModel> listObject;
    private CategoryLessonAdapter categoryLessonAdapter;
    private SharedPreferences sharedPreferencesLogin;

    public static LessonTabFragment newInstance(String id) {
        LessonTabFragment fragment = new LessonTabFragment();
        fragment.courseId = id;
        return fragment;
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        root = inflater.inflate(R.layout.fragment_tab_lesson, container, false);
        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        context = getActivity();
        initComponent();
        return root;
    }

    private void initComponent() {
        listObject = new ArrayList<>();
        listViewLesson = root.findViewById(R.id.listViewLesson);
        viewNoData = root.findViewById(R.id.viewNoData);
        lyProgress = root.findViewById(R.id.lyProgress);
        categoryLessonAdapter = new CategoryLessonAdapter(getContext(), listObject);
        categoryLessonAdapter.SetOnItemClickListener(new CategoryLessonAdapter.OnItemClickListener() {
            @Override
            public void onItemClick(View view, int position, LessonMenuModel item) {
                if (Tools.checkLogin(context)) {
                    String loginId = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, "");
                    final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                    DialogFragment dialogFragment = LessonFragment.newInstance(courseId, loginId, item.id);
                    FragmentTransaction transaction = fragmentManager.beginTransaction();
                    transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                    transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
                }
            }
        });
        listViewLesson.setAdapter(categoryLessonAdapter);
        categoryLessonAdapter.notifyDataSetChanged();
        listViewLesson.setLayoutManager(new GridLayoutManager(getContext(), 1));
        listViewLesson.setHasFixedSize(true);
        searchLesson();
    }


    private void searchLesson() {
        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/lesson/search?id=" + courseId)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<List<LessonMenuModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<List<LessonMenuModel>>>() {
                                }.getType());
                                lyProgress.setVisibility(View.GONE);
                                String statusCode = response.getString(Constants.StatusCode);
                                if (statusCode.equals("1") && resultObject.data.size() > 0) {
                                    listObject.clear();
                                    listObject.addAll(resultObject.data);
                                    categoryLessonAdapter.notifyDataSetChanged();
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
                            viewNoData.setVisibility(View.VISIBLE);
                            lyProgress.setVisibility(View.GONE);
                        }
                    });
        } catch (Exception ex) {
            viewNoData.setVisibility(View.VISIBLE);
            lyProgress.setVisibility(View.GONE);
        }
    }
}
