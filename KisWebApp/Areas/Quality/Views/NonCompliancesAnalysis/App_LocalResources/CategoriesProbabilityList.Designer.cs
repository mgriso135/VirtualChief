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
    public class CategoriesProbabilityList {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CategoriesProbabilityList() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KIS.Areas.Quality.Views.NonCompliancesAnalysis.App_LocalResources.CategoriesProba" +
                            "bilityList", typeof(CategoriesProbabilityList).Assembly);
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
        ///   Looks up a localized string similar to La frequenza della categoria di non conformità é il numero di volte che si presenta una non conformità legata a questa categoria; la frequenza relativa si ottiene dividendo la frequenza assoluta per il numero di non conformità..
        /// </summary>
        public static string lblCatDesc {
            get {
                return ResourceManager.GetString("lblCatDesc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nessuna categoria di non conformità incontrata nel periodo indicato.
        /// </summary>
        public static string lblErrNoCategories {
            get {
                return ResourceManager.GetString("lblErrNoCategories", resourceCulture);
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
        ///   Looks up a localized string similar to Frequenza di accadimento.
        /// </summary>
        public static string lblTHFrequency {
            get {
                return ResourceManager.GetString("lblTHFrequency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Frequenza relativa.
        /// </summary>
        public static string lblTHFrequencyRel {
            get {
                return ResourceManager.GetString("lblTHFrequencyRel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Probabilità di accadimento della categoria di non conformità.
        /// </summary>
        public static string lblTitle {
            get {
                return ResourceManager.GetString("lblTitle", resourceCulture);
            }
        }
    }
}
