import { IFileSystemItem } from "./file-system";

export interface IContent {
    hash: string;
    isInProgress: boolean;
    currentFolder: string;
    parentFolder?: string;
    fsItems: IFileSystemItem[];
}