import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layout/components/main-layout/main-layout.component';
import { LoginComponent } from './modules/login/pages/login/login.component';

export const routes: Routes = [
    // {
    //     path: '',
    //     component: LoginComponent,
    //     children: []
    // },
    {
        path: '',
        component: MainLayoutComponent,
        children: [
            {
                path: '',
                component: LoginComponent
            }
        ]
    }
];
