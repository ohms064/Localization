using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LanguageAudioDictionary = System.Collections.Generic.Dictionary<UnityEngine.SystemLanguage, UnityEngine.AudioClip>;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OhmsLibraries.Localization.Data {
    [CreateAssetMenu( menuName = "Language System/Audio Data" )]
    public class AudioData : SerializedScriptableObject {
        public static SystemLanguage CurrentLanguage => LanguageManager.Instance.currentLanguage;

        public SystemLanguage defaultLanguage = SystemLanguage.English;

        [SerializeField] protected LanguageAudioDictionary languageDict = new LanguageAudioDictionary();
        public AudioClip Data {
            get {
                if ( !languageDict.ContainsKey( CurrentLanguage ) ) {
                    Debug.Log( "Current Language error. Using Default" );
                    return languageDict[defaultLanguage];
                }
                return languageDict[CurrentLanguage];
            }
        }

#if UNITY_EDITOR
        public void Delete( SystemLanguage languageToKeep ) {
            List<SystemLanguage> delete = new List<SystemLanguage>();
            foreach ( var data in languageDict ) {
                if ( data.Key == languageToKeep ) {
                    continue;
                }
                delete.Add( data.Key );
            }

            foreach ( var del in delete ) {
                string assetPath = AssetDatabase.GetAssetPath( languageDict[del] );
                languageDict.Remove( del );
                AssetDatabase.DeleteAsset( assetPath );
            }
            EditorUtility.SetDirty( this );
        }

        public void Unreference( SystemLanguage languageToKeep ) {
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

        public string GetSavedLanguages() {
            string output = "";
            foreach ( var data in languageDict ) {
                output += string.Format( "{0},", data.Key );
            }
            return output;
        }
#endif
    }
}