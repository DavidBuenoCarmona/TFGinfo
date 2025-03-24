import { Component } from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { LayoutModule } from '../../layout.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'layout-header',
  imports: [LogoComponent, TranslateModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

}
