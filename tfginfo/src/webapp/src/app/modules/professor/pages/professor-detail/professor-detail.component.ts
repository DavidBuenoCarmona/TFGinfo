import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProfessorService } from '../../services/professor.service';
import { ProfessorDTO } from '../../models/professor.model';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { CommonModule, Location } from '@angular/common';
import { DepartmentService } from '../../../admin/services/department.service';
import { DepartmentDTO } from '../../../admin/models/department.model';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialog } from '@angular/material/dialog';
import { AuthCodeDialogComponent } from '../../../../core/layout/components/auth-code-dialog/auth-code-dialog.component';
import { TFGLineDTO } from '../../../tfg/models/tfg.model';
import { TfgService } from '../../../tfg/services/tfg.service';
import { TfgListComponent } from '../../../tfg/components/tfg-list/tfg-list.component';
import { RoleId } from '../../../admin/models/role.model';
import { ConfigurationService } from '../../../../core/services/configuration.service';
import { Filter } from '../../../../core/core.model';
import { SnackBarService } from '../../../../core/services/snackbar.service';

@Component({
    selector: 'professor-detail',
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatButtonModule,
        MatCheckboxModule,
        CommonModule,
        TfgListComponent
    ],
    templateUrl: './professor-detail.component.html',
    styleUrls: ['./professor-detail.component.scss']
})
export class ProfessorDetailComponent implements OnInit {
    id: string | null = null;
    professor: ProfessorDTO | null = null;
    creation: boolean = false;
    professorForm!: FormGroup;
    departments: DepartmentDTO[] = [];
    tfgs: TFGLineDTO[] = [];
    tfgDisplayedColumns: string[] = ['name', 'description', 'actions'];
    canEdit: boolean = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private professorService: ProfessorService,
        private fb: FormBuilder,
        private departmentService: DepartmentService,
        private dialog: MatDialog,
        private tfgService: TfgService,
        private location: Location,
        private configurationService: ConfigurationService,
        private snackbarService: SnackBarService
    ) { }

    ngOnInit(): void {
        let role = this.configurationService.getRole();
        this.canEdit = role === RoleId.Admin;
        this.id = this.route.snapshot.paramMap.get('id');
        if (this.id !== "new" && isNaN(Number(this.id))) {
            this.router.navigate(['/']);
        }
        this.creation = this.id === "new";

        if (!this.creation) {
            this.professorService.getProfessor(+this.id!).subscribe((data) => {
                this.professor = data;
                this.professorForm.patchValue(data);
                this.professorForm.get('departmentId')?.setValue(data.department?.id);
            });

            this.tfgService.getTfgsByProfessor(+this.id!).subscribe((data) => {
                this.tfgs = data;
            });
        }

        this.professorForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
            surname: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            departmentId: ['', Validators.required],
            department_boss: [false]
        });
        if (!this.canEdit) {
            this.professorForm.disable();
        }

         let universityId = this.configurationService.getSelectedUniversity()!;
            if (!universityId) {
                universityId = localStorage.getItem('selectedUniversity') ? parseInt(localStorage.getItem('selectedUniversity')!) : 0;
            }

            if (!universityId) {
                this.snackbarService.error('ERROR.UNIVERSITY_NOT_SELECTED');
            } else {
                let universityFilter: Filter[] = [];
                universityFilter.push({key: 'universityId', value: universityId.toString()});

                this.departmentService.searchDepartments(universityFilter).subscribe((data) => this.departments = data);
            }

    }

    onSubmit(): void {
        if (this.professorForm.valid) {
            const professorData = this.professorForm.value;
            if (this.creation) {
                this.professorService.createProfessor(professorData).subscribe((data) => this.openAuthCodeDialog(data.professor.email, data.auth_code));
            } else {
                this.professorService.updateProfessor(professorData).subscribe(() => this.location.back());
            }
        }
    }

    onCancel(): void {
        this.location.back();
    }

    openAuthCodeDialog(user: string, auth_code: string): void {
        const dialogRef = this.dialog.open(AuthCodeDialogComponent, {
            data: { user: user, auth_code: auth_code },
        });
        
        dialogRef.afterClosed().subscribe((result) => {
            this.location.back();
        });
    }
}