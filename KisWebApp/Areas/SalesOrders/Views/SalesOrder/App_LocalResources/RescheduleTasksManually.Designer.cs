﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResAddSalesOrder {
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
    public class RescheduleTasksManually {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RescheduleTasksManually() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.SalesOrders.Views.SalesOrder.App_LocalResources.RescheduleTasksManually" +
                            "", typeof(RescheduleTasksManually).Assembly);
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
        ///   Looks up a localized string similar to Attenzione: non hai i permessi necessari..
        /// </summary>
        public static string lblErrNoAuth {
            get {
                return ResourceManager.GetString("lblErrNoAuth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore: la data fornita deve essere nel futuro..
        /// </summary>
        public static string lblErrorDateInThePast {
            get {
                return ResourceManager.GetString("lblErrorDateInThePast", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore generico.
        /// </summary>
        public static string lblErrorGeneric {
            get {
                return ResourceManager.GetString("lblErrorGeneric", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore: la data di inizio task deve essere futura. Inoltre la data di fine task deve essere successiva alla data di inizio..
        /// </summary>
        public static string lblErrorInDates {
            get {
                return ResourceManager.GetString("lblErrorInDates", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore nei dati di input. Verifica il formato delle date ed orari di inzio e fine.
        /// </summary>
        public static string lblErrorInput {
            get {
                return ResourceManager.GetString("lblErrorInput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fine programmata.
        /// </summary>
        public static string lblTHEndDate {
            get {
                return ResourceManager.GetString("lblTHEndDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inizio programmato.
        /// </summary>
        public static string lblTHStartDate {
            get {
                return ResourceManager.GetString("lblTHStartDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Task.
        /// </summary>
        public static string lblTHTask {
            get {
                return ResourceManager.GetString("lblTHTask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gestisci manualmente la pianificazione dei task.
        /// </summary>
        public static string lblTitle {
            get {
                return ResourceManager.GetString("lblTitle", resourceCulture);
            }
        }
    }
}
