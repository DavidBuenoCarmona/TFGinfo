import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkingGroupBase, WorkingGroupMessage, WorkingGroupProfessor } from '../../models/group.model';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { CommonModule, Location } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { GroupService } from '../../services/group-service';
import { TfgListComponent } from '../../../tfg/components/tfg-list/tfg-list.component';
import { ProfessorListComponent } from '../../../professor/components/profesor-list/professor-list.component';
import { ProfessorDTO } from '../../../professor/models/professor.model';
import { RoleId } from '../../../admin/models/role.model';
import { StudentDTO } from '../../../admin/models/student.model';
import { forkJoin } from 'rxjs';
import { StudentListComponent } from '../../../admin/components/student-list/student-list.component';
import { ProfessorService } from '../../../professor/services/professor.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { MatIconModule } from '@angular/material/icon';

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
        MatDialogModule,
        MatButtonModule,
        CommonModule,
        ProfessorListComponent,
        StudentListComponent,
        CommonModule,
        MatIconModule
    ],
    templateUrl: './group-detail.component.html',
    styleUrls: ['./group-detail.component.scss']
})
export class GroupDetailComponent implements OnInit {
    id: string | null = null;
    group: WorkingGroupBase | null = null;
    professors: ProfessorDTO[] = [];
    professorColumns: string[] = ['name', 'surname', 'email'];
    students: StudentDTO[] = [];
    studentColumns: string[] = ['name', 'surname', 'email'];
    creation: boolean = false;
    groupForm!: FormGroup;
    canEdit: boolean = false;
    isAdmin: boolean = false;
    userAlreadyInGroup: boolean = false;
    professorList: ProfessorDTO[] = [];
    professor: number | null = null;

    constructor(
        private route: ActivatedRoute,
        private dialog: MatDialog,
        private router: Router,
        private groupService: GroupService,
        private fb: FormBuilder,
        private professorService: ProfessorService,
        private location: Location
    ) { }

    ngOnInit(): void {
        let role = Number.parseInt(localStorage.getItem('role')!);
        this.canEdit = role === RoleId.Admin || role === RoleId.Professor;
        this.isAdmin = role === RoleId.Admin;
        if (this.canEdit) {
            this.studentColumns = ['name', 'surname', 'email', 'actions'];
        }
        if (this.isAdmin) {
            this.professorColumns = ['name', 'surname', 'email', 'actions'];
        }
        this.id = this.route.snapshot.paramMap.get('id');
        if (this.id !== "new" && isNaN(Number(this.id))) {
            this.router.navigate(['/']);
        }
        this.creation = this.id === "new";

        this.groupForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
            description: ['', Validators.required],
            isPrivate: [false, Validators.required],
            newStudentEmail: ['', Validators.email],
            message: ['']
        });

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
                const user = localStorage.getItem('user');
                if (user) {
                    const userData = JSON.parse(user);
                    if (!this.canEdit) {
                        this.userAlreadyInGroup = this.students.some(student => student.id === userData.id);
                    } else {
                        this.userAlreadyInGroup = this.professors.some(professor => professor.id === userData.id);
                    }
                }
                this.canEdit = (this.canEdit && this.userAlreadyInGroup) || this.isAdmin;
                if (!this.canEdit) {
                    this.groupForm.disable();
                }
            });
        } else if (this.isAdmin) {
            this.professorService.getProfessors().subscribe((professors) => {
                this.professorList = professors;
            });
        } else if (!this.isAdmin && this.canEdit) {
            const user = localStorage.getItem('user');
            if (user) {
                const userData = JSON.parse(user);
                this.professor = userData.id;
            }

        }
    }

    onSubmit(): void {
        if (this.groupForm.valid) {
            const groupData = this.groupForm.value;
            if (this.creation) {
                var body: WorkingGroupProfessor = {
                    working_group: groupData,
                    professor: this.professor!
                };
                this.groupService.createGroup(body).subscribe(() => this.location.back());
            } else {
                this.groupService.updateGroup(groupData).subscribe(() => this.location.back());
            }
        }
    }

    cambiarProfessor(event: number): void {
        this.professor = event;
    }

    onCancel(): void {
        this.location.back();
    }

    onJoin(): void {
        let role = Number.parseInt(localStorage.getItem('role')!);
        const user = localStorage.getItem('user');
        if (user) {
            const userData = JSON.parse(user);
            if (role === RoleId.Professor) {
                this.groupService.addProfessorToGroup(this.group!.id!, userData.id).subscribe(() => {
                    this.router.navigate(['/working-group']);
                });
            } else if (role === RoleId.Student) {
                this.groupService.addStudentToGroup(this.group!.id!, userData.id).subscribe(() => {
                    this.router.navigate(['/working-group']);
                });
            }
        }
    }

    onLeave(): void {
        let role = Number.parseInt(localStorage.getItem('role')!);
        const user = localStorage.getItem('user');
        if (user) {
            const userData = JSON.parse(user);
            if (role === RoleId.Professor) {
                this.groupService.removeProfessorFromGroup(this.group!.id!, userData.id).subscribe(() => {
                    this.router.navigate(['/working-group']);
                });
            } else if (role === RoleId.Student) {
                this.groupService.removeStudentFromGroup(this.group!.id!, userData.id).subscribe(() => {
                    this.router.navigate(['/working-group']);
                });
            }
        }
    }

    onDelete() {
        const dialogRef = this.dialog.open(ConfirmDialogComponent);
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.groupService.deleteGroup(this.group!.id!).subscribe(() => {
                    this.router.navigate(['/working-group']);
                });
            }
        });

    }

    addStudentByEmail() {
        const email = this.groupForm.get('newStudentEmail')?.value;
        if (email) {
            this.groupService.addStudentToGroupByEmail(this.group!.id!, email).subscribe((student) => {
                this.groupForm.get('newStudentEmail')?.setValue('');
                this.students = [...this.students, student];
            });
        }
    }

    deleteStudent(student: number) {
        this.groupService.removeStudentFromGroup(this.group!.id!, student).subscribe(() =>
            this.students = this.students.filter(s => s.id !== student)
        );
    }

    deleteProfessor(professor: number) {
        this.groupService.removeProfessorFromGroup(this.group!.id!, professor).subscribe(() =>
            this.professors = this.professors.filter(p => p.id !== professor)
        );
    }

    sendMessage() {
        const message = this.groupForm.get('message')?.value;
        const user = localStorage.getItem('user');
        if (user && message) {
            const userData = JSON.parse(user);
            if (message) {
                let content: WorkingGroupMessage = {
                    working_group: this.group!.id!,
                    professor: userData.id,
                    message: message
                };
                this.groupService.sendMessage(content).subscribe();
            }
        }

    }
}