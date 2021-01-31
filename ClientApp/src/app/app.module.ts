import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { JwPaginationModule } from 'jw-angular-pagination';

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
    ConfigComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FontAwesomeModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    JwPaginationModule,
    AngularEditorModule,
    RouterModule.forRoot([
      { path: '', component: UpcomingEventsComponent, pathMatch: 'full' },
      { path: 'event/:eventId/suppliers/:supplierId', component: SupplierFormComponent },
      { path: 'event/:eventId/guests/:guestId', component: GuestFormComponent },
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
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
