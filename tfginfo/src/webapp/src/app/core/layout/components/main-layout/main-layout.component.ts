import { Component } from '@angular/core';
import { HeaderComponent } from '../header/header.component';
import { LayoutModule } from '../../layout.module';

@Component({
  selector: 'app-main-layout',
  imports: [HeaderComponent],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss'
})
export class MainLayoutComponent {

}
