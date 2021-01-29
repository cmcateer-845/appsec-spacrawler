export default class crawler_settings {
    constructor(seedUrl,
        maxDepth = 2,
        headless = false,
        screenShots = false,
        dumpDom = false)
    {
        this.seedUrl = seedUrl;
        this.maxDepth = maxDepth;
        this.headless = headless;
        this.screenShots = screenShots;
        this.dumpDom = dumpDom;
    }
}