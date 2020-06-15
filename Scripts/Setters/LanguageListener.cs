using OhmsLibraries.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OhmsLibraries.Localization.Setters {
    public abstract class LanguageListener : MonoBehaviour {
        public abstract void OnLanauageChanged ();

        protected virtual void Awake () {
            LanguageManager.Instance.Register( this );
        }

        protected virtual void OnDestroy () {
            LanguageManager.Instance?.Unregister( this );
        }
    }
}