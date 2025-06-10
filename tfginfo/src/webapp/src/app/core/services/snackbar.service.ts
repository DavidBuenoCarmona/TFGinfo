import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { TranslateService } from "@ngx-translate/core";

@Injectable({
    providedIn: "root"
})
export class SnackBarService {
   
    constructor(
        private snackbar: MatSnackBar,
        private translateService:TranslateService,
    ) { }

    show(message: string): void {
        message = this.translateService.instant(message);
        this.snackbar.open(message, "OK", {
            duration: 4000, // 4 seconds
            verticalPosition: 'top',
            panelClass: ['snackbar']
        });
    }

    error(message: string): void {
        message = this.translateService.instant(message);
        this.snackbar.open(message, "OK", {
            duration: 4000,
            verticalPosition: 'top',
            panelClass: ['snackbar-error']
        });
    }
   
}