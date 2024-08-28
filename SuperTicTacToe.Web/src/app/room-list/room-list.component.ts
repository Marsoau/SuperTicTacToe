import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { GameAPI } from '../../../services/GameAPI.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-room-list',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './room-list.component.html',
  styleUrl: './room-list.component.scss'
})
export class RoomListComponent {
	nameInputValue: string = "";
	passwordInputValue: string = "";
	
	constructor (
		public api: GameAPI,
		public router: Router
	) {

	}

	async CreateClick() {
		if (!this.nameInputValue) return;
		let password: string | null = null;
		if (this.passwordInputValue) password = this.passwordInputValue;

		let crationResponse = await this.api.GameCreateRoom(this.nameInputValue, password);
		if (!crationResponse) return;

		this.api.currentPlayerToken = crationResponse.playerToken;
		this.router.navigate(["/game"], {
			queryParams: {
				id: crationResponse.roomId
			}
		})
	}
}
