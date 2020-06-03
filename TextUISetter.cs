using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace OhmsLibraries.Localization {
    public class TextUISetter : LanguageListener {

        public TextData data;
        private Text uiText;

        private void Start() {
            uiText = GetComponent<Text>();
            uiText.text = data.Data;
        }

        public void SetText( TextData newData ) {
            data = newData;
            if ( uiText == null ) {
                uiText = GetComponent<Text>();
                return;
            }
            uiText.text = data.Data;
        }

        public void UpdateText() {
            uiText.text = data.Data;
        }

        public override void OnLanauageChanged() {
            UpdateText();
        }
#if UNITY_EDITOR
        private void Reset() {
            uiText = GetComponent<Text>();
        }
#endif
    }
}