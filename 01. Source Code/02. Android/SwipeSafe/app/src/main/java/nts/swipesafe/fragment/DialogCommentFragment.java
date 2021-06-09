package nts.swipesafe.fragment;

import android.app.Activity;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.widget.AppCompatButton;
import androidx.fragment.app.DialogFragment;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.androidnetworking.AndroidNetworking;
import com.androidnetworking.common.Priority;
import com.androidnetworking.error.ANError;
import com.androidnetworking.interfaces.JSONObjectRequestListener;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.squareup.picasso.Callback;
import com.squareup.picasso.Picasso;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.adapter.CommentChatAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.Tools;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.CommentModel;
import nts.swipesafe.model.FinishTestModel;
import nts.swipesafe.model.LessonHistoryModel;
import nts.swipesafe.model.LessonMenuModel;
import nts.swipesafe.model.MobileCommentResultModel;
import nts.swipesafe.model.MobileCommentSearchModel;
import nts.swipesafe.model.ResultApiModel;
import nts.swipesafe.model.SearchModel;
import nts.swipesafe.model.TestResultModel;

public class DialogCommentFragment extends DialogFragment {
    private View root;
    public static Activity context;
    private String lessonId = "", courseId = "", learnerid = "", title = "", userName, avatar;
    private RecyclerView rvComment;
    private TextView txtTitle, tvUserName, tvLetter,tvContentReply;
    private ImageView imgAvatar,ivRemoveReply;
    private AppCompatButton btSend;
    private EditText txtComment;
    private Long parentCommentId = null;
    private int type = 1;
    private SharedPreferences sharedPreferencesLogin;
    private LinearLayout lyProgress,llContentReply;
    private List<MobileCommentResultModel> listComment;

    public static DialogCommentFragment newInstance(String iCourseId, String iLessonId, int iType, String iTitle) {
        DialogCommentFragment fragment = new DialogCommentFragment();
        fragment.courseId = iCourseId;
        fragment.lessonId = iLessonId;
        fragment.type = iType;
        fragment.title = iTitle;
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        root = inflater.inflate(R.layout.dialog_comment, container, false);

        sharedPreferencesLogin = getActivity().getSharedPreferences(Constants.KeyInfoLogin, getActivity().MODE_PRIVATE);
        learnerid = sharedPreferencesLogin.getString(Constants.KeyInfoLoginId, "");
        userName = sharedPreferencesLogin.getString(Constants.KeyInfoLoginName, "");
        avatar = sharedPreferencesLogin.getString(Constants.KeyInfoLoginAvatar, "");

        initToolbar();

        initComponent();

        getComment();
        // Inflate the layout for this fragment
        return root;
    }

