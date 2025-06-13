import { AfterViewInit, Component, OnInit } from '@angular/core';
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
import { UniversityService } from '../../../../modules/admin/services/university.service';
import { UniversitySelectionService } from '../../../services/localstorage.service';

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

export class HeaderComponent implements OnInit, AfterViewInit {
    isAdmin: boolean = false;
    route = route;
    selectedUniversityName: string = '';

    constructor(
        private router: Router,
        private configService: ConfigurationService,
        private universityService: UniversityService,
        private universitySelectionService: UniversitySelectionService
    ) { }

    ngOnInit(): void {
        let role = this.configService.getRole();
        this.isAdmin = role === RoleId.Admin;

    }

    selectUniversity() {
        this.router.navigate(["/admin/university"]);
    }

    ngAfterViewInit(): void {
        if (localStorage.getItem('selectedUniversity')) {
            this.universityService.getUniversity(Number(localStorage.getItem('selectedUniversity'))).subscribe(u => this.selectedUniversityName = u.name);
        }
        this.universitySelectionService.universityId$.subscribe(id => {
            if (id) {
                this.universityService.getUniversity(id).subscribe(u => this.selectedUniversityName = u.name);
            } else {
                this.selectedUniversityName = '';
            }
        });
    }
}
