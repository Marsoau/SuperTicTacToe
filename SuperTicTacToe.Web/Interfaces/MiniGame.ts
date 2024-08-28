import { TTTChar } from "../Enumerators/TTTChar";
import { TTTResult } from "../Enumerators/TTTResult";

export interface IMiniGame {
    table: Array<TTTChar | null>;
    isEnabled: boolean;
    finalResult: TTTResult;
}

