package nts.swipesafe.fragment;

import android.app.Activity;
import android.app.Dialog;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.http.SslError;
import android.os.Bundle;
import android.os.CountDownTimer;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.webkit.SslErrorHandler;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.viewpager2.widget.ViewPager2;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.adapter.CategoryLessonAdapter;
import nts.swipesafe.adapter.PaginationAdapter;
import nts.swipesafe.adapter.QuestionAdapter;
import nts.swipesafe.common.ChromeClient;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.Tools;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.AnswerLearnerTempModel;
import nts.swipesafe.model.FinishTestModel;
import nts.swipesafe.model.LessonHistoryModel;
import nts.swipesafe.model.LessonMenuModel;
import nts.swipesafe.model.LessonModel;
import nts.swipesafe.model.QuestionModel;
import nts.swipesafe.model.ResultApiModel;
import nts.swipesafe.model.TestResultModel;

import static java.lang.Math.abs;
import static java.lang.Math.log;

public class LessonFragment extends DialogFragment {
    private View root;
    public static Activity context;
    private ViewPager2 vpQuestion;
    private LinearLayout lyProgress, llTheory, llQuestion, llTimeView, lySubmit, lyPage, llFinish, lyFinishResult;
    private RelativeLayout viewNoData;
    private QuestionAdapter questionAdapter;
    private List<QuestionModel> listQuestion = new ArrayList<>();
    private String lessonCurrentId = "", courseId = "", learnerid = "";
    private TextView txtLessonName, tvTime, tvTitle, tvTotalCorrect, tvTotalQuestion;
    private WebView wvLessonContent;
    private Dialog dialogMenuLesson;
    private FloatingActionButton fabMenuLesson;
    private RecyclerView rvLesson, rvPagination;
    private CountDownTimer countDownTimer;
    private int totalTime = 0;
    private PaginationAdapter paginationAdapter;
    private CategoryLessonAdapter categoryLessonAdapter;
    private List<LessonMenuModel> lessonMenu;
    private Dialog dialogNotify = null;
    private Dialog dialogConfirmStart = null;
    private Dialog dialogConfirmFinish = null;
    private LessonModel lessonCurrent;
    private boolean isExam = false;
    private ImageButton imbComment;

    public static LessonFragment newInstance(String iCourseId, String loginId, String lessonId) {
        LessonFragment fragment = new LessonFragment();
        fragment.courseId = iCourseId;
        fragment.learnerid = loginId;
        if (!Utils.isEmpty(lessonId)) {
            fragment.lessonCurrentId = lessonId;
        }
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        root = inflater.inflate(R.layout.fragment_lesson, container, false);

        initToolbar();

        initComponent();

        initDialogMenu();

        if (!Utils.isEmpty(lessonCurrentId)) {
            getLesson(lessonCurrentId);
        }

        //Danh sách bài giảng theo d khóa học
        getLessonByCourse(courseId);
        // Inflate the layout for this fragment
        return root;
    }

