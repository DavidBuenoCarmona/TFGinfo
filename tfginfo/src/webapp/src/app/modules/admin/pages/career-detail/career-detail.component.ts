import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CareerService } from '../../services/career.service';
import { CareerDTO, CareerFlatDTO } from '../../models/career.model';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { UniversityBase } from '../../models/university.model';
import { MatSelectModule } from '@angular/material/select';
import { UniversityService } from '../../services/university.service';
import { MatCheckboxModule } from '@angular/material/checkbox';

@Component({
    selector: 'career-detail',
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatCheckboxModule,
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
    showCareers: boolean = false;
    careers: CareerDTO[] = [];

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
            universityId: [null, Validators.required],
            checkboxDoubleCareer: [false],
            primaryCareer: [null],
            secondaryCareer: [null],
        });

        this.universityService.getUniversities().subscribe((data) => {
            this.universities = data;
        });

        this.careerService.getCareers().subscribe((data) => {
            this.careers = data.filter(career => career.id !== +this.id! && !career.doubleCareer);
        });

        if (!this.creation) {
            this.careerService.getCareer(+this.id!).subscribe((data) => {
                this.career = data;
                this.careerForm.patchValue(data);
                if (this.career.doubleCareer) {
                    this.showCareers = true;
                    this.careerForm.get('checkboxDoubleCareer')?.setValue(true);
                    this.careerForm.get('primaryCareer')?.setValue(this.career.doubleCareers![0].id);
                    this.careerForm.get('secondaryCareer')?.setValue(this.career.doubleCareers![1].id);
                    this.careerForm.get('universityId')?.removeValidators(Validators.required);
                    this.careerForm.get('universityId')?.disable();
                } else {
                    this.careerForm.get('universityId')?.setValue(data.university!.id);
                }

            });
        }
    }

    onSubmit(): void {
        if (this.careerForm.valid) {
            const careerData: CareerFlatDTO = {
                id: this.careerForm.get('id')?.value,
                name: this.careerForm.get('name')?.value,
                universityId: this.careerForm.get('universityId')?.value,
                doubleCareer: this.careerForm.get('checkboxDoubleCareer')?.value,
                doubleCareers: this.careerForm.get('primaryCareer')?.value && this.careerForm.get('secondaryCareer')?.value ? [
                    this.careerForm.get('primaryCareer')?.value,
                    this.careerForm.get('secondaryCareer')?.value
                ] : [],
            };
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

    onCheckboxChange(event: any) {
        if (event.checked) {
            this.showCareers = true;
            this.careerForm.get('primaryCareer')?.setValidators([Validators.required]);
            this.careerForm.get('secondaryCareer')?.setValidators([Validators.required]);

            this.careerForm.get('universityId')?.removeValidators(Validators.required);
            this.careerForm.get('universityId')?.patchValue(null);
            this.careerForm.get('universityId')?.disable();

        } else {
            this.showCareers = false;
            this.careerForm.get('primaryCareer')?.clearValidators();
            this.careerForm.get('secondaryCareer')?.clearValidators();

            this.careerForm.get('universityId')?.setValidators(Validators.required);
            this.careerForm.get('universityId')?.enable();

            this.careerForm.get('primaryCareer')?.patchValue(null);
            this.careerForm.get('secondaryCareer')?.patchValue(null);
        }
    }
}