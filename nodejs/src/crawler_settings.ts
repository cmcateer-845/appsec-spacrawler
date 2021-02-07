import validUrl from 'valid-url';

export default class crawler_settings {
    private seedUrl: string
    private maxDepth: number
    private headless: boolean
    private screenShots: boolean
    private dumpDom: boolean

    getSeedUrl() {
        return this.seedUrl;
    }

    getMaxDepth() {
        return this.maxDepth;
    }

    isHeadlessSet() {
        return this.headless;
    }

    isScreenShotsSet() {
        return this.screenShots;
    }

    isDumpDomSet() {
        return this.dumpDom;
    }

    private validate() {
        if(this.seedUrl === undefined || !this.seedUrl.length || !validUrl.isUri(this.seedUrl)) {
            console.log(`Error: Seed URL is not valid. Cancelling crawl.`);
            process.exit(1);
        }

        if(this.maxDepth < 1) {
            console.log(`Error: Max depth is not valid: ${this.maxDepth}. Cancelling crawl.`);
            process.exit(1);
        }
    }

    constructor(seedUrl: string,
        maxDepth = 2,
        headless = false,
        screenShots = false,
        dumpDom = false) {
            this.seedUrl = seedUrl;
            this.maxDepth = maxDepth;
            this.headless = headless;
            this.screenShots = screenShots;
            this.dumpDom = dumpDom;

            this.validate();
    }
}