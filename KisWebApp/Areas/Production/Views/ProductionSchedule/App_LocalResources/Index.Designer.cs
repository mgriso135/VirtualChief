﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResProductionSchedule {
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
    public class Index {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Index() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Production.Views.ProductionSchedule.App_LocalResources.Index", typeof(Index).Assembly);
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
        ///   Looks up a localized string similar to Clienti.
        /// </summary>
        public static string lblBtnCustomers {
            get {
                return ResourceManager.GetString("lblBtnCustomers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reparti.
        /// </summary>
        public static string lblBtnDepartments {
            get {
                return ResourceManager.GetString("lblBtnDepartments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Prodotti.
        /// </summary>
        public static string lblBtnProducts {
            get {
                return ResourceManager.GetString("lblBtnProducts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Status.
        /// </summary>
        public static string lblBtnStatus {
            get {
                return ResourceManager.GetString("lblBtnStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Utente non autorizzato a visualizzare il piano produzione.
        /// </summary>
        public static string lblErrUserAuth {
            get {
                return ResourceManager.GetString("lblErrUserAuth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In esecuzione.
        /// </summary>
        public static string lblStatusI {
            get {
                return ResourceManager.GetString("lblStatusI", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non pianificato.
        /// </summary>
        public static string lblStatusN {
            get {
                return ResourceManager.GetString("lblStatusN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pianificato in produzione.
        /// </summary>
        public static string lblStatusP {
            get {
                return ResourceManager.GetString("lblStatusP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Piano produzione.
        /// </summary>
        public static string lblTitle {
            get {
                return ResourceManager.GetString("lblTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reparto non assegnato.
        /// </summary>
        public static string lblUnassignedDepartment {
            get {
                return ResourceManager.GetString("lblUnassignedDepartment", resourceCulture);
            }
        }
    }
}
