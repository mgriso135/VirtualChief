﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResUsers {
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
    public class ActivateUser {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ActivateUser() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Users.Views.Users.App_LocalResources.ActivateUser", typeof(ActivateUser).Assembly);
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
        ///   Looks up a localized string similar to Attivazione effettuata con successo!!!.
        /// </summary>
        public static string lblActivated1 {
            get {
                return ResourceManager.GetString("lblActivated1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tra poco verrai reindirizzato alla pagina di login!.
        /// </summary>
        public static string lblActivated2 {
            get {
                return ResourceManager.GetString("lblActivated2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ci dispiace, ma non è stato possibile effettuare l&apos;attivazione dell&apos;utente richiesto. Verifica il link sull&apos;e-mail che ti abbiamo inviato e ritenta.
        ///
        ///Se il problema dovesse persistere, contatta il nostro servizio clienti..
        /// </summary>
        public static string lblNotActivated {
            get {
                return ResourceManager.GetString("lblNotActivated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attivazione utente.
        /// </summary>
        public static string lblTitle {
            get {
                return ResourceManager.GetString("lblTitle", resourceCulture);
            }
        }
    }
}
