// Minimal stubs so the legacy .NET Framework business classes (KisWebApp/App_Sources,
// namespace KIS.App_Code / KIS.App_Sources) compile on .NET Core / Linux.
// These are COMPILE-ONLY shims: the web-only code paths are never executed by tests.
using System.Collections.Specialized;
using System.Security.Claims;

namespace System.Web
{
    public class HttpContext
    {
        public static HttpContext? Current { get; set; }
        public HttpSessionState? Session { get; set; }
        public HttpServerUtility? Server { get; set; }
        public HttpRequest? Request { get; set; }
        public HttpResponse? Response { get; set; }
        public HttpApplication? ApplicationInstance { get; set; }
        public HttpApplicationState Application { get; set; } = new();
        public System.Collections.IDictionary Items { get; set; } = new System.Collections.Hashtable();
        public Exception? Error => null;
        public OwinContext GetOwinContext() => new();
        public object GetSection(string sectionName) => null;
        public static bool IsDebuggingEnabled => false;
    }

    public class HttpApplicationState : System.Collections.Specialized.NameObjectCollectionBase
    {
        public object? this[string key] { get => null; set { } }
        public void Add(string name, object value) { }
        public void Set(string name, object value) { }
        public void Remove(string name) { }
        public void RemoveAll() { }
        public void Clear() { }
        public object? Get(string name) => null;
        public void Lock() { }
        public void UnLock() { }
    }

    public class HttpServerUtility
    {
        public string MapPath(string path) => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Data", "Quality");
        public string HtmlEncode(string s) => System.Net.WebUtility.HtmlEncode(s);
        public string HtmlDecode(string s) => System.Net.WebUtility.HtmlDecode(s);
        public string UrlEncode(string s) => System.Net.WebUtility.UrlEncode(s);
        public string UrlDecode(string s) => System.Net.WebUtility.UrlDecode(s);
        public string UrlEncodeUnicode(string s) => Uri.EscapeDataString(s ?? "");
        public string UrlPathEncode(string s) => s;
        public string HtmlEncode(object s) => s == null ? null : HtmlEncode(s.ToString());
        public string HtmlDecode(object s) => s == null ? null : HtmlDecode(s.ToString());
        public string MapPath(string path, string baseVirtualDir, bool allowCrossAppMapping) => MapPath(path);
        public void Execute(string path) { }
        public void Transfer(string path) { }
        public void Transfer(string path, bool preserveForm) { }
        public void TransferRequest(string path) { }
        public void ClearError() { }
        public Exception GetLastError() => null;
    }

    public class HttpSessionState
    {
        public object? this[string key] { get => null; set { } }
        public int Count => 0;
        public void Abandon() { }
        public void Clear() { }
        public void Remove(string name) { }
        public void RemoveAll() { }
        public System.Collections.Specialized.NameObjectCollectionBase.KeysCollection Keys => null;
        public object? this[int index] => null;
    }

    public class OwinContext
    {
        public Authentication Authentication { get; } = new();
    }

    public class Authentication
    {
        public ClaimsPrincipal User { get; set; } = new();
    }
}

namespace System.Web.Security
{
    public static class Membership
    {
        public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            var s = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
            return length <= s.Length ? s[..length] : s + new string('x', length - s.Length);
        }
    }
}

namespace System.Web
{
    public class HttpRequestBase
    {
        public virtual string this[string key] => null;
        public virtual NameValueCollection QueryString => new();
        public virtual NameValueCollection Form => new();
        public virtual NameValueCollection Headers => new();
        public virtual NameValueCollection ServerVariables => new();
        public virtual HttpFileCollectionBase Files => new();
        public virtual string HttpMethod => "GET";
        public virtual string UrlReferrer => null;
        public virtual string RawUrl => "";
        public virtual string UserAgent => "";
        public virtual string UserHostAddress => "";
        public virtual bool IsAuthenticated => false;
        public virtual Uri Url => new Uri("http://localhost");
        public virtual string ApplicationPath => "";
        public virtual string Path => "";
        public virtual string PhysicalPath => "";
        public virtual string ContentType => "";
        public virtual int ContentLength => 0;
        public virtual System.Collections.Specialized.NameValueCollection Params { get; } = new();
        public virtual string RequestType => "GET";
        public virtual string FilePath => "";
        public virtual string CurrentExecutionFilePath => "";
        public virtual string MapPath(string virtualPath) => System.IO.Directory.GetCurrentDirectory();
        public virtual string[] UserLanguages => Array.Empty<string>();
        public virtual string Browser => "";
        public virtual System.Security.Principal.WindowsIdentity LogonUserIdentity => null;
    }

