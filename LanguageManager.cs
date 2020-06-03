using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WasdStudio.GameConfig;

namespace OhmsLibraries.Localization {
    [DefaultExecutionOrder( -100 )]
    public class LanguageManager : MonoBehaviour {
        public SystemLanguage currentLanguage = SystemLanguage.English;
        public bool useSystemLanguage;
        public static bool alreadyInitialized = false;

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
            if ( useSystemLanguage && !GameConfig.Instance.Language.HasSaveData() ) {
                currentLanguage = Application.systemLanguage;
            }
            else if ( GameConfig.Instance.Language.HasSaveData() ) {
                currentLanguage = GameConfig.Instance.Language.SavedData;
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

    public abstract class LanguageListener : MonoBehaviour {
        public abstract void OnLanauageChanged ();

        protected virtual void OnEnable () {
            LanguageManager.Instance.Register( this );
        }

        protected virtual void OnDisable () {
            LanguageManager.Instance?.Unregister( this );
        }
    }
}