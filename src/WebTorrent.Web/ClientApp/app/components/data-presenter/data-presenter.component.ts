import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy, ViewChild, ChangeDetectorRef } from
    "@angular/core";
import { Response } from "@angular/http";

import {TdDataTableService, TdDataTableSortingOrder, ITdDataTableSortChangeEvent, ITdDataTableColumn,
    TdSearchBoxComponent, TdDialogService} from "@covalent/core";

import { IContent } from "../../models/content";
import { DataService } from "../../services/data.service";
import { ContentService } from "../../services/content.service";
import { ClickedItem } from "../../models/clicked-Item";
import { TorrentProgressService } from "../../services/torrent-progress.service";

@Component({
    selector: "data-presenter",
    templateUrl: "./data-presenter.component.html",
    styleUrls: ["./data-presenter.component.css"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DataPresenterComponent {
    hideMenuItems = true;
    showFolder = true;
    filteredData: ClickedItem[];
    tmpArray: ClickedItem[];
    searchTerm: string;
    sortBy: string;
    hasData = true;
    sortOrder: TdDataTableSortingOrder;
    @Input()
    parentFolder: string;
    @Input()
    currentFolder: string;
    @Input()
    data: IContent[];
    @Output()
    onItemClick = new EventEmitter<ClickedItem>(true);

    @ViewChild("searchBox")
    searchBox: TdSearchBoxComponent;

    constructor(private dataTableService: TdDataTableService,
        private cd: ChangeDetectorRef,
        private dialogService: TdDialogService,
        private dataService: DataService,
        private content: ContentService,
        private torrentProgress: TorrentProgressService) {
    }

    private name: ITdDataTableColumn = {
        name: "name",
        label: "NAME #",
        tooltip: "Folder or file name",
        sortable: true
    }
    private size: ITdDataTableColumn = {
        name: "size",
        label: "SIZE",
        tooltip: "Folder or file size",
        sortable: true,
        numeric: true,
        format: v => v.toFixed(2)
    }
    private changed: ITdDataTableColumn = {
        name: "lastChanged",
        label: "LAST CHANGED",
        tooltip: "Folder or file last changed date",
        sortable: true,
        numeric: true
    }

    ngOnChanges(): void {
        this.initData();
        this.updateDataTable(null);
        this.searchBox.value = "";
    }

    private sort(sortEvent: ITdDataTableSortChangeEvent): void {
        this.sortBy = sortEvent.name;
        this.sortOrder = sortEvent.order === TdDataTableSortingOrder.Ascending
            ? TdDataTableSortingOrder.Descending
            : TdDataTableSortingOrder.Ascending;
    }

    private search(searchTerm: string): void {
        this.searchTerm = searchTerm;
        this.updateDataTable("filter");
    }

    private initData() {
        if (this.data) {
            this.tmpArray = new Array();
            for (let data of this.data) {
                for (let fs of data.fsItems) {
                    const newElem = new ClickedItem();
                    newElem.folder = data.parentFolder;
                    newElem.hash = data.hash;
                    newElem.isInProgress = data.isInProgress;
                    newElem.id = fs.id;
                    newElem.downloadPath = fs.downloadPath;
                    newElem.isStreaming = fs.isStreaming;
                    newElem.name = fs.name;
                    newElem.lastChanged = fs.lastChanged;
                    newElem.size = fs.size;
                    newElem.stream = fs.stream;
                    newElem.type = fs.type;
                    this.tmpArray.push(newElem);
                }
            }
        }
    }

    private updateDataTable(action: string): void {
        const newData = this.tmpArray;

        switch (action) {
        case "filter":
            if (this.searchTerm) {
                this.filteredData = this.dataTableService.filterData(newData, this.searchTerm, true);
                if (this.filteredData && this.filteredData.length > 0) {
                    this.hasData = true;
                } else {
                    this.hasData = false;
                }
            } else {
                this.filteredData = newData;
            }
            break;
        default:
            this.filteredData = newData;
            break;
        }
    }

    private onUp(item) {
        setTimeout(() => {
                this.showFolder = true;
                this.cd.markForCheck();
            },
            100);

        const itemObj = new ClickedItem();
        itemObj.folder = item;
        itemObj.showFiles = false;
        this.onItemClick.emit(itemObj);
    }

    private onClick(item: ClickedItem) {
        setTimeout(() => {
                this.showFolder = false;
                this.cd.markForCheck();
            },
            100);

        if (item.isInProgress) {
            this.openAlert(item.hash, () => this.onUp(item.folder));
        }

        this.parentFolder = item.folder;
        item.showFiles = true;
        this.onItemClick.emit(item);
    }

    private openAlert(hash: string, callback: Function): void {
        this.dataService.getTorrentInfo(hash).subscribe((response: Response) => {
            this.dialogService.openAlert(({
                message: response.text(),
                disableClose: true,
                title: "Torrent is in progress"
            })).afterClosed().subscribe(() => callback());
        });
    }

    private onDelete(hash: string): void {
        this.dialogService.openConfirm({
            message: "Please confirm you want to delete this torrent.",
            disableClose: true,
            title: "Confirm Removal",
            cancelButton: "No",
            acceptButton: "Yes"
        }).afterClosed().subscribe((accept: boolean) => {
            if (accept) {
                this.dataService.deleteTorrent(hash, `api/Torrent/DeleteTorrent`).subscribe(response => {
                    console.log(response);
                    this.torrentProgress.unSibscribe(hash);
                    this.content.getContent(null, false, null);
                });
            }
        });
    }
}