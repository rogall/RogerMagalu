import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Cliente, Produto } from '../_models';

@Injectable()
export class ClientesService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<Cliente[]>(`${config.apiUrl}/Clientes`);
    }

    getById(id: string) {
        return this.http.get(`${config.apiUrl}/Clientes/GetCliente/` + id);
    }

    register(cliente: Cliente) {
        return this.http.post(`${config.apiUrl}/Clientes`, cliente);
    }

    update(cliente: Cliente) {
        return this.http.put(`${config.apiUrl}/Clientes/` + cliente.id, cliente);
    }

    delete(id: string) {
        return this.http.delete(`${config.apiUrl}/Clientes/` + id);
    }   

    addFavorito(produto: Produto){           
        return this.http.post(`${config.apiUrl}/Clientes/AddOrRemoveProduto`, produto);
    }

    removeFavorito(produto: Produto){               
        return this.http.post(`${config.apiUrl}/Clientes/AddOrRemoveProduto`, produto);
    }

    getAllProdutos(paginacao: number){
        return this.http.get<Produto[]>(`${config.apiUrl}/Produtos`);
    }    
}