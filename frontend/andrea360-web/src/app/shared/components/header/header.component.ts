import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-header',
  imports: [TranslatePipe, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  open = false;
  currentLang: string;

  @Output() sidebarToggle = new EventEmitter<void>(); // Za prikaz sidebar-a

  constructor(private translate: TranslateService) {
    this.currentLang = this.translate.getCurrentLang() ?? 'sr';
  }

  switchLang(lang: string) {
    this.translate.use(lang);
    this.currentLang = lang;
  }

}
