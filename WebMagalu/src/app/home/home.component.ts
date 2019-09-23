import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../_models';
import { Cliente } from '../_models';
import { UserService } from '../_services';
import { ClientesService } from '../_services';

@Component({templateUrl: 'home.component.html'})
export class HomeComponent implements OnInit {
    currentUser: User;
    users: User[] = [];
    clientes: Cliente[] = [];

    constructor(private userService: UserService, private clientesService: ClientesService) {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    }

    ngOnInit() {
        this.loadAllClientes();
    }

    deleteCliente(id: string) {
        this.clientesService.delete(id).pipe(first()).subscribe(() => { 
            this.loadAllClientes() 
        });
    }

    private loadAllClientes() {
        this.clientesService.getAll().pipe(first()).subscribe(clientes => { 
            this.clientes = clientes; 
        });
    }
}