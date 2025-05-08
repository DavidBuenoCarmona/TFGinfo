import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { WorkingGroupBase } from '../../models/group.model';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { GroupService } from '../../services/group-service';
import { RoleId } from '../../../admin/models/role.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'group-list',
  standalone: true,
  imports: [
    TranslateModule,
    MatTableModule,
    MatDialogModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    CommonModule
  ],
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.scss']
})
export class GroupListComponent implements OnInit {
  @Input() groups: WorkingGroupBase[] = [];
  @Input() displayedColumns: string[] = ['name', 'description', 'isPrivate', 'actions'];
  @Output() onDeleteGroup = new EventEmitter<number>();
  public canEdit: boolean = false;

  constructor(
    private dialog: MatDialog,
    private groupService: GroupService,
    private router: Router,
    private route: ActivatedRoute
  ) {
  }


  ngOnInit(): void {
    let role = Number.parseInt(localStorage.getItem('role')!);
    this.canEdit = role === RoleId.Admin;
  }

  onEdit(group: WorkingGroupBase) {
    this.router.navigate(["/working-group/" + group.id]);
  }

  onDelete(group: WorkingGroupBase) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.onDeleteGroup.emit(group.id);
      }
    });
  }
}