import { Pipe, PipeTransform } from "@angular/core"

import {IFileSystemItem} from "../models/file-system";

@Pipe({ name: "showFiles" })
export class ShowFilesPipe implements PipeTransform {
    transform(data: IFileSystemItem[], showFolder) {

        if (showFolder) {
            return data.filter(f => f.type == "folder");
        }
        return data.filter(v => v.type == "file");
    }
}