    private void initToolbar() {
        TextView tvTitle = (TextView) root.findViewById(R.id.tvTitle);
        ImageButton imbClose = (ImageButton) root.findViewById(R.id.imbClose);
        tvTitle.setText("Hỏi đáp");
        imbClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dismiss();
            }
        });
    }

    private void initComponent() {
        context = getActivity();

        rvComment = root.findViewById(R.id.rvComment);
        rvComment.setLayoutManager(new GridLayoutManager(context, 1));
        rvComment.setHasFixedSize(true);

        txtTitle = root.findViewById(R.id.txtTitle);
        tvUserName = root.findViewById(R.id.tvUserName);
        tvLetter = root.findViewById(R.id.tvLetter);
        imgAvatar = root.findViewById(R.id.imgAvatar);
        btSend = root.findViewById(R.id.btSend);
        txtComment = root.findViewById(R.id.txtComment);
        lyProgress = root.findViewById(R.id.lyProgress);
        llContentReply = root.findViewById(R.id.llContentReply);
        tvContentReply = root.findViewById(R.id.tvContentReply);
        ivRemoveReply = root.findViewById(R.id.ivRemoveReply);

        txtTitle.setText(title);
        if (Utils.isEmpty(avatar)) {
            imgAvatar.setImageResource(R.drawable.shape_circle_grey);
            if (!Utils.isEmpty(userName)) {
                String[] name = userName.toUpperCase().split(" ");
                tvLetter.setText((name[0].substring(0, 1) + name[name.length - 1].substring(0, 1)));
            }
        } else {
            Picasso.with(getContext())
                    .load(avatar)
                    .resize(40, 40)
                    .error(R.drawable.shape_circle_grey)
                    .centerCrop().into(imgAvatar, new Callback() {
                @Override
                public void onSuccess() {

                }

                @Override
                public void onError() {
                    if (!Utils.isEmpty(userName)) {
                        String[] name = userName.toUpperCase().split(" ");
                        tvLetter.setText((name[0].substring(0, 1) + name[name.length - 1].substring(0, 1)));
                    }
                }
            });
        }
        tvUserName.setText(userName);

        btSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                sendComment();
            }
        });

        ivRemoveReply.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                llContentReply.setVisibility(View.GONE);
                tvContentReply.setText("");
            }
        });
    }

    /***
     * Lấy danh sách comment
     */
    private void getComment() {
        JSONObject jsonModel = new JSONObject();
        try {
            MobileCommentSearchModel commentSearch = new MobileCommentSearchModel();
            commentSearch.lessonId = lessonId;
            commentSearch.userId = learnerid;
            commentSearch.courseId = courseId;
            commentSearch.type = type;
            jsonModel = new JSONObject(new Gson().toJson(commentSearch));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }

        lyProgress.setVisibility(View.VISIBLE);
        try {
            AndroidNetworking.post(Constants.ApiElearning + "api/mobile/comment/search")
                    .addJSONObjectBody(jsonModel)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            try {
                                lyProgress.setVisibility(View.GONE);
                                ResultApiModel<SearchModel<List<MobileCommentResultModel>>> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<SearchModel<List<MobileCommentResultModel>>>>() {
                                }.getType());
                                if (resultObject.statusCode.equals("1")) {
                                    listComment = resultObject.data.dataResults;
                                    CommentChatAdapter commentChatAdapter = new CommentChatAdapter(context, listComment);
                                    rvComment.setAdapter(commentChatAdapter);

                                    commentChatAdapter.SetOnItemClickListener(new CommentChatAdapter.OnItemClickListener() {
                                        @Override
                                        public void onReplyClick(View view, int position, MobileCommentResultModel comment) {
                                            parentCommentId = comment.id;
                                            llContentReply.setVisibility(View.VISIBLE);
                                            tvContentReply.setText(comment.content);
                                        }
                                    });
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
     * Gửi hỏi đáp
     */
    private void sendComment() {
        if (Utils.isEmpty(txtComment.getText().toString())) {
            Toast.makeText(context, "Hãy nhập nội dung hỏi đáp để gửi đến chúng tôi.", Toast.LENGTH_LONG).show();
            return;
        }

        JSONObject jsonModel = new JSONObject();
        try {
            CommentModel commentModel = new CommentModel();
            commentModel.courseId = courseId;
            commentModel.learnerId = learnerid;
            commentModel.lessonId = lessonId;
            commentModel.parentCommentId = parentCommentId;
            commentModel.type = type;
            commentModel.content = txtComment.getText().toString();
            jsonModel = new JSONObject(new Gson().toJson(commentModel));
        } catch (JSONException e) {
            // MessageUtils.Show(getActivity(), e.getMessage());
        }
        try {
            AndroidNetworking.post(Constants.ApiElearning + "api/mobile/comment/create-comment")
                    .addJSONObjectBody(jsonModel)
                    .setPriority(Priority.MEDIUM)
                    .build()
                    .getAsJSONObject(new JSONObjectRequestListener() {
                        @Override
                        public void onResponse(JSONObject response) {
                            ResultApiModel<String> resultObject = new Gson().fromJson(response.toString(), new TypeToken<ResultApiModel<String>>() {
                            }.getType());
                            if (resultObject.statusCode.equals(Constants.Status_Success)) {
                                parentCommentId = null;
                                txtComment.setText("");
                                llContentReply.setVisibility(View.GONE);
                                tvContentReply.setText("");
                                getComment();
                            }
                        }

                        @Override
                        public void onError(ANError anError) {
                        }
                    });
        } catch (Exception ex) {
        }
    }
}
