import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-header',
  imports: [TranslateModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
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
}
