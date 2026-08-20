// Compile-only shim for the third-party "GenCode128" barcode generator
// referenced by a few legacy PDF/barcode code-behinds. No code runs here.
using System.Drawing;

namespace GenCode128
{
    public static class Code128Rendering
    {
        public static System.Drawing.Image MakeBarcodeImage(string data, float height, bool addQuietZone) => new Bitmap(1, 1);
    }
}
