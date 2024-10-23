import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService)
  //usersFromHomeComponent = input.required<any>();
  private toastr = inject(ToastrService);
  cancelRegister = output<boolean>();
  model: any = {};

  register() {
    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.toastr.success("Registration successful, Welcome " + this.model.username);
        this.cancel();
      },
      error: (error) => {
        this.toastr.error(error.error.errors.request);
      }
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
