using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OhmsLibraries.Localization {
    public class BaseData<T> : MonoBehaviour {

        public static SystemLanguage currentLanguage, currentDefault;

        protected Dictionary<SystemLanguage, T> languageDict;

        public T Data {
            get {
                if ( !languageDict.ContainsKey( currentLanguage ) ) {
                    return languageDict[currentDefault];
                }
                return languageDict[currentLanguage];
            }
        }

#if UNITY_EDITOR
        public BasicData<T>[] languageStrings = new BasicData<T>[] { new BasicData<T> { language = SystemLanguage.Spanish }, new BasicData<T> { language = SystemLanguage.English }, new BasicData<T> { language = SystemLanguage.Korean }, new BasicData<T> { language = SystemLanguage.Portuguese } };
        public SystemLanguage language, defaultLanguage;

        public void Save() {
            languageDict = new Dictionary<SystemLanguage, T>();
            foreach ( var data in languageStrings ) {
                languageDict.Add( data.language, data.val );
            }
        }

        public void SetDefault() {
            currentDefault = defaultLanguage;
        }

#endif
    }

#if UNITY_EDITOR
    [System.Serializable]
    public class BasicData<T> {
        public SystemLanguage language;
        public T val;
    }
#endif
}