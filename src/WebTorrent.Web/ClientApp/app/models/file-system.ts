export interface IFileSystemItem {
    id: number;
    stream: string;
    downloadPath?: string;
    fullName: string;
    name: string;
    size: number;
    lastChanged: Date;
    type: string;
    isStreaming: boolean;
}