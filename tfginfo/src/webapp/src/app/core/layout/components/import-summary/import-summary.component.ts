import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-import-summary',
  imports: [MatDialogModule, CommonModule, MatButtonModule, TranslateModule],
  templateUrl: './import-summary.component.html',
  styleUrl: './import-summary.component.scss'
})
export class ImportSummaryComponent {
  constructor(
    public dialogRef: MatDialogRef<ImportSummaryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { items: string[], success: number }
  ) { }
}
