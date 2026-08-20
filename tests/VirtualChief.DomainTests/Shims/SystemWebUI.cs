// Minimal COMPILE-ONLY shims for the WebForms control surface so the legacy
// code-behind files (KisWebApp/**/*.aspx.cs, *.ascx.cs + their *.designer.cs)
// compile on .NET Core / Linux. These classes are NEVER executed by tests; they
// only give the C# compiler the type/member surface the generated code expects.
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Specialized;
using System.IO;
using System.Web.UI.WebControls;

namespace System.Web
{
    public class HttpApplication
    {
        public HttpContext Context => HttpContext.Current;
        public HttpRequest Request => HttpContext.Current?.Request;
        public HttpResponse Response => HttpContext.Current?.Response;
        public HttpServerUtility Server => HttpContext.Current?.Server;
        public HttpSessionState Session => HttpContext.Current?.Session;
        public event EventHandler BeginRequest;
        public event EventHandler EndRequest;
        public event EventHandler Error;
        public event EventHandler AuthenticateRequest;
        public event EventHandler PostAuthenticateRequest;
        public event EventHandler AuthorizeRequest;
        public event EventHandler PostAuthorizeRequest;
        public event EventHandler ResolveRequestCache;
        public event EventHandler PostResolveRequestCache;
        public event EventHandler MapRequestHandler;
        public event EventHandler PostMapRequestHandler;
        public event EventHandler AcquireRequestState;
        public event EventHandler PostAcquireRequestState;
        public event EventHandler PreRequestHandlerExecute;
        public event EventHandler PostRequestHandlerExecute;
        public event EventHandler ReleaseRequestState;
        public event EventHandler PostReleaseRequestState;
        public event EventHandler UpdateRequestCache;
        public event EventHandler PostUpdateRequestCache;
        public event EventHandler LogRequest;
        public event EventHandler PostLogRequest;
    }

    public class HttpPostedFile
    {
        public int ContentLength => 0;
        public string ContentType => "";
        public string FileName => "";
        public Stream InputStream => Stream.Null;
        public void SaveAs(string filename) { }
    }

    public class HttpCachePolicy
    {
        public HttpCacheVaryByParams VaryByParams => new();
        public HttpCacheVaryByHeaders VaryByHeaders => new();
        public void SetCacheability(HttpCacheability cacheability) { }
        public void SetExpires(DateTime date) { }
        public void SetMaxAge(TimeSpan delta) { }
        public void SetNoStore() { }
        public void SetNoServerCaching() { }
        public void SetSlidingExpiration(bool sliding) { }
        public void SetValidUntilExpires(bool validUntilExpires) { }
        public void SetOmitVaryStar(bool omit) { }
    }

    public class HttpCacheVaryByParams
    {
        public bool IgnoreParams { get; set; }
        public void Clear() { }
        public void SetParams(params string[] parameters) { }
    }

    public class HttpCacheVaryByHeaders
    {
        public bool VaryByUnspecifiedParameters { get; set; }
        public void Clear() { }
        public void SetHeaders(params string[] headers) { }
    }

    public enum HttpCacheability { NoCache, Private, Public, Server, ServerAndNoCache, ServerAndPrivate }

    public class HttpUtility
    {
        public static string HtmlEncode(string s) => System.Net.WebUtility.HtmlEncode(s);
        public static string HtmlDecode(string s) => System.Net.WebUtility.HtmlDecode(s);
        public static string UrlEncode(string s) => Uri.EscapeDataString(s ?? "");
        public static string UrlDecode(string s) => Uri.UnescapeDataString(s ?? "");
        public static string JavaScriptStringEncode(string s) => System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(s ?? ""));
        public static NameValueCollection ParseQueryString(string query) => new();
    }

    public class HttpContextBase
    {
        public virtual HttpRequestBase Request => null;
        public virtual HttpResponseBase Response => null;
        public virtual HttpServerUtilityBase Server => null;
        public virtual HttpSessionStateBase Session => null;
        public virtual IDictionary Items => new Hashtable();
        public virtual System.Security.Principal.IPrincipal User { get; set; }
    }

    public class HttpServerUtilityBase
    {
        public virtual string MapPath(string path) => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), path.TrimStart('~', '/'));
        public virtual string HtmlEncode(string s) => System.Net.WebUtility.HtmlEncode(s);
        public virtual string HtmlDecode(string s) => System.Net.WebUtility.HtmlDecode(s);
        public virtual string UrlEncode(string s) => Uri.EscapeDataString(s ?? "");
        public virtual string UrlDecode(string s) => Uri.UnescapeDataString(s ?? "");
        public virtual object GetLastError() => null;
    }

    public class HttpSessionStateBase
    {
        public virtual object this[string key] { get => null; set { } }
        public virtual int Count => 0;
        public virtual void Clear() { }
        public virtual void Abandon() { }
    }

    public class HttpBrowserCapabilities
    {
        public bool IsMobileDevice => false;
        public string Browser => "";
        public string Version => "";
        public string Platform => "";
        public string Type => "";
        public bool JavaScript => false;
        public bool Cookies => false;
    }

    public class HttpCachePolicyBase
    {
        public virtual void SetCacheability(HttpCacheability cacheability) { }
    }

}

namespace System.Web.UI
{
    public interface ITemplate
    {
        void InstantiateIn(Control container);
    }

    public interface IAttributeAccessor
    {
        string GetAttribute(string name);
        void SetAttribute(string name, string value);
    }

    public interface INamingContainer { }

    public interface IPostBackEventHandler
    {
        void RaisePostBackEvent(string eventArgument);
    }

