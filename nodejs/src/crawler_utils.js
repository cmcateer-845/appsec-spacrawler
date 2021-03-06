function extractHrefsJs(sameOrigin = true) {
    const allElements = [];

    const findAllElements = function(nodes) {
      for (let i = 0, el; el = nodes[i]; ++i) {
        allElements.push(el);
        // If the element has a shadow root, dig deeper.
        if (el.shadowRoot) {
          findAllElements(el.shadowRoot.querySelectorAll('*'));
        }
      }
    };
  
    findAllElements(document.querySelectorAll('*'));
  
    const filtered = allElements
      .filter(el => el.localName === 'a' && el.href) // element is an anchor with an href.
      .filter(el => el.href !== location.href) // link doesn't point to page's own URL.
      .filter(el => {
        if (sameOrigin) {
          return new URL(location).origin === new URL(el.href).origin;
        }
        return true;
      })
      .map(a => a.href);
  
    return Array.from(new Set(filtered));
}

export async function extractHrefs(page, sameOrigin = true)
{
    return await page.evaluate(extractHrefsJs);
}

export async function extractEventListeners(page) {
    const client = await page.target().createCDPSession();

    const { result } = await client.send('Runtime.evaluate', { expression: 'document' });
    return await client.send('DOMDebugger.getEventListeners', { objectId: result.objectId });
}

export async function extractLocalStorage(page) {
    const localStorageData = await page.evaluate(() => {
        let json = {};
        for(let i = 0; i < localStorage.length; i++) {
            const key = localStorage.key(i);
            json[key] = localStorage.getItem(key);
        }
        return json;
    });
    return localStorageData;
}

export async function extractSessionStorage(page) {
    const sessionStorageData = await page.evaluate(() => {
        let json = {};
        for(let i = 0; i < sessionStorage.length; i++) {
            const key = sessionStorage.key(i);
            json[key] = sessionStorage.getItem(key);
        }
        return json;
    });
    return sessionStorageData;
}