import { extractHrefs } from './crawler_utils.js';
import puppeteer from 'puppeteer';

export default class crawler {
    #browser;
    #crawledPages;
    #settings;
    #result;

    async #initialize(settings) {
        this.#settings = settings;
        this.#browser = await puppeteer.launch({
            headless: this.#settings.headless,
            defaultViewport: { width: 0, height: 0},
            args: []
        });
        this.#crawledPages = new Map();
        this.#result = {};
    }

    async #uninitialize() {
        await this.#browser.close();
    }

    async #crawlPage(page, depth)
    {
        if(depth > this.#settings.maxDepth) {
            return;
        }

        if(this.#crawledPages.has(page.url)) {
            console.log(`Reusing route: ${page.url}`);

            const item = crawledPages.get(page.url);
            page.title = item.title;
            page.img = item.img;
            page.children = item.children;

            // Fill in the children with details (if they already exist).
            page.children.forEach(c => {
              const item = crawledPages.get(c.url);
              c.title = item ? item.title : '';
              c.img = item ? item.img : null;
            });

            return;
        } else {
            console.log(`Loading: ${page.url}`);

            const newPage = await this.#browser.newPage();
            await newPage.goto(page.url, { waitUntil: 'networkidle2' });

            let hrefs = await extractHrefs.call(newPage);
            hrefs = hrefs.filter(a => a !== this.#settings.seedUrl);

            page.title = await newPage.evaluate('document.title');
            page.children = hrefs.map(url => ({url}));

            // screenshots
            // dom dumps
            // other things we may want to do on each page visited

            this.#crawledPages.set(page.url, page);
            await newPage.close();
        }

        for(const child of page.children) {
            await this.#crawlPage(child, depth + 1);
        }
    }

    async run(settings) {
        await this.#initialize(settings);
        var seedPage;
        seedPage.url = settings.seedUrl;
        //await this.#crawlPage();
        await this.#uninitialize();
    }
}