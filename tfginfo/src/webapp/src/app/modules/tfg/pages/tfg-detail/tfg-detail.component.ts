import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TfgService } from '../../services/tfg.service';
import { TFGLineDTO } from '../../models/tfg.model';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { DepartmentService } from '../../../admin/services/department.service';
import { DepartmentDTO } from '../../../admin/models/department.model';

@Component({
    selector: 'tfg-detail',
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        MatCheckboxModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './tfg-detail.component.html',
    styleUrls: ['./tfg-detail.component.scss']
})
export class TfgDetailComponent implements OnInit {
    id: string | null = null;
    tfg: TFGLineDTO | null = null;
    creation: boolean = false;
    tfgForm!: FormGroup;
    departments: DepartmentDTO[] = [];

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private tfgService: TfgService,
        private fb: FormBuilder,
        private departmentService: DepartmentService
    ) { }

    ngOnInit(): void {
        this.id = this.route.snapshot.paramMap.get('id');
        this.creation = this.id == "new";

        this.tfgForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
            description: ['', Validators.required],
            departmentId: ['', Validators.required],
            slots: [1, [Validators.required, Validators.min(1)]],
            group: [false],
        });

        this.departmentService.getDepartments().subscribe((data) => this.departments = data);

        if (!this.creation) {
            this.tfgService.getTfg(+this.id!).subscribe((data) => {
                this.tfg = data;
                this.tfgForm.patchValue(data);
                this.tfgForm.get('departmentId')?.setValue(data.department?.id);
            });
        }

    }

    onSubmit(): void {
        if (this.tfgForm.valid) {
            const tfgData = this.tfgForm.value;
            if (this.creation) {
                this.tfgService.createTfg(tfgData).subscribe(() => this.router.navigate(['/tfg']));
            } else {
                this.tfgService.updateTfg(tfgData).subscribe(() => this.router.navigate(['/tfg']));
            }
        }
    }

    onCancel(): void {
        this.router.navigate(['/tfg']);
    }
}