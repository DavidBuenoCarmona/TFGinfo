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
import { CommonModule } from '@angular/common';
import { DepartmentService } from '../../../admin/services/department.service';
import { DepartmentDTO } from '../../../admin/models/department.model';
import { MatCheckboxModule } from '@angular/material/checkbox';

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
        CommonModule
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

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private professorService: ProfessorService,
        private fb: FormBuilder,
        private departmentService: DepartmentService
    ) { }

    ngOnInit(): void {
        this.id = this.route.snapshot.paramMap.get('id');
        if (this.id !== "new" && isNaN(Number(this.id))) {
            this.router.navigate(['/']);
        }
        this.creation = this.id === "new";

        this.professorForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
            surname: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            departmentId: ['', Validators.required],
            departmentBoss: [false]
        });

        this.departmentService.getDepartments().subscribe((data) => this.departments = data);

        if (!this.creation) {
            this.professorService.getProfessor(+this.id!).subscribe((data) => {
                this.professor = data;
                this.professorForm.patchValue(data);
                this.professorForm.get('departmentId')?.setValue(data.department?.id);
            });
        }
    }

    onSubmit(): void {
        if (this.professorForm.valid) {
            const professorData = this.professorForm.value;
            if (this.creation) {
                this.professorService.createProfessor(professorData).subscribe(() => this.router.navigate(['/professor']));
            } else {
                this.professorService.updateProfessor(professorData).subscribe(() => this.router.navigate(['/professor']));
            }
        }
    }

    onCancel(): void {
        this.router.navigate(['/professor']);
    }
}