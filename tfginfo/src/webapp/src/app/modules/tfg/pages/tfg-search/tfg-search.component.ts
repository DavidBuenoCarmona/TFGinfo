import { Component, OnInit } from '@angular/core';
import { TfgListComponent } from '../../components/tfg-list/tfg-list.component';
import { TfgService } from '../../services/tfg.service';
import { TFGLineDTO } from '../../models/tfg.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-tfg-search',
    imports: [TfgListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './tfg-search.component.html',
    styleUrl: './tfg-search.component.scss'
})

export class TfgSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public tfgs: TFGLineDTO[] = [];
    public filteredTfgs: TFGLineDTO[] = [];

    constructor(
        public tfgService: TfgService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            filter: [''] 
        });

        this.tfgService.getTfgs().subscribe(tfgs => {
            this.tfgs = tfgs;
            this.filteredTfgs = [...this.tfgs];
        });
    }
    
    onCreate(): void {
        this.router.navigate(['new'], { relativeTo: this.route });
    }

    onSearch(): void {
        const filterValue = this.filterForm.get('filter')?.value.toLowerCase();
        this.filteredTfgs = this.tfgs.filter(tfg =>
            tfg.name.toLowerCase().includes(filterValue)
        );
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredTfgs = [...this.tfgs]; // Restaurar la lista completa
    }

    deleteTfg(tfgId: number): void {
        this.tfgService.deleteTfg(tfgId).subscribe({
            next: () => {},
            error: (err) => console.error(err),
            complete: () => {
              this.tfgs = this.tfgs.filter((item) => item.id !== tfgId);
              this.filteredTfgs = this.filteredTfgs.filter((item) => item.id !== tfgId);
            }
          });
        }

}