    public interface IPostBackDataHandler
    {
        bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection);
        void RaisePostDataChangedEvent();
    }

    public interface IValidator
    {
        bool IsValid { get; set; }
        string ErrorMessage { get; set; }
        void Validate();
    }

    public class Control : IAttributeAccessor, INamingContainer
    {
        public Control() { ID = Guid.NewGuid().ToString("N"); }
        public virtual string ID { get; set; }
        public virtual string ClientID => ID;
        public virtual string UniqueID => ID;
        public virtual bool Visible { get; set; } = true;
        public virtual bool EnableViewState { get; set; } = true;
        public virtual Control Parent { get; set; }
        public virtual Page Page { get; set; }
        public virtual ControlCollection Controls => new(this);
        public virtual bool HasControls() => false;
        public virtual string SkinID { get; set; }
        public virtual bool EnableTheming { get; set; }
        public virtual string ClientIDMode { get; set; }
        public virtual object DataItem { get; set; }
        public virtual Control FindControl(string id) => null;
        public virtual void DataBind() { }
        public virtual void Dispose() { }
        public virtual void Focus() { }
        public virtual void RenderControl(System.Web.UI.HtmlTextWriter writer) { }
        public virtual void ApplyStyleSheetSkin(Page page) { }
        public virtual string GetAttribute(string name) => null;
        public virtual void SetAttribute(string name, string value) { }
        public event EventHandler DataBinding;
        public event EventHandler Disposed;
        public event EventHandler Init;
        public event EventHandler Load;
        public event EventHandler PreRender;
        public event EventHandler Unload;
        protected virtual void OnDataBinding(EventArgs e) { }
        protected virtual void OnLoad(EventArgs e) { }
        protected virtual void OnInit(EventArgs e) { }
        protected virtual void OnPreRender(EventArgs e) { }
        protected virtual void OnUnload(EventArgs e) { }
        protected virtual void TrackViewState() { }
        protected virtual object SaveViewState() => null;
        protected virtual void LoadViewState(object savedState) { }
        protected virtual StateBag ViewState { get; } = new();
        protected virtual System.Web.HttpContext Context => System.Web.HttpContext.Current;
        protected virtual HttpRequest Request => System.Web.HttpContext.Current?.Request;
        protected virtual HttpResponse Response => System.Web.HttpContext.Current?.Response;
        protected virtual HttpServerUtility Server => System.Web.HttpContext.Current?.Server;
        protected virtual System.Web.HttpApplication Application => new();
        protected virtual HttpSessionState Session => System.Web.HttpContext.Current?.Session;
        protected virtual System.Security.Principal.IPrincipal User => null;
        protected virtual System.Web.UI.StateBag ViewStateBag => ViewState;
        protected virtual bool IsPostBack => false;
        protected virtual string Title { get; set; }
        public virtual IDictionary Items => new Hashtable();
    }

    public class ControlCollection : IList, ICollection, IEnumerable
    {
        readonly List<Control> _list = new();
        readonly Control _owner;
        public ControlCollection(Control owner) { _owner = owner; }
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public Control this[int index] { get => _list[index]; set => _list[index] = value; }
        public Control this[string id] => _list.Find(c => c.ID == id);
        object IList.this[int index] { get => _list[index]; set => _list[index] = (Control)value; }
        bool IList.IsFixedSize => false;
        public void Add(Control child) { child.Page = _owner.Page; _list.Add(child); }
        public void AddAt(int index, Control child) { child.Page = _owner.Page; _list.Insert(index, child); }
        public void Clear() => _list.Clear();
        public bool Contains(Control c) => _list.Contains(c);
        public int IndexOf(Control c) => _list.IndexOf(c);
        public void Remove(Control c) => _list.Remove(c);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(Array array, int index) { ((ICollection)_list).CopyTo(array, index); }
        public int Add(object value) { Add((Control)value); return _list.Count - 1; }
        public bool Contains(object value) => _list.Contains((Control)value);
        public int IndexOf(object value) => _list.IndexOf((Control)value);
        public void Insert(int index, object value) => AddAt(index, (Control)value);
        public void Remove(object value) => Remove((Control)value);
    }

    public class StateBag : IDictionary, ICollection, IEnumerable
    {
        readonly Dictionary<string, object> _store = new(StringComparer.OrdinalIgnoreCase);
        public object this[string key] { get => _store.TryGetValue(key, out var v) ? v : null; set => _store[key] = value; }
        public object this[object key] { get => this[key as string]; set => this[key as string] = value; }
        public int Count => _store.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public bool IsFixedSize => false;
        public object SyncRoot => this;
        public ICollection Keys => _store.Keys;
        public ICollection Values => _store.Values;
        public void Add(object key, object value) => _store[key as string] = value;
        public void Clear() => _store.Clear();
        public bool Contains(object key) => _store.ContainsKey(key as string);
        public IDictionaryEnumerator GetEnumerator() => new System.Collections.Specialized.ListDictionary().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _store.GetEnumerator();
        public void Remove(object key) => _store.Remove(key as string);
        public void CopyTo(Array array, int index) { }
    }

    public class TemplateControl : Control
    {
        public object GetLocalResourceObject(string resourceKey) => null;
        public object GetLocalResourceObject(string resourceKey, Type objType, string propName) => null;
        public object GetGlobalResourceObject(string className, string resourceKey) => null;
        public Control LoadControl(string virtualPath) => new();
        public Control LoadControl(Type t, object[] parameters) => new();
        public virtual string MapPath(string virtualPath) => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), virtualPath.TrimStart('~', '/'));
    }

    public class Page : TemplateControl
    {
        public Page() { }
        public virtual bool IsPostBack => false;
        public virtual bool IsCallback => false;
        public virtual bool IsAsync => false;
        public virtual string Title { get; set; }
        public virtual HttpRequest Request => System.Web.HttpContext.Current?.Request;
        public virtual HttpResponse Response => System.Web.HttpContext.Current?.Response;
        public virtual HttpServerUtility Server => System.Web.HttpContext.Current?.Server;
        public virtual HttpSessionState Session => System.Web.HttpContext.Current?.Session;
        public virtual System.Web.HttpApplication Application => new();
        public virtual System.Security.Principal.IPrincipal User => null;
        public virtual System.Web.UI.ClientScriptManager ClientScript => new(this);
        public virtual NameValueCollection Form => System.Web.HttpContext.Current?.Request?.Form;
        public virtual NameValueCollection QueryString => System.Web.HttpContext.Current?.Request?.QueryString;
        public virtual HttpContext Context => System.Web.HttpContext.Current;
        public virtual IDictionary Items => new Hashtable();
        public virtual string AppRelativeVirtualPath { get; set; }
        public virtual string MapPath(string virtualPath) => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), virtualPath.TrimStart('~', '/'));
        public virtual void RegisterClientScriptBlock(Type type, string key, string script) { }
        public virtual void RegisterClientScriptBlock(Type type, string key, string script, bool addScriptTags) { }
        public virtual void RegisterClientScriptInclude(string key, string url) { }
        public virtual void RegisterStartupScript(Type type, string key, string script) { }
        public virtual void RegisterStartupScript(Type type, string key, string script, bool addScriptTags) { }
        public virtual void RegisterStartupScript(Type type, string key, string script, bool addScriptTags, bool addStartupScript) { }
        public virtual void RegisterHiddenField(string hiddenFieldName, string hiddenFieldInitialValue) { }
        public virtual void RegisterArrayDeclaration(string arrayName, string arrayValue) { }
        public virtual void RegisterRequiresPostBack(Control control) { }
        public virtual void RegisterRequiresViewStateEncryption() { }
        public virtual void Validate() { }
        public virtual bool IsValid => true;
        public virtual ValidatorCollection Validators => new();
        public virtual void SetFocus(Control control) { }
        public virtual void SetFocus(string clientID) { }
        public virtual bool SmartNavigation { get; set; }
        public virtual bool MaintainScrollPositionOnPostBack { get; set; }
        public virtual string ErrorPage { get; set; }
        public virtual string Theme { get; set; }
        public virtual string StyleSheetTheme { get; set; }
        public virtual MasterPage Master { get; set; }
        public virtual string MasterPageFile { get; set; }
        public virtual int PageStatePersister { get; set; }
        public virtual HtmlTextWriter Writer => new(System.IO.TextWriter.Null);
        public virtual string GetPostBackClientHyperlink(Control control, string argument) => "";
        public virtual string GetPostBackEventReference(Control control) => "";
        public virtual string GetPostBackEventReference(Control control, string argument) => "";
        public virtual string GetPostBackClientEvent(Control control, string argument) => "";
        public virtual string ResolveUrl(string relativeUrl) => relativeUrl;
        public virtual string ResolveClientUrl(string relativeUrl) => relativeUrl;
        public virtual bool IsCrossPagePostBack => false;
        public virtual bool EnableEventValidation { get; set; }
        public virtual bool EnableViewStateMac { get; set; }
        public virtual string ViewStateEncryptionMode { get; set; }
        public virtual string AspCompatMode { get; set; }
        public virtual bool Buffer { get; set; }
        public virtual string Culture { get; set; }
        public virtual string UICulture { get; set; }
        public virtual string CodePage { get; set; }
        public virtual string LCID { get; set; }
        public virtual System.Resources.ResourceManager GetLocalResourceObject(string resourceKey) => null;
        public virtual object GetGlobalResourceObject(string className, string resourceKey) => null;
        public virtual void Execute(string path) { }
        public virtual void Execute(string path, System.IO.TextWriter writer) { }
        public virtual void Transfer(string path) { }
        public virtual void Transfer(string path, bool preserveForm) { }
        public virtual void TransferRequest(string path) { }
        public virtual event EventHandler InitComplete;
        public virtual event EventHandler LoadComplete;
        public virtual event EventHandler PreInit;
        public virtual event EventHandler PreLoad;
        public virtual event EventHandler PreRenderComplete;
        public virtual event EventHandler SaveStateComplete;
        public virtual event EventHandler Error;
        protected virtual void OnPreInit(EventArgs e) { }
        protected virtual void OnPreLoad(EventArgs e) { }
        protected virtual void OnLoadComplete(EventArgs e) { }
        protected virtual void OnInitComplete(EventArgs e) { }
        protected virtual void OnPreRenderComplete(EventArgs e) { }
        protected virtual void OnSaveStateComplete(EventArgs e) { }
    }

    public class UserControl : TemplateControl, INamingContainer
    {
        public UserControl() { }
        public virtual HttpRequest Request => System.Web.HttpContext.Current?.Request;
        public virtual HttpResponse Response => System.Web.HttpContext.Current?.Response;
        public virtual HttpServerUtility Server => System.Web.HttpContext.Current?.Server;
        public virtual HttpSessionState Session => System.Web.HttpContext.Current?.Session;
        public virtual System.Web.HttpApplication Application => new();
        public virtual System.Security.Principal.IPrincipal User => null;
        public virtual bool IsPostBack => false;
        public virtual ClientScriptManager ClientScript => new(Page);
        public virtual void Focus() { }
        public virtual void RegisterClientScriptBlock(string key, string script) { }
        public virtual void RegisterStartupScript(string key, string script) { }
        public virtual void RegisterHiddenField(string hiddenFieldName, string hiddenFieldInitialValue) { }
        public virtual void RegisterArrayDeclaration(string arrayName, string arrayValue) { }
    }

    public class ClientScriptManager
    {
        readonly Page _page;
        public ClientScriptManager(Page page) { _page = page; }
        public void RegisterClientScriptBlock(Type type, string key, string script) { }
        public void RegisterClientScriptBlock(string key, string script) { }
        public void RegisterClientScriptBlock(Type type, string key, string script, bool addScriptTags) { }
        public void RegisterStartupScript(Type type, string key, string script) { }
        public void RegisterStartupScript(string key, string script) { }
        public void RegisterStartupScript(Type type, string key, string script, bool addScriptTags) { }
        public void RegisterClientScriptInclude(string key, string url) { }
        public void RegisterHiddenField(string hiddenFieldName, string hiddenFieldInitialValue) { }
        public void RegisterArrayDeclaration(string arrayName, string arrayValue) { }
        public string GetPostBackEventReference(Control control, string argument) => "";
        public string GetPostBackEventReference(Control control) => "";
        public string GetPostBackClientHyperlink(Control control, string argument) => "";
        public bool IsClientScriptBlockRegistered(string key) => false;
        public bool IsClientScriptIncludeRegistered(string key) => false;
        public bool IsStartupScriptRegistered(string key) => false;
        public void RegisterForEventValidation(string uniqueId, string argument) { }
        public string GetCallbackEventReference(Control control, string argument, string clientCallback, string context) => "";
        public string GetCallbackEventReference(Control control, string argument, string clientCallback, string context, string clientErrorCallback, bool useAsync) => "";
    }

    public class HtmlTextWriter : System.IO.TextWriter
    {
        readonly System.IO.TextWriter _inner;
        public HtmlTextWriter(System.IO.TextWriter writer) { _inner = writer; }
        public HtmlTextWriter(System.IO.TextWriter writer, string tabString) { _inner = writer; }
        public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
        public void AddAttribute(string name, string value) { }
        public void AddAttribute(HtmlTextWriterAttribute key, string value) { }
        public void AddStyleAttribute(string name, string value) { }
        public void AddStyleAttribute(HtmlTextWriterStyle key, string value) { }
        public void RenderBeginTag(HtmlTextWriterTag tag) { }
        public void RenderBeginTag(string tagName) { }
        public void RenderEndTag() { }
        public override void Write(string value) => _inner.Write(value);
        public override void Write(char value) => _inner.Write(value);
        public override void Write(char[] buffer, int index, int count) => _inner.Write(buffer, index, count);
        public void WriteAttribute(string name, string value) { }
        public void WriteAttribute(string name, string value, bool fEncode) { }
        public void WriteBeginTag(string tagName) { }
        public void WriteEndTag(string tagName) { }
        public void WriteFullBeginTag(string tagName) { }
        public void WriteLine(string s) { _inner.WriteLine(s); }
        public void WriteStyleAttribute(string name, string value) { }
        public void WriteEncodedText(string text) { }
        public void Flush() { }
    }

    public enum HtmlTextWriterAttribute { Accesskey, Cellpadding, Cellspacing, Class, Cols, Colspan, Disabled, Href, Id, Name, Rows, Size, Src, Style, Target, Title, Type, Value, Width }

    public enum HtmlTextWriterStyle { BackgroundColor, BorderStyle, BorderWidth, Color, Display, Height, Width, FontSize, FontFamily, FontWeight, TextAlign, Visibility, Position, Left, Top, ZIndex, Margin, Padding, Cursor }

    public enum HtmlTextWriterTag { A, Area, B, Body, Br, Button, Caption, Center, Col, Div, Em, Form, H1, H2, H3, H4, H5, H6, Head, Hr, Html, I, Img, Input, Label, Legend, Li, Link, Meta, Ol, Option, P, Script, Select, Span, Strong, Style, Table, Tbody, Td, Textarea, Th, Thead, Title, Tr, Ul, Xml, Unknown }

    public class ValidatorCollection : IList, ICollection, IEnumerable
    {
        readonly List<IValidator> _list = new();
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public IValidator this[int index] { get => _list[index]; set => _list[index] = value; }
        object IList.this[int index] { get => _list[index]; set => _list[index] = (IValidator)value; }
        bool IList.IsFixedSize => false;
        void IList.RemoveAt(int index) => _list.RemoveAt(index);
        public void Add(IValidator v) => _list.Add(v);
        public void Remove(IValidator v) => _list.Remove(v);
        public void Clear() => _list.Clear();
        public bool Contains(IValidator v) => _list.Contains(v);
        public int IndexOf(IValidator v) => _list.IndexOf(v);
        public void Insert(int index, IValidator v) => _list.Insert(index, v);
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(Array array, int index) { }
        public int Add(object value) { _list.Add((IValidator)value); return _list.Count - 1; }
        public bool Contains(object value) => _list.Contains((IValidator)value);
        public int IndexOf(object value) => _list.IndexOf((IValidator)value);
        public void Insert(int index, object value) => _list.Insert(index, (IValidator)value);
        public void Remove(object value) => _list.Remove((IValidator)value);
    }

    public class MasterPage : UserControl { }

    public class ContentPlaceHolder : Control { }

    public class UpdatePanel : Control
    {
        public UpdatePanel() { }
        public string UpdateMode { get; set; }
        public string RenderMode { get; set; }
        public bool ChildrenAsTriggers { get; set; }
        public UpdatePanelTriggerCollection Triggers => new();
        public event EventHandler<System.Web.UI.AsyncPostBackEventArgs> DataLoad;
        public void Update() { }
        public string ContentTemplate { get; set; }
    }

    public class UpdatePanelTriggerCollection : System.Collections.CollectionBase
    {
        public void Add(UpdatePanelTrigger trigger) { List.Add(trigger); }
        public void Remove(UpdatePanelTrigger trigger) { List.Remove(trigger); }
        public UpdatePanelTrigger this[int index] => (UpdatePanelTrigger)List[index];
    }

    public class UpdatePanelTrigger { }

    public class AsyncPostBackTrigger : UpdatePanelTrigger
    {
        public string ControlID { get; set; }
        public string EventName { get; set; }
    }

    public class PostBackTrigger : UpdatePanelTrigger
    {
        public string ControlID { get; set; }
    }

    public class UpdateProgress : Control
    {
        public UpdateProgress() { }
        public string ProgressTemplate { get; set; }
        public bool DisplayAfter { get; set; }
        public bool DynamicLayout { get; set; }
        public string AssociatedUpdatePanelID { get; set; }
    }

    public class Timer : Control
    {
        public Timer() { }
        public int Interval { get; set; }
        public bool Enabled { get; set; }
        public event EventHandler<EventArgs> Tick;
    }

    public class ScriptManager : Control
    {
        public ScriptManager() { }
        public bool EnablePartialRendering { get; set; }
        public bool EnablePageMethods { get; set; }
        public string ScriptPath { get; set; }
        public bool AsyncPostBackTimeout { get; set; }
        public CompositeScriptReferenceCollection CompositeScript => new();
        public ScriptReferenceCollection Scripts => new();
        public ServiceReferenceCollection Services => new();
        public static ScriptManager GetCurrent(Page page) => new();
        public static void RegisterClientScriptResource(Control control, Type type, string resourceName) { }
        public static void RegisterNamedClientScriptResource(Control control, string resourceName) { }
        public static void RegisterStartupScript(Control control, Type type, string key, string script, bool addScriptTags) { }
        public static void RegisterClientScriptBlock(Control control, Type type, string key, string script, bool addScriptTags) { }
        public static void RegisterClientScriptInclude(Control control, Type type, string key, string url) { }
        public static void RegisterDataForPostBack(Control control, string eventArgument) { }
        public static void RegisterExpandoAttribute(Control control, string controlId, string attributeName, string attributeValue, bool encode) { }
        public static void SetFocus(Control control) { }
        public event EventHandler<ScriptManagerAsyncPostBackErrorEventArgs> AsyncPostBackError;
        public event EventHandler<EventArgs> ResolveScriptReference;
        public void SetScriptMode(ScriptMode mode) { }
    }

    public class ScriptMode { public static readonly ScriptMode Release = new(); }

    public class ScriptManagerAsyncPostBackErrorEventArgs : EventArgs
    {
        public Exception Exception => null;
    }

    public class CompositeScriptReferenceCollection : System.Collections.CollectionBase
    {
        public void Add(CompositeScriptReference reference) { List.Add(reference); }
    }

    public class CompositeScriptReference { public string Path { get; set; } }

    public class ScriptReferenceCollection : System.Collections.CollectionBase
    {
        public void Add(ScriptReference reference) { List.Add(reference); }
        public void Clear() { List.Clear(); }
    }

    public class ScriptReference
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Assembly { get; set; }
        public bool NotifyScriptLoaded { get; set; }
    }

    public class ServiceReferenceCollection : System.Collections.CollectionBase
    {
        public void Add(ServiceReference reference) { List.Add(reference); }
    }

    public class ServiceReference { public string Path { get; set; } }

    public class AsyncPostBackEventArgs : EventArgs
    {
        public Exception Error => null;
    }

    public class DataSourceSelectArguments
    {
        public static readonly DataSourceSelectArguments Empty = new();
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SortExpression { get; set; }
        public string RetrieveTotalRowCount { get; set; }
        public bool RetrieveTotalRowCount2 { get; set; }
        public void AddSupportedCapabilities(DataSourceCapabilities capabilities) { }
    }

    public enum DataSourceCapabilities { None, Sort, Page, RetrieveTotalRowCount }

    public interface IDataSource
    {
        event EventHandler DataSourceChanged;
        DataSourceView GetView(string viewName);
        ICollection GetViewNames();
    }

    public class DataSourceView { }

    public class ClientIDMode { public static readonly string AutoID = "AutoID"; }

    public class ControlBuilder { }

    public class BaseParser { }
}

