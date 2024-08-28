import { Injectable } from "@angular/core";
import { IRoomInfo } from "../Interfaces/RoomInfo";
import { IGameRoom } from "../Interfaces/GameRoom";
import { HttpClient } from "@angular/common/http";
import { GameHttp } from "./GameHttp";
import { IRoomCreationInfo } from "../Interfaces/RoomCreationInfo";
import { GameEvents } from "./GameEvents";

@Injectable({
    providedIn: 'root',
})
export class GameAPI {
    availableRooms: IRoomInfo[] = [];

    currentPlayerId: number | null = null;
    get currentPlayerToken(): string | null {
        return localStorage.getItem("player-token")
    }
    set currentPlayerToken(value) {
        if (!value) localStorage.removeItem("player-token");
        else localStorage.setItem("player-token", value ?? "")
    }
    currentRoom: IGameRoom | null = null;

    http: GameHttp;
    events: GameEvents;

    public constructor (
        private readonly _http: HttpClient
    ) {
        this.http = new GameHttp(this, _http);
        this.events = new GameEvents(this);
    }

    async GameCreateRoom(name: string, password: string | null = null) {
        let creationResponse = await this.http.Get<IRoomCreationInfo>(`Game/Create?name=${name}&password=${password ?? ""}`);

        console.log("creation responce");
        console.log(creationResponse);
        
        return creationResponse;
    }
    async GameJoinRoom(roomId: number, password: string | null = null, playerToken: string | null = null, onInvalidPassword: any | null = null) {
        let joinResponse = await this.http.Get<any>(`Game/Join?id=${roomId}&password=${password ?? ""}${playerToken ? `&player-token=${playerToken}` : ""}`, (errorCode: number, errorMessage: string) => {
            if (errorCode == 401) {
                if (onInvalidPassword) onInvalidPassword();
            }
        });
        console.log(joinResponse);

        if (!joinResponse) return;

        console.log("join responce");
        console.log(joinResponse);

        this.currentPlayerId = joinResponse.playerId;
        this.currentPlayerToken = joinResponse.playerToken;
        this.currentRoom = joinResponse.room;

        this.events.ConnectRoomEvents();
    }
}