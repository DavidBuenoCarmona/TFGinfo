import { Component } from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { TranslateModule } from '@ngx-translate/core';
import { Router, RouterModule } from '@angular/router';
import { route } from '../../../../routes';
import { ProfileDropdownComponent } from '../profile-dropdown/profile-dropdown.component';
import { ThemeToggleComponent } from '../theme-toggle/theme-toggle.component';
import { LanguageToggleComponent } from '../language-toggle/language-toggle.component';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'layout-header',
    imports: [
        CommonModule,
        LogoComponent,
        TranslateModule,
        RouterModule,
        ProfileDropdownComponent,
        ThemeToggleComponent,
        LanguageToggleComponent],
    templateUrl: './header.component.html',
    styleUrl: './header.component.scss'
})

export class HeaderComponent {

    route = route;
}
