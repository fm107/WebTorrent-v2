import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs/BehaviorSubject";

import { IContent } from "../models/content";
import { DataService } from "./data.service";

@Injectable()
export class ContentService {
    metaData: BehaviorSubject<IContent[]>;
    parentFolder: BehaviorSubject<string>;
    currentFolder: BehaviorSubject<string>;

    constructor(private data: DataService) {
        this.metaData = new BehaviorSubject<IContent[]>(null);
        this.parentFolder = new BehaviorSubject<string>(null);
        this.currentFolder = new BehaviorSubject<string>(null);
    }

    getContent(folder: string, needFiles: boolean, hash: string) {
        this.data.getFolderContent(folder, needFiles, hash).subscribe(result => {
            const res = JSON.parse((result)) as IContent[];
            this.metaData.next(res);
            for (let r of res) {
                this.parentFolder.next(r.parentFolder);
                this.currentFolder.next(r.currentFolder);
            }
        });
    }
}