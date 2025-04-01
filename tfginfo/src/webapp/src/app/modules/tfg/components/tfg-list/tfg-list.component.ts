import { Component, Input } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TFGLineDTO } from '../../models/tfg.model';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { TfgService } from '../../services/tfg.service';
import { Router } from '@angular/router';

@Component({
  selector: 'tfg-list',
  imports: [TranslateModule, MatDialogModule],
  templateUrl: './tfg-list.component.html',
  styleUrl: './tfg-list.component.scss'
})
export class TfgListComponent {
    @Input() tfgs: TFGLineDTO[] = [];
    constructor(
        private dialog: MatDialog,
        private tfgService: TfgService,
        private router: Router
    ) {}

    onEdit(tfg: TFGLineDTO) {
        this.router.navigate([tfg.id], { relativeTo: this.router.routerState.root }); // Agrega el ID a la ruta actual
    }

    onDelete(tfg: TFGLineDTO) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent);

        dialogRef.afterClosed().subscribe((result) => {
        if (result) {
            this.tfgService.deleteTfg(tfg.id!).subscribe({
                next: () => {},
                error: (err) => console.error(err),
                complete: () => {
                    this.tfgs = this.tfgs.filter((item) => item.id !== tfg.id);
                }
            });
        }
        });
    }
}