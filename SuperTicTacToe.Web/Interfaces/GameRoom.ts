import { TTTChar } from "../Enumerators/TTTChar";
import { IPlayer } from "./Player";
import { ISuperGame } from "./SuperGame";

export interface IGameRoom {
    id: number;
    name: string;
    spectators: IPlayer[];
    playerX: IPlayer | null;
    playerO: IPlayer | null;
    superGame: ISuperGame;
    currentTurnChar: TTTChar;
}