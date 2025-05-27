import { Component, OnInit } from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { TranslateModule } from '@ngx-translate/core';
import { Router, RouterModule } from '@angular/router';
import { route } from '../../../../routes';
import { ProfileDropdownComponent } from '../profile-dropdown/profile-dropdown.component';
import { ThemeToggleComponent } from '../theme-toggle/theme-toggle.component';
import { LanguageToggleComponent } from '../language-toggle/language-toggle.component';
import { CommonModule } from '@angular/common';
import { RoleId } from '../../../../modules/admin/models/role.model';
import { ConfigurationService } from '../../../services/configuration.service';

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

export class HeaderComponent implements OnInit {
    isAdmin: boolean = false;
    route = route;

    constructor(
        private router: Router,
        private configService: ConfigurationService
    ) { }

    ngOnInit(): void {
        let role = this.configService.getRole();
        this.isAdmin = role === RoleId.Admin;
    }
}
