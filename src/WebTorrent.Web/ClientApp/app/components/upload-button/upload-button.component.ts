import { Component } from "@angular/core";

import { TdLoadingService, TdFileUploadComponent } from "@covalent/core";

import { AlertService, MessageSeverity, DialogType } from '../../services/alert.service';

import { ContentService } from "../../services/content.service";
import { DataService } from "../../services/data.service";

@Component({
    selector: "upload-button",
    templateUrl: "./upload-button.component.html",
    styleUrls: ["./upload-button.component.css"]
})
export class UploadButtonComponent {
    constructor(private alertService: AlertService,
        private content: ContentService,
        private data: DataService,
        private loadingService: TdLoadingService) {
    }

    private selectEvent(file: File, uploadComponent: TdFileUploadComponent): void {
        if (file.type != "application/x-bittorrent") {
            uploadComponent.cancel();
            this.alertService.showMessage("File Error", "No torrents detected in given file", MessageSeverity.error);

        } else {
            if (file.size > 1048576) {
                uploadComponent.cancel();

                this.alertService.showMessage("File Size Exceeded", `${file.name} exceeds the limit`, MessageSeverity.error);
            }
        }
    };

    private uploadEvent(file: File, uploadComponent: TdFileUploadComponent): void {
        this.data.submitTorrentFile(file, this.content.currentFolder.getValue()).subscribe(response => {
            this.alertService.showMessage("File Uploaded", `${response} uploaded successfully`, MessageSeverity.success);
            
            this.content.getContent(null, false, null);
        });

        uploadComponent.cancel();
    };
}