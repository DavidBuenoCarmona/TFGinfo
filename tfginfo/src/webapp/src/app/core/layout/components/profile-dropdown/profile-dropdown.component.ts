import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { route } from '../../../../routes';
import { CommonModule } from '@angular/common';
import { RoleId } from '../../../../modules/admin/models/role.model';
import { ConfigurationService } from '../../../services/configuration.service';

@Component({
    selector: 'profile-dropdown',
    imports: [TranslateModule, RouterModule, CommonModule],
    templateUrl: './profile-dropdown.component.html',
    styleUrl: './profile-dropdown.component.scss'
})
export class ProfileDropdownComponent implements OnInit {
    route = route;
    isAdmin: boolean = false;
    isProfessor: boolean = false;
    userId!: string;
    profileRoute: string = '';

    constructor(
        public router: Router,
        private configurationService: ConfigurationService) {}

    ngOnInit(): void {
        let role = this.configurationService.getRole();
        this.isAdmin = role === RoleId.Admin;
        this.isProfessor = role === RoleId.Professor;
        this.userId = this.configurationService.getUser()?.id || '';
        if (this.isProfessor) {
            this.profileRoute = route.professor.list + '/' + this.userId;
        } else {
            this.profileRoute = route.student.list + '/' + this.userId;
        }
    }
    
    logout() {
        localStorage.removeItem('token');
        this.configurationService.setUser(null);
        this.router.navigate(['/login']);
    }

    changeUniversity() {
        this.router.navigate(['/admin/university']);
    }
}