namespace System.Web.UI.WebControls
{
    public class WebControl : Control, IAttributeAccessor
    {
        public WebControl() { }
        public WebControl(HtmlTextWriterTag tag) { }
        public virtual string CssClass { get; set; }
        public virtual string AccessKey { get; set; }
        public virtual string ToolTip { get; set; }
        public virtual bool Enabled { get; set; } = true;
        public virtual int TabIndex { get; set; }
        public virtual Unit Width { get; set; }
        public virtual Unit Height { get; set; }
        public virtual Color BackColor { get; set; }
        public virtual Color ForeColor { get; set; }
        public virtual FontInfo Font => new();
        public virtual BorderStyle BorderStyle { get; set; }
        public virtual Unit BorderWidth { get; set; }
        public virtual Color BorderColor { get; set; }
        public virtual System.Web.UI.WebControls.AttributeCollection Attributes => new();
        public virtual StyleCollection ControlStyle => new();
        public virtual bool ApplyStyle(Style style) => false;
        public virtual bool MergeStyle(Style style) => false;
        public virtual void CopyBaseAttributes(WebControl control) { }
        public virtual string TagName => "";
        public virtual string GetAttribute(string name) => null;
        public virtual void SetAttribute(string name, string value) { }
        public virtual CssStyleCollection Style => new();
    }

    public class AttributeCollection
    {
        readonly Dictionary<string, string> _attrs = new(StringComparer.OrdinalIgnoreCase);
        public string this[string key] { get => _attrs.TryGetValue(key, out var v) ? v : null; set => _attrs[key] = value; }
        public int Count => _attrs.Count;
        public System.Collections.ICollection Keys => _attrs.Keys;
        public void Add(string key, string value) => _attrs[key] = value;
        public void Remove(string key) => _attrs.Remove(key);
        public void Clear() => _attrs.Clear();
        public string GetRenderName(string name) => name;
        public string CssStyle => "";
    }

    public class CssStyleCollection
    {
        public string this[string key] { get => null; set { } }
        public int Count => 0;
        public void Add(string key, string value) { }
        public void Remove(string key) { }
        public void Clear() { }
        public string CssText => "";
        public string Value { get; set; }
    }

    public class StyleCollection
    {
        public string this[string key] { get => null; set { } }
        public void Add(string key, string value) { }
        public void Remove(string key) { }
    }

    public class Style
    {
        public Style() { }
        public Style(HtmlTextWriterStyle[] s) { }
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
        public string CssClass { get; set; }
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public FontInfo Font => new();
        public BorderStyle BorderStyle { get; set; }
        public Unit BorderWidth { get; set; }
        public Color BorderColor { get; set; }
        public VerticalAlign VerticalAlign { get; set; }
        public HorizontalAlign HorizontalAlign { get; set; }
        public string FontSize { get; set; }
        public string FontFamily { get; set; }
        public string FontWeight { get; set; }
        public bool FontUnderline { get; set; }
        public bool FontItalic { get; set; }
        public bool FontBold { get; set; }
        public bool FontStrikeout { get; set; }
        public bool FontOverline { get; set; }
        public bool IsEmpty => false;
        public void CopyFrom(Style s) { }
        public void MergeWith(Style s) { }
        public void AddAttributesToRender(HtmlTextWriter writer, WebControl owner) { }
    }

    public class FontInfo
    {
        public string Name { get; set; }
        public string Names { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }
        public bool Overline { get; set; }
        public bool Strikeout { get; set; }
        public FontUnit Size { get; set; }
        public void CopyFrom(FontInfo f) { }
        public void MergeWith(FontInfo f) { }
    }

    public struct FontUnit
    {
        public static readonly FontUnit Empty = new();
        public static readonly FontUnit Small = new();
        public static readonly FontUnit Medium = new();
        public static readonly FontUnit Large = new();
        public static readonly FontUnit XLarge = new();
        public static readonly FontUnit Smaller = new();
        public static readonly FontUnit Larger = new();
        public double Value { get; set; }
        public UnitType Type { get; set; }
        public FontUnit(double value) { Value = value; Type = UnitType.Point; }
        public FontUnit(double value, UnitType type) { Value = value; Type = type; }
        public static FontUnit Point(double n) => new(n, UnitType.Point);
        public static FontUnit Pixel(double n) => new(n, UnitType.Pixel);
        public static FontUnit Em(double n) => new(n, UnitType.Em);
        public static FontUnit Percentage(double n) => new(n, UnitType.Percentage);
        public static FontUnit Parse(string s) => new();
        public bool IsEmpty => Value == 0;
        public override string ToString() => Value.ToString();
        public static bool operator ==(FontUnit a, FontUnit b) => a.Value == b.Value;
        public static bool operator !=(FontUnit a, FontUnit b) => !(a == b);
        public override bool Equals(object o) => o is FontUnit u && u.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
    }

    public struct Unit
    {
        public static readonly Unit Empty = new();
        public double Value { get; set; }
        public UnitType Type { get; set; }
        public Unit(double value) { Value = value; Type = UnitType.Pixel; }
        public Unit(double value, UnitType type) { Value = value; Type = type; }
        public Unit(string value) { Value = 0; Type = UnitType.Pixel; }
        public static Unit Pixel(double n) => new(n, UnitType.Pixel);
        public static Unit Point(double n) => new(n, UnitType.Point);
        public static Unit Percentage(double n) => new(n, UnitType.Percentage);
        public static Unit Parse(string s) => new();
        public static bool operator ==(Unit a, Unit b) => a.Value == b.Value;
        public static bool operator !=(Unit a, Unit b) => !(a == b);
        public override bool Equals(object o) => o is Unit u && u.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
        public bool IsEmpty => Value == 0;
        public static implicit operator Unit(int n) => new(n);
        public static implicit operator Unit(double n) => new(n);
        public override string ToString() => Value.ToString();
    }

    public enum UnitType { Pixel, Point, Percentage, Pica, Inch, Mm, Cm, Em, Ex }

    public enum VerticalAlign { NotSet, Top, Middle, Bottom, Baseline }

    public enum HorizontalAlign { NotSet, Left, Center, Right, Justify }

    public enum BorderStyle { NotSet, None, Dotted, Dashed, Solid, Double, Groove, Ridge, Inset, Outset }

    public class ListItem
    {
        public ListItem() { }
        public ListItem(string text) { Text = text; }
        public ListItem(string text, string value) { Text = text; Value = value; }
        public ListItem(string text, string value, bool enabled) { Text = text; Value = value; Enabled = enabled; }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Selected { get; set; }
        public string SelectedValue { get; set; }
        public override string ToString() => Text;
        public static ListItem FromString(string s) => new(s);
        public static implicit operator ListItem(string s) => new(s);
    }

    public class ListItemCollection : IList, ICollection, IEnumerable
    {
        readonly List<ListItem> _list = new();
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public ListItem this[int index] { get => _list[index]; set => _list[index] = value; }
        object IList.this[int index] { get => _list[index]; set => _list[index] = (ListItem)value; }
        bool IList.IsFixedSize => false;
        public void Add(ListItem item) => _list.Add(item);
        public void Add(string item) => _list.Add(new ListItem(item));
        public void AddRange(ListItem[] items) => _list.AddRange(items);
        public void Insert(int index, ListItem item) => _list.Insert(index, item);
        public void Clear() => _list.Clear();
        public bool Contains(ListItem item) => _list.Contains(item);
        public void Remove(ListItem item) => _list.Remove(item);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        public int IndexOf(ListItem item) => _list.IndexOf(item);
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(Array array, int index) { }
        public int Add(object value) { _list.Add((ListItem)value); return _list.Count - 1; }
        public bool Contains(object value) => _list.Contains((ListItem)value);
        public int IndexOf(object value) => _list.IndexOf((ListItem)value);
        public void Insert(int index, object value) => _list.Insert(index, (ListItem)value);
        public void Remove(object value) => _list.Remove((ListItem)value);
    }

    public abstract class BaseDataBoundControl : WebControl
    {
        public virtual object DataSource { get; set; }
        public virtual string DataSourceID { get; set; }
        public virtual string DataMember { get; set; }
        public virtual bool DataBind() { return true; }
    }

    public abstract class DataBoundControl : BaseDataBoundControl
    {
        public virtual string DataTextField { get; set; }
        public virtual string DataValueField { get; set; }
        public virtual string DataTextFormatString { get; set; }
        public virtual string DataValueFormatString { get; set; }
        public virtual string EmptyDataText { get; set; }
        public virtual ITemplate EmptyDataTemplate { get; set; }
        public virtual ITemplate HeaderTemplate { get; set; }
        public virtual ITemplate FooterTemplate { get; set; }
        public virtual bool SelectingEnabled { get; set; }
    }

    public abstract class ListControl : DataBoundControl, ITextControl
    {
        public virtual int SelectedIndex { get; set; }
        public virtual string SelectedValue { get; set; }
        public virtual ListItem SelectedItem => null;
        public virtual ListItemCollection Items => new();
        public virtual bool AutoPostBack { get; set; }
        public virtual bool AppendDataBoundItems { get; set; }
        public virtual string DataTextField2 { get; set; }
        public virtual int SelectedIndexChanged { get; set; }
        public virtual string Text { get; set; }
        public virtual void ClearSelection() { }
        public event EventHandler SelectedIndexChangedEvent;
        public event EventHandler TextChangedEvent;
        protected virtual void OnSelectedIndexChanged(EventArgs e) { }
        protected virtual void OnTextChanged(EventArgs e) { }
    }

