import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-header',
  imports: [TranslateModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit{
  @Output() toggleSidebar = new EventEmitter<void>();

  @Input() isSidebarOpen: boolean = true;

  open = false;
  currentLang: string;

  constructor(public translate: TranslateService) {
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
}
