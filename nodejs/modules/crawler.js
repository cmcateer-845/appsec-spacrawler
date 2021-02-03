import { extractHrefs } from './crawler_utils.js';
import crawler_page from './crawler_page.js';
import crawler_result from './crawler_result.js';
import puppeteer from 'puppeteer';
import sharp from 'sharp';
import util from 'util';
import path from 'path';
import fs from 'fs';
import { exit } from 'process';

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
        this.#result = new crawler_result(this.#settings.seedUrl);
    }

    async #uninitialize() {
        await this.#browser.close();
    }

    async #crawlPage(page, depth = 0)
    {
        if(depth > this.#settings.maxDepth) {
            return;
        }

        if(this.#crawledPages.has(page.getUrl())) {
            console.log(`Reusing route: ${page.getUrl()}`);

            debugger;
            const item = this.#crawledPages.get(page.getUrl());
            page.setTitle(item.getTitle());
            page.setImg(item.getImg());
            page.setImgFilePath(item.getImgFilePath());
            page.setHtml(item.getHtml());
            page.setHtmlFilePath(item.getHtmlFilePath());
            page.setChildren(item.getChildren());

            // Fill in the children with details (if they already exist).
            page.getChildren().forEach(c => {
              const item = this.#crawledPages.get(c.url);
              c.title = item ? item.title : '';
              c.img = item ? item.img : null;
            });

            return;
        } else {
            console.log(`Loading: ${page.getUrl()}`);

            const newPage = await this.#browser.newPage();
            await newPage.goto(page.getUrl(), { waitUntil: 'networkidle2' });

            let hrefs = await extractHrefs(newPage);
            hrefs = hrefs.filter((url) => {
                // Filter out urls tat are the same url as seed.
                if((url.replace(/\/+$/, '') !== this.#settings.seedUrl.replace(/\/+$/, '')))
                {
                    return true;
                }
                // Filer out urls that do not fit the crawl regex.
            });

            page.setTitle(await newPage.evaluate('document.title'));
            page.setChildren(hrefs.map(url => ({url})));

            // screenshots
            if(this.#settings.screenShots) {
                var imgFilepath = path.join(this.#result.getOutputFolder(), `${page.getGuid()}_screenshot.png`);
                let imgBuffer = await newPage.screenshot();
                imgBuffer = await sharp(imgBuffer).resize(null, 600).toBuffer();
                util.promisify(fs.writeFile)(imgFilepath, imgBuffer);
                page.setImg(`data:img/png;base64,${imgBuffer.toString('base64')}`);
                page.setImgFilePath(imgFilepath);
            }

            // dom dumps
            if(this.#settings.dumpDom) {
                var htmlFilepath = path.join(this.#result.getOutputFolder(), `${page.getGuid()}_dom.html`);
                let html = await newPage.content();
                util.promisify(fs.writeFile)(htmlFilepath, html);
                page.setHtml(html);
                page.setHtmlFilePath(htmlFilepath);
            }

            // other things we may want to do on each page visited

            this.#crawledPages.set(page.getUrl(), page);
            await newPage.close();
        }

        for(const child of page.getChildren()) {
            await this.#crawlPage(new crawler_page(child), depth + 1);
        }
    }

    async run(settings) {
        await this.#initialize(settings);

        let seedPageDetails = Object;
        seedPageDetails.url = this.#settings.seedUrl;
        await this.#crawlPage(new crawler_page(seedPageDetails));

        await this.#uninitialize();
    }
}