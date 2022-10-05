import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EnvServiceProvider } from './env.service.provider';
import { HttpErrorInterceptor } from './shared/interceptors/http-error.interceptor';
import { HttpLoadingInterceptor } from './shared/interceptors/http-loading.interceptor';
import { HttpReqHeaderInterceptor } from './shared/interceptors/http-req-header.interceptor';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatSnackBarModule,
    MatProgressBarModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpLoadingInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpReqHeaderInterceptor,
      multi: true,
    },
    EnvServiceProvider,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
