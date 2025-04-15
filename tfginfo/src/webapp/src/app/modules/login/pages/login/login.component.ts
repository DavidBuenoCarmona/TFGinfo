import { Component, OnInit } from '@angular/core';
import { LogoComponent } from '../../../../core/layout/components/logo/logo.component';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-login',
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

    constructor(private fb: FormBuilder, private router: Router) { }
  
    ngOnInit(): void {
      // Configuración del tema
      localStorage.getItem('theme') === 'dark' ? document.body.classList.add('dark-mode') : document.body.classList.remove('dark-mode');
  
      // Inicialización del formulario reactivo
      this.loginForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]]
      });
    }
  
    onSubmit(): void {
      if (this.loginForm.valid) {
        const { email, password } = this.loginForm.value;
        console.log('Datos de inicio de sesión:', { email, password });
        // Aquí puedes agregar la lógica para autenticar al usuario
        this.router.navigate(['']); // Redirige al usuario después del inicio de sesión
      } else {
        console.log('Formulario inválido');
      }
    }
  
}
