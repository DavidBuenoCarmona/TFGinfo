import { Component, Input, OnInit, Output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { ProfessorDTO } from '../../models/professor.model';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { EventEmitter } from '@angular/core';
import { RoleId } from '../../../admin/models/role.model';
import { CommonModule } from '@angular/common';
import { ConfigurationService } from '../../../../core/services/configuration.service';

@Component({
    selector: 'professor-list',
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
    templateUrl: './professor-list.component.html',
    styleUrls: ['./professor-list.component.scss']
})
export class ProfessorListComponent implements OnInit {
    @Input() professors: ProfessorDTO[] = [];
    @Input() displayedColumns: string[] = ['name', 'surname', 'email', 'department', 'actions'];
    @Output() onDeleteProfessor = new EventEmitter<number>();
    @Input() groupId: number | undefined = undefined;
    public canEdit: boolean = false;

    constructor(
        private dialog: MatDialog,
        private router: Router,
        private route: ActivatedRoute,
        private configurationService: ConfigurationService
    ) { }

    ngOnInit(): void {
        let role = this.configurationService.getRole();
        this.canEdit = role === RoleId.Admin;
    }
    onEdit(professor: ProfessorDTO) {
        this.router.navigate([professor.id], { relativeTo: this.route });
    }

    onDelete(professor: ProfessorDTO) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.onDeleteProfessor.emit(professor.id!);
            }
        });
    }
}