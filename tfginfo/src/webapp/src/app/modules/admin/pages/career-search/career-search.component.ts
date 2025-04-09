import { Component, OnInit } from '@angular/core';
import { CareerListComponent } from '../../components/career-list/career-list.component';
import { CareerService } from '../../services/career.service';
import { CareerDTO } from '../../models/career.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-career-search',
    imports: [CareerListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './career-search.component.html',
    styleUrl: './career-search.component.scss'
})

export class CareerSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public careers: CareerDTO[] = [];
    public filteredCareers: CareerDTO[] = [];

    constructor(
        public careerService: CareerService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            filter: [''] 
        });

        this.careerService.getCareers().subscribe(careers => {
            this.careers = careers;
            this.filteredCareers = [...this.careers];
        });
    }
    
    onCreate(): void {
        this.router.navigate(['new'], { relativeTo: this.route });
    }

    onSearch(): void {
        const filterValue = this.filterForm.get('filter')?.value.toLowerCase();
        this.filteredCareers = this.careers.filter(career =>
            career.name.toLowerCase().includes(filterValue)
        );
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredCareers = [...this.careers]; // Restaurar la lista completa
    }

    deleteCareer(careerId: number): void {
        this.careerService.deleteCareer(careerId).subscribe({
            next: () => {},
            error: (err) => console.error(err),
            complete: () => {
                this.careers = this.careers.filter((item) => item.id !== careerId);
                this.filteredCareers = this.filteredCareers.filter((item) => item.id !== careerId);
            }
        });
    }
}