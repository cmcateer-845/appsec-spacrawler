import args from 'commander';
import validUrl from 'valid-url';
import enquirer from 'enquirer';
import asciiart from 'asciiart-logo';
import chalk from 'chalk';
import commander from 'commander';
const program = commander.program;

import crawler from  './crawler.js';
import crawler_settings from './crawler_settings.js';

export default class crawler_init {
    constructor() {
        this.#init();
    }

    async #init() {
        const options = await this.#parseCommandLineArguments();
        console.log(options);
    
        const settings = new crawler_settings(
             options["seedUrl"],
             options["depth"],
             options["screenShots"],
             options["headless"],
             options["domDump"]
        );
    
        await new crawler().run(settings);
    }

    async #parseCommandLineArguments() {
        program.addHelpText('beforeAll', `Example call: $ appsec-spacrawler --help`);
    
        program
            .option('--interactive', 'use interactive mode to configure the crawl.', false)
            .option('--seedUrl <url>', 'url to start crawling from.')
            .option('--depth <depth>', 'maximum crawl depth.', 2)
            .option('--screenShots', 'take a screenshot of each page visited.', false)
            .option('--dumpDom', 'dump the document object model of each page visted.', false)
            .option('--headless', 'use the headless browser.', false)
            program.version('v1.0.0');
    
    
        program.parse(process.argv);
    
        const options = program.opts();
    
        if(options.interactive !== false)
        {
            await this.#renderLogo();
            return await this.#interactiveCommandPrompt();
        }
    
        return options;
    }

    async #interactiveCommandPrompt() {
        const menu = [
            {
                name: 'seedUrl',
                type: 'input',
                message: 'Where do you want to start crawling from?',
                validate: function(value) {
                    if(!value.length || !validUrl.isUri(value))
                    {
                        return "Please enter a valid url to crawl from.";
                    }
                    return true;
                }
            },
            {
                name: 'depth',
                type: 'input',
                message: 'How deep do you want the crawl to go?',
                validate: function(value) {
                    if(!value > 0)
                    {
                        return "Please enter a valid depth to crawl from.";
                    }
                    return true;
                }
            },
            {
                name: 'screenShots',
                type: 'toggle',
                message: 'For each page, do you want to output a screenshot?',
                enabled: 'Yes',
                disabled: 'No'
            },
            {
                name: 'headless',
                type: 'toggle',
                message: 'Do you want to run the browser in headless mode?',
                enabled: 'Yes',
                disabled: 'No'
            },
            {
                name: 'domDump',
                type: 'toggle',
                message: 'For each page, do you want to output a dump of the DOM?',
                enabled: 'Yes',
                disabled: 'No'
            }
        ];
        return enquirer.prompt(menu);
    }
    
    async #renderLogo() {
        const colorChoices = [
            'magenta',
            'cyan',
            'white',
            'green',
            'orange',
            'blue',
            'yellow',
            'red'
        ]
        const rnd = Math.floor(Math.random() * colorChoices.length);
        const color = colorChoices[rnd];

        console.log(asciiart({
            name: 'appsec-spacrawler',
            font: 'Speed',
            lineChars: 50,
            padding: 2,
            margin: 0,
            borderColor: color,
            logoColor: color,
            textColor: color,
        })
        .emptyLine()
        .right('version 1.0.0')
        .emptyLine()
        .right('by Conor McAteer')
        .render());
    }
}