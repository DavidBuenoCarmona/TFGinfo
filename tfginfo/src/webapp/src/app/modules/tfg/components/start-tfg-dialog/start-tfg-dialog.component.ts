import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProfessorDTO } from '../../../professor/models/professor.model';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TranslateModule } from '@ngx-translate/core';
import { TFGRequest } from '../../models/tfg.model';
import { TfgService } from '../../services/tfg.service';

@Component({
    selector: 'app-start-tfg-dialog',
    imports: [
        CommonModule,
        ReactiveFormsModule,
        TranslateModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatButtonModule
    ],
    templateUrl: './start-tfg-dialog.component.html',
    styleUrls: ['./start-tfg-dialog.component.scss']
})
export class StartTfgDialogComponent {
    tutorForm: FormGroup;
    showExternalTutorFields = false;
    filteredProfessors: ProfessorDTO[] = [];

    constructor(
        public dialogRef: MatDialogRef<StartTfgDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { professors: ProfessorDTO[]; tfgLineId: number },
        private fb: FormBuilder,
        private tfgService: TfgService
    ) {
        this.tutorForm = this.fb.group({
            primaryTutor: ['', Validators.required], // Tutor obligatorio
            secondaryTutor: [''], // Tutor opcional
            externalTutorName: [''], // Nombre del tutor externo
            externalTutorEmail: ['', [Validators.email]] // Email del tutor externo
        });
        this.tutorForm.get('secondaryTutor')?.disable(); // Deshabilitar el campo de tutor secundario inicialmente
        this.tutorForm.get('primaryTutor')?.valueChanges.subscribe((value) => {
            this.tutorForm.get('secondaryTutor')?.enable();
            this.tutorForm.get('secondaryTutor')?.setValue('');
            this.filteredProfessors = this.data.professors.filter((professor) => professor.id != value );
        })
    }

    // Manejar la selección de tutor externo
    onExternalTutorSelected(): void {
        this.showExternalTutorFields = true;
        this.tutorForm.get('externalTutorName')?.setValidators(Validators.required);
        this.tutorForm.get('externalTutorEmail')?.setValidators([Validators.required, Validators.email]);
        this.tutorForm.get('secondaryTutor')?.clearValidators();
        this.tutorForm.get('secondaryTutor')?.setValue('');
        this.tutorForm.get('secondaryTutor')?.updateValueAndValidity();
    }

    // Manejar la selección de un tutor interno
    onInternalTutorSelected(): void {
        this.showExternalTutorFields = false;
        this.tutorForm.get('externalTutorName')?.clearValidators();
        this.tutorForm.get('externalTutorEmail')?.clearValidators();
        this.tutorForm.get('externalTutorName')?.setValue('');
        this.tutorForm.get('externalTutorEmail')?.setValue('');
        this.tutorForm.get('externalTutorName')?.updateValueAndValidity();
        this.tutorForm.get('externalTutorEmail')?.updateValueAndValidity();
    }

    // Cerrar el diálogo y devolver los datos
    onSubmit(): void {
        if (this.tutorForm.valid) {
            const user = JSON.parse(localStorage.getItem('user')!);
            const studentEmail = user.username;
            let tfgRequest: TFGRequest = {
                studentEmail: studentEmail,
                secondaryProfessorId: this.tutorForm.value.secondaryTutor != "" ? this.tutorForm.value.secondaryTutor : null,
                professorId: this.tutorForm.value.primaryTutor,
                tfg: {
                    external_tutor_name: this.tutorForm.value.externalTutorName,
                    external_tutor_email: this.tutorForm.value.externalTutorEmail,
                    accepted: false,
                    tfgLineId: this.data.tfgLineId,
                }
            }
            this.tfgService.requestTfg(tfgRequest).subscribe(() => {
                this.dialogRef.close(this.tutorForm.value);
            }   );
        }
    }

    // Cancelar y cerrar el diálogo
    onCancel(): void {
        this.dialogRef.close();
    }
}