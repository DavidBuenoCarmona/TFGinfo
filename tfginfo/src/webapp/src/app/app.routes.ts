import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layout/components/main-layout/main-layout.component';
import { LoginComponent } from './modules/login/pages/login/login.component';
import { BookingsComponent } from './modules/bookings/pages/bookings/bookings.component';
import { TfgListComponent } from './modules/tfg/components/tfg-list/tfg-list.component';
import { TfgSearchComponent } from './modules/tfg/pages/tfg-search/tfg-search.component';

export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent,
        children: []
    },
    {
        path: '',
        component: MainLayoutComponent,
        children: [
            {
                path: '',
                component: BookingsComponent
            },
            {
                path: 'tfg',
                component: TfgSearchComponent
            }
        ]
    }
];
