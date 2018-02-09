export interface ITorrentInfo {
    size: number;
    progress: number;
    downloaded: number;
    uploaded: number;
    ratio: number;
    uploadSpeed: number;
    downloadSpeed: number;
    eta: number;
    peersConnected: number;
    seedsConnected: number;
    availability: number;
    remaining: number;
}