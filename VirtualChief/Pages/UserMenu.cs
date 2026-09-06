using System.Security.Claims;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages
{
    /// <summary>One entry of the upper menu (legacy WebForms MenuItem equivalent).</summary>
    public class MenuItem
    {
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public List<MenuItem> Children { get; set; } = new List<MenuItem>();
    }

    /// <summary>
    /// Builds the upper navigation menu from the user's groups, exactly like
    /// the legacy menuBar.ascx: for the active workspace it loads the groups of
    /// the UserAccount (useraccountsgroups), the menu entries assigned to each
    /// group (menugroups -> menuvoci), deduplicates them and attaches the child
    /// entries (menualbero).
    ///
    /// Legacy URLs (~/...aspx / MVC areas) are translated to the migrated Razor
    /// Pages when available; targets not migrated yet point to
    /// /UnderConstruction so the menu always mirrors the group configuration.
    /// </summary>
    public static class UserMenu
    {
        /// <summary>
        /// Legacy target (basename, case-insensitive) -> migrated Razor Page.
        /// Extend as more pages are migrated.
        /// </summary>
        private static readonly Dictionary<string, string> PageMap =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["commesse"] = "/Commesse/commesse",
                ["produzione"] = "/Produzione/produzione",
                ["clienti"] = "/Clienti/Clienti",
                ["listreparti"] = "/Reparti/listReparti",
                ["listusers"] = "/Users/listUsers",
                ["customer/list"] = "/Customers/Customer/List",
                ["editcliente"] = "/Customers/Customer/Edit",
                ["editcontattodetails"] = "/Customers/Customer/ContactDetail",
                ["analysis"] = "/Analysis/analysis",
                ["kisAdmin"] = "/Admin/kisAdmin",
                ["configandoncompleto"] = "/Andon/configAndonCompleto",
                ["managepostazionilavoro"] = "/Postazioni/managePostazioniLavoro",
                ["wzAddCommessa"] = "/Commesse/NuovoOrdine",
                ["commesseDaProdurre"] = "/Produzione/CommesseDaProdurre",
                ["pianoproduzione"] = "/Produzione/PianoProduzione",
                ["pianoproduzionecompleto"] = "/Produzione/PianoProduzione",
            };

        public static List<MenuItem> Build(ClaimsPrincipal user)
        {
            var result = new List<MenuItem>();

            var uidStr = user?.FindFirst("uid")?.Value;
            var tenant = CurrentWorkspace.Of(user);
            if (!int.TryParse(uidStr, out var uid) || string.IsNullOrEmpty(tenant))
            {
                return result; // forms-fallback users have no vcmain account: no dynamic menu
            }

            try
            {
                var ws = new Workspace(tenant);
                if (ws.id == -1)
                {
                    return result;
                }

                var curr = new UserAccount(uid);
                curr.loadGroups(ws.id);

                // Union of the group menus, deduplicated by entry id (legacy behaviour).
                var voci = new List<VoceMenu>();
                foreach (var g in curr.groups)
                {
                    g.loadMenu();
                    foreach (var voce in g.VociDiMenu)
                    {
                        if (!voci.Any(v => v.ID == voce.ID))
                        {
                            voci.Add(voce);
                        }
                    }
                }

                foreach (var voce in voci)
                {
                    var item = new MenuItem
                    {
                        Title = voce.Titolo ?? "",
                        Url = Translate(voce.URL, voce.Titolo)
                    };

                    voce.loadFigli();
                    foreach (var figlia in voce.VociFiglie)
                    {
                        var childUrl = Translate(figlia.URL, figlia.Titolo);
                        if (childUrl != "")
                        {
                            item.Children.Add(new MenuItem { Title = figlia.Titolo ?? "", Url = childUrl });
                        }
                    }

                    // Only self-referential login entries produce an empty URL.
                    if (item.Url != "")
                    {
                        result.Add(item);
                    }
                }
            }
            catch
            {
                // Menu must never break the page: return whatever was collected.
            }

            return result;
        }

        /// <summary>
        /// Translates a legacy URL to the target shown by the menu: the migrated
        /// Razor Page when available, otherwise /UnderConstruction carrying the
        /// original title and URL. Only self-referential login entries are dropped.
        /// </summary>
        private static string Translate(string legacyUrl, string title)
        {
            if (string.IsNullOrEmpty(legacyUrl))
            {
                return $"/UnderConstruction?title={Uri.EscapeDataString(title ?? "")}";
            }

            // "~/Produzione/produzione.aspx" | "~/Workplace/WebGemba/Index"
            var segments = legacyUrl.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var lastSegment = segments.LastOrDefault() ?? "";
            lastSegment = lastSegment.Replace("~", "");
            if (lastSegment.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                lastSegment = lastSegment[..^5];
            }

            // Area-style MVC URLs match on "controller/action"
            // (e.g. "~/Customers/Customer/List" -> "customer/list")
            if (segments.Length >= 2)
            {
                var prevSegment = segments[^2].Replace("~", "");
                if (PageMap.TryGetValue($"{prevSegment}/{lastSegment}", out var areaPage))
                {
                    return areaPage;
                }
            }

            if (lastSegment.Equals("login", StringComparison.OrdinalIgnoreCase))
            {
                return ""; // login entries make no sense inside the app menu
            }

            if (PageMap.TryGetValue(lastSegment, out var page))
            {
                return page;
            }

            return $"/UnderConstruction?title={Uri.EscapeDataString(title ?? lastSegment)}"
                 + $"&legacy={Uri.EscapeDataString(legacyUrl)}";
        }
    }
}
