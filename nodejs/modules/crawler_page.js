import { v4 as uuid } from 'uuid';

export default class crawler_page {
    #guid
    #html
    #imgFilePath
    #htmlFilePath
    #details

    getGuid() {
        return this.#guid;
    }

    getUrl() {
        return this.#details.url;
    }
    setUrl(url) {
        if(url !== undefined) {
            this.#details.url = url;
        } else {
            this.#details.url = "";
        }
    }

    getImg() {
        return this.#details.img;
    }
    setImg(img) {
        if(img !== undefined) {
            this.#details.img = img;
        } else {
            this.#details.img = "";
        }
    }

    getImgFilePath() {
        return this.#imgFilePath;
    }
    setImgFilePath(imgFilePath) {
        if(imgFilePath !== undefined) {
            this.#imgFilePath = imgFilePath;
        } else {
            this.#imgFilePath = "";
        }
    }

    getHtml() {
        return this.#html;
    }
    setHtml(html) {
        if(html !== undefined) {
            this.#html = html;
        } else {
             this.#html = "";
        }
    }

    getHtmlFilePath() {
        return this.#htmlFilePath;
    }
    setHtmlFilePath(htmlFilePath) {
        if(htmlFilePath !== undefined) {
            this.#htmlFilePath = htmlFilePath;
        } else {
             this.#htmlFilePath = "";
        }
    }

    getTitle() {
        return this.#details.title;
    }
    setTitle(title) {
        if(title !== undefined) {
            this.#details.title = title;
        } else {
            this.#details.title = "";
        }
    }

    getChildren() {
        return this.#details.children;
    }
    setChildren(children) {
        if(children !== undefined) {
            this.#details.children = children;
        } else {
            this.#details.children = new Array();
        }
    }

    getPage() {
        return this.#details;
    }

    constructor(page) {
        this.#guid = uuid();
        this.#html = "";
        this.#details = page;
    }
}