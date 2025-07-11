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
import { RoleId } from '../../../admin/models/role.model';
import { MatIconModule } from '@angular/material/icon';
import { Filter } from '../../../../core/core.model';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { ConfigurationService } from '../../../../core/services/configuration.service';

@Component({
    selector: 'app-group-search',
    imports: [GroupListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatIconModule,
        MatCheckboxModule,
        MatOptionModule,
        MatSelectModule,
        CommonModule],
    templateUrl: './group-search.component.html',
    styleUrl: './group-search.component.scss'
})

export class GroupSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public groups: WorkingGroupBase[] = [];
    public filteredGroups: WorkingGroupBase[] = [];
    public canEdit: boolean = false;
    public showExtraFilters: boolean = false;
    public isAdmin: boolean = false;
    public isProfessor: boolean = false;
    public filters: Filter[] = [];

    constructor(
        public groupService: GroupService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private configurationService: ConfigurationService
    ) { }

    ngOnInit(): void {
        let role = this.configurationService.getRole();
        this.canEdit = role === RoleId.Admin || role === RoleId.Professor;
        this.isAdmin = role === RoleId.Admin;
        this.isProfessor = role === RoleId.Professor;
        this.filterForm = this.fb.group({
            generic: [''],
            name: [''],
            description: [''],
            isPrivate: [-1],
        });
        if (this.isAdmin) {
            this.filters.push({ key: 'university', value: localStorage.getItem('selectedUniversity') || '0' });
        } else if (this.isProfessor) {
            this.filters.push({ key: 'department', value: this.configurationService.getUser()!.department!.toString() || '0' });
        } else {
            this.filters.push({ key: 'universities', value: this.configurationService.getSelectedUniversities()?.map(id => id.toString()).join(',') || '0' });
        }

        this.groupService.searchGroups(this.filters).subscribe(groups => {
            this.groups = groups;
            this.filteredGroups = [...this.groups];
        });

    }

    onCreate(): void {
        this.router.navigate(['new'], { relativeTo: this.route });
    }

    onSearch(): void {
        const formValues = this.filterForm.value;
        Object.keys(formValues).forEach(key => {
            this.filters = this.filters.filter(filter => filter.key !== key);
        });

        Object.keys(formValues).forEach(key => {
            const value = formValues[key].toString();
            if (value !== null && value !== undefined && value !== '') {
                this.filters.push({ key, value });
            }
        });

        this.groupService.searchGroups(this.filters).subscribe(groups => {
            this.filteredGroups = groups;
        });
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredGroups = [...this.groups]; // Restaurar la lista completa

        const formValues = this.filterForm.value;
        Object.keys(formValues).forEach(key => {
            this.filters = this.filters.filter(filter => filter.key !== key);
        });

        this.groupService.searchGroups(this.filters).subscribe(groups => {
            this.filteredGroups = groups;
        });
    }

    deleteGroup(groupId: number): void {
        this.groupService.deleteGroup(groupId).subscribe(() => {
            this.groups = this.groups.filter(group => group.id !== groupId);
            this.filteredGroups = [...this.groups]; // Actualizar la lista filtrada
        });
    }

    onShowExtraFilters(): void {
        this.showExtraFilters = !this.showExtraFilters;
        if (!this.showExtraFilters) {
            this.filterForm.get('isPrivate')?.setValue(-1); // Reset isPrivate filter when hiding extra filters
            this.filterForm.get('description')?.setValue(''); // Reset description filter when hiding extra filters
            this.filterForm.get('name')?.setValue(''); // Reset name filter when hiding extra filters
        }
    }
}