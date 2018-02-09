import { Component, AfterViewInit, ElementRef, Input, ViewChild, NgZone } from "@angular/core";

declare var Hls: any;

@Component({
    selector: "flowplayer",
    templateUrl: "./flowplayer.component.html"
})
export class FlowplayerComponent implements AfterViewInit {

    // video asset url
    @Input()
    url: string;

    // index to create unique ID for component
    @Input()
    idx: string;

    hlsPlayer: any;

    constructor(public zone: NgZone) {}

    ngOnInit() {


        ////const container = document.getElementById("player"); //this.player.nativeElement; //
        ////console.log(container);
        //const self = $("#player1")[0];
        //console.log(self);


        //    // install flowplayer into selected container
        //flowplayer(self,{
        //        splash: true,
        //        overlay: true,
        //        autoplay: true,
        //        clip: {
        //            live: true,
        //            sources: [
        //                //{
        //                //    type: "application/x-mpegurl",
        //                //    src: "http://nextiptv.ddns.net:2580/get.php?username=6085F2CBdb&password=6085F2CBdb&type=m3u"
        //                //},
        //                {
        //                    type:"video/mp4",
        //                    src:"http://techslides.com/demos/sample-videos/small.mp4"
        //                }
        //            ]
        //        }
        //    }).on("ready", (e, api, video) => {

        //        console.log("ready");
        //        // Make up an aspect ratio
        //        var aspectRatio = 364 / 540;

        //        // internal method to handle a window resize event to adjust the video player
        //        function resizeVideoJS() {
        //            var width = document.getElementById("player1").parentElement.offsetWidth;
        //            this.width(width).height(width * aspectRatio);
        //        }

        //        // Initialize resizeVideoJS()
        //        resizeVideoJS();

        //        // Then on resize call resizeVideoJS()
        //        window.onresize = resizeVideoJS;

        //        console.log(this.url);
        //        document.querySelector("player.fp-title").innerHTML =
        //            api.engine.engineName + " engine playing " + video.type;

        //    });
    }

    ngAfterViewInit() {
        this.zone.runOutsideAngular(() => {
            if (Hls.isSupported()) {
                var video = document.getElementById(`video_${this.idx}`);
                this.hlsPlayer = new Hls();
                this.hlsPlayer.loadSource(this.url);
                this.hlsPlayer.attachMedia(video);
                this.hlsPlayer.on(Hls.Events.MANIFEST_PARSED,
                    () => {
                        console.log("Successfully created hlsPlayer instance");
                    });
            }
        });
    }
}