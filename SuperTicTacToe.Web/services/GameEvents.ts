import { Config } from "../Config";
import { TTTChar } from "../Enumerators/TTTChar";
import { TTTResult } from "../Enumerators/TTTResult";
import { IPlayer } from "../Interfaces/Player";
import { GameAPI } from "./GameAPI.service";

export class GameEvents {
    public constructor (
        private readonly _api: GameAPI,
    ) {

    }

    ConnectRoomEvents() {
        if (!this._api.currentRoom) return;

        let events = new EventSource(`${Config.apiUrl}/Game/Events/Game?id=${this._api.currentRoom.id}`);

        this.SubscribeToGameEvents(events);
    }

    public AddEventListener(es: EventSource, eventHeader: string, handler: any) {
        es.addEventListener(eventHeader, (message) => {
            handler(JSON.parse(message.data));
        });
    }

    SubscribeToGameEvents(es: EventSource) {
        console.log("ssg")
        this.AddEventListener(es, "XPlayerChanged", (newPlayer: IPlayer | null) => {
            if (!this._api.currentRoom) return;
            this._api.currentRoom.playerX = newPlayer;
        })
        this.AddEventListener(es, "OPlayerChanged", (newPlayer: IPlayer | null) => {
            if (!this._api.currentRoom) return;
            this._api.currentRoom.playerO = newPlayer;
        })
        this.AddEventListener(es, "SpectatorPlayersChanged", (newPlayers: IPlayer[]) => {
            if (!this._api.currentRoom) return;
            this._api.currentRoom.spectators = newPlayers;
        })

        this.AddEventListener(es, "SuperGameResultChanged", (newResult: TTTResult) => {
            if (!this._api.currentRoom) return;
            this._api.currentRoom.superGame.finalResult = newResult
        })
        this.AddEventListener(es, "MiniGameResultChanged", (data: any) => {
            if (!this._api.currentRoom) return;
            this._api.currentRoom.superGame.miniGames[data.gameI].finalResult = data.newResult;
        })
        this.AddEventListener(es, "MiniGameCellWasSet", (data: any) => {
            if (!this._api.currentRoom) return;
            this._api.currentRoom.superGame.miniGames[data.gameI].table[data.x + data.y * 3] = data.c;
        })
        this.AddEventListener(es, "MiniGameStateChanged", (data: any) => {
            if (!this._api.currentRoom) return;
            this._api.currentRoom.superGame.miniGames[data.gameI].isEnabled = data.newState;
        })
        this.AddEventListener(es, "TurnCharChanged", (newChar: TTTChar) => {
            if (!this._api.currentRoom) return;
            this._api.currentRoom.currentTurnChar = newChar;
        })
    }
}