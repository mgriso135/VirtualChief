﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResScrollTypeView {
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
    public class DepartmentScrollTypeView {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DepartmentScrollTypeView() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Config.Views.AndonConfig.App_LocalResources.DepartmentScrollTypeView", typeof(DepartmentScrollTypeView).Assembly);
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
        ///   Looks up a localized string similar to Scroll continuo.
        /// </summary>
        public static string lblContinuousScroll {
            get {
                return ResourceManager.GetString("lblContinuousScroll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tempo di scrolling (da fine a inizio).
        /// </summary>
        public static string lblContinuousScrollBackSpeed {
            get {
                return ResourceManager.GetString("lblContinuousScrollBackSpeed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tempo di avanzamento (da inizio a fine).
        /// </summary>
        public static string lblContinuousScrollGoSpeed {
            get {
                return ResourceManager.GetString("lblContinuousScrollGoSpeed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore: utente non autorizzato..
        /// </summary>
        public static string lblNoAuth {
            get {
                return ResourceManager.GetString("lblNoAuth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Scroll automatico disabilitato.
        /// </summary>
        public static string lblNoScroll {
            get {
                return ResourceManager.GetString("lblNoScroll", resourceCulture);
            }
        }
    }
}
