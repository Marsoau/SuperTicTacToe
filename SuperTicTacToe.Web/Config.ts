export abstract class Config {
    public static apiHost = `localhost`;
    public static apiPort = 58632;
    public static apiUrl = `http://${this.apiHost}:${this.apiPort}`;
}