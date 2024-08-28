import { Component } from '@angular/core';
import { GameAPI } from '../../../services/GameAPI.service';

@Component({
  selector: 'app-xoplayers',
  standalone: true,
  imports: [],
  templateUrl: './xoplayers.component.html',
  styleUrl: './xoplayers.component.scss'
})
export class XOPlayersComponent {
	constructor(
		public api: GameAPI
	) {

	}

	SwitchToX() {
		this.api.http.InvokeCommand("SwitchToX");
	}
	SwitchToO() {
		this.api.http.InvokeCommand("SwitchToO");
	}
}