    public interface ITextControl
    {
        string Text { get; set; }
    }

    public interface IEditableTextControl : ITextControl { }

    public interface IButtonControl
    {
        string Text { get; set; }
        string CommandName { get; set; }
        string CommandArgument { get; set; }
        string ValidationGroup { get; set; }
        bool CausesValidation { get; set; }
        string PostBackUrl { get; set; }
        bool UseSubmitBehavior { get; set; }
        bool CausesValidation2 { get; set; }
        event EventHandler Click;
        event CommandEventHandler Command;
    }

    public class Label : WebControl, ITextControl
    {
        public virtual string Text { get; set; }
        public virtual string AssociatedControlID { get; set; }
        public virtual string OnTextChanged { get; set; }
        public virtual string Text2 { get; set; }
    }

    public class Literal : WebControl, ITextControl
    {
        public virtual string Text { get; set; }
        public virtual string Mode { get; set; }
    }

    public class LiteralControl : Control
    {
        public LiteralControl() { }
        public LiteralControl(string text) { Text = text; }
        public virtual string Text { get; set; }
    }

    public class TextBox : WebControl, ITextControl, IEditableTextControl
    {
        public virtual string Text { get; set; }
        public virtual TextBoxMode TextMode { get; set; }
        public virtual int MaxLength { get; set; }
        public virtual int Columns { get; set; }
        public virtual int Rows { get; set; }
        public virtual bool ReadOnly { get; set; }
        public virtual bool AutoPostBack { get; set; }
        public virtual bool Wrap { get; set; }
        public virtual bool TextChanged { get; set; }
        public virtual string ValidationGroup { get; set; }
        public virtual string TextChanged2 { get; set; }
        public event EventHandler TextChangedEvent;
        public event EventHandler TextChanged2Event;
        protected virtual void OnTextChanged(EventArgs e) { }
        public virtual string ClientIDMode2 { get; set; }
    }

    public enum TextBoxMode { SingleLine, MultiLine, Password, Color, Date, DateTime, DateTimeLocal, Email, Month, Number, Range, Search, Tel, Time, Url, Week }

    public class Button : WebControl, IButtonControl, IPostBackEventHandler
    {
        public virtual string Text { get; set; }
        public virtual string CommandName { get; set; }
        public virtual string CommandArgument { get; set; }
        public virtual string ValidationGroup { get; set; }
        public virtual bool CausesValidation { get; set; } = true;
        public virtual string PostBackUrl { get; set; }
        public virtual bool UseSubmitBehavior { get; set; }
        public virtual bool CausesValidation2 { get; set; }
        public virtual string OnClientClick { get; set; }
        public virtual string AccessKey2 { get; set; }
        public virtual string ImageUrl { get; set; }
        public event EventHandler Click;
        public event CommandEventHandler Command;
        public void RaisePostBackEvent(string eventArgument) { }
        protected virtual void OnClick(EventArgs e) { }
        protected virtual void OnCommand(CommandEventArgs e) { }
        public virtual void ClickEvent() { }
    }

    public class LinkButton : WebControl, IButtonControl, IPostBackEventHandler
    {
        public virtual string Text { get; set; }
        public virtual string CommandName { get; set; }
        public virtual string CommandArgument { get; set; }
        public virtual string ValidationGroup { get; set; }
        public virtual bool CausesValidation { get; set; } = true;
        public virtual string PostBackUrl { get; set; }
        public virtual bool UseSubmitBehavior { get; set; }
        public virtual bool CausesValidation2 { get; set; }
        public virtual string OnClientClick { get; set; }
        public virtual string NavigateUrl { get; set; }
        public event EventHandler Click;
        public event CommandEventHandler Command;
        public void RaisePostBackEvent(string eventArgument) { }
        protected virtual void OnClick(EventArgs e) { }
        protected virtual void OnCommand(CommandEventArgs e) { }
    }

    public class ImageButton : WebControl, IButtonControl, IPostBackEventHandler
    {
        public virtual string ImageUrl { get; set; }
        public virtual string AlternateText { get; set; }
        public virtual string Text { get; set; }
        public virtual string CommandName { get; set; }
        public virtual string CommandArgument { get; set; }
        public virtual string ValidationGroup { get; set; }
        public virtual bool CausesValidation { get; set; } = true;
        public virtual string PostBackUrl { get; set; }
        public virtual bool UseSubmitBehavior { get; set; }
        public virtual bool CausesValidation2 { get; set; }
        public virtual string OnClientClick { get; set; }
        public virtual ImageAlign ImageAlign { get; set; }
        public event EventHandler Click;
        public event ImageClickEventHandler Click2;
        public event CommandEventHandler Command;
        event EventHandler IButtonControl.Click
        {
            add { Click += value; }
            remove { Click -= value; }
        }
        public void RaisePostBackEvent(string eventArgument) { }
        protected virtual void OnClick(ImageClickEventArgs e) { }
        protected virtual void OnCommand(CommandEventArgs e) { }
    }

    public enum ImageAlign { NotSet, Left, Right, Baseline, Top, Middle, Bottom, AbsBottom, AbsMiddle, TextTop }

    public class HyperLink : WebControl, ITextControl
    {
        public virtual string Text { get; set; }
        public virtual string NavigateUrl { get; set; }
        public virtual string Target { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string ToolTip2 { get; set; }
    }

    public class Image : WebControl
    {
        public virtual string ImageUrl { get; set; }
        public virtual string AlternateText { get; set; }
        public virtual ImageAlign ImageAlign { get; set; }
        public virtual string DescriptionUrl { get; set; }
    }

    public class HiddenField : Control, ITextControl
    {
        public virtual string Value { get; set; }
        public virtual string Text { get; set; }
        public virtual event EventHandler ValueChanged;
        protected virtual void OnValueChanged(EventArgs e) { }
    }

    public class CheckBox : WebControl, ITextControl, IPostBackDataHandler
    {
        public virtual bool Checked { get; set; }
        public virtual string Text { get; set; }
        public virtual string TextAlign { get; set; }
        public virtual bool AutoPostBack { get; set; }
        public virtual string ValidationGroup { get; set; }
        public virtual string InputAttributes { get; set; }
        public virtual string LabelAttributes { get; set; }
        public event EventHandler CheckedChanged;
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) => false;
        public void RaisePostDataChangedEvent() { }
        protected virtual void OnCheckedChanged(EventArgs e) { }
    }

    public class RadioButton : CheckBox { }

    public class RadioButtonList : ListControl
    {
        public virtual int RepeatColumns { get; set; }
        public virtual RepeatDirection RepeatDirection { get; set; }
        public virtual RepeatLayout RepeatLayout { get; set; }
        public virtual string TextAlign { get; set; }
        public virtual string CellPadding { get; set; }
        public virtual string CellSpacing { get; set; }
        public virtual string BorderColor { get; set; }
        public virtual string BorderStyle { get; set; }
        public virtual string BorderWidth { get; set; }
        public virtual string Font { get; set; }
        public virtual ListItemCollection Items2 => new();
    }

    public enum RepeatDirection { Horizontal, Vertical }

    public enum RepeatLayout { Table, Flow, OrderedList, UnorderedList }

    public class CheckBoxList : ListControl
    {
        public virtual int RepeatColumns { get; set; }
        public virtual RepeatDirection RepeatDirection { get; set; }
        public virtual RepeatLayout RepeatLayout { get; set; }
        public virtual string TextAlign { get; set; }
        public virtual string CellPadding { get; set; }
        public virtual string CellSpacing { get; set; }
        public virtual string BorderColor { get; set; }
        public virtual ListItemCollection Items2 => new();
    }

