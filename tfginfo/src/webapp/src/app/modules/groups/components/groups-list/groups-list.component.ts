import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { WorkingGroupBase } from '../../models/group.model';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { GroupService } from '../../services/group-service';
import { RoleId } from '../../../admin/models/role.model';
import { CommonModule } from '@angular/common';
import { ConfigurationService } from '../../../../core/services/configuration.service';

@Component({
    selector: 'group-list',
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
    templateUrl: './groups-list.component.html',
    styleUrls: ['./groups-list.component.scss']
})
export class GroupListComponent implements OnInit {
    @Input() groups: WorkingGroupBase[] = [];
    @Input() displayedColumns: string[] = ['name', 'description', 'isPrivate', 'actions'];
    @Output() onDeleteGroup = new EventEmitter<number>();
    public canEdit: boolean = false;
    public universitiesSelected: number[] | undefined;
    public columnsInputCloned: string[] = [];

    constructor(
        private dialog: MatDialog,
        private groupService: GroupService,
        private router: Router,
        private route: ActivatedRoute,
        private configurationService: ConfigurationService
    ) {
    }


    ngOnInit(): void {
        this.columnsInputCloned = this.displayedColumns;
        let role = this.configurationService.getRole();
        this.canEdit = role === RoleId.Admin;
        this.universitiesSelected = this.configurationService.getSelectedUniversities();
        if (!this.universitiesSelected || this.universitiesSelected.length === 0) {
            this.universitiesSelected = localStorage.getItem('selectedUniversity') ? [parseInt(localStorage.getItem('selectedUniversity')!)] : undefined;
        }
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
            this.displayedColumns = this.displayedColumns.filter(col => col !== 'description');   
        } else {
            this.displayedColumns = this.columnsInputCloned;
        }
    }

    onEdit(group: WorkingGroupBase) {
        this.router.navigate(["/working-group/" + group.id]);
    }

    onDelete(group: WorkingGroupBase) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.onDeleteGroup.emit(group.id);
            }
        });
    }

    changeUniversity() {
        this.router.navigate(['/admin/university']);
    }
}