import { Component, Input, OnInit } from '@angular/core';
import { Guest } from './guest';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-guest',
  templateUrl: './guest.component.html',
  styleUrls: ['./guest.component.css']
})
export class GuestComponent implements OnInit {
  @Input() guests: Guest[];
  guest: Guest = new Guest();
  guestIndex: number = -1;
  faEdit = faEdit;
  faTrash = faTrash;

  constructor() { }

  ngOnInit() {
  }

  onWillAttendChange(entry): void {
    this.guest.willAttend = entry;
  }

  clearGuest() {
    this.guest = new Guest();
    this.guestIndex = -1;
  }

  submitGuest() {
    if (this.guestIndex > -1)
      this.guests[this.guestIndex] = this.guest;
    else
      this.guests.push(this.guest);
    this.clearGuest();
  }

  editGuest(index: number, guest: Guest) {
    this.guest = { ...guest };
    this.guestIndex = index;
  }

  deleteGuest(index: number) {
    this.guests.splice(index, 1);
  }
}
