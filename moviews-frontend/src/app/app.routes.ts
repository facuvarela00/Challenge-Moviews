import { Routes } from '@angular/router';
import { HomeComponent } from './views/home/home.component';
import { LoginComponent } from './views/login/login.component';
import { MovieComponent } from './views/movie/movie.component';

import { authGuard } from './core/guards/auth-guard';
import { loginGuard } from './core/guards/login-guard';

export const routes: Routes = [

    { path: '', redirectTo: 'login', pathMatch: 'full' },

    { path: 'login', component: LoginComponent, canActivate: [loginGuard] },

    { path: 'inicio', component: HomeComponent, canActivate: [authGuard] },

    { path: 'pelicula/:id', component: MovieComponent, canActivate: [authGuard] },

    { path: '**', redirectTo: 'login' }
];
