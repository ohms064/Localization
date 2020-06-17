using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using OhmsLibraries.Localization.Setters;
using Sirenix.Serialization;

namespace OhmsLibraries.Localization {
    [DefaultExecutionOrder( -100 )]
    public class LanguageManager : SerializedMonoBehaviour {
        public SystemLanguage currentLanguage = SystemLanguage.English;
        public bool useSystemLanguage;
        public static bool alreadyInitialized = false;
        [OdinSerialize]
        private ILanguageObtainer _languageObtainer;

        private event System.Action _onLanguageChanged;

        public static LanguageManager Instance { get; private set; }

#if UNITY_EDITOR
        [ShowInInspector, LabelText( "Listeners" ), InlineEditor]
        private LanguageListener[] Editor_Listeners {
            get => FindObjectsOfType<LanguageListener>();
        }
#endif

        private void Awake () {
            if ( Instance != null ) {
                return;
            }
            DontDestroyOnLoad( gameObject );
            Instance = this;
            if ( alreadyInitialized )
                return;
            alreadyInitialized = true;
            //TextData.currentDefault = defaultLanguage;
        }
        private void Start () {
            if ( _languageObtainer == null ) return;
            Debug.Log( $"Language obtainer present. {_languageObtainer.HasSavedLanguage()}" );
            if ( useSystemLanguage && !_languageObtainer.HasSavedLanguage() ) {
                currentLanguage = Application.systemLanguage;
            }
            else if ( _languageObtainer.HasSavedLanguage() ) {
                currentLanguage = _languageObtainer.GetLanguage();
            }
        }


        private void OnDestroy () {
            if ( Instance == this ) {
                Instance = null;
            }
        }

        public void Register ( LanguageListener listener ) {
            _onLanguageChanged += listener.OnLanauageChanged;
        }

        public void Unregister ( LanguageListener listener ) {
            _onLanguageChanged -= listener.OnLanauageChanged;
        }

        public void ChangeLanguage ( SystemLanguage language ) {
            Instance.currentLanguage = language;
            Instance._onLanguageChanged?.Invoke();
        }

        public void ChangeLanguage ( int language ) {
            ChangeLanguage( (SystemLanguage) language );
        }

    }


}