import { Component, OnInit } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { WorkingGroupBase } from '../../models/group.model';
import { GroupService } from '../../services/group-service';
import { GroupListComponent } from '../../components/groups-list/groups-list.component';

@Component({
    selector: 'app-group-search',
    imports: [GroupListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './group-search.component.html',
    styleUrl: './group-search.component.scss'
})

export class GroupSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public groups: WorkingGroupBase[] = [];
    public filteredGroups: WorkingGroupBase[] = [];

    constructor(
        public groupService: GroupService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            filter: [''] 
        });

        this.groupService.getGroups().subscribe(groups => {
            this.groups = groups;
            this.filteredGroups = [...this.groups];
        });

    }
    
    onCreate(): void {
        this.router.navigate(['new'], { relativeTo: this.route });
    }

    onSearch(): void {
        const filterValue = this.filterForm.get('filter')?.value.toLowerCase();
        this.filteredGroups = this.groups.filter(group =>
            group.name.toLowerCase().includes(filterValue)
        );
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredGroups = [...this.groups]; // Restaurar la lista completa
    }

    deleteGroup(groupId: number): void {
        this.groupService.deleteGroup(groupId).subscribe(() => {
            this.groups = this.groups.filter(group => group.id !== groupId);
            this.filteredGroups = [...this.groups]; // Actualizar la lista filtrada
        });
    }

}