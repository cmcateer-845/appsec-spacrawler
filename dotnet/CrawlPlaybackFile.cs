using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using PuppeteerSharp.Media;
using PuppeteerSharp.Mobile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpaCrawler
{
    public enum PlaybackType
    {
        None,
        Login,
        Traffic
    }
    public enum PlaybackActionEnum
    {
        None,
        AddScriptTagAsync,
        AddStyleTagAsync,
        AuthenticateAsync,
        BringToFrontAsync,
        ClickAsync,
        CloseAsync,
        DeleteCookieAsync,
        Dispose,
        DisposeAsync,
        EmulateAsync,
        EmulateMediaAsync,
        EmulateMediaFeaturesAsync,
        EmulateMediaTypeAsync,
        EmulateTimezoneAsync,
        EvaluateExpressionAsync,
        EvaluateExpressionHandleAsync,
        EvaluateExpressionOnNewDocumentAsync,
        EvaluateFunctionAsync,
        EvaluateFunctionHandleAsync,
        EvaluateFunctionOnNewDocumentAsync,
        EvaluateOnNewDocumentAsync,
        ExposeFunctionAsync,
        FocusAsync,
        GetContentAsync,
        GetCookiesAsync,
        GetTitleAsync,
        GoBackAsync,
        GoForwardAsync,
        GoToAsync,
        GoToAsyncTimeoutWaitUntilNavigation,
        GoToAsyncWaitUntilNavigation,
        HoverAsync,
        MetricsAsync,
        PdfAsync,
        PdfAsyncPdfOptions,
        PdfDataAsync,
        PdfDataAsyncPdfOptions,
        PdfStreamAsync,
        PdfStreamAsyncPdfOptions,
        QueryObjectsAsync,
        QuerySelectorAllAsync,
        QuerySelectorAllHandleAsync,
        QuerySelectorAsync,
        ReloadAsync,
        ReloadAsyncTimeoutWaitUntilNavigation,
        ScreenshotAsync,
        ScreenshotAsyncScreenshotOptions,
        ScreenshotBase64Async,
        ScreenshotBase64AsyncScreenshotOptions,
        ScreenshotDataAsync,
        ScreenshotDataAsyncScreenshotOptions,
        ScreenshotStreamAsync,
        ScreenshotStreamAsyncScreenshotOptions,
        SelectAsync,
        SetBurstModeOffAsync,
        SetBypassCSPAsync,
        SetCacheEnabledAsync,
        SetContentAsync,
        SetCookieAsync,
        SetExtraHttpHeadersAsync,
        SetGeolocationAsync,
        SetJavaScriptEnabledAsync,
        SetOfflineModeAsync,
        SetRequestInterceptionAsync,
        SetUserAgentAsync,
        SetViewportAsync,
        TapAsync,
        TypeAsync,
        WaitForExpressionAsync,
        WaitForFileChooserAsync,
        WaitForFunctionAsync,
        WaitForFunctionAsyncWaitForFunctionOptions,
        WaitForNavigationAsync,
        WaitForRequestAsync,
        WaitForRequestAsyncPredicate,
        WaitForResponseAsync,
        WaitForResponseAsyncPredicate,
        WaitForSelectorAsync,
        WaitForTimeoutAsync,
        WaitForXPathAsync,
        XPathAsync
    }
    public class PlaybackActionLookup
    {
        static public Dictionary<string, PlaybackActionMeta> _mapping =
            new Dictionary<string, PlaybackActionMeta>
            {
                { "AddScriptTagAsync", new PlaybackActionMeta(PlaybackActionEnum.AddScriptTagAsync, new Func<Page, string, Task<ElementHandle>>(async (Page page, string url) => { return await page.AddScriptTagAsync(url); }), new Type[] { typeof(string) }, typeof(Task<ElementHandle>)) },
                { "AddStyleTagAsync", new PlaybackActionMeta(PlaybackActionEnum.AddStyleTagAsync,  new Func<Page, string, Task<ElementHandle>>(async (Page page, string url) => { return await page.AddStyleTagAsync(url); }),  new Type[] { typeof(string) }, typeof(Task<ElementHandle>)) },
                { "AuthenticateAsync", new PlaybackActionMeta(PlaybackActionEnum.AuthenticateAsync, new Action<Page, Credentials>(async (Page page, Credentials creds) => { await page.AuthenticateAsync(creds); }), new Type[] { typeof(Credentials) }, typeof(Task)) },
                { "BringToFrontAsync", new PlaybackActionMeta(PlaybackActionEnum.BringToFrontAsync, new Action<Page>(async (Page page) => { await page.BringToFrontAsync(); }), new Type[] { }, typeof(Task)) },
                { "ClickAsync", new PlaybackActionMeta(PlaybackActionEnum.ClickAsync, new Action<Page, string, ClickOptions>(async (Page page, string selector, ClickOptions options) => { await page.ClickAsync(selector, options); }), new Type[] { typeof(string), typeof(ClickOptions) }, typeof(Task)) },
                { "CloseAsync", new PlaybackActionMeta(PlaybackActionEnum.CloseAsync, new Action<Page, PageCloseOptions>(async (Page page, PageCloseOptions options) => { await page.CloseAsync(options); }), new Type[] { typeof(PageCloseOptions) }, typeof(Task)) },
                { "DeleteCookieAsync", new PlaybackActionMeta(PlaybackActionEnum.DeleteCookieAsync, new Action<Page, CookieParam[]>(async (Page page, CookieParam[] cookies) => { await page.DeleteCookieAsync(cookies); }), new Type[] { typeof(CookieParam[]) }, typeof(Task)) },
                { "Dispose", new PlaybackActionMeta(PlaybackActionEnum.Dispose, new Action<Page>((Page page) => { page.Dispose(); }), new Type[] { }, typeof(void)) },
                { "DisposeAsync", new PlaybackActionMeta(PlaybackActionEnum.DisposeAsync, new Action<Page>(async (Page page) => { await page.DisposeAsync(); }), new Type[] { }, typeof(ValueTask)) },
                { "EmulateMediaAsync", new PlaybackActionMeta(PlaybackActionEnum.EmulateMediaAsync, new Action<Page, MediaType>(async (Page page, MediaType type) => { await page.EmulateMediaAsync(type); }), new Type[] { typeof(MediaType) }, typeof(Task)) },
                { "EmulateMediaFeaturesAsync", new PlaybackActionMeta(PlaybackActionEnum.EmulateMediaFeaturesAsync, new Action<Page, IEnumerable<MediaFeatureValue>>(async (Page page, IEnumerable<MediaFeatureValue> features) => { await page.EmulateMediaFeaturesAsync(features); }), new Type[] { typeof(IEnumerable<MediaFeatureValue>) }, typeof(Task)) },
                { "EmulateMediaTypeAsync", new PlaybackActionMeta(PlaybackActionEnum.EmulateMediaTypeAsync, new Action<Page, MediaType>(async (Page page, MediaType type) => { await page.EmulateMediaTypeAsync(type); }), new Type[] { typeof(MediaType) }, typeof(Task)) },
                { "EmulateTimezoneAsync", new PlaybackActionMeta(PlaybackActionEnum.EmulateTimezoneAsync, new Action<Page, string>(async (Page page, string timezoneId) => { await page.EmulateTimezoneAsync(timezoneId); }), new Type[] { typeof(string) }, typeof(Task)) },
                { "EvaluateExpressionAsync", new PlaybackActionMeta(PlaybackActionEnum.EvaluateExpressionAsync, new Func<Page, string, Task<JToken>>(async (Page page, string script) => { return await page.EvaluateExpressionAsync(script); }), new Type[] { typeof(string) }, typeof(Task<JToken>)) },
                { "EvaluateExpressionHandleAsync", new PlaybackActionMeta(PlaybackActionEnum.EvaluateExpressionHandleAsync, new Func<Page, string, Task<JSHandle>>(async (Page page, string script) => { return await page.EvaluateExpressionHandleAsync(script); }), new Type[] { typeof(string) }, typeof(Task<JSHandle>)) },
                { "EvaluateExpressionOnNewDocumentAsync", new PlaybackActionMeta(PlaybackActionEnum.EvaluateExpressionOnNewDocumentAsync, new Action<Page, string>(async (Page page, string expression) => { await page.EvaluateExpressionOnNewDocumentAsync(expression); }), new Type[] { typeof(string) }, typeof(Task)) },
                { "EvaluateFunctionAsync", new PlaybackActionMeta(PlaybackActionEnum.EvaluateFunctionAsync, new Func<Page, string, object[], Task<JToken>>(async (Page page, string script, object[] args) => { return await page.EvaluateFunctionAsync(script, args); }), new Type[] { typeof(string), typeof(object[]) }, typeof(Task<JToken>)) },
                { "EvaluateFunctionHandleAsync", new PlaybackActionMeta(PlaybackActionEnum.EvaluateFunctionHandleAsync, new Func<Page, string, object[], Task<JSHandle>>(async (Page page, string script, object[] args) => { return await page.EvaluateFunctionHandleAsync(script, args); }), new Type[] { typeof(string), typeof(object[]) }, typeof(Task<JSHandle>)) },
                { "EvaluateFunctionOnNewDocumentAsync", new PlaybackActionMeta(PlaybackActionEnum.EvaluateFunctionOnNewDocumentAsync, new Action<Page, string, object[]>(async (Page page, string script, object[] args) => { await page.EvaluateFunctionOnNewDocumentAsync(script, args); }), new Type[] { typeof(string), typeof(object[]) }, typeof(Task)) },
                { "EvaluateOnNewDocumentAsync", new PlaybackActionMeta(PlaybackActionEnum.EvaluateOnNewDocumentAsync, new Action<Page, string, object[]>(async (Page page, string script, object[] args) => { await page.EvaluateOnNewDocumentAsync(script, args); }), new Type[] { typeof(string), typeof(object[]) }, typeof(Task)) },
                { "ExposeFunctionAsync", new PlaybackActionMeta(PlaybackActionEnum.ExposeFunctionAsync, new Action<Page, string, Action>(async (Page page, string name, Action puppeteerFunction) => { await page.ExposeFunctionAsync(name, puppeteerFunction); }), new Type[] { typeof(string), typeof(Action) }, typeof(Task)) },
                { "FocusAsync", new PlaybackActionMeta(PlaybackActionEnum.FocusAsync, new Action<Page, string>(async (Page page, string selector) => { await page.FocusAsync(selector); }), new Type[] { typeof(string) }, typeof(Task)) },
                { "GetContentAsync", new PlaybackActionMeta(PlaybackActionEnum.GetContentAsync, new Func<Page, Task<string>>(async (Page page) => { return await page.GetContentAsync(); }), new Type[] { }, typeof(Task)) },
                { "GetCookiesAsync", new PlaybackActionMeta(PlaybackActionEnum.GetCookiesAsync, new Func<Page, string[], Task<CookieParam[]>>(async (Page page, string[] urls) => { return await page.GetCookiesAsync(urls); }), new Type[] { typeof(string[]) }, typeof(Task)) },
                { "GetTitleAsync", new PlaybackActionMeta(PlaybackActionEnum.GetTitleAsync, new Func<Page, Task<string>>(async (Page page) => { return await page.GetTitleAsync(); }), new Type[] { }, typeof(Task<string>)) },
                { "GoBackAsync", new PlaybackActionMeta(PlaybackActionEnum.GoBackAsync, new Func<Page, NavigationOptions, Task<Response>>(async (Page page, NavigationOptions options) => { return await page.GoBackAsync(options); }), new Type[] { typeof(NavigationOptions) }, typeof(Task<Response>)) },
                { "GoForwardAsync", new PlaybackActionMeta(PlaybackActionEnum.GoForwardAsync, new Func<Page, NavigationOptions, Task<Response>>(async (Page page, NavigationOptions options) => { return await page.GoForwardAsync(options); }), new Type[] { typeof(NavigationOptions) }, typeof(Task<Response>)) },
                { "GoToAsync", new PlaybackActionMeta(PlaybackActionEnum.GoToAsync, new Func<Page, string, NavigationOptions, Task<Response>>(async (Page page, string url, NavigationOptions options) => { return await page.GoToAsync(url, options); }), new Type[] { typeof(string), typeof(NavigationOptions) }, typeof(Task<Response>)) },
                { "GoToAsyncTimeoutWaitUntilNavigation", new PlaybackActionMeta(PlaybackActionEnum.GoToAsyncTimeoutWaitUntilNavigation, new Func<Page, string, int, WaitUntilNavigation[], Task<Response>>(async (Page page, string url, int timeout, WaitUntilNavigation[] waitUntil) => { return await page.GoToAsync(url, timeout, waitUntil); }), new Type[] { typeof(string), typeof(int), typeof(WaitUntilNavigation[]) }, typeof(Task<Response>)) },
                { "GoToAsyncWaitUntilNavigation", new PlaybackActionMeta(PlaybackActionEnum.GoToAsyncWaitUntilNavigation, new Func<Page, string, WaitUntilNavigation[], Task<Response>>(async (Page page, string url, WaitUntilNavigation[] waitUntil) => { return await page.GoToAsync(url, null, waitUntil); }), new Type[] { typeof(string), null, typeof(WaitUntilNavigation[]) }, typeof(Task<Response>)) },
                { "HoverAsync", new PlaybackActionMeta(PlaybackActionEnum.HoverAsync, new Action<Page, string>(async (Page page, string selector) => { await page.HoverAsync(selector); }), new Type[] { typeof(string) }, typeof(Task)) },
                { "MetricsAsync", new PlaybackActionMeta(PlaybackActionEnum.MetricsAsync, new Func<Page, Task<Dictionary<string, decimal>>>(async (Page page) => { return await page.MetricsAsync(); }), new Type[] { }, typeof(Task<Dictionary<string, decimal>>)) },
                { "PdfAsync", new PlaybackActionMeta(PlaybackActionEnum.PdfAsync, new Action<Page, string>(async (Page page, string file) => { await page.PdfAsync(file); }), new Type[] { typeof(string) }, typeof(Task)) },
                { "PdfAsyncPdfOptions", new PlaybackActionMeta(PlaybackActionEnum.PdfAsyncPdfOptions, new Action<Page, string, PdfOptions>(async (Page page, string file, PdfOptions options) => { await page.PdfAsync(file, options); }), new Type[] { typeof(string), typeof(PdfOptions) }, typeof(Task)) },
                { "PdfDataAsync", new PlaybackActionMeta(PlaybackActionEnum.PdfDataAsync, new Func<Page, Task<byte[]>>(async (Page page) => { return await page.PdfDataAsync(); }), new Type[] { }, typeof(Task<byte[]>)) },
                { "PdfDataAsyncPdfOptions", new PlaybackActionMeta(PlaybackActionEnum.PdfDataAsyncPdfOptions, new Func<Page, PdfOptions, Task<byte[]>>(async (Page page, PdfOptions options) => { return await page.PdfDataAsync(options); }), new Type[] { typeof(PdfOptions) }, typeof(Task<byte[]>)) },
                { "PdfStreamAsync", new PlaybackActionMeta(PlaybackActionEnum.PdfStreamAsync, new Func<Page, Task<Stream>>(async (Page page) => { return await page.PdfStreamAsync(); }), new Type[] { }, typeof(Task<Stream>)) },
                { "PdfStreamAsyncPdfOptions", new PlaybackActionMeta(PlaybackActionEnum.PdfStreamAsyncPdfOptions, new Func<Page, PdfOptions, Task<Stream>>(async (Page page, PdfOptions options) => { return await page.PdfStreamAsync(options); }), new Type[] { typeof(PdfOptions) }, typeof(Task<Stream>)) },
                { "QueryObjectsAsync", new PlaybackActionMeta(PlaybackActionEnum.QueryObjectsAsync, new Func<Page, JSHandle, Task<JSHandle>>(async (Page page, JSHandle prototypeHandle) => { return await page.QueryObjectsAsync(prototypeHandle); }), new Type[] { typeof(JSHandle) }, typeof(Task<JSHandle>)) },
                { "QuerySelectorAllAsync", new PlaybackActionMeta(PlaybackActionEnum.QuerySelectorAllAsync, new Func<Page, string, Task<ElementHandle[]>>(async (Page page, string selector) => { return await page.QuerySelectorAllAsync(selector); }), new Type[] { typeof(string) }, typeof(Task<ElementHandle[]>)) },
                { "QuerySelectorAllHandleAsync", new PlaybackActionMeta(PlaybackActionEnum.QuerySelectorAllHandleAsync, new Func<Page, string, Task<JSHandle>>(async (Page page, string selector) => { return await page.QuerySelectorAllHandleAsync(selector); }), new Type[] { typeof(string) }, typeof(Task<JSHandle>)) },
                { "QuerySelectorAsync", new PlaybackActionMeta(PlaybackActionEnum.QuerySelectorAsync, new Func<Page, string, Task<ElementHandle>>(async (Page page, string selector) => { return await page.QuerySelectorAsync(selector); }), new Type[] { typeof(string) }, typeof(Task<ElementHandle>)) },
                { "ReloadAsync", new PlaybackActionMeta(PlaybackActionEnum.ReloadAsync, new Func<Page, NavigationOptions, Task<Response>>(async (Page page, NavigationOptions options) => { return await page.ReloadAsync(options); }), new Type[] { typeof(NavigationOptions) }, typeof(Task<Response>)) },
                { "ReloadAsyncTimeoutWaitUntilNavigation", new PlaybackActionMeta(PlaybackActionEnum.ReloadAsyncTimeoutWaitUntilNavigation, new Func<Page, int, WaitUntilNavigation[], Task<Response>>(async (Page page, int timeout, WaitUntilNavigation[] waitUntil) => { return await page.ReloadAsync(timeout, waitUntil); }), new Type[] { typeof(int), typeof(WaitUntilNavigation[]) }, typeof(Task<Response>)) },
                { "ScreenshotAsync", new PlaybackActionMeta(PlaybackActionEnum.ScreenshotAsync, new Action<Page, string>(async (Page page, string file) => { await page.ScreenshotAsync(file); }), new Type[] { typeof(string) }, typeof(Task)) },
                { "ScreenshotAsyncScreenshotOptions", new PlaybackActionMeta(PlaybackActionEnum.ScreenshotAsyncScreenshotOptions, new Action<Page, string, ScreenshotOptions>(async (Page page, string file, ScreenshotOptions options) => { await page.ScreenshotAsync(file, options); }), new Type[] { typeof(string), typeof(ScreenshotOptions) }, typeof(Task)) },
                { "ScreenshotBase64Async", new PlaybackActionMeta(PlaybackActionEnum.ScreenshotBase64Async, new Func<Page, Task<string>>(async (Page page) => { return await page.ScreenshotBase64Async(); }), new Type[] { }, typeof(Task<string>)) },
                { "ScreenshotBase64AsyncScreenshotOptions", new PlaybackActionMeta(PlaybackActionEnum.ScreenshotBase64AsyncScreenshotOptions, new Func<Page, ScreenshotOptions, Task<string>>(async (Page page, ScreenshotOptions options) => { return await page.ScreenshotBase64Async(options); }), new Type[] { typeof(ScreenshotOptions) }, typeof(Task<string>)) },
                { "ScreenshotDataAsync", new PlaybackActionMeta(PlaybackActionEnum.ScreenshotDataAsync, new Func<Page, Task<byte[]>>(async (Page page) => { return await page.ScreenshotDataAsync(); }), new Type[] { }, typeof(Task<byte[]>)) },
                { "ScreenshotDataAsyncScreenshotOptions", new PlaybackActionMeta(PlaybackActionEnum.ScreenshotDataAsyncScreenshotOptions, new Func<Page, ScreenshotOptions, Task<byte[]>>(async (Page page, ScreenshotOptions options) => { return await page.ScreenshotDataAsync(options); }), new Type[] { typeof(ScreenshotOptions) }, typeof(Task<byte[]>)) },
                { "ScreenshotStreamAsync", new PlaybackActionMeta(PlaybackActionEnum.ScreenshotStreamAsync, new Func<Page, Task<Stream>>(async (Page page) => { return await page.ScreenshotStreamAsync(); }), new Type[] { }, typeof(Task<Stream>)) },
                { "ScreenshotStreamAsyncScreenshotOptions", new PlaybackActionMeta(PlaybackActionEnum.ScreenshotStreamAsyncScreenshotOptions, new Func<Page, ScreenshotOptions, Task<Stream>>(async (Page page, ScreenshotOptions options) => { return await page.ScreenshotStreamAsync(options); }), new Type[] { typeof(ScreenshotOptions) }, typeof(Task<Stream>)) },
                { "SelectAsync", new PlaybackActionMeta(PlaybackActionEnum.SelectAsync, new Func<Page, string, string[], Task<string[]>>(async (Page page, string selector, string[] values) => { return await page.SelectAsync(selector, values); }), new Type[] { typeof(string), typeof(string[]) }, typeof(Task<string[]>)) },
                { "SetBurstModeOffAsync", new PlaybackActionMeta(PlaybackActionEnum.SetBurstModeOffAsync, new Action<Page>(async (Page page) => { await page.SetBurstModeOffAsync(); }), new Type[] { }, typeof(Task)) },
                { "SetBypassCSPAsync", new PlaybackActionMeta(PlaybackActionEnum.SetBypassCSPAsync, new Action<Page, bool>(async (Page page, bool enabled) => { await page.SetBypassCSPAsync(enabled); }), new Type[] { typeof(bool) }, typeof(Task)) },
                { "SetCacheEnabledAsync", new PlaybackActionMeta(PlaybackActionEnum.SetCacheEnabledAsync, new Action<Page, bool>(async (Page page, bool enabled) => { await page.SetCacheEnabledAsync(enabled); }), new Type[] { typeof(bool) }, typeof(Task)) },
                { "SetContentAsync", new PlaybackActionMeta(PlaybackActionEnum.SetContentAsync, new Action<Page, string, NavigationOptions>(async (Page page, string html, NavigationOptions options) => { await page.SetContentAsync(html, options); }), new Type[] { typeof(string), typeof(NavigationOptions) }, typeof(Task)) },
                { "SetCookieAsync", new PlaybackActionMeta(PlaybackActionEnum.SetCookieAsync, new Action<Page, CookieParam[]>(async (Page page, CookieParam[] cookies) => { await page.SetCookieAsync(cookies); }), new Type[] { typeof(CookieParam[]) }, typeof(Task)) },
                { "SetExtraHttpHeadersAsync", new PlaybackActionMeta(PlaybackActionEnum.SetExtraHttpHeadersAsync, new Action<Page, Dictionary<string, string>>(async (Page page, Dictionary<string, string> headers) => { await page.SetExtraHttpHeadersAsync(headers); }), new Type[] { typeof(Dictionary<string, string>) }, typeof(Task)) },
                { "SetGeolocationAsync", new PlaybackActionMeta(PlaybackActionEnum.SetGeolocationAsync, new Action<Page, GeolocationOption>(async (Page page, GeolocationOption options) => { await page.SetGeolocationAsync(options); }), new Type[] { typeof(GeolocationOption) }, typeof(Task)) },
                { "SetJavaScriptEnabledAsync", new PlaybackActionMeta(PlaybackActionEnum.SetJavaScriptEnabledAsync, new Action<Page, bool>(async (Page page, bool enabled) => { await page.SetJavaScriptEnabledAsync(enabled); }), new Type[] { typeof(bool) }, typeof(Task)) },
                { "SetOfflineModeAsync", new PlaybackActionMeta(PlaybackActionEnum.SetOfflineModeAsync, new Action<Page, bool>(async (Page page, bool value) => { await page.SetOfflineModeAsync(value); }), new Type[] { typeof(bool) }, typeof(Task)) },
                { "SetRequestInterceptionAsync", new PlaybackActionMeta(PlaybackActionEnum.SetRequestInterceptionAsync, new Action<Page, bool>(async (Page page, bool value) => { await page.SetRequestInterceptionAsync(value); }), new Type[] { typeof(bool) }, typeof(Task)) },
                { "SetUserAgentAsync", new PlaybackActionMeta(PlaybackActionEnum.SetUserAgentAsync, new Action<Page, string>(async (Page page, string userAgent) => { await page.SetUserAgentAsync(userAgent); }), new Type[] { typeof(string) }, typeof(Task)) },
                { "SetViewportAsync", new PlaybackActionMeta(PlaybackActionEnum.SetViewportAsync, new Action<Page, ViewPortOptions>(async (Page page, ViewPortOptions viewport) => { await page.SetViewportAsync(viewport); }), new Type[] { typeof(ViewPortOptions) }, typeof(Task)) },
                { "TapAsync", new PlaybackActionMeta(PlaybackActionEnum.TapAsync, new Action<Page, string>(async (Page page, string selector) => { await page.TapAsync(selector); }), new Type[] { typeof(string) }, typeof(Task)) },
                { "TypeAsync", new PlaybackActionMeta(PlaybackActionEnum.TypeAsync, new Action<Page, string, string, TypeOptions>(async (Page page, string selector, string text, TypeOptions options) => { await page.TypeAsync(selector, text, options); }), new Type[] { typeof(string), typeof(string), typeof(TypeOptions) }, typeof(Task)) },
                { "WaitForExpressionAsync", new PlaybackActionMeta(PlaybackActionEnum.WaitForExpressionAsync, new Func<Page, string, WaitForFunctionOptions, Task<JSHandle>>(async (Page page, string script, WaitForFunctionOptions options) => { return await page.WaitForExpressionAsync(script, options); }), new Type[] { typeof(string), typeof(WaitForFunctionOptions) }, typeof(Task<JSHandle>)) },
                { "WaitForFileChooserAsync", new PlaybackActionMeta(PlaybackActionEnum.WaitForFileChooserAsync, new Func<Page, WaitForFileChooserOptions, Task<FileChooser>>(async (Page page, WaitForFileChooserOptions options) => { return await page.WaitForFileChooserAsync(options); }), new Type[] { typeof(WaitForFileChooserOptions) }, typeof(Task<FileChooser>)) },
                { "WaitForFunctionAsync", new PlaybackActionMeta(PlaybackActionEnum.WaitForFunctionAsync, new Func<Page, string, object[], Task<JSHandle>>(async (Page page, string script, object[] args) => { return await page.WaitForFunctionAsync(script, args); }), new Type[] { typeof(string), typeof(object[]) }, typeof(Task<JSHandle>)) },
                { "WaitForFunctionAsyncWaitForFunctionOptions", new PlaybackActionMeta(PlaybackActionEnum.WaitForFunctionAsyncWaitForFunctionOptions, new Func<Page, string, WaitForFunctionOptions, object[], Task<JSHandle>>(async (Page page, string script, WaitForFunctionOptions options, object[] args) => { return await page.WaitForFunctionAsync(script, options, args); }), new Type[] { typeof(string), typeof(WaitForFunctionOptions), typeof(object[]) }, typeof(Task<JSHandle>)) },
                { "WaitForNavigationAsync", new PlaybackActionMeta(PlaybackActionEnum.WaitForNavigationAsync, new Func<Page, NavigationOptions, Task<Response>>(async (Page page, NavigationOptions options) => { return await page.WaitForNavigationAsync(options); }), new Type[] { typeof(NavigationOptions) }, typeof(Task<Response>)) },
                { "WaitForRequestAsync", new PlaybackActionMeta(PlaybackActionEnum.WaitForRequestAsync, new Func<Page, string, WaitForOptions, Task<Request>>(async (Page page, string url, WaitForOptions options) => { return await page.WaitForRequestAsync(url, options); }), new Type[] { typeof(string), typeof(WaitForOptions) }, typeof(Task<Request>)) },
                { "WaitForRequestAsyncPredicate", new PlaybackActionMeta(PlaybackActionEnum.WaitForRequestAsyncPredicate, new Func<Page, Func<Request, bool>, WaitForOptions, Task<Request>>(async (Page page, Func<Request, bool> predicate, WaitForOptions options) => { return await page.WaitForRequestAsync(predicate, options); }), new Type[] { typeof(Func<Request, bool>), typeof(WaitForOptions) }, typeof(Task<Request>)) },
                { "WaitForResponseAsync", new PlaybackActionMeta(PlaybackActionEnum.WaitForResponseAsync, new Func<Page, string, WaitForOptions, Task<Response>>(async (Page page, string url, WaitForOptions options) => { return await page.WaitForResponseAsync(url, options); }), new Type[] { typeof(string), typeof(WaitForOptions) }, typeof(Task<Response>)) },
                { "WaitForResponseAsyncPredicate", new PlaybackActionMeta(PlaybackActionEnum.WaitForResponseAsyncPredicate, new Func<Page, Func<Response, bool>, WaitForOptions, Task<Response>>(async (Page page, Func<Response, bool> predicate, WaitForOptions options) => { return await page.WaitForResponseAsync(predicate, options); }), new Type[] { typeof(Func<Response, bool>), typeof(WaitForOptions) }, typeof(Task<Response>)) },
                { "WaitForSelectorAsync", new PlaybackActionMeta(PlaybackActionEnum.WaitForSelectorAsync, new Func<Page, string, WaitForSelectorOptions, Task<ElementHandle>>(async (Page page, string selector, WaitForSelectorOptions options) => { return await page.WaitForSelectorAsync(selector, options); }), new Type[] { typeof(string), typeof(WaitForSelectorOptions) }, typeof(Task<ElementHandle>)) },
                { "WaitForTimeoutAsync", new PlaybackActionMeta(PlaybackActionEnum.WaitForTimeoutAsync, new Action<Page, int>(async (Page page, int milliseconds) => { await page.WaitForTimeoutAsync(milliseconds); }), new Type[] { typeof(int) }, typeof(Task)) },
                { "WaitForXPathAsync", new PlaybackActionMeta(PlaybackActionEnum.WaitForXPathAsync, new Func<Page, string, WaitForSelectorOptions, Task<ElementHandle>>(async (Page page, string xpath, WaitForSelectorOptions options) => { return await page.WaitForXPathAsync(xpath, options); }), new Type[] { typeof(string), typeof(WaitForSelectorOptions) }, typeof(Task<ElementHandle>)) },
                { "XPathAsync", new PlaybackActionMeta(PlaybackActionEnum.XPathAsync, new Func<Page, string, Task<ElementHandle[]>>(async (Page page, string expression) => { return await page.XPathAsync(expression); }), new Type[] { typeof(string) }, typeof(Task<ElementHandle[]>)) }
            };
    }
    public class PlaybackAction
    {
        public PlaybackActionMeta Metadata { get; }
        public PlaybackActionEnum Type { get; }
        public Delegate Method { get; }
        public object[] Arguments { get; }
        public object Return { get; }
        private void Parse(string json)
        {
            if(JObject.Parse(json).Count > 1)
            {

            }
        }
        public PlaybackAction(PlaybackActionMeta meta, string json)
        {
            Metadata = meta;
            Type = Metadata.Type;
            Method = Metadata.Method;
            Parse(json);
        }
    }
    public class PlaybackActionMeta
    {
        public PlaybackActionEnum Type { get; }
        public Delegate Method { get; }
        public Type[] ArgumentTypes { get; }
        public Type ReturnType { get; }
        public PlaybackActionMeta(PlaybackActionEnum type, Delegate method, Type[] argumentTypes, Type returnType)
        {
            Type = type;
            Method = method;
            ArgumentTypes = argumentTypes;
            ReturnType = returnType;
        }
        public PlaybackActionMeta(PlaybackActionEnum type, Delegate method)
        {
            Type = type;
            Method = method;

            // Use reflection to get the argument types and return type.
            List<Type> argumentTypes = new List<Type>();
            foreach(var argument in Method.GetMethodInfo().GetParameters())
            {
                argumentTypes.Add(argument.ParameterType);
            }
            ArgumentTypes = argumentTypes.ToArray();

            ReturnType = Method.GetMethodInfo().ReturnType;
        }
    }

    public class PlaybackObject
    {
        public Uri Url { get; }
        public PlaybackType Type { get; }
        //public CrawlPlaybackAction[] Actions { get; }
        public PlaybackObject(string json)
        {

        }
    }

    public class CrawlPlaybackFile
    {
        private string _filePath { get; set; }
        private List<PlaybackObject> _crawlPlaybacks { get; set; }

        // Array of Playback objects containing:
            // Url
            // Type (e.g. login, normal traffic etc.)
            // Array of Map<selector, value>
            // Selectors can be xpath, or css selectors etc.

            // As the selector playback is being played, any cookies, session storage etc. generated should be
            // saved somewhere to keep, incase it needs to be injected into a subsequent browser.

        // Long term, we could have an adapter that takes a Selenium, Puppeteer, Playwright recording and
        // convert into an object of this type to be consumed by the crawler.
    }
}
