import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { route } from '../../../../routes';
import { CommonModule } from '@angular/common';
import { RoleId } from '../../../../modules/admin/models/role.model';
import { ConfigurationService } from '../../../services/configuration.service';
import { UniversitySelectionService } from '../../../services/localstorage.service';

@Component({
    selector: 'profile-dropdown',
    imports: [TranslateModule, RouterModule, CommonModule],
    templateUrl: './profile-dropdown.component.html',
    styleUrl: './profile-dropdown.component.scss'
})
export class ProfileDropdownComponent implements OnInit {
    @Output() onNavigation: EventEmitter<void> = new EventEmitter<void>();
    route = route;
    isAdmin: boolean = false;
    isProfessor: boolean = false;
    userId!: string;
    profileRoute: string = '';

    constructor(
        public router: Router,
        private configurationService: ConfigurationService,
        private universitySelectionService: UniversitySelectionService) {}

    ngOnInit(): void {
        let role = this.configurationService.getRole();
        this.isAdmin = role === RoleId.Admin;
        this.isProfessor = role === RoleId.Professor;
        this.userId = this.configurationService.getUser()?.id.toString() || '';
        if (this.isProfessor) {
            this.profileRoute = route.professor.list + '/' + this.userId;
        } else {
            this.profileRoute = route.student.list + '/' + this.userId;
        }
    }
    
    logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('selectedUniversity');
        this.universitySelectionService.setUniversityId(null);
        this.configurationService.setUser(undefined);
        this.router.navigate(['/login']);
    }

    changeUniversity() {
        this.onNavigation.emit();
        this.router.navigate(['/admin/university']);
    }
}
