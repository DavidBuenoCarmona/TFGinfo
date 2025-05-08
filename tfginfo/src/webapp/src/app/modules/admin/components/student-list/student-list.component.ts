import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { StudentDTO } from '../../models/student.model';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'student-list',
    standalone: true,
    imports: [
        TranslateModule,
        MatTableModule,
        MatDialogModule,
        MatIconModule,
        MatButtonModule,
        MatCardModule,
        CommonModule
    ],
    templateUrl: './student-list.component.html',
    styleUrls: ['./student-list.component.scss']
})
export class StudentListComponent {
    @Input() students: StudentDTO[] = [];
    @Input() displayedColumns: string[] = ['name', 'surname', 'email', 'career', 'actions'];
    @Input() groupId: number | undefined = undefined;
    @Output() onDeleteStudent = new EventEmitter<number>();

    constructor(
        private dialog: MatDialog,
        private router: Router,
        private route: ActivatedRoute
    ) { }

    onEdit(student: StudentDTO) {
        this.router.navigate([student.id], { relativeTo: this.route });
    }

    onDelete(student: StudentDTO) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent);
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.onDeleteStudent.emit(student.id!);
            }
        });
    }
}