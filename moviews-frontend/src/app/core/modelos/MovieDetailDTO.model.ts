import { ReviewDTO } from './ReviewDTO.model';

export interface MovieDetailDTO {
    id: string;
    title: string;
    genre: string;
    synopsis: string;
    poster: string;
    releaseYear: number;
    reviews: ReviewDTO[];
}