import { Component, Input } from '@angular/core';
import { ICONS } from './sidebar-icons';
import { HasRolesDirective } from 'keycloak-angular';
import { AsyncPipe, CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { RouterModule } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-sidebar',
  imports: [HasRolesDirective, CommonModule, TranslateModule, RouterModule, AsyncPipe],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  @Input() isOpen: boolean = true;
  navLinks;

  constructor(private sanitizer: DomSanitizer, public translate: TranslateService) {
    this.navLinks = [
      {
        route: '/locations',
        label: 'SIDEBAR.LOCATIONS',
        iconPath: sanitizer.bypassSecurityTrustHtml(ICONS.location),
        roles: ['admin'],
      },
      {
        route: '/employees',
        label: 'SIDEBAR.EMPLOYEES',
        iconPath: sanitizer.bypassSecurityTrustHtml(ICONS.employees),
        roles: ['admin'],
      },
      {
        route: '/members',
        label: 'SIDEBAR.MEMBERS',
        iconPath: sanitizer.bypassSecurityTrustHtml(ICONS.members),
        roles: ['admin', 'employee'],
      },
      {
        route: '/services',
        label: 'SIDEBAR.SERVICES',
        iconPath: sanitizer.bypassSecurityTrustHtml(ICONS.services),
        roles: ['admin', 'employee'],
      },
      {
        route: '/schedule',
        label: 'SIDEBAR.SCHEDULE',
        iconPath: sanitizer.bypassSecurityTrustHtml(ICONS.calendar),
        roles: ['admin', 'employee'],
      },
      {
        route: '/shop',
        label: 'SIDEBAR.SHOP',
        iconPath: sanitizer.bypassSecurityTrustHtml(ICONS.cart),
        roles: ['member'],
      },
      {
        route: '/booking',
        label: 'SIDEBAR.BOOKING',
        iconPath: sanitizer.bypassSecurityTrustHtml(ICONS.calendarBooking),
        roles: ['member'],
      },
    ];
  }

  onDrawerToggle(event: Event) {
    const input = event.target as HTMLInputElement;
    this.isOpen = !!input.checked;
  }
}
