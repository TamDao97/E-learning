import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Configuration } from 'src/app/shared';
import { Observable } from 'rxjs';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const httpOptionsJson = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
    providedIn: 'root'
})
export class CourseService {

    constructor(
        private http: HttpClient,
        private config: Configuration
    ) { }

    // Tạo mới khóa học
    createUpdateCourse(model): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'courses', model, httpOptions);
        return tr
    }

    //Lấy danh sách các giảng viên của khóa học
    searchMentor(id): Observable<any> {
        var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'courses/search-mentor/' + id, httpOptions);
        return tr
    }

    // Tìm kiếm khóa học
    searchCourse(model): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'courses/search', model, httpOptions);
        return tr
    }

    // Xóa khóa học
    deleteCourse(id: string): Observable<any> {
        var tr = this.http.delete<any>(this.config.ServerWithApiUrl + 'courses/' + id, httpOptions);
        return tr
    }

    updateStatusCourse(id: string): Observable<any> {
        var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'courses/update-status/' + id, httpOptions);
        return tr
    }

    getListLesson(model): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'courses/search-lesson', model, httpOptions);
        return tr
    }
    // Tạo mới khóa học
    createCourse(model): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'courses', model, httpOptions);
        return tr
    }

    // Lấy thông tin khóa học
    getCourseInfo(id: string): Observable<any> {
        var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'courses/' + id, httpOptions);
        return tr
    }

    getListOrder(): Observable<any> {
        return this.http.get<any>(this.config.ServerWithApiUrl + 'courses/getListOrder');
    }

    // Cập nhật khóa học
    updateCourse(id, model): Observable<any> {
        var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'courses/' + id, model, httpOptions);
        return tr
    }

    searchLessonByCourseId(courseId): Observable<any> {
        var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'courses/' + courseId + '/search-lesson-by-courseid', httpOptions);
        return tr
    }

    //Lấy danh sách người học
    searchLearner(id): Observable<any> {
        var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'courses/search-learner/' + id, httpOptions);
        return tr
    }

    //Lấy danh sách người học
    getProgress(model): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'courses/progress', model, httpOptions);
        return tr
    }

    getEmployeeCourse(programId): Observable<any> {
        var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'courses/get-employee-course/' + programId, httpOptions);
        return tr
    }

    //Lấy danh sách người học
    getTestResult(model): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'courses/test-result', model, httpOptions);
        return tr
    }
    getQuestion(testId): Observable<any> {
        var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'courses/detail-result/' + testId, httpOptions);
        return tr
    }

    getFileTemplates(): Observable<any> {
        var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'courses/file-templates', httpOptions);
        return tr
    }

    printCertificate(model): Observable<any> {
        var tr = this.http.post<any>(this.config.ServerWithApiUrl + 'courses/print-certificate', model, httpOptions);
        return tr
    }

    requestCourse(id: string, model: any): Observable<any> {
        var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'courses/request/' + id, model, httpOptions);
        return tr;
    }

    approvalCourse(id: string, model: any): Observable<any> {
        var tr = this.http.put<any>(this.config.ServerWithApiUrl + 'courses/approval/' + id, model, httpOptions);
        return tr;
    }

    approvalHistiry(id: string): Observable<any> {
        var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'courses/approval-histiry/' + id, httpOptions);
        return tr;
    }

    exam(id: string): Observable<any> {
        var tr = this.http.get<any>(this.config.ServerWithApiUrl + 'courses/exam/' + id, httpOptions);
        return tr;
    }
}
