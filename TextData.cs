using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Sirenix.OdinInspector;
using UnityEngine;
using LanguageDictionary = System.Collections.Generic.Dictionary<UnityEngine.SystemLanguage, string>;
namespace OhmsLibraries.Localization {
    [CreateAssetMenu( menuName = "Language System/Text Data" )]
    public class TextData : SerializedScriptableObject {
        public SystemLanguage CurrentLanguage => LanguageManager.Instance.currentLanguage;
        public SystemLanguage defaultLanguage = SystemLanguage.English;

        [SerializeField] private LanguageDictionary languageDict = new LanguageDictionary() { { SystemLanguage.English, "" } };

        /// <summary>
        /// Returns the string in the current selected language
        /// </summary>
        public virtual string Data {
            get {
                if ( !languageDict.ContainsKey( CurrentLanguage ) ) {
                    Debug.LogWarningFormat( "Current language not found {0}. Displaying default {1}: {2} ", CurrentLanguage, defaultLanguage, languageDict[defaultLanguage] );
                    return languageDict[defaultLanguage];
                }
                return languageDict[CurrentLanguage];
            }
        }

        public virtual string GetData( SystemLanguage language ) {
            if ( !languageDict.ContainsKey( language ) ) {
                return null;
            }
            return languageDict[language];
        }

#if UNITY_EDITOR
        public virtual void Delete( SystemLanguage languageToKeep ) {
            List<SystemLanguage> delete = new List<SystemLanguage>();
            foreach ( var data in languageDict ) {
                if ( data.Key == languageToKeep ) {
                    continue;
                }
                delete.Add( data.Key );
            }
            foreach ( var del in delete ) {
                languageDict.Remove( del );
            }
            EditorUtility.SetDirty( this );
        }

        public virtual string GetSavedLanguages() {
            string output = "";
            foreach ( var data in languageDict ) {
                output += string.Format( "{0},", data.Key );
            }
            return output;
        }
#endif
    }

}