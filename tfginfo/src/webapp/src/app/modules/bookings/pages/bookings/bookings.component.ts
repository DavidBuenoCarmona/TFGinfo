import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TfgListComponent } from '../../../tfg/components/tfg-list/tfg-list.component';
import { GroupListComponent } from '../../../groups/components/groups-list/groups-list.component';
import { TFGLineDTO } from '../../../tfg/models/tfg.model';
import { TfgService } from '../../../tfg/services/tfg.service';
import { RoleId } from '../../../admin/models/role.model';
import { WorkingGroupBase } from '../../../groups/models/group.model';
import { fork } from 'child_process';
import { forkJoin } from 'rxjs';
import { GroupService } from '../../../groups/services/group-service';

@Component({
    selector: 'app-bookings',
    imports: [TranslateModule, TfgListComponent, GroupListComponent],
    templateUrl: './bookings.component.html',
    styleUrl: './bookings.component.scss'
})
export class BookingsComponent implements OnInit {
    public displayedTFGColumns = ['name', 'department', 'actions']
    public displayedGroupColumns = ['name', 'isPrivate', 'actions']
    public tfgs: TFGLineDTO[] = []
    public workingGroups: WorkingGroupBase[] = []

    constructor(
        private tfgService: TfgService,
        private workingGroupService: GroupService,
    ){}

    ngOnInit(): void {
        const user = JSON.parse(localStorage.getItem("user")!);
        const role = JSON.parse(localStorage.getItem('role')!);
        if (role === RoleId.Student) {
            forkJoin([this.workingGroupService.getGroupByStudent(user.id), this.tfgService.getTfgsByStudent(user.id)]).subscribe(([groups, tfgs]) => {
                this.workingGroups = groups;
                this.tfgs = tfgs;
            });
        } else {
            forkJoin([this.workingGroupService.getGroupByProfessor(user.id), this.tfgService.getTfgsByProfessor(user.id)]).subscribe(([groups, tfgs]) => {
                this.workingGroups = groups;
                this.tfgs = tfgs;
            });
        }
    }
}
