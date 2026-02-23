import { Component, OnInit } from '@angular/core';
import { MovieService } from '../../core/servicios/movieService/movie.service';
import { MovieDTO } from '../../core/modelos/MovieDTO.model';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/servicios/authService/auth.service';
import { Router } from '@angular/router';
import { Observable, combineLatest, map, BehaviorSubject } from 'rxjs';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faPen, faTrash, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors } from '@angular/forms';
import { FormsModule } from '@angular/forms';

function anioMinimo1800(control: AbstractControl): ValidationErrors | null {
    return control.value && control.value < 1800 ? { anioMinimo: true } : null;
}

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    standalone: true,
    imports: [CommonModule, HeaderComponent, FontAwesomeModule, ReactiveFormsModule, FormsModule],
})
export class HomeComponent implements OnInit {

    peliculas$!: Observable<MovieDTO[]>;
    peliculasFiltradas$!: Observable<MovieDTO[]>;

    isAdmin = false;
    faPen = faPen;
    faTrash = faTrash;
    faPlus = faPlus;
    mostrarCrearModal = false;
    mostrarEditarModal = false;
    peliculaSeleccionadaId: string | null = null;
    movieForm!: FormGroup;
    isSaving = false;

    filtroTitulo$ = new BehaviorSubject<string>('');
    filtroGenero$ = new BehaviorSubject<string>('');
    generos: string[] = ['Drama', 'Thriller', 'Crime', 'Musical', 'Sci-Fi', 'Fantasy', 'Comedy'];

    get filtroTitulo() { return this.filtroTitulo$.value; }
    set filtroTitulo(v: string) { this.filtroTitulo$.next(v); }

    get filtroGenero() { return this.filtroGenero$.value; }
    set filtroGenero(v: string) { this.filtroGenero$.next(v); }

    constructor(
        private movieService: MovieService,
        private authService: AuthService,
        private router: Router,
        private fb: FormBuilder
    ) { }

    ngOnInit() {
        this.isAdmin = this.authService.esAdmin();

        this.movieForm = this.fb.group({
            title: ['', [Validators.required, Validators.maxLength(40)]],
            genre: ['', Validators.required],
            synopsis: ['', Validators.required],
            poster: ['', Validators.required],
            releaseYear: ['', [Validators.required, Validators.min(1800), anioMinimo1800]]
        });

        this.peliculas$ = this.movieService.obtenerPeliculas();

        this.peliculasFiltradas$ = combineLatest([
            this.peliculas$,
            this.filtroTitulo$,
            this.filtroGenero$
        ]).pipe(
            map(([peliculas, titulo, genero]) =>
                peliculas.filter(p =>
                    (!titulo || p.title.toLowerCase().includes(titulo.toLowerCase())) &&
                    (!genero || p.genre === genero)
                )
            )
        );
    }

    campoInvalido(campo: string): boolean {
        const control = this.movieForm.get(campo);
        return !!(control && control.invalid && control.touched);
    }

    cerrarSesion() {
        this.authService.logout();
        this.router.navigate(['/login']);
    }

    irDetalle(id: string) {
        this.router.navigate(['/pelicula', id]);
    }

    irACrear() {
        this.mostrarCrearModal = true;
        this.movieForm.reset();
    }

    editarPelicula(id: string) {
        this.peliculaSeleccionadaId = id;
        this.mostrarEditarModal = true;
        this.movieService.obtenerPorId(id).subscribe(movie => {
            this.movieForm.patchValue(movie);
        });
    }

    guardarNueva() {
        this.movieForm.markAllAsTouched();
        if (this.movieForm.invalid || this.isSaving) return;

        this.isSaving = true;
        this.movieService.crear(this.movieForm.value).subscribe({
            next: () => {
                this.mostrarCrearModal = false;
                this.movieForm.reset();
                this.peliculas$ = this.movieService.obtenerPeliculas();
                this.isSaving = false;
            },
            error: () => { this.isSaving = false; }
        });
    }

    guardarEdicion() {
        this.movieForm.markAllAsTouched();
        if (this.movieForm.invalid || !this.peliculaSeleccionadaId) return;

        this.movieService.actualizar(this.peliculaSeleccionadaId, this.movieForm.value).subscribe(() => {
            this.mostrarEditarModal = false;
            this.peliculas$ = this.movieService.obtenerPeliculas();
        });
    }

    borrarPelicula(id: string) {
        if (confirm('¿Seguro que querés borrar esta película?')) {
            this.movieService.borrar(id).subscribe(() => { });
        }
    }
}