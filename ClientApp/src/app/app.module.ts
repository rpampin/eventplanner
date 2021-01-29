import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

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

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    EventTypesComponent,
    SupplierTypesComponent,
    EventComponent,
    UpcomingEventsComponent,
    GuestComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FontAwesomeModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'event', component: EventComponent },
      { path: 'event/:id', component: EventComponent },
      { path: 'event-types', component: EventTypesComponent },
      { path: 'supplier-types', component: SupplierTypesComponent },
      { path: 'upcoming-events', component: UpcomingEventsComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
