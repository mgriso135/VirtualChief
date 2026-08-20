// Compile-only shims for auxiliary ASP.NET namespaces used by the legacy
// code-behind layer but not needed on Linux: System.Web.Mvc (a few razor-era
// pages), System.Web.Services / System.Web.Helpers (unused usings), and the
// KIS.Configuration.WizConfig master page (compiled from WizConfig.Master.cs).
// No code here runs; these types only make the code-behinds compile.
using System;
using System.Web;
using System.Web.UI;

namespace System.Web.Mvc
{
    public class ViewUserControl : UserControl
    {
        public ViewDataDictionary ViewData { get; set; } = new();
        public dynamic ViewBag => ViewData;
    }

    public class ViewPage : Page
    {
        public ViewDataDictionary ViewData { get; set; } = new();
        public dynamic ViewBag => ViewData;
    }

    public class ViewDataDictionary
    {
        public object this[string key] { get => null; set { } }
        public int Count => 0;
    }
}

namespace System.Web.Services
{
    [AttributeUsage(AttributeTargets.Method)]
    public class WebMethodAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class WebServiceAttribute : Attribute { }

    public class WebService { }
}

namespace System.Web.Helpers
{
    public static class Json
    {
        public static string Encode(object value) => value?.ToString() ?? "";
        public static string Decode(string value) => value;
    }

    public class WebImage
    {
        public WebImage(byte[] content) { }
        public WebImage(string filePath) { }
        public WebImage(System.IO.Stream stream) { }
        public WebImage Resize(int width, int height, bool preserveAspectRatio = true, bool preventEnlarge = false) => this;
        public WebImage Crop(int top = 0, int left = 0, int bottom = 0, int right = 0) => this;
        public WebImage RotateLeft() => this;
        public WebImage RotateRight() => this;
        public WebImage FlipVertical() => this;
        public WebImage FlipHorizontal() => this;
        public WebImage Write(string requestedFormat = null) => this;
        public string FileName { get; set; }
        public string ImageFormat { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] GetBytes() => Array.Empty<byte>();
        public void Save(string filename, string imageFormat = null, bool forceCorrectExtension = true) { }
    }
}

namespace KIS.Configuration
{
    public partial class WizConfig : System.Web.UI.MasterPage
    {
        public string section { get; set; }
    }
}