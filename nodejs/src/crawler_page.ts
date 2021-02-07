import { Page } from 'puppeteer';
import { v4 as uuid } from 'uuid';

export default class crawler_page {
    private guid: string
    private url: string
    private title: string
    private img: string
    private imgFilePath: string
    private html: string
    private htmlFilePath: string
    private children: Set<crawler_page>

    public getGuid() {
        return this.guid;
    }

    public getUrl() {
        return this.url;
    }
    public setUrl(url: string) {
        if(url !== undefined) {
            this.url = url;
        } else {
            this.url = "";
        }
    }

    public getImg() {
        return this.img;
    }
    public setImg(img: string) {
        if(img !== undefined) {
            this.img = img;
        } else {
            this.img = "";
        }
    }

    public getImgFilePath() {
        return this.imgFilePath;
    }
    public setImgFilePath(imgFilePath: string) {
        if(imgFilePath !== undefined) {
            this.imgFilePath = imgFilePath;
        } else {
            this.imgFilePath = "";
        }
    }

    public getHtml() {
        return this.html;
    }
    public setHtml(html: string) {
        if(html !== undefined) {
            this.html = html;
        } else {
             this.html = "";
        }
    }

    public getHtmlFilePath() {
        return this.htmlFilePath;
    }
    public setHtmlFilePath(htmlFilePath: string) {
        if(htmlFilePath !== undefined) {
            this.htmlFilePath = htmlFilePath;
        } else {
             this.htmlFilePath = "";
        }
    }

    public getTitle() {
        return this.title;
    }
    public setTitle(title: string) {
        if(title !== undefined) {
            this.title = title;
        } else {
            this.title = "";
        }
    }

    public getChildren() {
        return this.children;
    }
    public setChildren(children: Set<crawler_page>) {
        if(children !== undefined && children !== null ) {
            this.children = children;
        } else {
            this.children = new Set<crawler_page>();
        }
    }
    public addChild(child: crawler_page) {
        this.children.add(child);
    }
    public removeChild(child: crawler_page) {
        this.children.delete(child);
    }
    public hasChild(child: crawler_page) {
        return this.children.has(child);
    }

    public constructor(url: string,
        title: string = "",
        img: string = "",
        imgFilePath: string = "",
        html: string = "",
        htmlFilePath: string = "",
        children: Set<crawler_page> = new Set<crawler_page>()) {
            this.guid = uuid();
            this.url = url;
            this.title = title;
            this.img = img;
            this.imgFilePath = imgFilePath;
            this.html = html;
            this.htmlFilePath = htmlFilePath;
            this.children = children;
    }
}