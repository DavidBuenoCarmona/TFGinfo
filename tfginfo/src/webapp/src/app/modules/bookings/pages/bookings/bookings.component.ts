import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TfgListComponent } from '../../../tfg/components/tfg-list/tfg-list.component';
import { GroupListComponent } from '../../../groups/components/groups-list/groups-list.component';

@Component({
  selector: 'app-bookings',
  imports: [TranslateModule, TfgListComponent, GroupListComponent],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.scss'
})
export class BookingsComponent {
  public displayedTFGColumns = ['name', 'department', 'slots']
  public displayedGroupColumns = ['name', 'isPrivate']
}
