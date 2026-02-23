import { UserDTO } from './UserDTO.model';

export interface ReviewDTO {
    id: string;
    rating: number;
    comment: string;
    user: UserDTO;
}