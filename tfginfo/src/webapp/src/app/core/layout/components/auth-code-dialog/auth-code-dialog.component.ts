import { Component, Inject } from '@angular/core';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-auth-code-dialog',
  standalone: true,
  imports: [
    MatButtonModule,
    MatDialogModule,
    TranslateModule
  ],
  templateUrl: './auth-code-dialog.component.html',
  styleUrl: './auth-code-dialog.component.scss'
})
export class AuthCodeDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { user: string, auth_code: string } // Recibir el mensaje como data
  ) {  }

  onConfirm(): void {
    this.dialogRef.close();
  }
}
