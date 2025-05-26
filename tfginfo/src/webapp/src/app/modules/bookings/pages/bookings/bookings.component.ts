import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TfgListComponent } from '../../../tfg/components/tfg-list/tfg-list.component';
import { GroupListComponent } from '../../../groups/components/groups-list/groups-list.component';
import { TFGDTO, TFGLineDTO, TFGRequestDTO } from '../../../tfg/models/tfg.model';
import { TfgService } from '../../../tfg/services/tfg.service';
import { RoleId } from '../../../admin/models/role.model';
import { WorkingGroupBase } from '../../../groups/models/group.model';
import { fork } from 'child_process';
import { forkJoin } from 'rxjs';
import { GroupService } from '../../../groups/services/group-service';
import { TfgRequestListComponent } from '../../../tfg/components/tfg-request-list/tfg-request-list.component';
import { CommonModule } from '@angular/common';

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
    ){}

    ngOnInit(): void {
        const user = JSON.parse(localStorage.getItem("user")!);
        const role = JSON.parse(localStorage.getItem('role')!);
        this.isProfessor = role === RoleId.Professor;
        if (!this.isProfessor) {
            forkJoin([this.workingGroupService.getGroupByStudent(user.id), this.tfgService.getTfgsByStudent(user.id)]).subscribe(([groups, tfgs]) => {
                this.workingGroups = groups;
                this.tfgs = tfgs;
            });
        } else {
            forkJoin([this.workingGroupService.getGroupByProfessor(user.id), this.tfgService.getTfgsByProfessor(user.id), this.tfgService.getPendingRequestsByProfessor(user.id)]).subscribe(([groups, tfgs, pendingTfgs]) => {
                this.workingGroups = groups;
                this.tfgs = tfgs;
                this.pendingTfgs = pendingTfgs;
            });
        }
    }

    removeRequest(tfgId: number): void {
        const user = JSON.parse(localStorage.getItem("user")!);
        this.pendingTfgs = this.pendingTfgs.filter((tfg) => tfg.tfgId !== tfgId);
        this.workingGroupService.getGroupByProfessor(user.id).subscribe((groups) => {
            this.workingGroups = groups;
        });
    }
}
