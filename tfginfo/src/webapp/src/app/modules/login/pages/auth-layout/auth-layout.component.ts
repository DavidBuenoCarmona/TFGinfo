import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-auth-layout',
  imports: [RouterOutlet],
  templateUrl: './auth-layout.component.html',
  styleUrl: './auth-layout.component.scss'
})
export class AuthLayoutComponent implements OnInit {
  constructor(public router: Router) { }

  ngOnInit(): void {
    localStorage.getItem('theme') === 'dark' ? document.body.classList.add('dark-mode') : document.body.classList.remove('dark-mode');
    
    if (localStorage.getItem('user')) {
      this.router.navigate(['/']);
    }
  }

}
