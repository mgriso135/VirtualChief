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
    public class CausesRiskList {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CausesRiskList() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Quality.Views.NonCompliancesAnalysis.App_LocalResources.CausesRiskList", typeof(CausesRiskList).Assembly);
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
        ///   Looks up a localized string similar to Costo unitario medio pesato [danno].
        /// </summary>
        public static string lblCost {
            get {
                return ResourceManager.GetString("lblCost", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Frequenza di accadimento nel periodo selezionato [probabilità].
        /// </summary>
        public static string lblFrequency {
            get {
                return ResourceManager.GetString("lblFrequency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rischio = probabilità x danno.
        /// </summary>
        public static string lblRisk {
            get {
                return ResourceManager.GetString("lblRisk", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Il rischio delle cause di non conformità è la moltiplicazione della frequenza (o probabilità) di accadimento di una non conformità appartenente ad una certa causa ed il suo costo medio calcolato..
        /// </summary>
        public static string lblRiskDesc {
            get {
                return ResourceManager.GetString("lblRiskDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cause di non conformità.
        /// </summary>
        public static string lblTHCauseName {
            get {
                return ResourceManager.GetString("lblTHCauseName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rischio associato alle cause di non conformità.
        /// </summary>
        public static string lblTitle {
            get {
                return ResourceManager.GetString("lblTitle", resourceCulture);
            }
        }
    }
}
