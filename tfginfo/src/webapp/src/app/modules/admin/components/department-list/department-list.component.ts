import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { DepartmentDTO } from '../../models/department.model';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'department-list',
  standalone: true,
  imports: [
    TranslateModule,
    MatTableModule,
    MatDialogModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    CommonModule,
  ],
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.scss']
})
export class DepartmentListComponent implements OnInit {
  @Input() departments: DepartmentDTO[] = [];
  @Input() displayedColumns: string[] = ['name', 'acronym', 'universities', 'actions'];
  @Output() onDeleteDepartment = new EventEmitter<number>();

  constructor(
    private dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.setDisplayedColumns();
    window.addEventListener('resize', this.onResize);
  }

  ngOnDestroy(): void {
    window.removeEventListener('resize', this.onResize);
  }

  onResize = () => {
    this.setDisplayedColumns();
  };

  setDisplayedColumns() {
    if (window.innerWidth < 600) {
      this.displayedColumns = ['name', 'universities', 'actions'];
    } else {
      this.displayedColumns = ['name', 'acronym', 'universities', 'actions'];
    }
  }

  onEdit(department: DepartmentDTO) {
    this.router.navigate([department.id], { relativeTo: this.route });
  }

  onDelete(department: DepartmentDTO) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.onDeleteDepartment.emit(department.id!);
      }
    });
  }

}