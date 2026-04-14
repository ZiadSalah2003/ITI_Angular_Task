import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './not-found-component.html',
})
export class NotFoundComponent {}
