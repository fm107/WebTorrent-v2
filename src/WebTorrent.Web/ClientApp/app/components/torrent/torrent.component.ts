import { Component, OnInit, ViewChild, ViewContainerRef, ComponentFactoryResolver } from "@angular/core";

import { Subject } from "rxjs/Subject";

import { IMessage } from "../../services/message";
import { WebSocketService } from "../../services/websocket.service";
import { ContentService } from "../../services/content.service";
import { InvokePlayerComponent } from "../invoke-player/invoke-player.component";
import { ClickedItem } from "../../models/clicked-item"

@Component({
    selector: "torrent",
    templateUrl: "./torrent.component.html",
    styleUrls: ["./torrent.component.css"]
})
export class TorrentComponent implements OnInit {
    metaData = this.content.metaData;
    parentFolder = this.content.parentFolder;
    currentFolder = this.content.currentFolder;

    private messages: IMessage[] = new Array();

    messagesObs: Subject<IMessage>;

    @ViewChild("videojs", { read: ViewContainerRef })
    parent: ViewContainerRef;
    invokePlayerComponent: any;

    constructor(private content: ContentService,
        private wsService: WebSocketService,
        private componentFactoryResolver: ComponentFactoryResolver) {

        this.invokePlayerComponent = this.componentFactoryResolver.resolveComponentFactory(InvokePlayerComponent);
    }

    ngOnInit() {
        this.content.getContent(null, false, null);

        //this.messagesObs = (this.wsService
        //    .connect(`ws://${window.location.hostname}:${window.location.port}/api/Torrent/Notifications`)
        //    .map((response: MessageEvent): IMessage => {
        //        const msg = JSON.parse(response.data);
        //        return {
        //            message: msg.message
        //        };
        //    }) as Subject<IMessage>);


        //this.messagesObs.subscribe(response => {
        //    console.log(response);
        //    this.messages.push(response);
        //});
    }

    onClick(item: ClickedItem) {
        //this.messagesObs.next({ message: "Test message" });
        if (item.type == "file") {
            const cmp = this.parent.createComponent(this.invokePlayerComponent);
            const inv = (cmp.instance) as InvokePlayerComponent;
            inv.idx = String(item.id);

            //todo untill play list not created for media file isStreaming also false. Review this.
            if (item.isStreaming) {
                inv.url = item.stream;
                inv.showVideo = true;
                inv.initVideo(cmp, this.parent, String(item.id));
            } else {
                inv.url = item.downloadPath;
                inv.initContent(cmp, this.parent);
            }
        } else {
            this.content.getContent(item.folder, item.showFiles, item.hash);
        }
    }
}