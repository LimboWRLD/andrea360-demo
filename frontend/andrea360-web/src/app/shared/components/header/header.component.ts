import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthService } from '../../../core/auth.service';
import Keycloak from 'keycloak-js';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [TranslateModule, CommonModule, RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit{
  keycloak = inject(Keycloak);
  @Output() toggleSidebar = new EventEmitter<void>();

  @Input() isSidebarOpen: boolean = true;

  open = false;
  currentLang: string;

  constructor(public translate: TranslateService, public auth: AuthService) {
    this.currentLang = this.translate.getCurrentLang() ?? 'sr';
  }

  switchLang(lang: string) {
    this.translate.use(lang).subscribe(() => {
      this.currentLang = lang;
    });
  }

  onToggleClick(): void {
    this.toggleSidebar.emit();
  }

  theme: 'light' | 'dark' = 'light';

  ngOnInit() {
    const stored = localStorage.getItem('theme');
    this.theme = (stored as any) || 'light';
    document.documentElement.setAttribute('data-theme', this.theme);
  }

  toggleTheme() {
    this.theme = this.theme === 'light' ? 'dark' : 'light';
    localStorage.setItem('theme', this.theme);
    document.documentElement.setAttribute('data-theme', this.theme);
  }

  logout() {
    this.keycloak.logout();
  }
}
