import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { User } from '../_models';

@Injectable()
export class UserService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<User[]>(`${config.apiUrl}/Admin`);
    }

    getById(id: string) {
        return this.http.get(`${config.apiUrl}/Admin/` + id);
    }

    register(user: User) {
        return this.http.post(`${config.apiUrl}/Admin/register`, user);
    }

    update(user: User) {
        return this.http.put(`${config.apiUrl}/Admin/` + user.id, user);
    }

    delete(id: string) {
        return this.http.delete(`${config.apiUrl}/Admin/` + id);
    }
}