    private void initToolbar() {
        tvTitle = (TextView) root.findViewById(R.id.tvTitle);
        ImageButton imbClose = (ImageButton) root.findViewById(R.id.imbClose);
        imbComment = (ImageButton) root.findViewById(R.id.imbComment);
        tvTitle.setText("Bài giảng");
        imbClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dismiss();
            }
        });

        imbComment.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                final FragmentManager fragmentManager = getActivity().getSupportFragmentManager();
                DialogFragment dialogFragment = DialogCommentFragment.newInstance(courseId, lessonCurrentId, Constants.Comment_Lesson_Type, lessonCurrent.name);
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }
        });
    }

    private void initComponent() {
        context = getActivity();
        viewNoData = root.findViewById(R.id.viewNoData);
        lyProgress = root.findViewById(R.id.lyProgress);

        llTheory = root.findViewById(R.id.llTheory);
        llQuestion = root.findViewById(R.id.llQuestion);
        txtLessonName = root.findViewById(R.id.txtLessonName);
        wvLessonContent = root.findViewById(R.id.wvLessonContent);
        wvLessonContent.setBackgroundColor(0);
//        wvLessonContent.setLayerType(View.LAYER_TYPE_HARDWARE, null);
        llTimeView = root.findViewById(R.id.llTimeView);
        lySubmit = root.findViewById(R.id.lySubmit);
        tvTime = root.findViewById(R.id.tvTime);
        llFinish = root.findViewById(R.id.llFinish);
        lyPage = root.findViewById(R.id.lyPage);
        lyFinishResult = root.findViewById(R.id.lyFinishResult);
        tvTotalCorrect = root.findViewById(R.id.tvTotalCorrect);
        tvTotalQuestion = root.findViewById(R.id.tvTotalQuestion);

        fabMenuLesson = root.findViewById(R.id.fabMenuLesson);

        vpQuestion = root.findViewById(R.id.vpQuestion);
        vpQuestion.registerOnPageChangeCallback(new ViewPager2.OnPageChangeCallback() {
            @Override
            public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {
                super.onPageScrolled(position, positionOffset, positionOffsetPixels);
            }

            @Override
            public void onPageSelected(int position) {
                super.onPageSelected(position);
                paginationAdapter.setPageCurent(position + 1);
                paginationAdapter.notifyDataSetChanged();
            }
        });

        countDownTimer = new CountDownTimer(59000, 1000) {
            public void onTick(long millisUntilFinished) {
                String time = totalTime < 10 ? "0" + totalTime : "" + totalTime;
                long second = millisUntilFinished / 1000;
                tvTime.setText(time + ":" + (second < 10 ? "0" + second : "" + second));
            }

            public void onFinish() {
                if (totalTime <= 0) {
                    finishTest();
                } else {
                    totalTime--;
                    countDownTimer.start();
                }
            }
        };
        llFinish.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finishTest();
            }
        });


        //Phân trang câu hỏi
        rvPagination = root.findViewById(R.id.rvPagination);

        fabMenuLesson.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogMenuLesson.show();
            }
        });
    }

    public void initDialogMenu() {
        dialogMenuLesson = new Dialog(getContext());
        dialogMenuLesson.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialogMenuLesson.setContentView(R.layout.popup_menu_lesson);
        dialogMenuLesson.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialogMenuLesson.getWindow().getAttributes().windowAnimations = R.style.DialogMenuLesson;
        dialogMenuLesson.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.WRAP_CONTENT);
        dialogMenuLesson.getWindow().setGravity(Gravity.RIGHT | Gravity.BOTTOM);

        rvLesson = (RecyclerView) dialogMenuLesson.findViewById(R.id.rvLesson);
        rvLesson.setLayoutManager(new GridLayoutManager(getContext(), 1));
        rvLesson.setHasFixedSize(true);

        ImageButton btCloseMenu = (ImageButton) dialogMenuLesson.findViewById(R.id.btCloseMenu);
        btCloseMenu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialogMenuLesson.hide();
            }
        });
    }

    /***
     * Lấy thông tin bài giảng theo id khóa học
     * @param id
     */
    private void getLessonByCourse(String id) {
        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/lesson/search")
                    .addQueryParameter("id", id)
                    .addQueryParameter("userId", learnerid)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<List<LessonMenuModel>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<List<LessonMenuModel>>>() {
                                }.getType());
                                lyProgress.setVisibility(View.GONE);
                                if (resultObject.statusCode.equals("1") && resultObject.data.size() > 0) {

                                    if (Utils.isEmpty(lessonCurrentId)) {
                                        lessonCurrentId = resultObject.data.get(0).id;
                                        //Lấy thông tin bài giảng theo id
                                        getLesson(lessonCurrentId);
                                    }

                                    lessonMenu = resultObject.data;
                                    categoryLessonAdapter = new CategoryLessonAdapter(getContext(), lessonMenu);
                                    rvLesson.setAdapter(categoryLessonAdapter);
                                    categoryLessonAdapter.SetOnItemClickListener(new CategoryLessonAdapter.OnItemClickListener() {
                                        @Override
                                        public void onItemClick(View view, int position, LessonMenuModel item) {
                                            lessonCurrentId = item.id;
                                            getLesson(lessonCurrentId);
                                        }
                                    });
                                } else {
                                    viewNoData.setVisibility(View.VISIBLE);
                                }
                            } catch (Exception ex) {
                                viewNoData.setVisibility(View.VISIBLE);
                            }
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

    /***
     * Lấy thông tin bài giảng theo id bài giảng
     * @param id
     */
    private void getLesson(String id) {
        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.get(Constants.ApiElearning + "api/mobile/lesson/getLessonBylessonId")
                    .addQueryParameter("lessonId", id)
                    .addQueryParameter("learnerid", learnerid)
                    .addQueryParameter("courseId", courseId)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                ResultApiModel<LessonModel> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<LessonModel>>() {
                                }.getType());
                                lyProgress.setVisibility(View.GONE);
                                if (resultObject.statusCode.equals("1")) {
                                    viewNoData.setVisibility(View.GONE);
                                    lessonCurrent = resultObject.data;

                                    if (lessonCurrent.typeLesson == Constants.Lesson_Type_Exam && !lessonCurrent.isFinish && Utils.isEmpty(lessonCurrent.startDate)) {
                                        dialogConfirmStart = Tools.showConfirm(context, "Bạn có muốn bắt đầu làm bài thi: " + lessonCurrent.name + " không?", onClickCloseConfirmStart, onClickOkConfirmStart, "Không", "Bắt đầu");
                                    } else {
                                        startExam(lessonCurrent);
                                    }
                                } else {
                                    dialogMenuLesson.hide();
                                    llTheory.setVisibility(View.GONE);
                                    llQuestion.setVisibility(View.GONE);
                                    viewNoData.setVisibility(View.VISIBLE);

                                }
                            } catch (Exception ex) {
                                dialogMenuLesson.hide();
                                viewNoData.setVisibility(View.VISIBLE);
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            dialogMenuLesson.hide();
                            viewNoData.setVisibility(View.VISIBLE);
                            lyProgress.setVisibility(View.GONE);
                        }
                    });
        } catch (Exception ex) {
            viewNoData.setVisibility(View.VISIBLE);
            lyProgress.setVisibility(View.GONE);
        }
    }

    /***
     * Lấy thông tin bài giảng theo id bài giảng
     */
    private void finishTest() {
        isExam = false;
        JSONObject jsonModel = new JSONObject();
        try {
            FinishTestModel finishTestModel = new FinishTestModel();
            finishTestModel.listQuestion = listQuestion;
            finishTestModel.lessonId = lessonCurrentId;
            finishTestModel.learnerId = learnerid;
            finishTestModel.courseId = courseId;
            jsonModel = new JSONObject(new Gson().toJson(finishTestModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }

        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.post(Constants.ApiElearning + "api/mobile/test/finish-test")
                    .addJSONObjectBody(jsonModel)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                lyProgress.setVisibility(View.GONE);
                                ResultApiModel<TestResultModel> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<TestResultModel>>() {
                                }.getType());
                                if (resultObject.statusCode.equals("1")) {
                                    llTimeView.setVisibility(View.GONE);
                                    llTimeView.setVisibility(View.GONE);
                                    countDownTimer.cancel();
                                    getLesson(lessonCurrentId);
                                    TestResultModel testResult = resultObject.data;

                                    dialogNotify = Tools.showCustomDialog(context, "Đã hoàn thành bài thi", "Bạn đã trả lời đúng " + testResult.totalRightAnswer + "/" + testResult.totalQuestion + " câu hỏi.", onClickCloseNotify);
                                }
                            } catch (Exception ex) {

                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                            lyProgress.setVisibility(View.GONE);
                        }
                    });
        } catch (Exception ex) {
            lyProgress.setVisibility(View.GONE);
        }
    }

    /***
     * Lưu tạm câu trả lời của câu hỏi
     */
    private void saveTemp(AnswerLearnerTempModel dataTemp) {
        JSONObject jsonModel = new JSONObject();
        try {
            jsonModel = new JSONObject(new Gson().toJson(dataTemp));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        try {
            AndroidNetworking.post(Constants.ApiElearning + "api/mobile/test/save-temp")
                    .addJSONObjectBody(jsonModel)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                        }

                        @Override
                        public void onError(ANError anError) {
                        }
                    });
        } catch (Exception ex) {
        }
    }

    /***
     * Lưu lịch sử bài học
     */
    private void saveLessonHistory() {
        JSONObject jsonModel = new JSONObject();
        try {
            LessonHistoryModel lessonHistory = new LessonHistoryModel();
            lessonHistory.CourseId = courseId;
            lessonHistory.LearnerId = learnerid;
            lessonHistory.LessonId = lessonCurrentId;
            jsonModel = new JSONObject(new Gson().toJson(lessonHistory));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        try {
            AndroidNetworking.post(Constants.ApiElearning + "api/mobile/lesson/create-lesson-history")
                    .addJSONObjectBody(jsonModel)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            ResultApiModel<String> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<String>>() {
                            }.getType());
                            if (resultObject.statusCode.equals("1")) {
                                for (LessonMenuModel itemLesson : lessonMenu) {
                                    if (itemLesson.id.equals(lessonCurrentId)) {
                                        itemLesson.status = true;
                                        break;
                                    }
                                }
                                categoryLessonAdapter.notifyDataSetChanged();
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                        }
                    });
        } catch (Exception ex) {
        }
    }

    private void startExam(LessonModel lessonModel) {
        tvTitle.setText(lessonModel.name);
        llTimeView.setVisibility(View.GONE);
        if (lessonModel.typeLesson == Constants.Lesson_Type_Exam || lessonModel.typeLesson == Constants.Lesson_Type_Study) {
            imbComment.setVisibility(View.GONE);
            //Hiển thị số câu hỏi
            paginationAdapter = new PaginationAdapter(getContext(), lessonModel.listQuestion.size(), 1);
            rvPagination.setAdapter(paginationAdapter);
            paginationAdapter.SetOnItemClickListener(new PaginationAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(int pageOld, int position) {
                    AnswerLearnerTempModel answerLearnerTemp = new AnswerLearnerTempModel();
                    answerLearnerTemp.courseId = courseId;
                    answerLearnerTemp.learnerId = learnerid;
                    answerLearnerTemp.lessonId = lessonCurrentId;
                    QuestionModel questionModel = listQuestion.get(pageOld - 1);
                    answerLearnerTemp.questionId = questionModel.questionId;
                    answerLearnerTemp.listAnswer = questionModel.listAnswer;
                    saveTemp(answerLearnerTemp);

                    vpQuestion.setCurrentItem(position - 1);
                    paginationAdapter.notifyDataSetChanged();
                }
            });

            lyPage.setVisibility(View.VISIBLE);
            lySubmit.setVisibility(lessonModel.isFinish ? View.GONE : View.VISIBLE);
            llTheory.setVisibility(View.GONE);
            llQuestion.setVisibility(View.VISIBLE);

            listQuestion = lessonModel.listQuestion;
            questionAdapter = new QuestionAdapter(context, listQuestion, lessonModel.isFinish);
            vpQuestion.setAdapter(questionAdapter);
            vpQuestion.setCurrentItem(0);
            vpQuestion.setOffscreenPageLimit(1);
            tvTitle.setText("Bài trắc nghiệm");
            if (lessonModel.typeLesson == Constants.Lesson_Type_Exam && !lessonModel.isFinish && lessonModel.remainingTime > 0) {
                tvTitle.setText("Bài thi");
                llTimeView.setVisibility(View.VISIBLE);
                totalTime = (int) lessonModel.remainingTime;
                countDownTimer.start();
                isExam = true;
            } else if (lessonModel.typeLesson == Constants.Lesson_Type_Exam && !lessonModel.isFinish && lessonModel.remainingTime <= 0) {
                tvTitle.setText("Bài thi");
                llTimeView.setVisibility(View.GONE);
                totalTime = 0;
                finishTest();
            }

            //Bài thi kết thúc
            if (lessonModel.isFinish) {
                lyFinishResult.setVisibility(View.VISIBLE);
                tvTotalCorrect.setText(String.valueOf(lessonModel.totalCorrect));
                tvTotalQuestion.setText(String.valueOf(lessonModel.totalQuestion));
            } else {
                lyFinishResult.setVisibility(View.GONE);
            }
        } else {
            tvTitle.setText("Bài giảng");
            imbComment.setVisibility(View.VISIBLE);
            lyPage.setVisibility(View.GONE);
            lySubmit.setVisibility(View.GONE);
            llTheory.setVisibility(View.VISIBLE);
            llQuestion.setVisibility(View.GONE);

            txtLessonName.setText(lessonModel.name);

            wvLessonContent.setWebChromeClient(new ChromeClient(getActivity()));
            wvLessonContent.getSettings().setPluginState(WebSettings.PluginState.ON_DEMAND);
            wvLessonContent.getSettings().setDomStorageEnabled(true);
            wvLessonContent.getSettings().setJavaScriptEnabled(true);
            wvLessonContent.setBackgroundColor(Color.TRANSPARENT);

            wvLessonContent.setWebViewClient(new WebViewClient() {
                public void onPageFinished(WebView view, String url) {
                    wvLessonContent.loadUrl("javascript:(function() { document.getElementsByTagName('video')[0].play(); })()");
                }
            });

            String content = "<body style=\"padding: 10px; background-color:transparent\">" + lessonModel.content + "</body>";

            wvLessonContent.loadData(content, "text/html; charset=utf-8", "UTF-8");

        }
        dialogMenuLesson.hide();

        //Lưu lịch sử bài học
        saveLessonHistory();
    }

    /***
     * Đóng notify
     */
    private View.OnClickListener onClickCloseNotify = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            if (dialogNotify != null)
                dialogNotify.hide();
        }
    };

    /***
     * Đóng notify
     */
    private View.OnClickListener onClickCloseConfirmStart = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            if (dialogConfirmStart != null)
                dialogConfirmStart.hide();
            lyPage.setVisibility(View.GONE);
            lySubmit.setVisibility(View.GONE);
            llTheory.setVisibility(View.GONE);
            llQuestion.setVisibility(View.GONE);
        }
    };

    /***
     * Đóng notify
     */
    private View.OnClickListener onClickOkConfirmStart = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            if (dialogConfirmStart != null)
                dialogConfirmStart.hide();
            startExam(lessonCurrent);
        }
    };

    /***
     * Đóng notify
     */
    private View.OnClickListener onClickCloseConfirmFinish = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            if (dialogConfirmFinish != null)
                dialogConfirmFinish.hide();
        }
    };

    /***
     * Đóng notify
     */
    private View.OnClickListener onClickOkConfirmFinish = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            if (dialogConfirmFinish != null)
                dialogConfirmFinish.hide();
            finishTest();
        }
    };
}
