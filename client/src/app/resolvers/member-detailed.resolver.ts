import { ResolveFn } from '@angular/router';
 import { MembersService } from '../services/members.service';
 import { inject } from '@angular/core';
 import { Member } from '../models/member';
 
 export const memberDetailedResolver: ResolveFn<Member | null> = (route, state) => {
   const membersService = inject(MembersService);
   const username = route.paramMap.get("username");
 
   if (!username) return null;
   
   return membersService.getMember(username);
 };