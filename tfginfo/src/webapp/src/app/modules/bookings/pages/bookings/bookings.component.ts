import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TfgListComponent } from '../../../tfg/components/tfg-list/tfg-list.component';
import { GroupListComponent } from '../../../groups/components/groups-list/groups-list.component';
import { TFGDTO, TFGLineDTO, TFGRequestDTO, TFGStatus } from '../../../tfg/models/tfg.model';
import { TfgService } from '../../../tfg/services/tfg.service';
import { RoleId } from '../../../admin/models/role.model';
import { WorkingGroupBase } from '../../../groups/models/group.model';
import { fork } from 'child_process';
import { forkJoin } from 'rxjs';
import { GroupService } from '../../../groups/services/group-service';
import { TfgRequestListComponent } from '../../../tfg/components/tfg-request-list/tfg-request-list.component';
import { CommonModule } from '@angular/common';
import { ConfigurationService } from '../../../../core/services/configuration.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-bookings',
    imports: [TranslateModule, TfgListComponent, GroupListComponent, TfgRequestListComponent, CommonModule],
    templateUrl: './bookings.component.html',
    styleUrl: './bookings.component.scss'
})
export class BookingsComponent implements OnInit {
    public displayedTFGColumns = ['name', 'department', 'actions']
    public displayedGroupColumns = ['name', 'isPrivate', 'actions']
    public tfgs: TFGLineDTO[] = []
    public workingGroups: WorkingGroupBase[] = []
    public pendingTfgs: TFGRequestDTO[] = []
    public isProfessor: boolean = false

    constructor(
        private tfgService: TfgService,
        private workingGroupService: GroupService,
        private configurationService: ConfigurationService,
        private router: Router
    ){}

    ngOnInit(): void {
        const user = this.configurationService.getUser();
        const role = this.configurationService.getRole();
        if (role === RoleId.Admin) {
            this.router.navigate(['/tfg']);
            return;
        }
        this.isProfessor = role === RoleId.Professor;
        if (!this.isProfessor) {
            forkJoin([this.workingGroupService.getGroupByStudent(user!.id), this.tfgService.getTfgsByStudent(user!.id)]).subscribe(([groups, tfgs]) => {
                this.workingGroups = groups;
                this.tfgs = tfgs;
            });
        } else {
            forkJoin([this.workingGroupService.getGroupByProfessor(user!.id), this.tfgService.getTfgsByProfessor(user!.id), this.tfgService.getPendingRequestsByProfessor(user!.id)]).subscribe(([groups, tfgs, pendingTfgs]) => {
                this.workingGroups = groups;
                this.tfgs = tfgs;
                this.pendingTfgs = pendingTfgs;
            });
        }
    }

    removeRequest(tfgId: number): void {
        const user = this.configurationService.getUser();
        this.pendingTfgs = this.pendingTfgs.filter((tfg) => tfg.tfgId !== tfgId);
        this.workingGroupService.getGroupByProfessor(user!.id).subscribe((groups) => {
            this.workingGroups = groups;
        });
    }

    changeStatus(tfg: TFGRequestDTO) {
        // CHange the status of the TFG request in the pinding Tfgs array with the infor from tfg
        this.pendingTfgs = this.pendingTfgs.filter((t) => t.tfgId !== tfg.tfgId);
        tfg.tfgStatus = tfg.tfgStatus + 1;
        this.pendingTfgs.push(tfg);
    }
}
