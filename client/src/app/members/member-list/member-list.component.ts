import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../services/members.service';
import { Member } from '../../models/member';
import { MemberCardComponent } from '../member-card/member-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../services/account.service';
import { UserParams } from '../../models/userParams';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule,ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit{
  membersService = inject(MembersService);
  genderList = [{value: "female", display: "Females"}, {value: "male", display: "Males"}]
  
  ngOnInit(): void {
    if (!this.membersService.paginatedResult()){
      this.loadMembers();
    }
  }

  loadMembers(): void {
    this.membersService.getMembers();
  }
  
  resetFilters() {
    this.membersService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    if (this.membersService.userParams().pageNumber !== event.page) {
      this.membersService.userParams().pageNumber = event.page;
      this.loadMembers();
    }
  }
}
