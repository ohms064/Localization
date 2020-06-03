using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using LanguageSpriteDictionary = System.Collections.Generic.Dictionary<UnityEngine.SystemLanguage, UnityEngine.Sprite>;
namespace OhmsLibraries.Localization {
    [CreateAssetMenu( menuName = "Language System/Sprite Data" )]
    public class SpriteData : SerializedScriptableObject {
        public static SystemLanguage currentLanguage = SystemLanguage.English, currentDefault = SystemLanguage.English;

        [SerializeField] protected LanguageSpriteDictionary languageDict = new LanguageSpriteDictionary();

        public Sprite Data {
            get {
                if ( !languageDict.ContainsKey( currentLanguage ) ) {
                    Debug.Log( "Current Language error. Using Default" );
                    return languageDict[currentDefault];
                }
                return languageDict[currentLanguage];
            }
        }
    }
}