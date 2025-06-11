import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { Injectable } from "@angular/core";
import { NewStudentDTO, StudentDTO, StudentFlatDTO, StudentOptionalData } from "../models/student.model";
import { Filter } from "../../../core/core.model";

@Injectable({
    providedIn: 'root' // Esto permite que Angular maneje la inyección globalmente
})
export class StudentService extends BaseService {
  
    constructor(
        protected override http: HttpClient,
        protected override appConfigService: AppConfigService
    ) {
        super(http, appConfigService);
    }

    getStudents(): Observable<StudentDTO[]> {
        return this.get(`${this.url}/student`);
    }

    searchStudents(filters: Filter[]): Observable<StudentDTO[]> {
        return this.post(`${this.url}/student/search`, filters);
    }

    deleteStudent(id: number): Observable<any> {
        return this.delete(`${this.url}/student/${id}`);
    }

    getStudent(id: number): Observable<StudentDTO> {
        return this.get(`${this.url}/student/${id}`);
    }

    createStudent(student: StudentFlatDTO): Observable<NewStudentDTO> {
        return this.post(`${this.url}/student`, student);
    }

    updateStudent(student: StudentFlatDTO): Observable<StudentDTO> {
        return this.put(`${this.url}/student`, student);
    }

    updateStudentOptionalData(studentOptionalData: StudentOptionalData, id: number): Observable<StudentDTO> {
        return this.put(`${this.url}/student/${id}/optional-data`, studentOptionalData);
    }
}