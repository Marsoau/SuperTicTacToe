import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameAPI } from '../../../services/GameAPI.service';
import { FormsModule } from '@angular/forms';
import { SuperGameComponent } from "../super-game/super-game.component";
import { XOPlayersComponent } from "../xoplayers/xoplayers.component";

@Component({
	selector: 'app-game-room',
	standalone: true,
	imports: [FormsModule, SuperGameComponent, XOPlayersComponent],
	templateUrl: './game-room.component.html',
	styleUrl: './game-room.component.scss'
})
export class GameRoomComponent {
	displayPasswordInput: boolean = false;

	requestedId: number | null = null;
	passwordInputValue: string = "";

	constructor(
		public api: GameAPI,
		private route: ActivatedRoute
	) {
		console.log('Called game room constructor');
		this.route.queryParams.subscribe(params => {
			this.requestedId = params['id'];
			this.TryJoin();
		});
	}

	TryJoin() {
		this.displayPasswordInput = false;

		if (!this.requestedId) return;

		let password: string | null = null;
		if (this.passwordInputValue) password = this.passwordInputValue;

		this.api.GameJoinRoom(this.requestedId, password, this.api.currentPlayerToken, () => {
			this.displayPasswordInput = true;
		});
	}
}
