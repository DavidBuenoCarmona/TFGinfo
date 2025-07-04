import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'app-import-dialog',
    imports: [
        TranslateModule,
        MatDialogModule,
        MatButtonModule,
        CommonModule
    ],
    templateUrl: './import-dialog.component.html',
    styleUrls: ['./import-dialog.component.scss']
})
export class ImportDialogComponent {
    selectedFile: File | null = null;
    fileName: string = '';
    format: string = ""

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        public dialogRef: MatDialogRef<ImportDialogComponent>,
        public translateService: TranslateService)
        {
        this.format = this.translateService.instant(data.format);
        }

    onFileSelected(event: Event) {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.selectedFile = input.files[0];
            this.fileName = this.selectedFile.name;
        }
    }

    onImport() {
        if (this.selectedFile) {
            // Procesa el archivo aquí
            // ...
            this.dialogRef.close(this.selectedFile);
        }
    }
}