    public class DropDownList : ListControl, IPostBackDataHandler
    {
        public virtual int SelectedIndex2 { get; set; }
        public virtual ListItemCollection Items2 => new();
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) => false;
        public void RaisePostDataChangedEvent() { }
    }

    public class ListBox : ListControl
    {
        public virtual int Rows { get; set; }
        public virtual ListSelectionMode SelectionMode { get; set; }
        public virtual int SelectedIndex2 { get; set; }
        public virtual ListItemCollection Items2 => new();
    }

    public enum ListSelectionMode { Single, Multiple }

    public class Repeater : Control, INamingContainer
    {
        public virtual object DataSource { get; set; }
        public virtual string DataSourceID { get; set; }
        public virtual string DataMember { get; set; }
        public virtual ITemplate HeaderTemplate { get; set; }
        public virtual ITemplate ItemTemplate { get; set; }
        public virtual ITemplate AlternatingItemTemplate { get; set; }
        public virtual ITemplate SeparatorTemplate { get; set; }
        public virtual ITemplate FooterTemplate { get; set; }
        public virtual RepeaterItemCollection Items => new();
        
        public virtual bool DataBind() { return true; }
        public event RepeaterCommandEventHandler ItemCommand;
        public event RepeaterItemEventHandler ItemDataBound;
        public event RepeaterItemEventHandler ItemCreated;
        protected virtual void OnItemCommand(RepeaterCommandEventArgs e) { }
        protected virtual void OnItemDataBound(RepeaterItemEventArgs e) { }
        protected virtual void OnItemCreated(RepeaterItemEventArgs e) { }
        public virtual void DataBind2() { }
    }

    public class RepeaterItem : Control, INamingContainer
    {
        public RepeaterItem(int itemIndex, ListItemType itemType) { ItemIndex = itemIndex; ItemType = itemType; }
        public int ItemIndex { get; }
        public ListItemType ItemType { get; }
        public virtual object DataItem { get; set; }
        public virtual bool Display { get; set; }
    }

    public enum ListItemType { Header, Footer, Item, AlternatingItem, Separator, Pager, EditItem, SelectedItem, InsertItem }

    public class RepeaterItemCollection : IList, ICollection, IEnumerable
    {
        readonly List<RepeaterItem> _list = new();
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public RepeaterItem this[int index] { get => _list[index]; set => _list[index] = value; }
        object IList.this[int index] { get => _list[index]; set => _list[index] = (RepeaterItem)value; }
        bool IList.IsFixedSize => false;
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(Array array, int index) { }
        public int Add(object value) { _list.Add((RepeaterItem)value); return _list.Count - 1; }
        public bool Contains(object value) => _list.Contains((RepeaterItem)value);
        public int IndexOf(object value) => _list.IndexOf((RepeaterItem)value);
        public void Insert(int index, object value) => _list.Insert(index, (RepeaterItem)value);
        public void Remove(object value) => _list.Remove((RepeaterItem)value);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        public void Clear() => _list.Clear();
    }

    public class RepeaterItemEventArgs : EventArgs
    {
        public RepeaterItemEventArgs(RepeaterItem item) { Item = item; }
        public RepeaterItem Item { get; }
    }

    public class RepeaterCommandEventArgs : CommandEventArgs
    {
        public RepeaterCommandEventArgs(RepeaterItem item, object commandSource, CommandEventArgs originalArgs)
            : base(originalArgs.CommandName, originalArgs.CommandArgument) { Item = item; CommandSource = commandSource; }
        public RepeaterItem Item { get; }
        public object CommandSource { get; }
    }

    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(string commandName, object argument) { CommandName = commandName; CommandArgument = argument; }
        public string CommandName { get; }
        public object CommandArgument { get; }
    }

    public delegate void CommandEventHandler(object sender, CommandEventArgs e);

    public delegate void RepeaterCommandEventHandler(object source, RepeaterCommandEventArgs e);

    public delegate void RepeaterItemEventHandler(object sender, RepeaterItemEventArgs e);

    public delegate void ImageClickEventHandler(object sender, ImageClickEventArgs e);

    public class ImageClickEventArgs : EventArgs
    {
        public ImageClickEventArgs(int x, int y) { X = x; Y = y; }
        public ImageClickEventArgs(int x, int y, double xRaw, double yRaw) { X = x; Y = y; XRaw = xRaw; YRaw = yRaw; }
        public int X { get; }
        public int Y { get; }
        public double XRaw { get; }
        public double YRaw { get; }
    }

    public class DataGrid : WebControl, INamingContainer
    {
        public virtual object DataSource { get; set; }
        public virtual string DataSourceID { get; set; }
        public virtual bool AutoGenerateColumns { get; set; }
        public virtual DataGridColumnCollection Columns => new();
        public virtual int PageSize { get; set; }
        public virtual bool AllowPaging { get; set; }
        public virtual bool AllowSorting { get; set; }
        public virtual int CurrentPageIndex { get; set; }
        public virtual int PageCount { get; set; }
        public virtual bool ShowHeader { get; set; }
        public virtual bool ShowFooter { get; set; }
        public virtual string GridLines { get; set; }
        public virtual string CellPadding { get; set; }
        public virtual string CellSpacing { get; set; }
        public virtual DataKeyCollection DataKeys => new();
        public virtual string DataKeyField { get; set; }
        public virtual ITemplate HeaderTemplate { get; set; }
        public virtual ITemplate FooterTemplate { get; set; }
        public virtual ITemplate ItemTemplate { get; set; }
        public virtual ITemplate AlternatingItemTemplate { get; set; }
        public virtual ITemplate EditItemTemplate { get; set; }
        public virtual ITemplate SelectedItemTemplate { get; set; }
        public virtual ITemplate PagerTemplate { get; set; }
        
        public virtual bool DataBind() { return true; }
        public event DataGridCommandEventHandler ItemCommand;
        public event DataGridItemEventHandler ItemCreated;
        public event DataGridItemEventHandler ItemDataBound;
        public event DataGridPageChangedEventHandler PageIndexChanged;
        public event DataGridSortCommandEventHandler SortCommand;
        public event DataGridCommandEventHandler EditCommand;
        public event DataGridCommandEventHandler DeleteCommand;
        public event DataGridCommandEventHandler UpdateCommand;
        public event DataGridCommandEventHandler CancelCommand;
        protected virtual void OnItemCommand(DataGridCommandEventArgs e) { }
        protected virtual void OnItemCreated(DataGridItemEventArgs e) { }
        protected virtual void OnItemDataBound(DataGridItemEventArgs e) { }
        protected virtual void OnPageIndexChanged(DataGridPageChangedEventArgs e) { }
        protected virtual void OnSortCommand(DataGridSortCommandEventArgs e) { }
        protected virtual void OnEditCommand(DataGridCommandEventArgs e) { }
        protected virtual void OnDeleteCommand(DataGridCommandEventArgs e) { }
        protected virtual void OnUpdateCommand(DataGridCommandEventArgs e) { }
        protected virtual void OnCancelCommand(DataGridCommandEventArgs e) { }
    }

    public class DataGridColumnCollection : System.Collections.CollectionBase
    {
        public void Add(DataGridColumn column) { List.Add(column); }
        public void Clear() { List.Clear(); }
        public DataGridColumn this[int index] => (DataGridColumn)List[index];
    }

    public class DataGridColumn
    {
        public string HeaderText { get; set; }
        public string FooterText { get; set; }
        public string SortExpression { get; set; }
        public bool Visible { get; set; }
        public string HeaderImageUrl { get; set; }
        public string HeaderStyle { get; set; }
        public string ItemStyle { get; set; }
        public string FooterStyle { get; set; }
    }

    public class DataKeyCollection : System.Collections.CollectionBase
    {
        public object this[int index] => List[index];
        public int Count2 => List.Count;
    }

    public class DataGridItem : WebControl, INamingContainer
    {
        public DataGridItem(int itemIndex, int dataSetIndex, ListItemType itemType) { ItemIndex = itemIndex; DataSetIndex = dataSetIndex; ItemType = itemType; }
        public int ItemIndex { get; }
        public int DataSetIndex { get; }
        public ListItemType ItemType { get; }
        public virtual object DataItem { get; set; }
        public virtual TableCellCollection Cells => new();
    }

    public class DataGridItemEventArgs : EventArgs
    {
        public DataGridItemEventArgs(DataGridItem item) { Item = item; }
        public DataGridItem Item { get; }
    }

    public class DataGridCommandEventArgs : CommandEventArgs
    {
        public DataGridCommandEventArgs(DataGridItem item, object commandSource, CommandEventArgs originalArgs)
            : base(originalArgs.CommandName, originalArgs.CommandArgument) { Item = item; CommandSource = commandSource; }
        public DataGridItem Item { get; }
        public object CommandSource { get; }
    }

    public class DataGridPageChangedEventArgs : EventArgs
    {
        public DataGridPageChangedEventArgs(object source, int newPageIndex) { NewPageIndex = newPageIndex; }
        public int NewPageIndex { get; }
    }

    public class DataGridSortCommandEventArgs : EventArgs
    {
        public DataGridSortCommandEventArgs(object source, DataGridColumn column) { SortExpression = column.SortExpression; }
        public string SortExpression { get; }
    }

    public delegate void DataGridCommandEventHandler(object source, DataGridCommandEventArgs e);

    public delegate void DataGridItemEventHandler(object sender, DataGridItemEventArgs e);

    public delegate void DataGridPageChangedEventHandler(object source, DataGridPageChangedEventArgs e);

    public delegate void DataGridSortCommandEventHandler(object source, DataGridSortCommandEventArgs e);

    public class GridView : CompositeDataBoundControl
    {
        public virtual int PageIndex { get; set; }
        public virtual int PageSize { get; set; }
        public virtual bool AllowPaging { get; set; }
        public virtual bool AllowSorting { get; set; }
        public virtual int PageCount { get; set; }
        public virtual bool AutoGenerateColumns { get; set; }
        public virtual bool ShowHeader { get; set; }
        public virtual bool ShowFooter { get; set; }
        public virtual DataKeyArray DataKeys => new();
        public virtual string DataKeyNames { get; set; }
        public virtual GridViewColumnsCollection Columns => new();
        public virtual string EmptyDataText { get; set; }
        public virtual ITemplate EmptyDataTemplate { get; set; }
        public virtual ITemplate PagerTemplate { get; set; }
        public virtual string SortExpression { get; set; }
        public virtual SortDirection SortDirection { get; set; }
        public virtual string Caption { get; set; }
        public virtual event GridViewCommandEventHandler RowCommand;
        public virtual event GridViewRowEventHandler RowCreated;
        public virtual event GridViewRowEventHandler RowDataBound;
        public virtual event GridViewPageEventHandler PageIndexChanging;
        public virtual event GridViewSortEventHandler Sorting;
        public virtual event GridViewEditEventHandler RowEditing;
        public virtual event GridViewDeleteEventHandler RowDeleting;
        public virtual event GridViewUpdateEventHandler RowUpdating;
        public virtual event GridViewCancelEditEventHandler RowCancelingEdit;
        public virtual event GridViewSelectEventHandler SelectedIndexChanging;
        protected virtual void OnRowCommand(GridViewCommandEventArgs e) { }
        protected virtual void OnRowCreated(GridViewRowEventArgs e) { }
        protected virtual void OnRowDataBound(GridViewRowEventArgs e) { }
        protected virtual void OnPageIndexChanging(GridViewPageEventArgs e) { }
        protected virtual void OnSorting(GridViewSortEventArgs e) { }
        protected virtual void OnRowEditing(GridViewEditEventArgs e) { }
        protected virtual void OnRowDeleting(GridViewDeleteEventArgs e) { }
        protected virtual void OnRowUpdating(GridViewUpdateEventArgs e) { }
        protected virtual void OnRowCancelingEdit(GridViewCancelEditEventArgs e) { }
        protected virtual void OnSelectedIndexChanging(GridViewSelectEventArgs e) { }
    }

    public class CompositeDataBoundControl : DataBoundControl { }

    public class DataKeyArray : System.Collections.CollectionBase
    {
        public DataKey this[int index] => (DataKey)List[index];
    }

    public class DataKey
    {
        public object this[int index] => null;
        public object Value => null;
        public string Value2 { get; set; }
        public object[] Values => Array.Empty<object>();
    }

    public class GridViewColumnsCollection : System.Collections.CollectionBase
    {
        public void Add(DataControlField column) { List.Add(column); }
        public void Clear() { List.Clear(); }
        public DataControlField this[int index] => (DataControlField)List[index];
    }

    public abstract class DataControlField
    {
        public string HeaderText { get; set; }
        public string FooterText { get; set; }
        public bool Visible { get; set; }
        public string SortExpression { get; set; }
        public string HeaderImageUrl { get; set; }
        public string ItemStyle { get; set; }
        public string FooterStyle { get; set; }
        public string HeaderStyle { get; set; }
        public bool ShowHeader { get; set; }
        public bool InsertVisible { get; set; }
    }

    public class BoundField : DataControlField
    {
        public string DataField { get; set; }
        public string DataFormatString { get; set; }
        public bool ApplyFormatInEditMode { get; set; }
        public bool HtmlEncode { get; set; }
        public string ConvertEmptyStringToNull { get; set; }
        public string NullDisplayText { get; set; }
        public bool ReadOnly { get; set; }
    }

    public class TemplateField : DataControlField
    {
        public ITemplate ItemTemplate { get; set; }
        public ITemplate HeaderTemplate { get; set; }
        public ITemplate FooterTemplate { get; set; }
        public ITemplate EditItemTemplate { get; set; }
        public ITemplate InsertItemTemplate { get; set; }
        public ITemplate AlternatingItemTemplate { get; set; }
    }

    public class ButtonField : DataControlField
    {
        public string Text { get; set; }
        public string CommandName { get; set; }
        public string DataTextField { get; set; }
        public string DataTextFormatString { get; set; }
        public string ImageUrl { get; set; }
        public ButtonType ButtonType { get; set; }
        public bool CausesValidation { get; set; }
        public string ValidationGroup { get; set; }
    }

    public class CommandField : DataControlField
    {
        public bool ShowEditButton { get; set; }
        public bool ShowDeleteButton { get; set; }
        public bool ShowSelectButton { get; set; }
        public bool ShowInsertButton { get; set; }
        public bool ShowCancelButton { get; set; }
        public string EditText { get; set; }
        public string DeleteText { get; set; }
        public string SelectText { get; set; }
        public string InsertText { get; set; }
        public string CancelText { get; set; }
        public string UpdateText { get; set; }
        public ButtonType ButtonType { get; set; }
        public bool CausesValidation { get; set; }
        public string ValidationGroup { get; set; }
    }

    public class HyperLinkField : DataControlField
    {
        public string DataTextField { get; set; }
        public string DataTextFormatString { get; set; }
        public string DataNavigateUrlField { get; set; }
        public string DataNavigateUrlFormatString { get; set; }
        public string Text { get; set; }
        public string NavigateUrl { get; set; }
        public string Target { get; set; }
    }

    public class CheckBoxField : DataControlField
    {
        public string DataField { get; set; }
        public bool Text { get; set; }
        public bool ReadOnly { get; set; }
    }

    public class ImageField : DataControlField
    {
        public string DataImageUrlField { get; set; }
        public string DataImageUrlFormatString { get; set; }
        public string DataAlternateTextField { get; set; }
        public string DataAlternateTextFormatString { get; set; }
        public string AlternateText { get; set; }
        public string NullImageUrl { get; set; }
    }

    public class TextBoxField : DataControlField
    {
        public string DataField { get; set; }
        public string DataFormatString { get; set; }
    }

    public enum ButtonType { Button, Image, Link }

    public enum SortDirection { Ascending, Descending }

    public class GridViewRow : WebControl
    {
        public int RowIndex { get; }
        public int DataItemIndex { get; }
        public int RowType { get; set; }
        public object DataItem { get; set; }
        public TableCellCollection Cells => new();
    }

    public class GridViewRowEventArgs : EventArgs
    {
        public GridViewRowEventArgs(GridViewRow row) { Row = row; }
        public GridViewRow Row { get; }
    }

    public class GridViewCommandEventArgs : CommandEventArgs
    {
        public GridViewCommandEventArgs(GridViewRow row, object commandSource, CommandEventArgs originalArgs)
            : base(originalArgs.CommandName, originalArgs.CommandArgument) { Row = row; CommandSource = commandSource; }
        public GridViewRow Row { get; }
        public object CommandSource { get; }
    }

    public class GridViewPageEventArgs : EventArgs
    {
        public GridViewPageEventArgs(int newPageIndex) { NewPageIndex = newPageIndex; }
        public int NewPageIndex { get; set; }
    }

    public class GridViewSortEventArgs : EventArgs
    {
        public GridViewSortEventArgs(string sortExpression, SortDirection sortDirection) { SortExpression = sortExpression; SortDirection = sortDirection; }
        public string SortExpression { get; set; }
        public SortDirection SortDirection { get; set; }
    }

    public class GridViewEditEventArgs : EventArgs
    {
        public GridViewEditEventArgs(int newEditIndex) { NewEditIndex = newEditIndex; }
        public int NewEditIndex { get; set; }
    }

    public class GridViewDeleteEventArgs : EventArgs
    {
        public GridViewDeleteEventArgs(int rowIndex) { RowIndex = rowIndex; }
        public int RowIndex { get; set; }
    }

    public class GridViewUpdateEventArgs : EventArgs
    {
        public GridViewUpdateEventArgs(int rowIndex) { RowIndex = rowIndex; }
        public int RowIndex { get; set; }
    }

    public class GridViewCancelEditEventArgs : EventArgs
    {
        public GridViewCancelEditEventArgs(int rowIndex) { RowIndex = rowIndex; }
        public int RowIndex { get; set; }
    }

    public class GridViewSelectEventArgs : EventArgs
    {
        public GridViewSelectEventArgs(int newSelectedIndex) { NewSelectedIndex = newSelectedIndex; }
        public int NewSelectedIndex { get; set; }
    }

    public delegate void GridViewRowEventHandler(object sender, GridViewRowEventArgs e);

    public delegate void GridViewCommandEventHandler(object sender, GridViewCommandEventArgs e);

    public delegate void GridViewPageEventHandler(object sender, GridViewPageEventArgs e);

    public delegate void GridViewSortEventHandler(object sender, GridViewSortEventArgs e);

    public delegate void GridViewEditEventHandler(object sender, GridViewEditEventArgs e);

    public delegate void GridViewDeleteEventHandler(object sender, GridViewDeleteEventArgs e);

    public delegate void GridViewUpdateEventHandler(object sender, GridViewUpdateEventArgs e);

    public delegate void GridViewCancelEditEventHandler(object sender, GridViewCancelEditEventArgs e);

    public delegate void GridViewSelectEventHandler(object sender, GridViewSelectEventArgs e);

    public class Table : WebControl, INamingContainer
    {
        public virtual TableRowCollection Rows => new();
        public virtual string GridLines { get; set; }
        public virtual string CellPadding { get; set; }
        public virtual string CellSpacing { get; set; }
        public virtual string HorizontalAlign { get; set; }
        public virtual string BackImageUrl { get; set; }
        public virtual string Caption { get; set; }
        
        public event TableRowEventHandler RowCreated;
        public event TableRowEventHandler RowDataBound;
        protected virtual void OnRowCreated(TableRowEventArgs e) { }
        protected virtual void OnRowDataBound(TableRowEventArgs e) { }
        public virtual bool DataBind() { return true; }
    }

    public class TableRowCollection : IList, ICollection, IEnumerable
    {
        readonly List<TableRow> _list = new();
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public TableRow this[int index] { get => _list[index]; set => _list[index] = value; }
        object IList.this[int index] { get => _list[index]; set => _list[index] = (TableRow)value; }
        bool IList.IsFixedSize => false;
        public void Add(TableRow row) => _list.Add(row);
        public void AddAt(int index, TableRow row) => _list.Insert(index, row);
        public void Clear() => _list.Clear();
        public bool Contains(TableRow row) => _list.Contains(row);
        public void Remove(TableRow row) => _list.Remove(row);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(Array array, int index) { }
        public int Add(object value) { _list.Add((TableRow)value); return _list.Count - 1; }
        public bool Contains(object value) => _list.Contains((TableRow)value);
        public int IndexOf(object value) => _list.IndexOf((TableRow)value);
        public void Insert(int index, object value) => _list.Insert(index, (TableRow)value);
        public void Remove(object value) => _list.Remove((TableRow)value);
    }

    public class TableRow : WebControl
    {
        public virtual TableCellCollection Cells => new();
        public virtual HorizontalAlign HorizontalAlign { get; set; }
        public virtual VerticalAlign VerticalAlign { get; set; }
        public virtual string TableSection { get; set; }
    }

    public class TableCell : WebControl
    {
        public virtual string Text { get; set; }
        public virtual HorizontalAlign HorizontalAlign { get; set; }
        public virtual VerticalAlign VerticalAlign { get; set; }
        public virtual int ColumnSpan { get; set; }
        public virtual int RowSpan { get; set; }
        public virtual string Wrap { get; set; }
        public virtual string Nowrap { get; set; }
        public virtual string Width2 { get; set; }
        public virtual string Height2 { get; set; }
    }

    public class TableCellCollection : IList, ICollection, IEnumerable
    {
        readonly List<TableCell> _list = new();
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public TableCell this[int index] { get => _list[index]; set => _list[index] = value; }
        object IList.this[int index] { get => _list[index]; set => _list[index] = (TableCell)value; }
        bool IList.IsFixedSize => false;
        public void Add(TableCell cell) => _list.Add(cell);
        public void AddAt(int index, TableCell cell) => _list.Insert(index, cell);
        public void Clear() => _list.Clear();
        public bool Contains(TableCell cell) => _list.Contains(cell);
        public void Remove(TableCell cell) => _list.Remove(cell);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(Array array, int index) { }
        public int Add(object value) { _list.Add((TableCell)value); return _list.Count - 1; }
        public bool Contains(object value) => _list.Contains((TableCell)value);
        public int IndexOf(object value) => _list.IndexOf((TableCell)value);
        public void Insert(int index, object value) => _list.Insert(index, (TableCell)value);
        public void Remove(object value) => _list.Remove((TableCell)value);
    }

    public class TableRowEventArgs : EventArgs
    {
        public TableRowEventArgs(TableRow row) { Row = row; }
        public TableRow Row { get; }
    }

    public delegate void TableRowEventHandler(object sender, TableRowEventArgs e);

    public class Calendar : WebControl
    {
        public virtual DateTime SelectedDate { get; set; }
        public virtual SelectedDatesCollection SelectedDates => new();
        public virtual DateTime VisibleDate { get; set; }
        public virtual string FirstDayOfWeek { get; set; }
        public virtual string ShowGridLines { get; set; }
        public virtual string ShowTitle { get; set; }
        public virtual string ShowNextPrevMonth { get; set; }
        public virtual string TitleFormat { get; set; }
        public virtual string NextPrevFormat { get; set; }
        public virtual string CellPadding { get; set; }
        public virtual string CellSpacing { get; set; }
        public virtual string DayNameFormat { get; set; }
        public virtual string WeekendDayNameFormat { get; set; }
        public virtual DateTime TodaysDate { get; set; }
        public virtual string OtherMonthDayStyle { get; set; }
        public virtual string SelectedDayStyle { get; set; }
        public virtual string TodayDayStyle { get; set; }
        public virtual string DayHeaderStyle { get; set; }
        public virtual string TitleStyle { get; set; }
        public virtual string NextPrevStyle { get; set; }
        public virtual string WeekendStyle { get; set; }
        public virtual string SelectorStyle { get; set; }
        public virtual event EventHandler SelectionChanged;
        public virtual event MonthChangedEventHandler VisibleMonthChanged;
        public virtual event DayRenderEventHandler DayRender;
        protected virtual void OnSelectionChanged(EventArgs e) { }
        protected virtual void OnVisibleMonthChanged(MonthChangedEventArgs e) { }
        protected virtual void OnDayRender(DayRenderEventArgs e) { }
    }

    public class SelectedDatesCollection : System.Collections.ICollection
    {
        readonly List<DateTime> _dates = new();
        public DateTime this[int index] => _dates[index];
        public int Count => _dates.Count;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public void CopyTo(Array array, int index) { }
        public System.Collections.IEnumerator GetEnumerator() => _dates.GetEnumerator();
        public void Add(DateTime date) { _dates.Add(date); }
        public void Clear() { _dates.Clear(); }
        public bool Remove(DateTime date) => _dates.Remove(date);
        public void SelectRange(DateTime fromDate, DateTime toDate) { }
    }

    public class CalendarDay
    {
        public bool IsSelectable { get; set; }
        public bool IsSelected { get; set; }
        public bool IsToday { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsOtherMonth { get; set; }
        public DateTime Date { get; set; }
        public string DayNumberText { get; set; }
    }

    public class DayRenderEventArgs : EventArgs
    {
        public DayRenderEventArgs(TableCell cell, CalendarDay day) { Cell = cell; Day = day; }
        public TableCell Cell { get; }
        public CalendarDay Day { get; }
    }

    public class MonthChangedEventArgs : EventArgs
    {
        public MonthChangedEventArgs(DateTime newDate, DateTime previousDate) { NewDate = newDate; PreviousDate = previousDate; }
        public DateTime NewDate { get; }
        public DateTime PreviousDate { get; }
    }

    public delegate void MonthChangedEventHandler(object sender, MonthChangedEventArgs e);

    public delegate void DayRenderEventHandler(object sender, DayRenderEventArgs e);

    public class RequiredFieldValidator : BaseValidator { }

    public class RangeValidator : BaseValidator
    {
        public string MinimumValue { get; set; }
        public string MaximumValue { get; set; }
        public string Type { get; set; }
    }

    public class CompareValidator : BaseValidator
    {
        public string ControlToCompare { get; set; }
        public string ValueToCompare { get; set; }
        public string Operator { get; set; }
        public string Type { get; set; }
    }

    public class RegularExpressionValidator : BaseValidator
    {
        public string ValidationExpression { get; set; }
    }

    public class CustomValidator : BaseValidator
    {
        public string ClientValidationFunction { get; set; }
        public event ServerValidateEventHandler ServerValidate;
        protected virtual void OnServerValidate(string value) { }
    }

    public class ValidationSummary : WebControl
    {
        public string HeaderText { get; set; }
        public bool ShowSummary { get; set; }
        public bool ShowMessageBox { get; set; }
        public string DisplayMode { get; set; }
        public string ValidationGroup { get; set; }
    }

    public abstract class BaseValidator : Label, IValidator
    {
        public virtual string ControlToValidate { get; set; }
        public virtual string ErrorMessage { get; set; }
        public virtual string ValidationGroup { get; set; }
        public virtual string Text2 { get; set; }
        public virtual string Display { get; set; }
        public virtual bool EnableClientScript { get; set; }
        public virtual bool SetFocusOnError { get; set; }
        public virtual bool IsValid { get; set; } = true;
        public virtual event ServerValidateEventHandler ServerValidate;
        public virtual void Validate() { }
        protected virtual bool EvaluateIsValid() => true;
        protected virtual void OnServerValidate(string value) { }
    }

    public class ServerValidateEventArgs : EventArgs
    {
        public ServerValidateEventArgs(string value, bool isValid) { Value = value; IsValid = isValid; }
        public string Value { get; }
        public bool IsValid { get; set; }
    }

    public delegate void ServerValidateEventHandler(object source, ServerValidateEventArgs args);

    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public class Menu : WebControl
    {
        public virtual MenuItemCollection Items => new();
        public virtual string DataSource { get; set; }
        public virtual string DataSourceID { get; set; }
        public virtual string DataBind() { return ""; }
        public virtual Orientation Orientation { get; set; }
        public virtual string StaticDisplayLevels { get; set; }
        public virtual string MaximumDynamicDisplayLevels { get; set; }
        public virtual string Target { get; set; }
        public virtual string StaticMenuStyle { get; set; }
        public virtual string DynamicMenuStyle { get; set; }
        public virtual event MenuEventHandler MenuItemClick;
        protected virtual void OnMenuItemClick(MenuEventArgs e) { }
        public virtual void DataBind2() { }
    }

    public class MenuItem
    {
        public MenuItem() { }
        public MenuItem(string text) { Text = text; }
        public MenuItem(string text, string value) { Text = text; Value = value; }
        public MenuItem(string text, string value, string imageUrl) { Text = text; Value = value; ImageUrl = imageUrl; }
        public MenuItem(string text, string value, string imageUrl, string navigateUrl) { Text = text; Value = value; ImageUrl = imageUrl; NavigateUrl = navigateUrl; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string ImageUrl { get; set; }
        public string NavigateUrl { get; set; }
        public string Target { get; set; }
        public bool Selectable { get; set; }
        public bool Enabled { get; set; }
        public bool Selected { get; set; }
        public string ToolTip { get; set; }
        public MenuItemCollection ChildItems => new();
        public MenuItem Parent { get; set; }
    }

    public class MenuItemCollection : IList, ICollection, IEnumerable
    {
        readonly List<MenuItem> _list = new();
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public MenuItem this[int index] { get => _list[index]; set => _list[index] = value; }
        object IList.this[int index] { get => _list[index]; set => _list[index] = (MenuItem)value; }
        bool IList.IsFixedSize => false;
        public void Add(MenuItem item) => _list.Add(item);
        public void Add(string text) => _list.Add(new MenuItem(text));
        public void AddAt(int index, MenuItem item) => _list.Insert(index, item);
        public void Remove(MenuItem item) => _list.Remove(item);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        public void Clear() => _list.Clear();
        public bool Contains(MenuItem item) => _list.Contains(item);
        public int IndexOf(MenuItem item) => _list.IndexOf(item);
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(Array array, int index) { }
        public int Add(object value) { _list.Add((MenuItem)value); return _list.Count - 1; }
        public bool Contains(object value) => _list.Contains((MenuItem)value);
        public int IndexOf(object value) => _list.IndexOf((MenuItem)value);
        public void Insert(int index, object value) => _list.Insert(index, (MenuItem)value);
        public void Remove(object value) => _list.Remove((MenuItem)value);
    }

    public class MenuEventArgs : EventArgs
    {
        public MenuEventArgs(MenuItem item) { Item = item; }
        public MenuItem Item { get; }
    }

    public delegate void MenuEventHandler(object sender, MenuEventArgs e);

    public class FileUpload : WebControl
    {
        public virtual HttpPostedFile PostedFile => null;
        public virtual HttpFileCollection PostedFiles => null;
        public virtual bool HasFile => false;
        public virtual byte[] FileBytes => Array.Empty<byte>();
        public virtual string FileName => "";
        public virtual int FileContentLength { get; }
        public virtual Stream FileContent => Stream.Null;
        public virtual bool SaveAs(string filename) => false;
    }

    public class Panel : WebControl, INamingContainer
    {
        public virtual string BackImageUrl { get; set; }
        public virtual string Direction { get; set; }
        public virtual string GroupingText { get; set; }
        public virtual string ScrollBars { get; set; }
        public virtual string HorizontalAlign { get; set; }
        public virtual string Wrap { get; set; }
        public virtual string DefaultButton { get; set; }
    }

    public class PlaceHolder : Control, INamingContainer { }

    public class MultiView : Control
    {
        public virtual int ActiveViewIndex { get; set; }
        public virtual ViewCollection Views => new();
        public virtual event EventHandler ActiveViewChanged;
        protected virtual void OnActiveViewChanged(EventArgs e) { }
        public virtual void SetActiveView(View view) { }
    }

    public class ViewCollection : System.Collections.CollectionBase
    {
        public View this[int index] => (View)List[index];
        public void Add(View view) { List.Add(view); }
        public void Clear() { List.Clear(); }
    }

    public class View : Control { }

    public class Wizard : CompositeControl
    {
        public virtual WizardStepCollection WizardSteps => new();
        public virtual int ActiveStepIndex { get; set; }
        public virtual WizardStep ActiveStep => null;
        
        public virtual event WizardNavigationEventHandler ActiveStepChanged;
        public virtual event WizardNavigationEventHandler FinishButtonClick;
        public virtual event WizardNavigationEventHandler NextButtonClick;
        public virtual event WizardNavigationEventHandler PreviousButtonClick;
        public virtual event WizardNavigationEventHandler SideBarButtonClick;
        protected virtual void OnActiveStepChanged(WizardNavigationEventArgs e) { }
        protected virtual void OnFinishButtonClick(WizardNavigationEventArgs e) { }
        protected virtual void OnNextButtonClick(WizardNavigationEventArgs e) { }
        protected virtual void OnPreviousButtonClick(WizardNavigationEventArgs e) { }
        protected virtual void OnSideBarButtonClick(WizardNavigationEventArgs e) { }
    }

    public class CompositeControl : WebControl, INamingContainer { }

    public class WizardStepCollection : System.Collections.CollectionBase
    {
        public WizardStep this[int index] => (WizardStep)List[index];
        public void Add(WizardStep step) { List.Add(step); }
        public void Clear() { List.Clear(); }
    }

    public class WizardStep
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public bool AllowReturn { get; set; }
        public string StepType { get; set; }
    }

    public class WizardNavigationEventArgs : EventArgs
    {
        public WizardNavigationEventArgs(int currentStepIndex, int nextStepIndex) { CurrentStepIndex = currentStepIndex; NextStepIndex = nextStepIndex; }
        public int CurrentStepIndex { get; }
        public int NextStepIndex { get; }
        public bool Cancel { get; set; }
    }

    public delegate void WizardNavigationEventHandler(object sender, WizardNavigationEventArgs e);

    public class XmlDataSource : DataSourceControl
    {
        public virtual string DataFile { get; set; }
        public virtual string Data { get; set; }
        public virtual string XPath { get; set; }
        public virtual string TransformFile { get; set; }
        public virtual string Transform { get; set; }
        public virtual string TransformArgumentList { get; set; }
        public virtual System.Xml.XmlDocument Document { get; set; }
        public virtual string DataBind() { return ""; }
    }

    public class DataSourceControl : Control, IDataSource
    {
        public event EventHandler DataSourceChanged;
        public DataSourceView GetView(string viewName) => null;
        public ICollection GetViewNames() => Array.Empty<string>();
        public virtual void DataBind() { }
    }
}

