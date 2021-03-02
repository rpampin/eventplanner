import { BrowserModule, DomSanitizer } from '@angular/platform-browser';
import { NgModule, Pipe, PipeTransform } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { JwPaginationModule } from 'jw-angular-pagination';
import { NgxSpinnerModule } from "ngx-spinner";

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
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

@Pipe({ name: 'safeHtml' })
export class SafeHtmlPipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) { }
  transform(value) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}

@NgModule({
  declarations: [
    AppComponent,
    ToastComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
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
    EmailComponent
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
    AngularEditorModule,
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
      { path: 'upcoming-events', component: UpcomingEventsComponent }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
