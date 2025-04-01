import { Component, OnInit } from '@angular/core';
import { TfgListComponent } from '../../components/tfg-list/tfg-list.component';
import { TfgService } from '../../services/tfg.service';
import { TFGLineDTO } from '../../models/tfg.model';

@Component({
    selector: 'app-tfg-search',
    imports: [TfgListComponent],
    templateUrl: './tfg-search.component.html',
    styleUrl: './tfg-search.component.scss'
})

export class TfgSearchComponent implements OnInit {
    public filteredTfgs: TFGLineDTO[] = [];

    constructor(public tfgService: TfgService) { }

    ngOnInit(): void {
        this.tfgService.getTfgs().subscribe(tfgs => this.filteredTfgs = tfgs);
    }

}
