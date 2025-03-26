import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'language-toggle',
  imports: [CommonModule, TranslateModule],
  templateUrl: './language-toggle.component.html',
  styleUrl: './language-toggle.component.scss'
})
export class LanguageToggleComponent {
    public usedLang: string = "es";

    constructor(private translate: TranslateService) {}

    ngOnInit() {
      this.usedLang = localStorage.getItem('lang') || 'es';
      this.changeLanguage(this.usedLang);
    }

      
    changeLanguage(lang: string) {
        this.translate.use(lang);
        this.usedLang = lang;
        localStorage.setItem('lang', lang); // Guardar selección en localStorage
      }
}
