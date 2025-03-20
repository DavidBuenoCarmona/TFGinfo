import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { AppConfigService } from '../../../services/app-config.service';
import { map } from 'rxjs';

@Component({
    selector: 'logo',
    templateUrl: './logo.component.html',
    styleUrl: './logo.component.scss'
})
export class LogoComponent implements AfterViewInit {
    @Input() public isAuth: boolean = false;
    public logoUrl!: string;
    public logoWidth!: number;

    constructor(public appConfig: AppConfigService) { }

    ngAfterViewInit(): void {
        setTimeout(() => {
            const logoType = this.isAuth ? 'auth-width' : 'header-width';
            this.logoWidth =  this.appConfig.getConfig().app.logo[logoType];
            this.logoUrl = this.appConfig.getConfig().app.logo.src;
        }, 10);
    }

}
