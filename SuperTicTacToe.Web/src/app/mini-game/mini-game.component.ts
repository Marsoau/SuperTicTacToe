import { Component, Input } from '@angular/core';
import { IMiniGame } from '../../../Interfaces/MiniGame';
import { TTTChar } from '../../../Enumerators/TTTChar';
import { CommonModule } from '@angular/common';
import { GameAPI } from '../../../services/GameAPI.service';

@Component({
  selector: 'app-mini-game',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './mini-game.component.html',
  styleUrl: './mini-game.component.scss'
})
export class MiniGameComponent {
	@Input() gameIndex: number | null = null;
	@Input() miniGame: IMiniGame | null = null;

	constructor(
		public api: GameAPI
	) {

	}

	async CellClick(cellIndex: number) {
		if (this.gameIndex === null) return;
		console.log(this.gameIndex, cellIndex);

		let gameX = this.gameIndex % 3;
		let gameY = Math.floor(this.gameIndex / 3);
		let x = cellIndex % 3;
		let y = Math.floor(cellIndex / 3);

		console.log(gameX, gameY, x, y);

		let placementResult = await this.api.http.InvokeCommand(`PlaceAt&gameX=${gameX}&gameY=${gameY}&x=${x}&y=${y}`);
		console.log(`placement result: ${placementResult}`)
	}
}
