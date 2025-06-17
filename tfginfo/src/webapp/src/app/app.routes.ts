import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layout/components/main-layout/main-layout.component';
import { LoginComponent } from './modules/login/pages/login/login.component';
import { BookingsComponent } from './modules/bookings/pages/bookings/bookings.component';
import { TfgSearchComponent } from './modules/tfg/pages/tfg-search/tfg-search.component';
import { ProfessorSearchComponent } from './modules/professor/pages/profesor-search/professor-search.component';
import { GroupSearchComponent } from './modules/groups/pages/groups-search/group-search.component';
import { ProfileDetailComponent } from './modules/profile/pages/profile-detail/profile-detail.component';
import { DepartmentSearchComponent } from './modules/admin/pages/department-search/department-search.component';
import { UniversitySearchComponent } from './modules/admin/pages/university-search/university-search.component';
import { StudentSearchComponent } from './modules/admin/pages/student-search/student-search.component';
import { TfgDetailComponent } from './modules/tfg/pages/tfg-detail/tfg-detail.component';
import { GroupDetailComponent } from './modules/groups/pages/group-detail/group-detail.component';
import { ProfessorDetailComponent } from './modules/professor/pages/professor-detail/professor-detail.component';
import { UniversityDetailComponent } from './modules/admin/pages/university-detail/university-detail.component';
import { DepartmentDetailComponent } from './modules/admin/pages/department-detail/department-detail.component';
import { CareerSearchComponent } from './modules/admin/pages/career-search/career-search.component';
import { CareerDetailComponent } from './modules/admin/pages/career-detail/career-detail.component';
import { AuthLayoutComponent } from './modules/login/pages/auth-layout/auth-layout.component';
import { RoleId } from './modules/admin/models/role.model';
import { RoleGuard } from './core/services/RoleGuard';
import path from 'path';
import { NotFoundComponent } from './core/layout/pages/not-found/not-found.component';
import { AuthGuard } from './core/services/authGuard';


export const routes: Routes = [
    {
        path: '',
        component: MainLayoutComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: '',
                component: BookingsComponent,
                canActivate: [RoleGuard],
                data: { roles: [RoleId.Student, RoleId.Professor] }
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
                component: ProfessorSearchComponent
            },
            {
                path: 'professor/:id',
                component: ProfessorDetailComponent
            },
            {
                path: 'profile',
                component: ProfileDetailComponent
            },
            {
                path: 'student',
                component: StudentSearchComponent,
                canActivate: [RoleGuard],
                data: { roles: [RoleId.Admin] }
            },
            {
                path: 'student/:id',
                component: ProfileDetailComponent,
                canActivate: [RoleGuard],
                data: { roles: [RoleId.Admin, RoleId.Student] }
            },
            {
                path: 'admin',
                canActivate: [RoleGuard, AuthGuard],
                data: { roles: [RoleId.Admin] },
                children: [
                    {
                        path: 'department',
                        component: DepartmentSearchComponent
                    },
                    {
                        path: 'department/:id',
                        component: DepartmentDetailComponent
                    },
                    {
                        path: 'university',
                        component: UniversitySearchComponent
                    },
                    {
                        path: 'university/:id',
                        component: UniversityDetailComponent
                    },
                    {
                        path: 'department/:id',
                        component: DepartmentDetailComponent
                    },
                    {
                        path: 'career',
                        component: CareerSearchComponent
                    },
                    {
                        path: 'career/:id',
                        component: CareerDetailComponent
                    },
                ]
            }
        ]
    },
    {
        path: '',
        component: AuthLayoutComponent,
        children: [
            {
                path: 'login',
                component: LoginComponent,
                children: []
            },
            {
                path: 'not-found',
                component: NotFoundComponent,
                children: []
            },
        ]
    },
    {
        path: '**',
        redirectTo: '/not-found',
        pathMatch: 'full'
    }
];
