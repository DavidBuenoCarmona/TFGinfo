import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { StudentDTO } from '../../../admin/models/student.model';
import { StudentService } from '../../../admin/services/student.service';
import { CareerDTO } from '../../../admin/models/career.model';
import { CareerService } from '../../../admin/services/career.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AuthCodeDialogComponent } from '../../../../core/layout/components/auth-code-dialog/auth-code-dialog.component';

@Component({
    selector: 'profile-detail',
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatButtonModule,
        CommonModule,
        MatDialogModule,
    ],
    templateUrl: './profile-detail.component.html',
    styleUrls: ['./profile-detail.component.scss']
})
export class ProfileDetailComponent implements OnInit {
    id: string | null = null;
    profile: StudentDTO | null = null;
    creation: boolean = false;
    careers: CareerDTO[] = [];
    profileForm!: FormGroup;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private studentService: StudentService,
        private careerService: CareerService,
        private fb: FormBuilder,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.id = this.route.snapshot.paramMap.get('id');
        if (this.id !== "new" && isNaN(Number(this.id))) {
            this.router.navigate(['/']);
        }
        this.creation = this.id === "new";

        this.profileForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
            surname: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            dni: ['', Validators.required],
            careerId: ['', Validators.required],
            birthdate: [],
            phone: [''],
            address: ['']
        });

        this.careerService.getCareers().subscribe((data) => {
            this.careers = data;
        });

        if (!this.creation) {
            this.studentService.getStudent(+this.id!).subscribe((data) => {
                this.profile = data;
                this.profileForm.patchValue(data);
                this.profileForm.get('careerId')?.setValue(data.career.id);
                if (data.birthdate) {
                  const utcDate = new Date(data.birthdate);
                  const localDate = new Date(utcDate.getTime() - utcDate.getTimezoneOffset() * 60000);
                  this.profileForm.get('birthdate')?.setValue(localDate.toISOString().split('T')[0]);
              }
            });
        }
    }

    onSubmit(): void {
        if (this.profileForm.valid) {
            const profileData = this.profileForm.value;
            if (profileData.birthdate) {
              profileData.birthdate = new Date(profileData.birthdate).toISOString();
            }
            if (this.creation) {
                this.studentService.createStudent(profileData).subscribe((data) => this.openAuthCodeDialog(data.student.email, data.auth_code));
            } else {
                this.studentService.updateStudent(profileData).subscribe(() => this.router.navigate(['/admin/student']));
            }
        }
    }

    onCancel(): void {
        this.router.navigate(['/admin/student']);
    }

    openAuthCodeDialog(user: string, auth_code: string): void {
        const dialogRef = this.dialog.open(AuthCodeDialogComponent, {
          data: { user: user, auth_code: auth_code },
        });
      
        dialogRef.afterClosed().subscribe((result) => {
            this.router.navigate(['/admin/student'])
        });
    }
}