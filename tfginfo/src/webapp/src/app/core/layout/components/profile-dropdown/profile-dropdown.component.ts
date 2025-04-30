import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { route } from '../../../../routes';
import { CommonModule } from '@angular/common';
import { RoleId } from '../../../../modules/admin/models/role.model';

@Component({
    selector: 'profile-dropdown',
    imports: [TranslateModule, RouterModule, CommonModule],
    templateUrl: './profile-dropdown.component.html',
    styleUrl: './profile-dropdown.component.scss'
})
export class ProfileDropdownComponent implements OnInit {
    route = route;
    isAdmin: boolean = false;

    constructor(public router: Router) {}

    ngOnInit(): void {
        let role = Number.parseInt(localStorage.getItem('role')!);
        this.isAdmin = role === RoleId.Admin;
    }
    
    logout() {
        localStorage.removeItem('user');
        localStorage.removeItem('role');
        this.router.navigate(['/login']);
    }

    changeUniversity() {
        this.router.navigate(['/admin/university']);
    }
}
