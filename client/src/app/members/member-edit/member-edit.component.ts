import { Component, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../models/member';
import { AccountService } from '../../services/account.service';
import { MembersService } from '../../services/members.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [TabsModule, FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  @ViewChild("editForm") editForm?: NgForm;
  @HostListener("window:beforeunload", ["$event"]) notify($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member?: Member;
  private accountService = inject(AccountService);
  private membersService = inject(MembersService);
  private toaster = inject(ToastrService);

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const user = this.accountService.currentUser();
    if (!user) return;
    this.membersService.getMember(user.username).subscribe({
      next: member => this.member = member
    });
  }

  updateMember() {
    this.membersService.updateMember(this.editForm?.value).subscribe({
      next: () => {
        this.toaster.success("Profile updated successfully");
        this.editForm?.reset(this.member);
    },
      error: (error) => this.toaster.error(error)
    });
  }
}
