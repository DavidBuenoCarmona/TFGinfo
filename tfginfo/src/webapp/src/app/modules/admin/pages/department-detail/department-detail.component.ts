import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DepartmentService } from '../../services/department.service';
import { DepartmentDTO } from '../../models/department.model';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'department-detail',
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        CommonModule
    ],
    templateUrl: './department-detail.component.html',
    styleUrls: ['./department-detail.component.scss']
})
export class DepartmentDetailComponent implements OnInit {
    id: string | null = null;
    department: DepartmentDTO | null = null;
    creation: boolean = false;
    departmentForm!: FormGroup;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private departmentService: DepartmentService,
        private fb: FormBuilder
    ) { }

    ngOnInit(): void {
        this.id = this.route.snapshot.paramMap.get('id');
        if (this.id !== "new" && isNaN(Number(this.id))) {
            this.router.navigate(['/']);
        }
        this.creation = this.id === "new";

        this.departmentForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
        });

        if (!this.creation) {
            this.departmentService.getDepartment(+this.id!).subscribe((data) => {
                this.department = data;
                this.departmentForm.patchValue(data);
            });
        }
    }

    onSubmit(): void {
        if (this.departmentForm.valid) {
            const departmentData = this.departmentForm.value;
            if (this.creation) {
                this.departmentService.createDepartment(departmentData).subscribe(() => this.router.navigate(['/department']));
            } else {
                this.departmentService.updateDepartment(departmentData).subscribe(() => this.router.navigate(['/department']));
            }
        }
    }

    onCancel(): void {
        this.router.navigate(['admin/department']);
    }
}