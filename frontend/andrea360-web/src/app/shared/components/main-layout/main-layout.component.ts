import { Component } from '@angular/core';
import { SidebarComponent } from "../sidebar/sidebar.component";
import { HeaderComponent } from "../header/header.component";
import { RouterModule } from "@angular/router";
import { HasRolesDirective } from 'keycloak-angular';

@Component({
  selector: 'app-main-layout',
  imports: [SidebarComponent, HeaderComponent, RouterModule, HasRolesDirective],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css'
})
export class MainLayoutComponent {
  public sidebarOpen: boolean = true; 

  toggleSidebar(): void {
    this.sidebarOpen = !this.sidebarOpen;
  }

  // Keep checkbox-driven drawer in sync when it changes directly
  onDrawerChange(event: Event): void {
    const input = event.target as HTMLInputElement | null;
    if (!input) return;
    this.sidebarOpen = input.checked;
  }
}
