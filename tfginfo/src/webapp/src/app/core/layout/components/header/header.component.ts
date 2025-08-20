import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { TranslateModule } from '@ngx-translate/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { route } from '../../../../routes';
import { ProfileDropdownComponent } from '../profile-dropdown/profile-dropdown.component';
import { ThemeToggleComponent } from '../theme-toggle/theme-toggle.component';
import { LanguageToggleComponent } from '../language-toggle/language-toggle.component';
import { CommonModule } from '@angular/common';
import { RoleId } from '../../../../modules/admin/models/role.model';
import { ConfigurationService } from '../../../services/configuration.service';
import { UniversityService } from '../../../../modules/admin/services/university.service';
import { UniversitySelectionService } from '../../../services/localstorage.service';
declare var bootstrap: any; // Si usas Bootstrap 5 JS


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
    @ViewChild('navbarNav') navbarNav!: ElementRef;
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

        this.router.events.subscribe(event => {
            if (event instanceof NavigationEnd) {
                this.closeNavbar();
            }
        });
    }

    closeNavbar() {
        const navbar = this.navbarNav.nativeElement;
        if (navbar.classList.contains('show')) {
            const collapse = bootstrap.Collapse.getOrCreateInstance(navbar);
            collapse.hide();
        }
    }

    selectUniversity() {
        this.closeNavbar();
        this.router.navigate(["/admin/university"]);
    }

    ngAfterViewInit(): void {
        if (localStorage.getItem('selectedUniversity')) {
            this.universityService.getUniversity(Number(localStorage.getItem('selectedUniversity'))).subscribe(u => this.selectedUniversityName = u.acronym != "" ? u.acronym : u.name);
        }
        this.universitySelectionService.universityId$.subscribe(id => {
            if (id) {
                this.universityService.getUniversity(id).subscribe(u => this.selectedUniversityName = u.acronym != "" ? u.acronym : u.name);
            } else {
                this.selectedUniversityName = '';
            }
        });
    }
}
