import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TfgListComponent } from '../../../tfg/components/tfg-list/tfg-list.component';

@Component({
  selector: 'app-bookings',
  imports: [TranslateModule, TfgListComponent],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.scss'
})
export class BookingsComponent {

}
