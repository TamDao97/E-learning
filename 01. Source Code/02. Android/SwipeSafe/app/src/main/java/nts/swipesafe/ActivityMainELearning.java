package nts.swipesafe;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.fragment.app.Fragment;

import nts.swipesafe.R;
import nts.swipesafe.common.Tools;
import nts.swipesafe.fragment.CourseFragment;
import nts.swipesafe.fragment.FragmentSearchCourse;
import nts.swipesafe.fragment.ProgramInfoFragment;
import nts.swipesafe.fragment.UserFragment;
import nts.swipesafe.fragment.LessonFragment;

public class ActivityMainELearning extends AppCompatActivity {
    private Toolbar toolbar;
    private ActionBar actionBar;
    private LinearLayout llCourse, ll111, llSearchCourse, llUser;
    private Fragment fragment = null;
    private Activity thisActivity;
    private ImageView imageButtonCourse, imageButton111, imageButtonSearchCourse, imageButtonUser;
    private TextView txtButtonCourse, txtButton111, txtButtonSearchCourse, txtButtonUser;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_elearning);
        thisActivity = this;
        initComponent();
        initToolbar();
    }

    private void initComponent() {
        llCourse = findViewById(R.id.llCourse);
        ll111 = findViewById(R.id.ll111);
        llSearchCourse = findViewById(R.id.llSearchCourse);
        llUser = findViewById(R.id.llUser);

        imageButtonCourse = findViewById(R.id.imageButtonCourse);
        txtButtonCourse = findViewById(R.id.txtButtonCourse);
        imageButton111 = findViewById(R.id.imageButton111);
        txtButton111 = findViewById(R.id.txtButton111);
        imageButtonSearchCourse = findViewById(R.id.imageButtonSearchCourse);
        txtButtonSearchCourse = findViewById(R.id.txtButtonSearchCourse);
        imageButtonUser = findViewById(R.id.imageButtonUser);
        txtButtonUser = findViewById(R.id.txtButtonUser);
    }

    private void initToolbar() {
        toolbar = (Toolbar) findViewById(R.id.toolbar);
        toolbar.setNavigationIcon(R.drawable.ic_back_elearning);
        setSupportActionBar(toolbar);
        actionBar = getSupportActionBar();
        actionBar.setTitle("Khoá học");
        actionBar.setDisplayHomeAsUpEnabled(true);
        Tools.setSystemBarColor(this);
        fragment = new CourseFragment();
        Tools.displayFragment(thisActivity, fragment, null);
        imageButtonCourse.setColorFilter(getResources().getColor(R.color.colorAccentDark));
        txtButtonCourse.setTextColor(getResources().getColor(R.color.colorAccentDark));
    }

    public void clickAction(View view) {
        int menuId = view.getId();
        switch (menuId) {
            case R.id.llCourse:
                actionBar.setTitle("Khoá học");
                fragment = new CourseFragment();
                Tools.displayFragment(thisActivity, fragment, null);
                imageButtonCourse.setColorFilter(getResources().getColor(R.color.colorAccentDark));
                txtButtonCourse.setTextColor(getResources().getColor(R.color.colorAccentDark));
                imageButton111.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButton111.setTextColor(getResources().getColor(R.color.grey_600));
                imageButtonSearchCourse.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButtonSearchCourse.setTextColor(getResources().getColor(R.color.grey_600));
                imageButtonUser.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButtonUser.setTextColor(getResources().getColor(R.color.grey_600));
                break;
            case R.id.ll111:
                imageButton111.setColorFilter(getResources().getColor(R.color.colorAccentDark));
                txtButton111.setTextColor(getResources().getColor(R.color.colorAccentDark));
                imageButtonCourse.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButtonCourse.setTextColor(getResources().getColor(R.color.grey_600));
                imageButtonSearchCourse.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButtonSearchCourse.setTextColor(getResources().getColor(R.color.grey_600));
                imageButtonUser.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButtonUser.setTextColor(getResources().getColor(R.color.grey_600));
                actionBar.setTitle("Bài giảng");
                fragment = new LessonFragment();
                finish();
                Intent intent = new Intent(getApplication(), MainActivity.class);
                startActivity(intent);
                break;
            case R.id.llSearchCourse:
                imageButtonSearchCourse.setColorFilter(getResources().getColor(R.color.colorAccentDark));
                txtButtonSearchCourse.setTextColor(getResources().getColor(R.color.colorAccentDark));
                imageButtonCourse.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButtonCourse.setTextColor(getResources().getColor(R.color.grey_600));
                imageButton111.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButton111.setTextColor(getResources().getColor(R.color.grey_600));
                imageButtonUser.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButtonUser.setTextColor(getResources().getColor(R.color.grey_600));
                fragment = new FragmentSearchCourse();
                Tools.displayFragment(thisActivity, fragment, null);
                break;
            case R.id.llUser:
                imageButtonUser.setColorFilter(getResources().getColor(R.color.colorAccentDark));
                txtButtonUser.setTextColor(getResources().getColor(R.color.colorAccentDark));
                imageButtonSearchCourse.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButtonSearchCourse.setTextColor(getResources().getColor(R.color.grey_600));
                imageButtonCourse.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButtonCourse.setTextColor(getResources().getColor(R.color.grey_600));
                imageButton111.setColorFilter(getResources().getColor(R.color.grey_600));
                txtButton111.setTextColor(getResources().getColor(R.color.grey_600));
                actionBar.setTitle("Người dùng");
                fragment = new UserFragment();
                Tools.displayFragment(thisActivity, fragment, null);
                break;
        }
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (item.getItemId() == android.R.id.home) {
            Intent intent = new Intent(getApplication(), MainActivity.class);
            startActivity(intent);
            finish();
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        for (Fragment fragment : getSupportFragmentManager().getFragments()) {
            fragment.onActivityResult(requestCode, resultCode, data);
        }
    }
}
