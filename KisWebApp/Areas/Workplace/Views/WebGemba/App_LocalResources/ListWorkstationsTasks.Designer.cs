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
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ListWorkstationsTasks {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ListWorkstationsTasks() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Workplace.Views.WebGemba.App_LocalResources.ListWorkstationsTasks", typeof(ListWorkstationsTasks).Assembly);
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
        ///   Looks up a localized string similar to Attenzione: non hai il permesso di eseguire i tasks di questa postazione.
        /// </summary>
        public static string lblErrNoAuth {
            get {
                return ResourceManager.GetString("lblErrNoAuth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Istruzione di lavoro non presente per questo task.
        /// </summary>
        public static string lblErrWINotFound {
            get {
                return ResourceManager.GetString("lblErrWINotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Postazione di lavoro non trovata..
        /// </summary>
        public static string lblErrWorkstationNotFound {
            get {
                return ResourceManager.GetString("lblErrWorkstationNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Task della postazione.
        /// </summary>
        public static string lblTitle {
            get {
                return ResourceManager.GetString("lblTitle", resourceCulture);
            }
        }
    }
}
