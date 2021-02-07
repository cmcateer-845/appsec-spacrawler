import fs from 'fs';
import date from 'date-and-time';
import path from 'path';
import crawler_page from './crawler_page.js';

export default class crawler_result {
    private outputFolder: string;
    private seedUrl: string;
    private crawledPages: Map<string, crawler_page>;

    public getOutputFolder() {
        return this.outputFolder;
    }

    public getSeedUrl() {
        return this.seedUrl;
    }

    public getNumberCrawledPages() {
        return this.crawledPages.size;
    }

    public getCrawledPages() {
        return this.crawledPages;
    }
    public addCrawledPage(url: string, page: crawler_page) {
        this.crawledPages.set(url, page);
    }
    public removeCrawledPage(url: string) {
        this.crawledPages.delete(url);
    } 
    public getCrawledPage(url: string) {
        return this.crawledPages.get(url);
    }
    public hasCrawledPage(url: string) {
        return this.crawledPages.has(url);
    }

    public writeResults()
    {
        let summary = 
        "Crawl Summary\n" + 
        "-------------\n" +
        `Seed Url: ${this.seedUrl}\n` +
        `Number Crawled Pages: ${this.crawledPages.size}\n`;

        let linkCount = 1;
        this.crawledPages.forEach(page => {
            summary = summary + 
                `Crawled Link #${linkCount}: Url=${page.getUrl()}, Details=${page.getGuid()}\n`;

            linkCount++;
        });

        let summaryFilePath = path.join(this.outputFolder, "Summary.txt");

        try {
            fs.writeFileSync(summaryFilePath, summary);
        } catch(err) {
            console.log(`Error: Cannot output crawl results to output folder ${fs.realpathSync(summaryFilePath)}.`);
            process.exit(1);
        }
    }

    private generateResultsDirectory()
    {
        const now = new Date();
        const baseFolder = fs.realpathSync("../results");
        this.outputFolder = path.join(baseFolder, date.format(now, 'YYYY-MM-DD-HH-mm-ss'));

        try {
            if(!fs.existsSync(baseFolder)) {
                fs.mkdirSync(baseFolder);
            }

            fs.mkdirSync(this.outputFolder);
        }
        catch(err) {
            console.log(`Error: Cannot create crawl output folder ${fs.realpathSync(this.outputFolder)}.`);
            process.exit(1);
        }
    }

    public constructor(seedUrl: string, crawledPages: Map<string, crawler_page> = new Map<string, crawler_page>())
    {
        this.generateResultsDirectory();
        this.seedUrl = seedUrl;
        this.crawledPages = crawledPages;
    }
}