namespace System.Web.UI.HtmlControls
{
    public class HtmlControl : Control, IAttributeAccessor
    {
        public HtmlControl() { }
        public HtmlControl(string tag) { TagName = tag; }
        public string TagName { get; set; }
        public string InnerText { get; set; }
        public string InnerHtml { get; set; }
        public virtual string GetAttribute(string name) => null;
        public virtual void SetAttribute(string name, string value) { }
        public virtual System.Web.UI.WebControls.AttributeCollection Attributes => new();
        public virtual CssStyleCollection Style => new();
    }

    public class HtmlContainerControl : HtmlControl
    {
        public HtmlContainerControl() { }
        public HtmlContainerControl(string tag) : base(tag) { }
        public string InnerText { get; set; }
        public string InnerHtml { get; set; }
    }

    public class HtmlGenericControl : HtmlContainerControl
    {
        public HtmlGenericControl() { }
        public HtmlGenericControl(string tag) : base(tag) { }
    }

    public class HtmlInputControl : HtmlControl
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class HtmlInputText : HtmlInputControl { }

    public class HtmlInputButton : HtmlInputControl
    {
        public event EventHandler ServerClick;
        public string CausesValidation { get; set; }
        public string ValidationGroup { get; set; }
        protected virtual void OnServerClick(EventArgs e) { }
    }

