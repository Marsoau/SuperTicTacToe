import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, Routes } from '@angular/router';
import { RoomListComponent } from './room-list/room-list.component';
import { GameRoomComponent } from './game-room/game-room.component';
import { provideHttpClient } from '@angular/common/http';

const routes: Routes = [
    { path: '', component: RoomListComponent },
    { path: 'game', component: GameRoomComponent },
];

export const appConfig: ApplicationConfig = {
    providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes), provideHttpClient()]
};
