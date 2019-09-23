import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { User } from '../_models';
import { Produto } from '../_models';
import { UserService } from '../_services';
import { ClientesService } from '../_services';

@Component({templateUrl: 'produtos.component.html'})
export class ProdutosComponent implements OnInit {
    currentUser: User;
    users: User[] = [];
    produtos: Produto[] = [];
    nome: string;
    idCliente: string;   

    constructor(private userService: UserService, private clientesService: ClientesService, private route: ActivatedRoute) {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    }

    ngOnInit() {
        this.nome = this.route.snapshot.queryParams["nome"];
        this.idCliente = this.route.snapshot.queryParams["id"];
        this.loadAllProdutos();
    }

    addFavorito(produto: Produto) {         
        produto.idCliente = this.idCliente;         
        produto.tipo = "1";       
        this.clientesService.addFavorito(produto).pipe(first()).subscribe(() => { 
            this.loadAllProdutos() 
        });
    }

    removeFavorito(produto: Produto) {         
        produto.idCliente = this.idCliente;  
        produto.tipo = "0";         
        this.clientesService.removeFavorito(produto).pipe(first()).subscribe(() => { 
            this.loadAllProdutos() 
        });
    }

    private loadAllProdutos() {
        this.clientesService.getAllProdutos(1, this.idCliente).pipe(first()).subscribe(produtos => { 
            this.produtos = produtos; 
        });
    }
}