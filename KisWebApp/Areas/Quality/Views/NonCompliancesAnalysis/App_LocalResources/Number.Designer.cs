﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResAnalysis {
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
    public class Number {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Number() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Quality.Views.NonCompliancesAnalysis.App_LocalResources.Number", typeof(Number).Assembly);
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
        ///   Looks up a localized string similar to Analisi per numero di non conformità.
        /// </summary>
        public static string AnalysisNumTitle {
            get {
                return ResourceManager.GetString("AnalysisNumTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data di fine analisi.
        /// </summary>
        public static string lblDateEnd {
            get {
                return ResourceManager.GetString("lblDateEnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data di inizio analisi.
        /// </summary>
        public static string lblDateStart {
            get {
                return ResourceManager.GetString("lblDateStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Visualizzazione.
        /// </summary>
        public static string lblGraphFormat {
            get {
                return ResourceManager.GetString("lblGraphFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mensile.
        /// </summary>
        public static string lblGraphFormatMonthly {
            get {
                return ResourceManager.GetString("lblGraphFormatMonthly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Settimanale.
        /// </summary>
        public static string lblGraphFormatWeekly {
            get {
                return ResourceManager.GetString("lblGraphFormatWeekly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Errore: la data iniziale deve essere antecedente rispetto a quella finale.
        /// </summary>
        public static string lblInputDateError {
            get {
                return ResourceManager.GetString("lblInputDateError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pareto categorie di non conformità.
        /// </summary>
        public static string lblNCCategories {
            get {
                return ResourceManager.GetString("lblNCCategories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pareto cause di non conformità.
        /// </summary>
        public static string lblNCCause {
            get {
                return ResourceManager.GetString("lblNCCause", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non conformità per mese.
        /// </summary>
        public static string lblNCMonth {
            get {
                return ResourceManager.GetString("lblNCMonth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non conformità per settimana.
        /// </summary>
        public static string lblNCWeek {
            get {
                return ResourceManager.GetString("lblNCWeek", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Vedi dettagli.
        /// </summary>
        public static string lblViewDetails {
            get {
                return ResourceManager.GetString("lblViewDetails", resourceCulture);
            }
        }
    }
}
