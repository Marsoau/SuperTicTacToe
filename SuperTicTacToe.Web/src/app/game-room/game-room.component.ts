import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameAPI } from '../../../services/GameAPI.service';

@Component({
	selector: 'app-game-room',
	standalone: true,
	imports: [],
	templateUrl: './game-room.component.html',
	styleUrl: './game-room.component.scss'
})
export class GameRoomComponent {
	requestedId: number | null = null;

	constructor(
		public api: GameAPI,
		private route: ActivatedRoute
	) {
		console.log('Called game room constructor');
		this.route.queryParams.subscribe(params => {
			this.requestedId = params['id'];
		});
	}
}
