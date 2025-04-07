import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layout/components/main-layout/main-layout.component';
import { LoginComponent } from './modules/login/pages/login/login.component';
import { BookingsComponent } from './modules/bookings/pages/bookings/bookings.component';
import { TfgSearchComponent } from './modules/tfg/pages/tfg-search/tfg-search.component';
import { ProfesorSearchComponent } from './modules/professor/pages/profesor-search/profesor-search.component';
import { GroupSearchComponent } from './modules/groups/pages/groups-search/group-search.component';
import { ProfileDetailComponent } from './modules/profile/pages/profile-detail/profile-detail.component';
import { DepartmentSearchComponent } from './modules/admin/pages/department-search/department-search.component';
import { UniversitySearchComponent } from './modules/admin/pages/university-search/university-search.component';
import { UserSearchComponent } from './modules/admin/pages/user-search/user-search.component';
import { TfgDetailComponent } from './modules/tfg/pages/tfg-detail/tfg-detail.component';
import { GroupDetailComponent } from './modules/groups/pages/group-detail/group-detail.component';


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
                component: TfgSearchComponent,
            },
            {
                path: 'tfg/:id',
                component: TfgDetailComponent
            },
            {
                path: 'working-group',
                component: GroupSearchComponent
            },
            {
                path: 'working-group/:id',
                component: GroupDetailComponent
            },

            {
                path: 'professor',
                component: ProfesorSearchComponent
            },
            {
                path: 'profile',
                component: ProfileDetailComponent
            },
            {
                path: 'admin',
                children: [
                    {
                        path: 'department',
                        component: DepartmentSearchComponent
                    },
                    {
                        path: 'university',
                        component: UniversitySearchComponent
                    },
                    {
                        path: 'user',
                        component: UserSearchComponent
                    }
                ]
            }
        ]
    },
    {
        path: '**',
        redirectTo: '',
        pathMatch: 'full'
    }
];