    public class HtmlInputSubmit : HtmlInputButton { }

    public class HtmlInputReset : HtmlInputButton { }

    public class HtmlInputImage : HtmlInputControl
    {
        public string Src { get; set; }
        public string Alt { get; set; }
        public string Align { get; set; }
        public event ImageClickEventHandler ServerClick;
        protected virtual void OnServerClick(ImageClickEventArgs e) { }
    }

    public class HtmlInputCheckBox : HtmlInputControl
    {
        public bool Checked { get; set; }
        public string OnServerChange { get; set; }
        public event EventHandler ServerChange;
    }

    public class HtmlInputRadioButton : HtmlInputControl
    {
        public bool Checked { get; set; }
        public string OnServerChange { get; set; }
        public event EventHandler ServerChange;
    }

    public class HtmlInputHidden : HtmlInputControl { }

    public class HtmlInputPassword : HtmlInputText { }

    public class HtmlInputFile : HtmlInputControl
    {
        public string Accept { get; set; }
        public int MaxLength { get; set; }
        public string Size { get; set; }
        public string PostedFile => null;
    }

    public class HtmlSelect : HtmlContainerControl
    {
        public ListItemCollection Items => new();
        public string SelectedIndex { get; set; }
        public string Value { get; set; }
        public string DataSource { get; set; }
        public string DataTextField { get; set; }
        public string DataValueField { get; set; }
        public string OnServerChange { get; set; }
        public event EventHandler ServerChange;
    }

    public class HtmlTextArea : HtmlContainerControl
    {
        public string Value { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public string OnServerChange { get; set; }
        public event EventHandler ServerChange;
    }

    public class HtmlAnchor : HtmlContainerControl
    {
        public string HRef { get; set; }
        public string Target { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public event EventHandler ServerClick;
        protected virtual void OnServerClick(EventArgs e) { }
    }

    public class HtmlForm : HtmlContainerControl
    {
        public string Action { get; set; }
        public string Method { get; set; }
        public string Enctype { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public string SubmitDisabledControls { get; set; }
        public string DefaultButton { get; set; }
    }

    public class HtmlButton : HtmlContainerControl
    {
        public string CausesValidation { get; set; }
        public string ValidationGroup { get; set; }
        public event EventHandler ServerClick;
        protected virtual void OnServerClick(EventArgs e) { }
    }

