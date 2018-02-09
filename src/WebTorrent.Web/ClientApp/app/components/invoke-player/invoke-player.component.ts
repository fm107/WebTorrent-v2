import { Component, ViewContainerRef, ComponentRef, ViewChild } from "@angular/core";

import {ComponentInjectorService} from "../../services/component-injector.service";
//import { FlowplayerComponent } from "../flowplayer/flowplayer.component";

declare var $: any;
declare var lity: any;

@Component({
    templateUrl: "./invoke-player.component.html"
})
export class InvokePlayerComponent {
    idx: string;
    url: string;
    showVideo: boolean;

    @ViewChild("videoPlayer")
    videoPlayer: any;

    constructor(protected componentInjector: ComponentInjectorService) {
    }

    initVideo(component: ComponentRef<{}>, videojs: ViewContainerRef, id: string) {

        //let result: ComponentRef<any> = this.componentInjector.inject(this.injectContainer, "flowplayer");

        //console.log(result);

        $("#video").click();

        $(document).on("lity:close",
            (event, instance) => {
                console.log("Lightbox closed");

                videojs.clear();
                component.destroy();
                $(`#video_${id}`).remove();
                this.videoPlayer.player.dispose();
            });
    }

    initContent(component: ComponentRef<{}>, videojs: ViewContainerRef) {
        lity(this.url);

        $(document).on("lity:close",
            () => {
                console.log("Lightbox closed");

                videojs.clear();
                component.destroy();
            });
    }
}