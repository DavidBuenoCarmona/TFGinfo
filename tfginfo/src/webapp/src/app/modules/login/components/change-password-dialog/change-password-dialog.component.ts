import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-change-password-dialog',
  standalone: true,
  imports: [
    MatButtonModule,
    MatDialogModule,
    TranslateModule,
    ReactiveFormsModule,
    TranslateModule,
    CommonModule,
    MatInputModule,
    MatFormFieldModule
  ],
  templateUrl: './change-password-dialog.component.html',
  styleUrls: ['./change-password-dialog.component.scss']
})
export class ChangePasswordDialogComponent {
  changePasswordForm: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { username: string },
    public dialogRef: MatDialogRef<ChangePasswordDialogComponent>,
    private fb: FormBuilder,
    public AuthService: AuthService,
  ) {
    this.changePasswordForm = this.fb.group({
      OldPassword: ['', Validators.required],
      NewPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordsMatchValidator.bind(this) });
  }

  passwordsMatchValidator(group: FormGroup): { [key: string]: boolean } | null {
    const newPassword = group.get('NewPassword')?.value;
    const confirmPassword = group.get('OldPassword')?.value;
    return newPassword === confirmPassword ? null : { passwordsMismatch: true };
  }

  onConfirm(): void {
    if (this.changePasswordForm.valid) {
      const formData = this.changePasswordForm.value;
      this.AuthService.changePassword({
        username: this.data.username,
        OldPassword: formData.OldPassword,
        NewPassword: formData.NewPassword
      }).subscribe(
        response => {
          this.dialogRef.close(response);
        });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}