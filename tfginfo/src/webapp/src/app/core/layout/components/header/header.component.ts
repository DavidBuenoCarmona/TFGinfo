import { Component } from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { LayoutModule } from '../../layout.module';

@Component({
  selector: 'layout-header',
  imports: [LogoComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

}
