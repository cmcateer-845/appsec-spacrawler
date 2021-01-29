import fs from 'fs';
import date from 'dat-and-time';

export default class crawler_result {
    #outputDirectory;
    #seedUrl;
    #numberCrawledPages;
    #crawledPages;

    constructor(seedUrl = "")
    {
        this.#generateResultsDirectory();
        this.#seedUrl = seedUrl;
        this.#numberCrawledPages = 0;
        this.#crawledPages = new Map();
    }

    #generateResultsDirectory()
    {
        const now = new Date();
        this.#outputDirectory = '../results/' + date.format(now, 'YYYY-MM-DD-HH-mm-ss');
        fs.mkdirSync(this.#outputDirectory, function(err) {
            if(err) {
                console.log(err);
                process.exit(1);
            }
        })
    }

    #writeResults()
    {
        let summary = 
        "Crawl Summary\n" + 
        "-------------\n" +
        `Seed Url: ${this.#seedUrl}\n` +
        `Number Crawled Pages: ${this.#numberCrawledPages}\n`;

        let linkCount = 1;
        this.#crawledPages.forEach(element => {
            summary = summary + 
                `Crawled Link #${linkCount}: Url=${element.}`

            linkCount++;
        });

        fs.writeFile(`${this.#outputDirectory}`)
    }
}