    public class HtmlImage : HtmlControl
    {
        public string Src { get; set; }
        public string Alt { get; set; }
        public string Align { get; set; }
        public int Border { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class HtmlLink : HtmlControl
    {
        public string Href { get; set; }
    }

    public class HtmlMeta : HtmlControl
    {
        public string Content { get; set; }
        public string HttpEquiv { get; set; }
        public string Name { get; set; }
    }

    public class HtmlTitle : HtmlControl
    {
        public string Text { get; set; }
    }

    public class HtmlTable : HtmlContainerControl
    {
        public HtmlTable() { }
        public string Border { get; set; }
        public string CellPadding { get; set; }
        public string CellSpacing { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Align { get; set; }
        public string BgColor { get; set; }
        public string BorderColor { get; set; }
        public string BackImageUrl { get; set; }
        public HtmlTableRowCollection Rows => new();
        public string InnerHtml2 { get; set; }
    }

    public class HtmlTableRowCollection : IList, ICollection, IEnumerable
    {
        readonly List<HtmlTableRow> _list = new();
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public HtmlTableRow this[int index] { get => _list[index]; set => _list[index] = value; }
        object IList.this[int index] { get => _list[index]; set => _list[index] = (HtmlTableRow)value; }
        bool IList.IsFixedSize => false;
        public void Add(HtmlTableRow row) => _list.Add(row);
        public void AddAt(int index, HtmlTableRow row) => _list.Insert(index, row);
        public void Clear() => _list.Clear();
        public bool Contains(HtmlTableRow row) => _list.Contains(row);
        public void Remove(HtmlTableRow row) => _list.Remove(row);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(Array array, int index) { }
        public int Add(object value) { _list.Add((HtmlTableRow)value); return _list.Count - 1; }
        public bool Contains(object value) => _list.Contains((HtmlTableRow)value);
        public int IndexOf(object value) => _list.IndexOf((HtmlTableRow)value);
        public void Insert(int index, object value) => _list.Insert(index, (HtmlTableRow)value);
        public void Remove(object value) => _list.Remove((HtmlTableRow)value);
    }

    public class HtmlTableRow : HtmlContainerControl
    {
        public HtmlTableRow() { }
        public string Align { get; set; }
        public string Valign { get; set; }
        public string BgColor { get; set; }
        public string BorderColor { get; set; }
        public string Height { get; set; }
        public HtmlTableCellCollection Cells => new();
        public string InnerHtml2 { get; set; }
    }

    public class HtmlTableCellCollection : IList, ICollection, IEnumerable
    {
        readonly List<HtmlTableCell> _list = new();
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public HtmlTableCell this[int index] { get => _list[index]; set => _list[index] = value; }
        object IList.this[int index] { get => _list[index]; set => _list[index] = (HtmlTableCell)value; }
        bool IList.IsFixedSize => false;
        public void Add(HtmlTableCell cell) => _list.Add(cell);
        public void AddAt(int index, HtmlTableCell cell) => _list.Insert(index, cell);
        public void Clear() => _list.Clear();
        public bool Contains(HtmlTableCell cell) => _list.Contains(cell);
        public void Remove(HtmlTableCell cell) => _list.Remove(cell);
        public void RemoveAt(int index) => _list.RemoveAt(index);
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(Array array, int index) { }
        public int Add(object value) { _list.Add((HtmlTableCell)value); return _list.Count - 1; }
        public bool Contains(object value) => _list.Contains((HtmlTableCell)value);
        public int IndexOf(object value) => _list.IndexOf((HtmlTableCell)value);
        public void Insert(int index, object value) => _list.Insert(index, (HtmlTableCell)value);
        public void Remove(object value) => _list.Remove((HtmlTableCell)value);
    }

    public class HtmlTableCell : HtmlContainerControl
    {
        public HtmlTableCell() { }
        public HtmlTableCell(string tagName) : base(tagName) { }
        public string Align { get; set; }
        public string Valign { get; set; }
        public string BgColor { get; set; }
        public string BorderColor { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int ColSpan { get; set; }
        public int RowSpan { get; set; }
        public bool NoWrap { get; set; }
        public string InnerHtml2 { get; set; }
    }

    public class HtmlHead : HtmlGenericControl
    {
        public HtmlHead() : base("head") { }
        public string Title { get; set; }
    }

    public class HtmlBody : HtmlGenericControl
    {
        public HtmlBody() : base("body") { }
    }
}

namespace System.Web.UI.DataVisualization.Charting
{
    public class Chart : WebControl
    {
        public Chart() { }
        public ChartAreaCollection ChartAreas => new();
        public SeriesCollection Series => new();
        public LegendCollection Legends => new();
        public TitleCollection Titles => new();
        public string Palette { get; set; }
        public string PaletteCustomColors { get; set; }
        public string ImageType { get; set; }
        public string BackColor { get; set; }
        public string BackGradientStyle { get; set; }
        public string BackSecondaryColor { get; set; }
        public string BorderlineColor { get; set; }
        public string BorderlineDashStyle { get; set; }
        public string BorderlineWidth { get; set; }
        public string AntiAliasing { get; set; }
        public string TextAntiAliasingQuality { get; set; }
        public string OnClick { get; set; }
        public string OnLoad { get; set; }
        public event EventHandler Click;
        public event EventHandler Load;
        public object DataSource { get; set; }
        public string DataSourceID { get; set; }
        public void DataBind() { }
        public void DataBindTable(System.Collections.IEnumerable dataSource) { }
        public void SaveImage(string imageFileName, ChartImageFormat format) { }
        public void SaveImage(System.IO.Stream imageStream, ChartImageFormat format) { }
        public void AlignDataPointsByAxisLabel() { }
        public void AlignDataPointsByAxisLabel(string seriesNames) { }
        public System.Drawing.Image GetImage() => null;
    }

    public class ChartArea
    {
        public ChartArea() { }
        public ChartArea(string name) { Name = name; }
        public string Name { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public string BackGradientStyle { get; set; }
        public System.Drawing.Color BackSecondaryColor { get; set; }
        public Axis AxisX => new();
        public Axis AxisY => new();
        public Axis AxisX2 => new();
        public Axis AxisY2 => new();
        public string AlignWithChartArea { get; set; }
        public string Position { get; set; }
        public string InnerPlotPosition { get; set; }
        public string ShadowColor { get; set; }
        public int ShadowOffset { get; set; }
        public System.Drawing.Color BorderColor { get; set; }
        public string BorderDashStyle { get; set; }
        public int BorderWidth { get; set; }
        public ChartAreaCollection AxisX2_ { get; set; }
        public string AxisType { get; set; }
    }

    public class ChartAreaCollection : System.Collections.CollectionBase
    {
        public ChartArea this[int index] => (ChartArea)List[index];
        public ChartArea this[string name] => null;
        public void Add(ChartArea area) { List.Add(area); }
        public void Add(string name) { List.Add(new ChartArea(name)); }
        public void Clear() { List.Clear(); }
        public void Remove(ChartArea area) { List.Remove(area); }
        public void RemoveAt(int index) { List.RemoveAt(index); }
    }

    public class Series
    {
        public Series() { }
        public Series(string name) { Name = name; }
        public string Name { get; set; }
        public SeriesChartType ChartType { get; set; }
        public string ChartArea { get; set; }
        public string AxisLabel { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Color BorderColor { get; set; }
        public string BorderDashStyle { get; set; }
        public int BorderWidth { get; set; }
        public System.Drawing.Color Color { get; set; }
        public ChartValueType XValueType { get; set; }
        public ChartValueType YValueType { get; set; }
        public DataPoint EmptyPointStyle { get; set; } = new();
        public string XValueMember { get; set; }
        public string YValueMembers { get; set; }
        public string Legend { get; set; }
        public string LegendText { get; set; }
        public string ToolTip { get; set; }
        public string Label { get; set; }
        public string LabelFormat { get; set; }
        public bool IsValueShownAsLabel { get; set; }
        public bool IsVisibleInLegend { get; set; }
        public int ShadowOffset { get; set; }
        public System.Drawing.Color ShadowColor { get; set; }
        public int MarkerSize { get; set; }
        public string MarkerStyle { get; set; }
        public System.Drawing.Color MarkerColor { get; set; }
        public AxisType XAxisType { get; set; }
        public AxisType YAxisType { get; set; }
        public DataPointCollection Points => new();
        public string OnCustomPropertiesChanged { get; set; }
        public void SortByPointValue() { }
    }

    public class SeriesCollection : System.Collections.CollectionBase
    {
        public Series this[int index] => (Series)List[index];
        public Series this[string name] => null;
        public void Add(Series series) { List.Add(series); }
        public void Add(string name) { List.Add(new Series(name)); }
        public void Clear() { List.Clear(); }
        public void Remove(Series series) { List.Remove(series); }
        public void RemoveAt(int index) { List.RemoveAt(index); }
    }

    public class DataPoint
    {
        public double XValue { get; set; }
        public double[] YValues { get; set; }
        public double YValue { get; set; }
        public string AxisLabel { get; set; }
        public string Label { get; set; }
        public string ToolTip { get; set; }
        public System.Drawing.Color Color { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.Color BorderColor { get; set; }
        public int BorderWidth { get; set; }
        public string BorderDashStyle { get; set; }
        public string MarkerStyle { get; set; }
        public int MarkerSize { get; set; }
        public System.Drawing.Color MarkerColor { get; set; }
        public bool IsValueShownAsLabel { get; set; }
        public bool IsVisibleInLegend { get; set; }
        public bool IsEmpty { get; set; }
        public string LegendText { get; set; }
        public string LabelFormat { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
    }

    public class DataPointCollection : System.Collections.CollectionBase
    {
        public int Add(DataPoint point) { List.Add(point); return List.Count - 1; }
        public int Add(double y) { List.Add(new DataPoint { YValue = y }); return List.Count - 1; }
        public int Add(double x, double y) { List.Add(new DataPoint { XValue = x, YValue = y }); return List.Count - 1; }
        public int Add(double x, double y1, double y2) { List.Add(new DataPoint { XValue = x, YValues = new[] { y1, y2 } }); return List.Count - 1; }
        public int AddXY(object x, params object[] y) { List.Add(new DataPoint()); return List.Count - 1; }
        public int AddY(params object[] y) { List.Add(new DataPoint()); return List.Count - 1; }
        public DataPoint this[int index] => (DataPoint)List[index];
        public void RemoveAt(int index) { List.RemoveAt(index); }
        public void Remove(DataPoint point) { List.Remove(point); }
        public int Count2 => List.Count;
    }

    public class Axis
    {
        public string Title { get; set; }
        public string TitleFont { get; set; }
        public string TitleForeColor { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public System.Drawing.Color LineColor { get; set; }
        public int LineWidth { get; set; }
        public string LineDashStyle { get; set; }
        public LabelStyle LabelStyle => new();
        public Grid Grid => new();
        public string ArrowStyle { get; set; }
        public bool IsLabelAutoFit { get; set; }
        public Grid MajorGrid { get; set; } = new();
        public Grid MinorGrid { get; set; } = new();
        public TickMark MajorTickMark { get; set; } = new();
        public TickMark MinorTickMark { get; set; } = new();
        public string Interval { get; set; }
        public string IntervalType { get; set; }
        public bool Enabled { get; set; }
    }

    public class TickMark
    {
        public string Style { get; set; }
        public System.Drawing.Color LineColor { get; set; }
        public int LineWidth { get; set; }
        public double Size { get; set; }
        public bool Enabled { get; set; }
    }

    public class Grid2 { }

    public class LabelStyle
    {
        public string Font { get; set; }
        public string ForeColor { get; set; }
        public bool Enabled { get; set; }
        public string Format { get; set; }
        public string Interval { get; set; }
    }

    public class Grid
    {
        public bool Enabled { get; set; }
        public string LineColor { get; set; }
        public int LineWidth { get; set; }
        public string LineDashStyle { get; set; }
        public string Interval { get; set; }
    }

    public class Legend
    {
        public Legend() { }
        public Legend(string name) { Name = name; }
        public string Name { get; set; }
        public string BackColor { get; set; }
        public string Title { get; set; }
        public string TitleFont { get; set; }
        public string TitleForeColor { get; set; }
        public string Font { get; set; }
        public string ForeColor { get; set; }
        public string Alignment { get; set; }
        public Docking Docking { get; set; }
        public bool Enabled { get; set; }
        public string Position { get; set; }
    }

    public class LegendCollection : System.Collections.CollectionBase
    {
        public Legend this[int index] => (Legend)List[index];
        public Legend this[string name] => null;
        public void Add(Legend legend) { List.Add(legend); }
        public void Add(string name) { List.Add(new Legend(name)); }
        public void Clear() { List.Clear(); }
    }

    public class Title
    {
        public Title() { }
        public Title(string text) { Text = text; }
        public string Text { get; set; }
        public System.Drawing.Font Font { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public System.Drawing.ContentAlignment Alignment { get; set; }
        public TextStyle TextStyle { get; set; }
        public Docking Docking { get; set; }
        public bool Visible { get; set; }
    }

    public class TitleCollection : System.Collections.CollectionBase
    {
        public Title this[int index] => (Title)List[index];
        public void Add(Title title) { List.Add(title); }
        public void Add(string text) { List.Add(new Title(text)); }
        public void Clear() { List.Clear(); }
    }

    public enum ChartValueType { Auto, Double, Single, Int, Int32, Int64, UInt64, String, DateTime, Date, Time, TimeSpan }
    public enum SeriesChartType { Point, FastPoint, Bubble, Line, Spline, StepLine, FastLine, Bar, StackedBar, StackedBar100, Column, StackedColumn, StackedColumn100, Area, SplineArea, StackedArea, StackedArea100, Pie, Doughnut, Stock, Candlestick, Range, SplineRange, RangeBar, RangeColumn, Radar, Polar, ErrorBar, BoxPlot, Renko, ThreeLineBreak, Kagi, PointAndFigure, Funnel, Pyramid }
    public enum AxisType { Primary, Secondary }
    public enum TextStyle { Default, Shadow, Emboss, Frame }
    public enum Docking { Top, Bottom, Left, Right }
    public enum ChartImageFormat { Png, Jpeg, Gif, Bmp, Tiff, Emf }

    public class ChartArea2 { }
}