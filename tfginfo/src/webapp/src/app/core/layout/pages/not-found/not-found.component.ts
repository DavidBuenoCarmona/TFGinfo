import { Component } from '@angular/core';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatCardMdImage, MatCardModule } from '@angular/material/card';
import { Router } from '@angular/router';
import { ConfigurationService } from '../../../services/configuration.service';
import { RoleId } from '../../../../modules/admin/models/role.model';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'not-found',
    imports: [
        MatButtonModule,
        MatCardModule,
        TranslateModule
    ],
    templateUrl: './not-found.component.html',
    styleUrl: './not-found.component.scss'
})
export class NotFoundComponent {
    constructor(
        private router: Router,
        private configurationService: ConfigurationService) { }

    goHome() {
        let role = this.configurationService.getRole();
        if (role == RoleId.Admin) {
            this.router.navigate(['/tfg']);
        } else {
            this.router.navigate(['/']);
        }
        
    }

}
