package nts.swipesafe;

import android.content.Intent;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageButton;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;
import androidx.viewpager.widget.ViewPager;

import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.android.material.tabs.TabLayout;

import nts.swipesafe.adapter.SectionPagerCourseAdapter;
import nts.swipesafe.common.Constants;
import nts.swipesafe.common.Tools;
import nts.swipesafe.fragment.CourseFragment;
import nts.swipesafe.fragment.CourseGeneralInfoTabFragment;
import nts.swipesafe.fragment.DialogCommentFragment;
import nts.swipesafe.fragment.LessonTabFragment;

public class ActivityTabCourse extends AppCompatActivity {
    private ViewPager view_pager_course;
    private TabLayout tab_layout;
    private String courseName, courseId;
    private Intent intent;
    private FloatingActionButton fabChat;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_tab_course);
        intent = getIntent();
        initToolbar();
        initComponent();
    }

    private void initToolbar() {
        if (intent != null) {
            courseName = intent.getStringExtra("courseName");
            courseId = intent.getStringExtra("courseId");
        }
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        toolbar.setNavigationIcon(R.drawable.ic_back_elearning);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle(courseName);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        Tools.setSystemBarColor(this);
    }

    private void initComponent() {
        view_pager_course = (ViewPager) findViewById(R.id.view_pager_course);
        setupViewPager(view_pager_course);
        tab_layout = (TabLayout) findViewById(R.id.tab_layout);
        tab_layout.setupWithViewPager(view_pager_course);
        fabChat = (FloatingActionButton) findViewById(R.id.fabChat);

        fabChat.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                final FragmentManager fragmentManager = getSupportFragmentManager();
                DialogFragment dialogFragment = DialogCommentFragment.newInstance(courseId, "", Constants.Comment_Course_Type, courseName);
                FragmentTransaction transaction = fragmentManager.beginTransaction();
                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
                transaction.add(R.id.drawer_layout, dialogFragment).addToBackStack(null).commit();
            }
        });
    }

    private void setupViewPager(ViewPager viewPager) {
        SectionPagerCourseAdapter adapter = new SectionPagerCourseAdapter(getSupportFragmentManager(), 0);
        adapter.addFragment(CourseGeneralInfoTabFragment.newInstance(courseId), "Thông tin chung");
        adapter.addFragment(LessonTabFragment.newInstance(courseId), "Bài giảng");
        view_pager_course.setAdapter(adapter);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (item.getItemId() == android.R.id.home) {
            finish();
        }
        return super.onOptionsItemSelected(item);
    }
}
