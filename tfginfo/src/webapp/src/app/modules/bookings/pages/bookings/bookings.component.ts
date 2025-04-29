import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TfgListComponent } from '../../../tfg/components/tfg-list/tfg-list.component';
import { GroupListComponent } from '../../../groups/components/groups-list/groups-list.component';
import { TFGLineDTO } from '../../../tfg/models/tfg.model';
import { TfgService } from '../../../tfg/services/tfg.service';
import { RoleId } from '../../../admin/models/role.model';

@Component({
    selector: 'app-bookings',
    imports: [TranslateModule, TfgListComponent, GroupListComponent],
    templateUrl: './bookings.component.html',
    styleUrl: './bookings.component.scss'
})
export class BookingsComponent implements OnInit {
    public displayedTFGColumns = ['name', 'department', 'actions']
    public displayedGroupColumns = ['name', 'isPrivate']
    public tfgs: TFGLineDTO[] = []

    constructor(
        private tfgService: TfgService,
    ){}

    ngOnInit(): void {
        const user = JSON.parse(localStorage.getItem("user")!);
        const role = JSON.parse(localStorage.getItem('role')!);
        if (role === RoleId.Student) {
            this.tfgService.getTfgsByStudent(user.id).subscribe((tfgs) => {
                this.tfgs = tfgs;
            });
        } else {
            this.tfgService.getTfgs().subscribe((tfgs) => {
                this.tfgs = tfgs;
            });
        }
    }
}
