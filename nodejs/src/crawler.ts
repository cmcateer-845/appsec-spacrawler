import { extractHrefs } from './crawler_utils.js';
import crawler_page from './crawler_page.js';
import crawler_result from './crawler_result.js';
import puppeteer, { Browser } from 'puppeteer';
import sharp from 'sharp';
import util from 'util';
import path from 'path';
import fs from 'fs';
import crawler_settings from './crawler_settings.js';

export default class crawler {
    private browser: Browser
    private settings: crawler_settings
    private result: crawler_result

    private async initialize(settings: crawler_settings) {
        this.settings = settings;
        this.browser = await puppeteer.launch({
            headless: this.settings.isHeadlessSet(),
            defaultViewport: { width: 0, height: 0},
            args: []
        });
        this.result = new crawler_result(this.settings.getSeedUrl());
    }

    private async uninitialize() {
        await this.browser.close();
    }

    private async crawlPage(page: crawler_page, depth = 0)
    {
        if(depth > this.settings.getMaxDepth()) {
            return;
        }

        if(this.result.hasCrawledPage(page.getUrl())) {
            console.log(`Reusing route: ${page.getUrl()}`);

            const item = this.result.getCrawledPage(page.getUrl());
            page.setTitle(item.getTitle());
            page.setImg(item.getImg());
            page.setImgFilePath(item.getImgFilePath());
            page.setHtml(item.getHtml());
            page.setHtmlFilePath(item.getHtmlFilePath());
            page.setChildren(item.getChildren());

            // Fill in the children with details (if they already exist).
            debugger;
            page.getChildren().forEach(c => {
              const item = this.result.getCrawledPage(c.getUrl());
              if(item !== undefined) {
                c.setTitle(item.getTitle());
                c.setImg(item.getImg());
              }
            });

            return;
        } else {
            console.log(`Loading: ${page.getUrl()}`);

            const newPage = await this.browser.newPage();
            await newPage.goto(page.getUrl(), { waitUntil: 'networkidle2' });

            let hrefs = await extractHrefs(newPage) as Array<string>;
            hrefs = hrefs.filter((url: string) => {
                // Filter out urls that are the same url as seed.
                if((url.replace(/\/+$/, '') !== this.settings.getSeedUrl().replace(/\/+$/, '')))
                {
                    return true;
                }
                // Filer out urls that do not fit the crawl regex.
            });

            page.setUrl(await newPage.url());
            page.setTitle(await newPage.evaluate('document.title') as string);

            hrefs.forEach(link => {
                page.addChild(new crawler_page(link));                
            });

            // screenshots
            if(this.settings.isScreenShotsSet()) {
                var imgFilepath = path.join(this.result.getOutputFolder(), `${page.getGuid()}_screenshot.png`);
                let imgBuffer = await sharp(await newPage.screenshot()).resize(null, 600).toBuffer();
                util.promisify(fs.writeFile)(imgFilepath, imgBuffer);
                page.setImg(`data:img/png;base64,${imgBuffer.toString('base64')}`);
                page.setImgFilePath(imgFilepath);
            }

            // dom dumps
            if(this.settings.isDumpDomSet()) {
                var htmlFilepath = path.join(this.result.getOutputFolder(), `${page.getGuid()}_dom.html`);
                let html = await newPage.content();
                util.promisify(fs.writeFile)(htmlFilepath, html);
                page.setHtml(html);
                page.setHtmlFilePath(htmlFilepath);
            }

            // other things we may want to do on each page visited

            this.result.addCrawledPage(page.getUrl(), page);
            await newPage.close();
        }

        for(const child of page.getChildren()) {
            await this.crawlPage(child, depth + 1);
        }
    }

    async run(settings: crawler_settings) {
        await this.initialize(settings);
        await this.crawlPage(new crawler_page(this.settings.getSeedUrl()));
        await this.result.writeResults();
        await this.uninitialize();
    }
}