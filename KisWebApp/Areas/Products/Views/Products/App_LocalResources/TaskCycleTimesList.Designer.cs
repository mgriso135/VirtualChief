﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResProducts {
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
    public class TaskCycleTimesList {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TaskCycleTimesList() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Products.Views.Products.App_LocalResources.TaskCycleTimesList", typeof(TaskCycleTimesList).Assembly);
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
        ///   Looks up a localized string similar to Tempo ciclo.
        /// </summary>
        public static string lblCycleTime {
            get {
                return ResourceManager.GetString("lblCycleTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Default.
        /// </summary>
        public static string lblDefault {
            get {
                return ResourceManager.GetString("lblDefault", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non hai l&apos;autorizzazione per visualizzare i tempi ciclo.
        /// </summary>
        public static string lblNoAuth {
            get {
                return ResourceManager.GetString("lblNoAuth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non sono ancora presenti tempi ciclo..
        /// </summary>
        public static string lblNoItems {
            get {
                return ResourceManager.GetString("lblNoItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Numero operatori.
        /// </summary>
        public static string lblNOps {
            get {
                return ResourceManager.GetString("lblNOps", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tempo di setup.
        /// </summary>
        public static string lblSetupTime {
            get {
                return ResourceManager.GetString("lblSetupTime", resourceCulture);
            }
        }
    }
}
