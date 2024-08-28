import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { GameAPI } from '../../../services/GameAPI.service';

@Component({
  selector: 'app-room-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './room-list.component.html',
  styleUrl: './room-list.component.scss'
})
export class RoomListComponent {
	constructor (
		public api: GameAPI
	) {

	}
}
