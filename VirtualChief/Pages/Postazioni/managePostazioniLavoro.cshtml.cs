using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;
using System.Linq;

namespace VirtualChief.Pages.Postazioni
{
    /// <summary>
    /// Migrated from WebForms Postazioni/managePostazioniLavoro.aspx +
    /// viewElencoPostazioni.ascx + addPostazione.ascx: workstation list with
    /// task-aware delete, barcode label download (GenCode128 + iTextSharp, same
    /// as legacy) and the "new workstation" form.
    /// Permissions: "Postazione" R (list/barcode), "Postazione" W (add/delete).
    /// </summary>
    public class managePostazioniLavoroModel : PageModel
    {
        private readonly ILogger<managePostazioniLavoroModel> _logger;

        public managePostazioniLavoroModel(ILogger<managePostazioniLavoroModel> logger)
        {
            _logger = logger;
        }

        public class PostazioneItem
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Desc { get; set; } = "";
            public bool BarcodeAutoCheckIn { get; set; }
        }

        // Legacy resx: viewElencoPostazioni.ascx
        public const string MsgDelKOTasks = "Impossibile eliminare la postazione: sono collegate le seguenti attivit&agrave;:";
        public const string MsgGenericError = "Errore generico.";
        public const string MsgPostazioneNotFound = "Postazione non trovata.";

        public bool CanRead { get; private set; }
        public bool CanWrite { get; private set; }
        public List<PostazioneItem> Workstations { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        private string? Tenant => CurrentWorkspace.Of(User);

        private bool HasPermission(string level)
        {
            var uidStr = User.FindFirst("uid")?.Value;
            var tenant = Tenant;
            if (!int.TryParse(uidStr, out var uid) || string.IsNullOrEmpty(tenant))
            {
                return false;
            }
            try
            {
                var prm = new List<string[]> { new[] { "Postazione", level } };
                return new UserAccount(uid).ValidatePermissions(tenant, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Postazioni/managePostazioniLavoro: verifica permesso Postazione");
                return false;
            }
        }

        private void LoadWorkstations()
        {
            try
            {
                Workstations = new ElencoPostazioni(Tenant!).elenco?
                    .Select(p => new PostazioneItem
                    {
                        Id = p.id,
                        Name = p.name ?? "",
                        Desc = p.desc ?? "",
                        BarcodeAutoCheckIn = p.barcodeAutoCheckIn
                    })
                    .ToList() ?? new List<PostazioneItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Postazioni/managePostazioniLavoro: caricamento elenco");
            }
        }

        public void OnGet()
        {
            CanRead = HasPermission("R");
            CanWrite = HasPermission("W");
            if (CanRead)
            {
                LoadWorkstations();
            }
        }

        /// <summary>addPostazione.ascx.save_Click.</summary>
        public IActionResult OnPostAdd(string nome, string desc, string autoCheckIn)
        {
            if (!HasPermission("W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { handler = "" });
            }
            if (string.IsNullOrWhiteSpace(nome))
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var pst = new Postazione(Tenant!);
                bool rt = pst.add(
                    System.Net.WebUtility.HtmlEncode(nome),
                    System.Net.WebUtility.HtmlEncode(desc ?? ""),
                    autoCheckIn == "1");
                Message = rt ? $"Postazione '{nome}' creata." : MsgGenericError;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Postazioni/managePostazioniLavoro: creazione postazione");
                Message = MsgGenericError;
            }
            return RedirectToPage(new { handler = "" });
        }

        /// <summary>
        /// viewElencoPostazioni.ascx postazione_modify (delete): a workstation can
        /// be deleted only when no process activities are linked to it.
        /// </summary>
        public IActionResult OnPostDelete(int id)
        {
            if (!HasPermission("W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var p = new Postazione(Tenant!, id);
                if (p.id == -1)
                {
                    Message = MsgPostazioneNotFound;
                    return RedirectToPage(new { handler = "" });
                }
                p.loadTasks();
                if (p.tasks == null || p.tasks.Count == 0)
                {
                    bool rt = p.delete();
                    Message = rt ? "Postazione eliminata." : MsgGenericError + ": " + p.log;
                }
                else
                {
                    var names = string.Join(", ", p.tasks.Select(t => t.processName));
                    Message = MsgDelKOTasks + " " + names;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Postazioni/managePostazioniLavoro: eliminazione postazione {Id}", id);
                Message = MsgGenericError;
            }
            return RedirectToPage(new { handler = "" });
        }

        /// <summary>
        /// viewElencoPostazioni.ascx printBarCode: barcode "P{id}" rendered with
        /// GenCode128 and packaged in an A4-landscape PDF via iTextSharp.
        /// </summary>
        public IActionResult OnGetBarcode(int id)
        {
            if (!HasPermission("R"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var p = new Postazione(Tenant!, id);
                if (p.id == -1)
                {
                    return NotFound();
                }

                using var code = GenCode128.Code128Rendering.MakeBarcodeImage("P" + p.id.ToString(), 2, true);
                using var resized = new System.Drawing.Bitmap(code, 200, 200 * code.Height / code.Width);

                using var cartPDF = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 50, 50, 25, 25);
                using var ms = new MemoryStream();
                var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(cartPDF, ms);
                cartPDF.Open();

                var title = new iTextSharp.text.Paragraph($"Postazione: {p.name}");
                title.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                cartPDF.Add(title);

                var img = iTextSharp.text.Image.GetInstance(resized, System.Drawing.Imaging.ImageFormat.Png);
                img.ScaleToFit(300f, 300f);
                img.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                cartPDF.Add(img);

                cartPDF.Close();

                return File(ms.ToArray(), "application/pdf", $"postazione{p.id}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Postazioni/managePostazioniLavoro: barcode postazione {Id}", id);
                Message = "Errore durante la generazione del barcode.";
                return RedirectToPage(new { handler = "" });
            }
        }
    }
}
