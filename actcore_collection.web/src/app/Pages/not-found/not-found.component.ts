import { Component } from '@angular/core';
import { PageRoutingModule } from "../page-routing.module";
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-not-found',
    imports: [PageRoutingModule , RouterLink],
    templateUrl: './not-found.component.html',
    styleUrl: './not-found.component.scss'
})
export class NotFoundComponent {

}
