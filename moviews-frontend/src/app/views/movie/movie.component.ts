import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MovieService } from '../../core/servicios/movieService/movie.service';
import { ChangeDetectorRef } from '@angular/core';
import { AuthService } from '../../core/servicios/authService/auth.service';

@Component({
    selector: 'app-movie',
    standalone: true,
    imports: [CommonModule, HeaderComponent, ReactiveFormsModule],
    templateUrl: './movie.component.html',
    styleUrls: ['./movie.component.css']
})
export class MovieComponent implements OnInit {

    isAdmin = false;
    movie: any;
    reviewForm!: FormGroup;
    loading = true;
    errorMessage = '';
    idusuario: string | undefined | null;

    constructor(
        private route: ActivatedRoute,
        private movieService: MovieService,
        private fb: FormBuilder,
        private cd: ChangeDetectorRef,
        private authService: AuthService,
    ) { }

    ngOnInit(): void {
        this.isAdmin = this.authService.esAdmin();
        if (this.authService.obtenerUserId != null) {
            this.idusuario = this.authService.obtenerUserId();
        }
        const id = this.route.snapshot.paramMap.get('id');

        this.reviewForm = this.fb.group({
            rating: [5, Validators.required],
            comment: ['', Validators.required]
        });

        this.loadMovie(id!);
    }

    loadMovie(id: string) {
        this.loading = true;

        this.movieService.obtenerPorId(id).subscribe({
            next: (res) => {
                this.movie = res;
                this.loading = false;
                this.cd.detectChanges();
            },
            error: () => {
                this.loading = false;
                this.cd.detectChanges();
            }
        });
    }

    submitReview() {
        if (this.reviewForm.invalid) return;

        this.movieService.agregarReview(this.movie.id, this.reviewForm.value)
            .subscribe({
                next: () => {
                    this.reviewForm.reset({ rating: 5, comment: '' });
                    this.loadMovie(this.movie.id);
                },
                error: (err) => {
                    this.errorMessage = err.error;
                }
            });
    }
}
