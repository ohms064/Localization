using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LanguageRichTextDictionary = System.Collections.Generic.Dictionary<UnityEngine.SystemLanguage, OhmsLibraries.Localization.Data.RichText>;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace OhmsLibraries.Localization.Data {
    [CreateAssetMenu( menuName = "Language System/Multiline Text Data" )]
    public class FormatTextData : TextData {

        [SerializeField] protected LanguageRichTextDictionary multiLineDict = new LanguageRichTextDictionary();

        public override string Data {
            get {
                if ( !multiLineDict.ContainsKey( CurrentLanguage ) ) {
                    Debug.LogWarningFormat( "Current language not found {0}. Displaying default {1}: {2} ", CurrentLanguage, defaultLanguage, multiLineDict[CurrentLanguage].text );
                    return multiLineDict[CurrentLanguage].text;
                }
                return multiLineDict[CurrentLanguage].text;
            }
        }
#if UNITY_EDITOR
        public override void Delete( SystemLanguage languageToKeep ) {
            List<SystemLanguage> delete = new List<SystemLanguage>();
            foreach ( var data in multiLineDict ) {
                if ( data.Key == languageToKeep ) {
                    continue;
                }
                delete.Add( data.Key );
            }
            foreach ( var del in delete ) {
                multiLineDict.Remove( del );
            }
            EditorUtility.SetDirty( this );
        }

        public override string GetSavedLanguages() {
            string output = "";
            foreach ( var data in multiLineDict ) {
                output += string.Format( "{0},", data.Key );
            }
            return output;
        }

        public override string GetData( SystemLanguage language ) {
            if ( !multiLineDict.ContainsKey( language ) ) {
                return null;
            }
            return multiLineDict[language].text;
        }
#endif
    }

    [System.Serializable]
    public class RichText {
        [TextArea( 5, 80 )]
        public string text;
    }

}