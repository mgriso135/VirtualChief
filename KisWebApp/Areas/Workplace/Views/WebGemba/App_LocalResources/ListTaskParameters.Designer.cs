﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResWebGemba {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ListTaskParameters {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ListTaskParameters() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Workplace.Views.WebGemba.App_LocalResources.ListTaskParameters", typeof(ListTaskParameters).Assembly);
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
        ///   Looks up a localized string similar to Errore: è avvenuto un errore impostando il valore del parametro. Ricarica la pagina e prova ad eseguire nuovamente l&apos;operazione..
        /// </summary>
        public static string lblErrAdding {
            get {
                return ResourceManager.GetString("lblErrAdding", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore: è avvenuto un errore durante la cancellazione del parametro. Ricarica la pagina e prova ad eseguire nuovamente l&apos;operazione..
        /// </summary>
        public static string lblErrGeneric {
            get {
                return ResourceManager.GetString("lblErrGeneric", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore: il parametro sembra non esistere nel nostro modello. Ricarica la pagina e prova a ripetere l&apos;operazione..
        /// </summary>
        public static string lblErrNonExistentParameter {
            get {
                return ResourceManager.GetString("lblErrNonExistentParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore: il parametro è già impostato. Prova a ricaricare la pagina e a ripetere l&apos;operazione..
        /// </summary>
        public static string lblErrParamAlreadySet {
            get {
                return ResourceManager.GetString("lblErrParamAlreadySet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore: utente non autorizzato.
        /// </summary>
        public static string lblErrUserNotAuthorized {
            get {
                return ResourceManager.GetString("lblErrUserNotAuthorized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nessun parametro da impostare..
        /// </summary>
        public static string lblNoItems {
            get {
                return ResourceManager.GetString("lblNoItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Categoria.
        /// </summary>
        public static string lblTHCategory {
            get {
                return ResourceManager.GetString("lblTHCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Valore.
        /// </summary>
        public static string lblTHDescription {
            get {
                return ResourceManager.GetString("lblTHDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parametro.
        /// </summary>
        public static string lblTHParamName {
            get {
                return ResourceManager.GetString("lblTHParamName", resourceCulture);
            }
        }
    }
}
