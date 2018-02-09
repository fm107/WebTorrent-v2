import { Pipe, PipeTransform } from "@angular/core"
import {isNumeric} from "rxjs/util/isNumeric"

import { TdDataTableSortingOrder} from "@covalent/core";

@Pipe({ name: "sort" })
export class SortPipe implements PipeTransform {
    transform(data: any[], sortBy, sortOrder) {
        if (data) {
            return data.sort((a, b) => {
                var compA = a[sortBy];
                var compB = b[sortBy];
                var direction = 0;
                if (isNumeric(compA) && isNumeric(compB)) {
                    direction = compA - compB;
                } else {
                    if (compA < compB) {
                        direction = -1;
                    } else if (compA > compB) {
                        direction = 1;
                    }
                }
                return direction * (sortOrder === TdDataTableSortingOrder.Descending ? -1 : 1);
            });
        }

        return null;
    }
}