import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { RegisterComponent } from './register';
import { ClienteComponent } from './cliente';
import { AuthGuard } from './_guards';
import { ProdutosComponent } from './produtos';

const appRoutes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'saveCliente', component: ClienteComponent },   
    { path: 'produtos', component: ProdutosComponent }, 
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);