import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './guards/auth.guard';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorsComponent } from './errors/server-errors/server-errors.component';

export const routes: Routes = [
    { path: "", component: HomeComponent },
    {
        path: "",
        runGuardsAndResolvers: "always",
        canActivate: [authGuard],
        children: [
            { path: "members", component: MemberListComponent},
            { path: "members/:username", component: MemberDetailComponent },
            { path: "lists", component: ListsComponent },
            { path: "messages", component: MessagesComponent },]
    },
    { path: "not-found", component: NotFoundComponent },
    { path: "server-error", component: ServerErrorsComponent },
    { path: "errors", component: TestErrorsComponent },
    { path: "**", component: HomeComponent, pathMatch: "full" },
];