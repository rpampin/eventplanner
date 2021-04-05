import { BrowserModule, DomSanitizer } from '@angular/platform-browser';
import { LOCALE_ID, NgModule, Pipe, PipeTransform } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { FroalaEditorModule, FroalaViewModule } from 'angular-froala-wysiwyg';
import { JwPaginationModule } from 'jw-angular-pagination';
import { NgxSpinnerModule } from "ngx-spinner";
import localePh from '@angular/common/locales/en-PH';
registerLocaleData(localePh);

import 'froala-editor/js/plugins.pkgd.min.js';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { EventTypesComponent } from './event-types/event-types.component';
import { SupplierTypesComponent } from './supplier-types/supplier-types.component';
import { EventComponent } from './event/event.component';
import { UpcomingEventsComponent } from './upcoming-events/upcoming-events.component';
import { GuestComponent } from './guest/guest.component';
import { SupplierComponent } from './supplier/supplier.component';
import { GuestFormComponent } from './guest-form/guest-form.component';
import { SupplierFormComponent } from './supplier-form/supplier-form.component';
import { PlanComponent } from './plan/plan.component';
import { ToastComponent } from './toast.component';
import { ConfigComponent } from './config/config.component';
import { LoadingInterceptor } from './loading.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PrintReportComponent } from './print-report/print-report.component';
import { EmailComponent } from './email/email.component';
import { getCurrencySymbol, registerLocaleData } from '@angular/common';
import { HttpErrorInterceptor } from './error.interceptor';
import { PackagesComponent } from './packages/packages.component';

@Pipe({ name: 'safeHtml' })
export class SafeHtmlPipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) { }
  transform(value) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}
@Pipe({ name: "currencySymbol" })
export class CurrencySymbolPipe implements PipeTransform {
  transform(
    code: string,
    format: "wide" | "narrow" = "narrow",
    locale?: string
  ): string {
    return getCurrencySymbol(code, format, locale);
  }
}

@NgModule({
  declarations: [
    AppComponent,
    ToastComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent,
    EventTypesComponent,
    SupplierTypesComponent,
    EventComponent,
    UpcomingEventsComponent,
    GuestComponent,
    SupplierComponent,
    GuestFormComponent,
    SupplierFormComponent,
    PlanComponent,
    ConfigComponent,
    PrintReportComponent,
    SafeHtmlPipe,
    CurrencySymbolPipe,
    EmailComponent,
    PackagesComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    FontAwesomeModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    NgxSpinnerModule,
    JwPaginationModule,
    FroalaEditorModule.forRoot(),
    FroalaViewModule.forRoot(),
    BsDatepickerModule.forRoot(),
    AccordionModule.forRoot(),
    RouterModule.forRoot([
      { path: '', component: UpcomingEventsComponent, pathMatch: 'full' },
      { path: 'emailer', component: EmailComponent },
      { path: 'event/:eventId/suppliers/:supplierId', component: SupplierFormComponent },
      { path: 'event/:eventId/guests/:guestId', component: GuestFormComponent },
      { path: 'event/:eventId/report', component: PrintReportComponent },
      { path: 'event/:eventId/suppliers', component: SupplierComponent },
      { path: 'event/:eventId/program', component: PlanComponent },
      { path: 'event/:eventId/guests', component: GuestComponent },
      { path: 'event/:eventId', component: EventComponent },
      { path: 'event', component: EventComponent },
      { path: 'config', component: ConfigComponent },
      { path: 'event-types', component: EventTypesComponent },
      { path: 'supplier-types', component: SupplierTypesComponent },
      { path: 'packages', component: PackagesComponent },
      { path: 'upcoming-events', component: UpcomingEventsComponent }
    ])
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'en-PH' },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
