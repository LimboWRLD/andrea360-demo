import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideTranslateService, TranslateModule } from '@ngx-translate/core';
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AutoRefreshTokenService, createInterceptorCondition, INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG, IncludeBearerTokenCondition, includeBearerTokenInterceptor, provideKeycloak, UserActivityService, withAutoRefreshToken } from 'keycloak-angular';
import { environment } from '../environments/environment.development';
import { provideNgxStripe } from 'ngx-stripe';

const backendCondition: IncludeBearerTokenCondition = createInterceptorCondition<IncludeBearerTokenCondition>({
  urlPattern: /http:\/\/localhost:5000\/api\/.*/i,
  bearerPrefix: 'Bearer'
});

export const appConfig: ApplicationConfig = {
  providers: [
    provideNgxStripe(environment.stripePublicKey),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideKeycloak({
      config: {
        url: environment.keycloakURL,
        realm: environment.keycloakRealm,
        clientId: environment.keycloakClient,
      },
      initOptions: {
        onLoad: 'login-required',
        silentCheckSsoRedirectUri: `${window.location.origin}/silent-check-sso.html`,
      },
      features: [
        withAutoRefreshToken({
          onInactivityTimeout: 'logout',
          sessionTimeout: 300000,
        }),
      ],
      providers: [
        {
          provide: INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
          useValue: [backendCondition],
        },
        AutoRefreshTokenService,
        UserActivityService,
      ],
    }),
    provideHttpClient(withInterceptors([includeBearerTokenInterceptor])),
    importProvidersFrom(TranslateModule),
    provideTranslateService({
      lang: 'en',
      fallbackLang: 'en',
      loader: provideTranslateHttpLoader({
        prefix: './assets/i18n/',
        suffix: '.json',
      }),
    }),
    provideRouter(routes),
  ],
};
