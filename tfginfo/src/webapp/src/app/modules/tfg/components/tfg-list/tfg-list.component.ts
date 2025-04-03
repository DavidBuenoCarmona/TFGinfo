import { Component, Input } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { TFGLineDTO } from '../../models/tfg.model';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { TfgService } from '../../services/tfg.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'tfg-list',
  standalone: true,
  imports: [
    TranslateModule,
    MatTableModule,
    MatDialogModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './tfg-list.component.html',
  styleUrls: ['./tfg-list.component.scss']
})
export class TfgListComponent {
  @Input() tfgs: TFGLineDTO[] = [];
  @Input() displayedColumns: string[] = ['name', 'description', 'department', 'slots', 'actions'];

  constructor(
    private dialog: MatDialog,
    private tfgService: TfgService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  onEdit(tfg: TFGLineDTO) {
    this.router.navigate([tfg.id], { relativeTo: this.route });
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