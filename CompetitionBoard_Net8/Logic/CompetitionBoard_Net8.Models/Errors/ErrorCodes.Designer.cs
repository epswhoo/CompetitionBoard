﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CompetitionBoard_Net8.Models.Errors {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorCodes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorCodes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CompetitionBoard_Net8.Models.Errors.ErrorCodes", typeof(ErrorCodes).Assembly);
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
        ///   Looks up a localized string similar to 10.
        /// </summary>
        public static string IDBSvcException {
            get {
                return ResourceManager.GetString("IDBSvcException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 20.
        /// </summary>
        public static string IRnHRepoException {
            get {
                return ResourceManager.GetString("IRnHRepoException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 30.
        /// </summary>
        public static string IRnHRepoInsertNewWithOrderOutOfRange {
            get {
                return ResourceManager.GetString("IRnHRepoInsertNewWithOrderOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 26.
        /// </summary>
        public static string IRnHRepoSaveDisAndRank {
            get {
                return ResourceManager.GetString("IRnHRepoSaveDisAndRank", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 25.
        /// </summary>
        public static string IRnHRepoSaveMarkAndDis {
            get {
                return ResourceManager.GetString("IRnHRepoSaveMarkAndDis", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 24.
        /// </summary>
        public static string IRnHRepoSaveMarkOutOfRange {
            get {
                return ResourceManager.GetString("IRnHRepoSaveMarkOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 27.
        /// </summary>
        public static string IRnHRepoSaveRankAndNoMark {
            get {
                return ResourceManager.GetString("IRnHRepoSaveRankAndNoMark", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 28.
        /// </summary>
        public static string IRnHRepoSaveRnHNotFound {
            get {
                return ResourceManager.GetString("IRnHRepoSaveRnHNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 21.
        /// </summary>
        public static string IRnHRepoSaveStatusNotCompetitionDoneWithDis {
            get {
                return ResourceManager.GetString("IRnHRepoSaveStatusNotCompetitionDoneWithDis", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 23.
        /// </summary>
        public static string IRnHRepoSaveStatusNotCompetitionDoneWithIsRanked {
            get {
                return ResourceManager.GetString("IRnHRepoSaveStatusNotCompetitionDoneWithIsRanked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 22.
        /// </summary>
        public static string IRnHRepoSaveStatusNotCompetitionDoneWithMark {
            get {
                return ResourceManager.GetString("IRnHRepoSaveStatusNotCompetitionDoneWithMark", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 40.
        /// </summary>
        public static string RnHViewModelMarkIsNoDouble {
            get {
                return ResourceManager.GetString("RnHViewModelMarkIsNoDouble", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1.
        /// </summary>
        public static string UnknownError {
            get {
                return ResourceManager.GetString("UnknownError", resourceCulture);
            }
        }
    }
}