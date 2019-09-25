import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AlertService, ClientesService } from '../_services';

@Component({templateUrl: 'cliente.component.html'})
export class ClienteComponent implements OnInit {
    registerForm: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private clienteService: ClientesService,
        private alertService: AlertService,
        private route: ActivatedRoute) { }

    ngOnInit() {

        let id = this.route.snapshot.queryParams["id"];
        let nome = this.route.snapshot.queryParams["nome"];
        let email = this.route.snapshot.queryParams["email"];  
        if(id != null)
        {
            this.registerForm = this.formBuilder.group({  
                id: id,          
                email: [email, Validators.required],
                nome: [nome, Validators.required]
            });

            //this.registerForm.controls["email"].disable();
        }
        else
        {            
            this.registerForm = this.formBuilder.group({    
                id: null,                       
                email: ['', Validators.required],
                nome: ['', Validators.required]                
            });

            //this.registerForm.controls["email"].enable();
        }
    }

    // convenience getter for easy access to form fields
    get f() { return this.registerForm.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.registerForm.invalid) {
            return;
        }

        this.loading = true;
        if(this.registerForm.get('id').value != null){            
            this.clienteService.update(this.registerForm.value)
            .pipe(first())
            .subscribe(
                data => {
                    this.alertService.success('Cliente atualizado com sucesso', true);
                    this.router.navigate(['/']);
                },
                error => {                    
                    //this.alertService.error("Email já utilizado");
                    this.loading = false;
                    this.router.navigate(['/']);
                });
        }
        else{
            this.clienteService.register(this.registerForm.value)
            .pipe(first())
            .subscribe(
                data => {
                    this.alertService.success('Cliente cadastrado com sucesso', true);
                    this.router.navigate(['/']);
                },
                error => {                    
                    this.alertService.error("Email já utilizado");
                    this.loading = false;
                });
        }       
    }
}
