Date.prototype.toDateTimeString = function () {
    var date = this.toISOString();
    var indexOfDot = date.indexOf('.');    

    return date
        .replace('T', '--')
        .replace(/:\s*/g, '-')
        .substring(0, indexOfDot + 1);
}