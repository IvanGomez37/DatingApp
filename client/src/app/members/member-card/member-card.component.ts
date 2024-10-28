import { Component, input, ViewEncapsulation } from '@angular/core';
import { Member } from '../../models/member';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
  encapsulation: ViewEncapsulation.None
})

export class MemberCardComponent {
  member = input.required<Member>();
}