import { TTTResult } from "../Enumerators/TTTResult";
import { IMiniGame } from "./MiniGame";

export interface ISuperGame {
    miniGames: IMiniGame[];
    finalResult: TTTResult
}

