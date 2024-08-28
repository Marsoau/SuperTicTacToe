import { TTTChar } from "../Enumerators/TTTChar";
import { TTTResult } from "../Enumerators/TTTResult";

export interface IMiniGame {
    table: TTTChar[];
    isEnabled: boolean;
    finalResult: TTTResult;
}

