import { Injectable, ChangeDetectorRef } from "@angular/core";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { SimpleTimer } from "ng2-simple-timer";
import { Http, URLSearchParams, Response } from "@angular/http";
import { Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { Subject } from "rxjs/Subject";
import { AsyncSubject } from "rxjs/AsyncSubject";
import "rxjs/add/operator/map";
import { Subscription } from "rxjs/Subscription";
import { IContent } from "../models/content";
import { DataService } from "./data.service";

@Injectable()
export class TorrentProgressService {
    private subscriptions: Map<string, Subscription>;
    private torrents: Map<string, Subject<boolean>>;

    constructor(private dataService: DataService) {
        this.subscriptions = new Map<string, Subscription>();
        this.torrents = new Map<string, Subject<boolean>>();
    }

    getUpdates(hash: string): Subject<boolean> {
        if (!this.torrents.has(hash)) {
            const sub = new Subject<boolean>();
            const subscription = this.dataService.getStatusByTime(hash).subscribe((content) => {
                if (content) {
                    if (!this.subscriptions.has(hash)) {
                        this.subscriptions.set(hash, subscription);
                    }
                    sub.next(content.isInProgress);
                    if (!content.isInProgress) {
                        this.subscriptions.get(hash).unsubscribe();
                        sub.complete();
                    }
                }
            });
            this.torrents.set(hash, sub);
        }
        return this.torrents.get(hash);
    }

    unSibscribe(hash: string) {
        if (this.subscriptions.has(hash)) {
            this.subscriptions.get(hash).unsubscribe();
            this.subscriptions.delete(hash);
            this.dataService.st.delTimer(hash);
            console.log("subscriptions - " + this.subscriptions.size);
        }

        if (this.torrents.has(hash)) {
            this.torrents.get(hash).unsubscribe();
            this.torrents.delete(hash);
            console.log("torrents - " + this.torrents.size);
        }
    }
}