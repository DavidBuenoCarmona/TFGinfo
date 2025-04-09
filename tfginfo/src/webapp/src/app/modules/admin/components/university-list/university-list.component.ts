import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { UniversityBase } from '../../models/university.model';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'university-list',
  standalone: true,
  imports: [
    TranslateModule,
    MatTableModule,
    MatDialogModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './university-list.component.html',
  styleUrls: ['./university-list.component.scss']
})
export class UniversityListComponent {
  @Input() universities: UniversityBase[] = [];
  @Input() displayedColumns: string[] = ['name', 'address', 'actions'];
  @Output() onDeleteUniversity = new EventEmitter<number>();

  public selectedUniversity: number | undefined;

  constructor(
    private dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.selectedUniversity = Number(localStorage.getItem('selectedUniversity'));
   }

  onEdit(university: UniversityBase) {
    this.router.navigate([university.id], { relativeTo: this.route });
  }

  onDelete(university: UniversityBase) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        if (this.selectedUniversity === university.id) {
          this.selectedUniversity = undefined;
          localStorage.removeItem('selectedUniversity');
        }
        
        this.onDeleteUniversity.emit(university.id!);
      }
    });
  }

  selectUniversity(university: UniversityBase) {
    if (this.selectedUniversity === university.id) {
      this.selectedUniversity = undefined;
      localStorage.removeItem('selectedUniversity');
    } else {
      localStorage.setItem('selectedUniversity', university.id!.toString());
      this.selectedUniversity = university.id;
    }
  }
}