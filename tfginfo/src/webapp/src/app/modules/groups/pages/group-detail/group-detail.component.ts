import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkingGroupBase } from '../../models/group.model';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { GroupService } from '../../services/group-service';
import { TfgListComponent } from '../../../tfg/components/tfg-list/tfg-list.component';
import { ProfessorListComponent } from '../../../professor/components/profesor-list/professor-list.component';
import { ProfessorDTO } from '../../../professor/models/professor.model';
import { RoleId } from '../../../admin/models/role.model';
import { StudentDTO } from '../../../admin/models/student.model';
import { forkJoin } from 'rxjs';
import { StudentListComponent } from '../../../admin/components/student-list/student-list.component';

@Component({
    selector: 'group-detail',
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        MatCheckboxModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatButtonModule,
        CommonModule,
        ProfessorListComponent,
        StudentListComponent
    ],
    templateUrl: './group-detail.component.html',
    styleUrls: ['./group-detail.component.scss']
})
export class GroupDetailComponent implements OnInit {
    id: string | null = null;
    group: WorkingGroupBase | null = null;
    professors: ProfessorDTO[] = [];
    professorColumns: string[] = ['name','surname', 'email'];
    students: StudentDTO[] = [];
    studentColumns: string[] = ['name', 'surname', 'email'];
    creation: boolean = false;
    groupForm!: FormGroup;
    canEdit: boolean = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private groupService: GroupService,
        private fb: FormBuilder
    ) { }

    ngOnInit(): void {
        let role = Number.parseInt(localStorage.getItem('role')!);
        this.canEdit = role === RoleId.Admin || role === RoleId.Professor;
        this.id = this.route.snapshot.paramMap.get('id');
        if (this.id !== "new" && isNaN(Number(this.id))) {
            this.router.navigate(['/']);
        }
        this.creation = this.id === "new";

        this.groupForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
            description: ['', Validators.required],
            isPrivate: [false, Validators.required]
        });

        if (!this.canEdit) {
            this.groupForm.disable();
        }

        if (!this.creation) {
            this.groupService.getGroup(+this.id!).subscribe((data) => {
                this.group = data;
                this.groupForm.patchValue(data);
            });

            forkJoin([
                this.groupService.getGroupStudents(+this.id!),
                this.groupService.getGroupProfessors(+this.id!)
            ]).subscribe(([students, professors]) => {
                this.students = students;
                this.professors = professors;
            });
        }
    }

    onSubmit(): void {
        if (this.groupForm.valid) {
            const groupData = this.groupForm.value;
            if (this.creation) {
                this.groupService.createGroup(groupData).subscribe(() => this.router.navigate(['/working-group']));
            } else {
                this.groupService.updateGroup(groupData).subscribe(() => this.router.navigate(['/working-group']));
            }
        }
    }

    onCancel(): void {
        this.router.navigate(['/working-group']);
    }
}