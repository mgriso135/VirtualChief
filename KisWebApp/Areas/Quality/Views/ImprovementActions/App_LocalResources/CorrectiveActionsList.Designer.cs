﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResImprovementActions {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CorrectiveActionsList {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CorrectiveActionsList() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Quality.Views.ImprovementActions.App_LocalResources.CorrectiveActionsLi" +
                            "st", typeof(CorrectiveActionsList).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chiusa.
        /// </summary>
        public static string lblClosed {
            get {
                return ResourceManager.GetString("lblClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancellando l&apos;azione correttiva, verranno cancellati anche tutti i dati associati. Vuoi continuare?.
        /// </summary>
        public static string lblDelWarning {
            get {
                return ResourceManager.GetString("lblDelWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Si è verificato un errore durante la cancellazione dell&apos;azione correttiva..
        /// </summary>
        public static string lblErrorDelete {
            get {
                return ResourceManager.GetString("lblErrorDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non è presente nessuna azione correttiva..
        /// </summary>
        public static string lblNoCorrectiveActions {
            get {
                return ResourceManager.GetString("lblNoCorrectiveActions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore: non hai il permesso necessario per visualizzare le azioni correttive..
        /// </summary>
        public static string lblNotLoggedIn {
            get {
                return ResourceManager.GetString("lblNotLoggedIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aperta.
        /// </summary>
        public static string lblOpen {
            get {
                return ResourceManager.GetString("lblOpen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In corso.
        /// </summary>
        public static string lblRunning {
            get {
                return ResourceManager.GetString("lblRunning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Descrizione.
        /// </summary>
        public static string lblTHDescription {
            get {
                return ResourceManager.GetString("lblTHDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data di inizio prevista.
        /// </summary>
        public static string lblTHEarlyStartDate {
            get {
                return ResourceManager.GetString("lblTHEarlyStartDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data di fine prevista.
        /// </summary>
        public static string lblTHLateFinishDate {
            get {
                return ResourceManager.GetString("lblTHLateFinishDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Giorni di lavoro previsti.
        /// </summary>
        public static string lblTHLeadTime {
            get {
                return ResourceManager.GetString("lblTHLeadTime", resourceCulture);
            }
        }
    }
}
