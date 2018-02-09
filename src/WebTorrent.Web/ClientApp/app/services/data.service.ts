import { Injectable } from "@angular/core";
import { Http, URLSearchParams, Response } from "@angular/http";
import { Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { Subject } from "rxjs/Subject";
import { ReplaySubject } from "rxjs/ReplaySubject";
import "rxjs/add/operator/map";
import { Subscription } from "rxjs/Subscription";

import { TdLoadingService, LoadingType, LoadingMode, IUploadOptions, TdFileService } from "@covalent/core";
import { SimpleTimer } from "ng2-simple-timer";
import { IContent } from "../models/content";
import { ITorrentInfo } from "../models/torrentinfo";

@Injectable()
export class DataService {
    folderContent: Subject<any>;

    constructor(private http: Http,
        private loadingService: TdLoadingService,
        private fileUploadService: TdFileService,
        public st: SimpleTimer) {

        this.loadingService.create({
            name: "query",
            type: LoadingType.Circular,
            mode: LoadingMode.Indeterminate,
            color: "accent"
        });

        this.folderContent = new Subject<any>();
    }

    getFolderContent(folder: string, needFiles: boolean, hash: string) {
        const params = new URLSearchParams();
        params.set("folder", folder);
        params.set("needFiles", needFiles.toString());
        params.set("hash", hash);

        this.loadingService.register("query");
        this.http.get(`api/Content/GetFolder`, { search: params }).subscribe(
            result => {
                this.folderContent.next(result.text());
            },
            error => {
                this.loadingService.resolve("query");
                this.handleError(error);
            },
            () => {
                this.loadingService.resolve("query");
            });

        return this.folderContent.asObservable();
    }

    getTorrentInfo(hash: string) {
        const params = new URLSearchParams();
        params.set("hash", hash);

        const options = new RequestOptions({ search: params });

        this.loadingService.register("query");
        return this.http.get(`api/Torrent/GetTorrentInfo`, options)
            .map((response: Response) => {
                return response;
            })
            .catch(this.handleError)
            .finally(() => {
                this.loadingService.resolve("query");
            });
    }

    getTorrentStatus(hash: string, url: string) {
        return this.doGetRequest(hash, url);
    }

    deleteTorrent(hash: string, url: string) {
        return this.doGetRequest(hash, url);
    }

    private doGetRequest(hash: string, url: string): Observable<Response> {
        const params = new URLSearchParams();
        params.set("hash", hash);

        const options = new RequestOptions({ search: params });

        return this.http.get(url, options)
            .map((response: Response) => {
                return response;
            })
            .catch(this.handleError);
    }

    submitTorrentUrl(url: string, folder: string): Observable<Response> {
        const body = new FormData();
        body.append("url", url);
        body.append("folder", folder);

        this.loadingService.register("query");
        return this.http.post(`api/Torrent/UploadFromUrl`, body)
            .map((response: Response) => {
                return response;
            })
            .catch(this.handleError)
            .finally(() => {
                this.loadingService.resolve("query");
            });
    }

    submitTorrentFile(file: File, folder: string): Observable<Response> {
        const options: IUploadOptions = {
            url: `api/Torrent/UploadFile?folder=${folder}`,
            method: "post",
            file: file
        };

        return Observable.create(observer => {
            this.loadingService.register("query");
            this.fileUploadService.upload(options).subscribe(
                result => {
                    observer.next(result);
                    observer.complete();
                },
                error => {
                    this.loadingService.resolve("query");
                    this.handleError(error);
                },
                () => {
                    this.loadingService.resolve("query");
                });

        });
    }

    getStatusByTime(hash: string): Observable<IContent> {
        return Observable.create(observer => {
            if (this.st.newTimer(hash, 5)) {
                const timerId = this.st.subscribe(hash,
                    (e => this.getTorrentStatus(hash, `api/Torrent/GetTorrentStatus`)
                        .subscribe((result: Response) => {
                            const content = result.json() as IContent;
                            observer.next(content);

                            if (!content.isInProgress) {
                                observer.complete();
                                this.st.unsubscribe(timerId);
                                this.st.delTimer(hash);
                            }
                        })) as any);
            }
        });
    }

    getDetailsByTime(hash: string): Observable<ITorrentInfo> {
        return Observable.create(observer => {
            if (this.st.newTimer(hash, 5)) {
                const timerId = this.st.subscribe(hash,
                    (e => this.getTorrentStatus(hash, `api/Torrent/GetTorrentDetails`)
                        .subscribe((result: Response) => {
                            const content = result.json() as ITorrentInfo;
                            observer.next(content);

                            if (content.remaining === 0) {
                                observer.complete();
                                this.st.unsubscribe(timerId);
                                this.st.delTimer(hash);
                            }
                        })) as any);
            }
        });
    }

    handleError(error) {
        console.error(`Error during retriving data: ${error}`);
        return Observable.throw(error.json().error || "Server error");
    }
}