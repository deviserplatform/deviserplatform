export class Pagination {
    pageSizes: number[] = [10, 25, 50, 100];

    get currentWindowStart(): number {
        if (this.pageNo > 1 && this.pageSize > 0) {
            return (this.pageNo - 1) * this.pageSize;
        }

        return 1;
    }

    get currentWindowEnd(): number {
        if (this.pageNo > 1 && this.pageSize > 0) {
            const windowEnd = this.currentWindowStart + this.pageSize;
            if (windowEnd > this.totalRecordCount) {
                return this.totalRecordCount;
            }
            return windowEnd;
        }
        return 1;
    }

    constructor(
        public pageNo: number,
        public pageSize: number,
        public pageCount: number,
        public totalRecordCount: number) { }
}
