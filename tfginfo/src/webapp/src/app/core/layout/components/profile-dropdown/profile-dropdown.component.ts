import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { route } from '../../../../routes';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'profile-dropdown',
    imports: [TranslateModule, RouterModule, CommonModule],
    templateUrl: './profile-dropdown.component.html',
    styleUrl: './profile-dropdown.component.scss'
})
export class ProfileDropdownComponent {
    route = route;

    constructor(public router: Router) {}
    
    logout() {
        localStorage.removeItem('user');
        localStorage.removeItem('role');
        this.router.navigate(['/login']);
    }

    changeUniversity() {
        this.router.navigate(['/admin/university']);
    }
}
