import { Component, inject } from '@angular/core';
import Keycloak from 'keycloak-js';
import { MainLayoutComponent } from "./shared/components/main-layout/main-layout.component";

@Component({
  selector: 'app-root',
  imports: [MainLayoutComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'andrea360-web';

  keycloak = inject(Keycloak);

  ngOnInit(): void {
    if (this.keycloak.authenticated) {
    } else {
      const checkLoginInterval = setInterval(() => {
        if (this.keycloak.authenticated) {
          
          clearInterval(checkLoginInterval);
        }
      }, 100);
    }
  }
}
