import { guid } from 'uuid';

export default class crawler_page {
    #guid;
    #url;
    #title;
    #screenShot;
    #childPages;

    constructor(url, title = "") {
        this.#guid = guid();
        this.#url = url;
        this.#title = title;
    }
}