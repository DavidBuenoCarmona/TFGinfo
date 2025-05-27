import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { WorkingGroupBase, WorkingGroupMessage, WorkingGroupProfessor } from "../models/group.model";
import { ProfessorDTO } from "../../professor/models/professor.model";
import { StudentDTO } from "../../admin/models/student.model";
import { Filter } from "../../../core/core.model";

@Injectable({
    providedIn: 'root' // Esto permite que Angular maneje la inyección globalmente
})
export class GroupService extends BaseService {

    constructor(
        protected override http: HttpClient,
        protected override appConfigService: AppConfigService
    ) {
        super(http, appConfigService);
    }

    getGroups(): Observable<WorkingGroupBase[]> {
        return this.get(`${this.url}/working-group`);
    }

    getGroup(id: number): Observable<WorkingGroupBase> {
        return this.get(`${this.url}/working-group/${id}`);
    }

    createGroup(group: WorkingGroupProfessor): Observable<WorkingGroupBase> {
        return this.post(`${this.url}/working-group`, group);
    }

    updateGroup(group: WorkingGroupBase): Observable<WorkingGroupBase> {
        return this.put(`${this.url}/working-group`, group);
    }

    deleteGroup(id: number): Observable<any> {
        return this.delete(`${this.url}/working-group/${id}`);
    }

    getGroupProfessors(id: number): Observable<ProfessorDTO[]> {
        return this.get(`${this.url}/working-group/${id}/professor`);
    }

    getGroupStudents(id: number): Observable<StudentDTO[]> {
        return this.get(`${this.url}/working-group/${id}/student`);
    }

    getGroupByProfessor(id: number): Observable<WorkingGroupBase[]> {
        return this.get(`${this.url}/working-group/professor/${id}`);
    }

    getGroupByStudent(id: number): Observable<WorkingGroupBase[]> {
        return this.get(`${this.url}/working-group/student/${id}`);
    }

    addStudentToGroup(working_group: number, user: number): Observable<any> {
        return this.post(`${this.url}/working-group/add-student`, {working_group, user});
    }
    
    addStudentToGroupByEmail(working_group: number, email: string): Observable<StudentDTO> {
        return this.post(`${this.url}/working-group/${working_group}/add-student/${email}`, {});
    }

    removeStudentFromGroup(working_group: number, user: number): Observable<any> {
        return this.post(`${this.url}/working-group/remove-student`, {working_group, user});
    }

    addProfessorToGroup(working_group: number, user: number): Observable<any> {
        return this.post(`${this.url}/working-group/add-professor`, {working_group, user});
    }

    removeProfessorFromGroup(working_group: number, user: number): Observable<any> {
        return this.post(`${this.url}/working-group/remove-professor`, {working_group, user});
    }

    sendMessage(content: WorkingGroupMessage): Observable<any> {
        return this.post(`${this.url}/working-group/send-message`, content);
    }

    searchGroups(filters: Filter[]): Observable<WorkingGroupBase[]> {
        return this.post(`${this.url}/working-group/search`, filters);
    }
}