import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { UniversityBase } from '../../models/university.model';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { ConfigurationService } from '../../../../core/services/configuration.service';
import { CommonModule } from '@angular/common';
import { UniversitySelectionService } from '../../../../core/services/localstorage.service';

@Component({
    selector: 'university-list',
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
    templateUrl: './university-list.component.html',
    styleUrls: ['./university-list.component.scss']
})
export class UniversityListComponent implements OnInit{
    @Input() universities: UniversityBase[] = [];
    @Input() displayedColumns: string[] = ['name', 'acronym', 'address', 'actions'];
    @Output() onDeleteUniversity = new EventEmitter<number>();
    public columnsInputCloned: string[] = [];

    public selectedUniversity: number | undefined;

    constructor(
        private dialog: MatDialog,
        private router: Router,
        private route: ActivatedRoute,
        private configurationService: ConfigurationService,
        private universitySelectionService: UniversitySelectionService
    ) {
        if (localStorage.getItem('selectedUniversity')) {
            this.selectedUniversity = parseInt(localStorage.getItem('selectedUniversity')!);
        }

    }
    ngOnInit(): void {
        this.columnsInputCloned = this.displayedColumns;
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
            this.displayedColumns = ['nameAcronym', 'actions'];
        } else {
            this.displayedColumns = this.columnsInputCloned;
        }
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
                    this.configurationService.setSelectedUniversities(undefined);
                    this.universitySelectionService.setUniversityId(null);
                }

                this.onDeleteUniversity.emit(university.id!);
            }
        });
    }

    selectUniversity(university: UniversityBase) {
        if (this.selectedUniversity === university.id) {
            this.selectedUniversity = undefined;
            this.universitySelectionService.setUniversityId(null);
        } else {
            this.universitySelectionService.setUniversityId(university.id!);
            this.selectedUniversity = university.id;
        }
    }
}