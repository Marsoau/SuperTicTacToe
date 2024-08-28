import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { GameAPI } from "./GameAPI.service";
import { lastValueFrom } from "rxjs";
import { Config } from "../Config";

export class GameHttp {
    public constructor (
        private readonly _api: GameAPI,
        private readonly _http: HttpClient
    ) {

    }

    public async Get<T>(request: string, onerror: any = null) {
        try {
            return await lastValueFrom(this._http.get<T>(`${Config.apiUrl}/${request}`));
        }
        catch (x) {
            console.log(x)
            let error = x as HttpErrorResponse;
            console.log(error);

            let errorMessage = typeof error.error == "string" ? error.error : "Failed to connect";

            if (onerror) onerror(error.status, errorMessage);
            return null;
        }
    }
    
    public async InvokeCommand(commandString: string) {
		return await lastValueFrom(
            this._http.get(`${Config.apiUrl}/Game/Command?name=${commandString}`, {
                headers: new HttpHeaders({
                    "player-token": this._api.currentPlayerToken ?? ""
                }) 
            })
        );
    }
}