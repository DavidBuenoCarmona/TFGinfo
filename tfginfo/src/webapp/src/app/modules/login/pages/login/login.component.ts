import { Component, inject, OnInit } from '@angular/core';
import { LogoComponent } from '../../../../core/layout/components/logo/logo.component';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../services/auth.service';
import { ChangePasswordDialogComponent } from '../../components/change-password-dialog/change-password-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [
        LogoComponent,
        TranslateModule,
        CommonModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        MatButtonModule,
    ],
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
    loginForm!: FormGroup;

    constructor(
        private fb: FormBuilder,
        private router: Router,
        private dialog: MatDialog,
        private authService: AuthService,
    ) {}

    ngOnInit(): void {
        // Inicialización del formulario reactivo
        this.loginForm = this.fb.group({
            username: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required]]
        });
    }

    onSubmit(): void {
        const credentials = this.loginForm.value;
        this.authService.login(credentials).subscribe((response) => {
            if (response.firstLogin) {
                this.dialog.open(ChangePasswordDialogComponent, {
                    data: { username: credentials.username }
                });
            } else {
                localStorage.setItem('user', JSON.stringify(response.user));
                this.router.navigate(['/']);
            }
        });


    }

}
