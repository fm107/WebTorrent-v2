import { Component } from "@angular/core";
import { Response } from "@angular/http";

import { AlertService, MessageSeverity, DialogType } from '../../services/alert.service';

import { DataService } from "../../services/data.service";
import { ContentService } from "../../services/content.service";

@Component({
    selector: "upload-button-url",
    templateUrl: "./upload-button-url.component.html",
    styleUrls: ["./upload-button-url.component.css"]
})
export class UploadButtonUrlComponent {
    constructor(private data: DataService,
        private content: ContentService,
        private alertService: AlertService) {
    }

    onClick(url: any) {
        this.data.submitTorrentUrl(url.value, this.content.currentFolder.getValue()).subscribe(
            (res: Response) => {
                if (res.status === 200) {
                    this.alertService.showMessage("File Uploaded", `${res.text() } uploaded successfully`, MessageSeverity.success);

                    this.content.getContent(null, false, null);
                }
            },
            error => {
                console.error(`Error while file uploading: ${error}`);
                this.alertService.showMessage("File Error", "No torrents detected in given URL", MessageSeverity.error);
            });

        url.value = "";
    }
}