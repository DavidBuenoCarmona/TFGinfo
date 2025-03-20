import { NgModule } from "@angular/core";
import { HeaderComponent } from "./components/header/header.component";
import { MainLayoutComponent } from "./components/main-layout/main-layout.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { LogoComponent } from "./components/logo/logo.component";

@NgModule({
    declarations: [

    ],
    imports: [
        CommonModule,
        RouterModule
    ],
    providers: [],
    exports: []
})

export class LayoutModule { }