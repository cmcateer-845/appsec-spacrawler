import validUrl from 'valid-url';

export default class crawler_settings {
    #validate() {
        if(this.seedUrl === undefined || !this.seedUrl.length || !validUrl.isUri(this.seedUrl)) {
            console.log(`Error: Seed URL is not valid. Cancelling crawl.`);
            process.exit(1);
        }

        if(this.maxDepth < 1) {
            console.log(`Error: Max depth is not valid: ${this.maxDepth}. Cancelling crawl.`);
            process.exit(1);
        }
    }

    constructor(seedUrl,
        maxDepth = 2,
        headless = false,
        screenShots = false,
        dumpDom = false) {
            this.seedUrl = seedUrl;
            this.maxDepth = maxDepth;
            this.headless = headless;
            this.screenShots = screenShots;
            this.dumpDom = dumpDom;

            this.#validate();
    }
}