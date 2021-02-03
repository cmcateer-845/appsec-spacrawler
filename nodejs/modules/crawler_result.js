import fs from 'fs';
import date from 'date-and-time';
import path from 'path';

export default class crawler_result {
    #outputFolder;
    #seedUrl;
    #numberCrawledPages;
    #crawledPages;

    getOutputFolder() {
        return this.#outputFolder;
    }

    getSeedUrl() {
        return this.#seedUrl;
    }

    getNumberCrawledPages() {
        return this.#numberCrawledPages;
    }

    writeResults()
    {
        let summary = 
        "Crawl Summary\n" + 
        "-------------\n" +
        `Seed Url: ${this.#seedUrl}\n` +
        `Number Crawled Pages: ${this.#numberCrawledPages}\n`;

        let linkCount = 1;
        this.#crawledPages.forEach(element => {
            summary = summary + 
                `Crawled Link #${linkCount}: Url=${element.getUrl()}, Details=${element.getGuid()}`;

            linkCount++;
        });

        fs.writeFile(`${this.#outputFolder}`)
    }

    #generateResultsDirectory()
    {
        const now = new Date();
        const baseFolder = fs.realpathSync("../results");
        this.#outputFolder = path.join(baseFolder, date.format(now, 'YYYY-MM-DD-HH-mm-ss'));

        try {
            if(!fs.existsSync(baseFolder)) {
                fs.mkdirSync(baseFolder);
            }

            fs.mkdirSync(this.#outputFolder);
        }
        catch(err) {
            console.log(`Error: Cannot create crawl output folder ${fs.realpath(this.#outputFolder)}`);
            process.exit(1);
        }
    }

    constructor(seedUrl, crawledPages = new Map())
    {
        this.#generateResultsDirectory();
        this.#seedUrl = seedUrl;
        this.#numberCrawledPages = 0;
        this.#crawledPages = crawledPages;
    }
}