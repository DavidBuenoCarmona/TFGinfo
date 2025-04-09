import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UniversityService } from '../../services/university.service';
import { UniversityBase } from '../../models/university.model';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'university-detail',
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        CommonModule
    ],
    templateUrl: './university-detail.component.html',
    styleUrls: ['./university-detail.component.scss']
})
export class UniversityDetailComponent implements OnInit {
    id: string | null = null;
    university: UniversityBase | null = null;
    creation: boolean = false;
    universityForm!: FormGroup;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private universityService: UniversityService,
        private fb: FormBuilder
    ) { }

    ngOnInit(): void {
        this.id = this.route.snapshot.paramMap.get('id');
        if (this.id !== "new" && isNaN(Number(this.id))) {
            this.router.navigate(['/']);
        }
        this.creation = this.id === "new";

        this.universityForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
            address: ['', Validators.required]
        });

        if (!this.creation) {
            this.universityService.getUniversity(+this.id!).subscribe((data) => {
                this.university = data;
                this.universityForm.patchValue(data);
            });
        }
    }

    onSubmit(): void {
        if (this.universityForm.valid) {
            const universityData = this.universityForm.value;
            if (this.creation) {
                this.universityService.createUniversity(universityData).subscribe(() => this.router.navigate(['admin/university']));
            } else {
                this.universityService.updateUniversity(universityData).subscribe(() => this.router.navigate(['admin/university']));
            }
        }
    }

    onCancel(): void {
        this.router.navigate(['admin/university']);
    }
}