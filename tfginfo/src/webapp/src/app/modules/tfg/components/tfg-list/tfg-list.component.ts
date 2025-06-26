import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
import { RoleId } from '../../../admin/models/role.model';
import { CommonModule } from '@angular/common';
import { ConfigurationService } from '../../../../core/services/configuration.service';

@Component({
    selector: 'tfg-list',
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
    templateUrl: './tfg-list.component.html',
    styleUrls: ['./tfg-list.component.scss']
})
export class TfgListComponent implements OnInit {
    @Input() tfgs: TFGLineDTO[] = [];
    @Input() displayedColumns: string[] = ['name', 'description', 'department', 'slots', 'actions'];
    @Output() onDeleteTfg = new EventEmitter<number>();
    public isAdmin: boolean = false;
    public universitiesSelected: number[] | undefined;

    constructor(
        private dialog: MatDialog,
        private tfgService: TfgService,
        private router: Router,
        private route: ActivatedRoute,
        private configurationService: ConfigurationService
    ) { }

    ngOnInit(): void {
        let role = this.configurationService.getRole();
        this.isAdmin = role === RoleId.Admin;
        this.universitiesSelected = this.configurationService.getSelectedUniversities();
        if (!this.universitiesSelected || this.universitiesSelected.length === 0) {
            this.universitiesSelected = localStorage.getItem('selectedUniversity') ? [parseInt(localStorage.getItem('selectedUniversity')!)] : undefined;
        }
    }

    onEdit(tfg: TFGLineDTO) {
        this.router.navigate(["/tfg/" + tfg.id]);
    }

    onDelete(tfg: TFGLineDTO) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent);

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.onDeleteTfg.emit(tfg.id!);
            }
        });
    }

    changeUniversity() {
        this.router.navigate(['/admin/university']);
    }
}