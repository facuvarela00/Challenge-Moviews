
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../enviroment/enviroment';
import { MovieDTO } from '../../modelos/MovieDTO.model';
import { MovieDetailDTO } from '../../modelos/MovieDetailDTO.model';
import { ReviewDTO } from '../../modelos/ReviewDTO.model';

@Injectable({
    providedIn: 'root'
})

export class MovieService {

    private baseUrl = environment.URL_BASE;

    constructor(private http: HttpClient) { }

    obtenerPeliculas(): Observable<MovieDTO[]> {
        return this.http.get<MovieDTO[]>(`${this.baseUrl}/pelicula/listar`);
    }

    obtenerPorId(id: string): Observable<MovieDetailDTO> {
        return this.http.get<MovieDetailDTO>(
            `${this.baseUrl}/pelicula/obtener/${id}`
        );
    }

    crear(movie: any) {
        return this.http.post(`${this.baseUrl}/pelicula/crear`, movie);
    }

    actualizar(id: string, movie: any) {
        return this.http.put(`${this.baseUrl}/pelicula/actualizar/${id}`, movie);
    }

    borrar(id: string) {
        return this.http.delete(`${this.baseUrl}/pelicula/borrar/${id}`);
    }

    agregarReview(movieId: string, review: ReviewDTO): Observable<any> {
        return this.http.post(`${this.baseUrl}/pelicula/${movieId}/review`, review);
    }


}