    public class HttpRequest : HttpRequestBase
    {
        public new virtual string this[string key] => null;
        public new virtual NameValueCollection QueryString => new();
        public new virtual NameValueCollection Form => new();
        public new virtual NameValueCollection Headers => new();
        public new virtual NameValueCollection ServerVariables => new();
        public new virtual HttpFileCollection Files => new();
        public new virtual NameValueCollection Params { get; } = new();
        public virtual string PhysicalApplicationPath => "";
        public virtual string AppRelativeCurrentExecutionFilePath => "";
        public virtual System.Text.Encoding ContentEncoding => System.Text.Encoding.UTF8;
    }

    public class HttpResponseBase
    {
        public virtual System.IO.TextWriter Output { get; set; } = System.IO.TextWriter.Null;
        public virtual void Write(string s) { }
        public virtual void Write(object o) { }
        public virtual void Write(char c) { }
        public virtual void Redirect(string url) { }
        public virtual void Redirect(string url, bool endResponse) { }
        public virtual void End() { }
        public virtual void Flush() { }
        public virtual void Clear() { }
        public virtual void ClearContent() { }
        public virtual void ClearHeaders() { }
        public virtual void AddHeader(string name, string value) { }
        public virtual void AppendHeader(string name, string value) { }
        public virtual void AppendCookie(HttpCookie cookie) { }
        public virtual void SetCookie(HttpCookie cookie) { }
        public virtual void BinaryWrite(byte[] buffer) { }
        public virtual void TransmitFile(string filename) { }
        public virtual void TransmitFile(string filename, long offset, long length) { }
        public virtual void WriteFile(string filename) { }
        public virtual bool Buffer { get; set; }
        public virtual bool BufferOutput { get; set; }
        public virtual int StatusCode { get; set; }
        public virtual string Status { get; set; }
        public virtual string StatusDescription { get; set; }
        public virtual string ContentType { get; set; }
        public virtual System.Text.Encoding ContentEncoding { get; set; }
        public virtual NameValueCollection Headers { get; } = new();
        public virtual HttpCookieCollection Cookies { get; } = new();
        public virtual System.IO.Stream OutputStream => System.IO.Stream.Null;
        public virtual HttpCachePolicy Cache { get; } = new();
    }


    public class HttpResponse : HttpResponseBase { }

    public class HttpFileCollectionBase : System.Collections.Specialized.NameObjectCollectionBase
    {
        public virtual HttpPostedFileBase this[string key] => null;
        public virtual HttpPostedFileBase this[int index] => null;
    }

    public class HttpFileCollection : HttpFileCollectionBase { }

    public class HttpPostedFileBase
    {
        public virtual int ContentLength => 0;
        public virtual string ContentType => "";
        public virtual string FileName => "";
        public virtual System.IO.Stream InputStream => System.IO.Stream.Null;
        public virtual void SaveAs(string filename) { }
    }

    public class HttpCookieCollection : System.Collections.Specialized.NameObjectCollectionBase
    {
        public virtual HttpCookie this[string key] => null;
        public virtual HttpCookie this[int index] => null;
    }

    public class HttpCookie
    {
        public HttpCookie() { }
        public HttpCookie(string name) { Name = name; }
        public HttpCookie(string name, string value) { Name = name; Value = value; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime Expires { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public bool Shareable { get; set; }
        public NameValueCollection Values { get; } = new();
    }

    public class HttpRequestValidationException : Exception
    {
        public HttpRequestValidationException() { }
        public HttpRequestValidationException(string message) : base(message) { }
    }

    public static class MimeMapping
    {
        public static string GetMimeMapping(string fileName) => "application/octet-stream";
    }

}

// Empty namespaces to satisfy legacy `using System.Web.Mvc;` / `using System.Web.Hosting;`
// directives in files whose only MVC/Hosting usage is (in the tested layer) unused.
namespace System.Web.Mvc
{
}

namespace System.Web.Hosting
{
    public static class HostingEnvironment
    {
        public static string ApplicationPhysicalPath => System.IO.Directory.GetCurrentDirectory();
        public static string ApplicationVirtualPath => "/";
        public static string SiteName => "";
        public static bool IsHosted => false;
        public static string MapPath(string virtualPath) => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), virtualPath.TrimStart('~', '/'));
    }
}

