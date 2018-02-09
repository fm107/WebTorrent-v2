import { Pipe, PipeTransform } from "@angular/core"

@Pipe({ name: "filter" })
export class FilterPipe implements PipeTransform {
    transform(data, searchTerm, ignoreCase = true) {
        var filter = searchTerm ? (ignoreCase ? searchTerm.toLowerCase() : searchTerm) : "";
        if (filter) {
            data = data.filter(item => {
                var res = Object.keys(item).find(key => {
                    var preItemValue = (`${item[key]}`);
                    var itemValue = ignoreCase ? preItemValue.toLowerCase() : preItemValue;
                    return itemValue.indexOf(filter) > -1;
                });
                return !(typeof res === "undefined");
            });
        }

        return data;
    }
}