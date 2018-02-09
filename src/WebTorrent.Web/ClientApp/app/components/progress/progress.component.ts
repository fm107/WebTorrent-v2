import { Component, Input, OnChanges } from "@angular/core";
import { Response } from "@angular/http";
import { Subscription } from "rxjs/Subscription";
import { Subject } from "rxjs/Subject";
import { Observable } from "rxjs/Rx";
import { BehaviorSubject } from "rxjs/BehaviorSubject";

import { DataService } from "../../services/data.service";
import { TorrentProgressService } from "../../services/torrent-progress.service";
import { ITorrentInfo } from "../../models/torrentInfo";
import { ClickedItem } from "../../models/clicked-Item";

@Component({
    selector: "torrentProgress",
    templateUrl: "./progress.component.html",
    styleUrls: ["./progress.component.css"]
})
export class ProgressComponent implements OnChanges {
    private isInProgress = new BehaviorSubject<boolean>(null);
    private torrentDetails = new Subject<any>();
    private progressSub: Subscription;
    private torrentDetailsSub: Subscription;
    private progress: number;

    @Input("Item")
    item: ClickedItem;

    constructor(private progressService: TorrentProgressService,
        private dataService: DataService) {
    }

    ngOnChanges() {
        if (!this.progressSub) {
            this.progressSub = this.progressService.getUpdates(this.item.hash)
                .subscribe(isInProgress => {
                    this.isInProgress.next(isInProgress);

                    if (!this.torrentDetailsSub) {
                        this.torrentDetailsSub = Observable.timer(0, 5000).subscribe(() => {
                            this.dataService.getTorrentStatus(this.item.hash, `api/Torrent/GetTorrentDetails`)
                                .subscribe(
                                    (res: Response) => {
                                        const response = JSON.parse(res.text()) as ITorrentInfo;
                                        this.progress = response.progress;
                                        let details = "";
                                        for (let key of Object.keys(response)) {
                                            details += `${key}: ${response[key]}
                                `;
                                        }
                                        this.torrentDetails.next(details);

                                        if (this.isInProgress.getValue() === false) {
                                            this.torrentDetailsSub.unsubscribe();
                                        }
                                    });
                        });
                    }
                });
        }
    }

    ngOnDestroy() {
        if (this.progressSub) {
            this.progressSub.unsubscribe();
        }
        if (this.torrentDetailsSub) {
            this.torrentDetailsSub.unsubscribe();
        }
    }
}