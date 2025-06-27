import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { TranslateModule } from '@ngx-translate/core';
import { TFGRequestDTO, TFGStatus } from '../../models/tfg.model';
import { MatButtonModule } from '@angular/material/button';
import { TfgService } from '../../services/tfg.service';
import { CommonModule } from '@angular/common';
import { SnackBarService } from '../../../../core/services/snackbar.service';

@Component({
    selector: 'tfg-request-list',
    imports: [MatCardModule, TranslateModule, MatTableModule, MatIconModule, MatButtonModule, CommonModule],
    templateUrl: './tfg-request-list.component.html',
    styleUrl: './tfg-request-list.component.scss'
})
export class TfgRequestListComponent {
    @Input() tfgStudentRequests: TFGRequestDTO[] = [];
    @Output() procesedRequest: EventEmitter<number> = new EventEmitter<number>();
    @Output() requestAccepted: EventEmitter<TFGRequestDTO> = new EventEmitter<TFGRequestDTO>();
    displayedColumns: string[] = ['tfgName', 'studentName', 'status', 'actions'];
    TFGStatus = TFGStatus;

    constructor(
        private tfgService: TfgService,
        private snackBarService: SnackBarService
    ) {
    }

    onAccept(request: TFGRequestDTO): void {
        if (request.tfgStatus == TFGStatus.Pending) {
            this.tfgService.acceptRequest(request.tfgId).subscribe(() => {
                this.snackBarService.show("TFG.REQUEST_ACCEPTED")
                this.requestAccepted.emit(request);
            });
        } else {
            this.tfgService.changeTfgStatus(request.tfgId).subscribe(() => {
                this.snackBarService.show("TFG.STATUS_UPDATED")
                this.requestAccepted.emit(request);
            });
        }

    }

    onReject(request: TFGRequestDTO): void {
        this.tfgService.rejectRequest(request.tfgId).subscribe(() => {
            this.snackBarService.show("TFG.REQUEST_REJECTED")
            this.procesedRequest.emit(request.tfgId);
        });
    }
}
