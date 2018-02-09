import { Component } from "@angular/core";
import { Router } from "@angular/router";

import {ContentService} from "../../services/content.service";

@Component({
    selector: "nav-menu",
    templateUrl: "./navmenu.component.html",
    styleUrls: ["./navmenu.component.css"]
})
export class NavMenuComponent {
    constructor(private content: ContentService) {}

    onNavClick() {
        this.content.getContent(null, false, null);
    }
}