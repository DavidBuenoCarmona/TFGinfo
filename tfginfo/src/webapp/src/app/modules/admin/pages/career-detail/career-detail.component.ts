import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CareerService } from '../../services/career.service';
import { CareerDTO } from '../../models/career.model';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { UniversityBase } from '../../models/university.model';
import { MatSelectModule } from '@angular/material/select';
import { UniversityService } from '../../services/university.service';

@Component({
    selector: 'career-detail',
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatButtonModule,
        CommonModule
    ],
    templateUrl: './career-detail.component.html',
    styleUrls: ['./career-detail.component.scss']
})
export class CareerDetailComponent implements OnInit {
    id: string | null = null;
    career: CareerDTO | null = null;
    creation: boolean = false;
    public universities: UniversityBase[] = [];
    careerForm!: FormGroup;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private careerService: CareerService,
        private fb: FormBuilder,
        private universityService: UniversityService
    ) { }

    ngOnInit(): void {
        this.id = this.route.snapshot.paramMap.get('id');
        if (this.id !== "new" && isNaN(Number(this.id))) {
            this.router.navigate(['/']);
        }
        this.creation = this.id === "new";

        this.careerForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
            universityId: ['', Validators.required],
        });

        this.universityService.getUniversities().subscribe((data) => {
            this.universities = data;
        });

        if (!this.creation) {
            this.careerService.getCareer(+this.id!).subscribe((data) => {
                this.career = data;
                this.careerForm.patchValue(data);
                this.careerForm.get('universityId')?.setValue(data.university!.id);
            });
        }
    }

    onSubmit(): void {
        if (this.careerForm.valid) {
            const careerData = this.careerForm.value;
            if (this.creation) {
                this.careerService.createCareer(careerData).subscribe(() => this.router.navigate(['admin/career']));
            } else {
                this.careerService.updateCareer(careerData).subscribe(() => this.router.navigate(['admin/career']));
            }
        }
    }

    onCancel(): void {
        this.router.navigate(['admin/career']);
    }
}