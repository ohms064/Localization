using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using OhmsLibraries.Localization.Data;

namespace OhmsLibraries.Localization.Setters {
    public class TextMeshUISetter : LanguageListener {
        [Required, HideInInlineEditors]
        public TextMeshProUGUI meshText;
        public TextData data;

        private void Start() {
            meshText = GetComponent<TextMeshProUGUI>();
            UpdateText();
        }

        public void SetText( TextData newData ) {
            data = newData;
            if ( meshText == null ) {
                meshText = GetComponent<TextMeshProUGUI>();
                return;
            }
            meshText.text = data.Data;
        }

        public void UpdateText() {
            meshText.text = data.Data;
        }

        public override void OnLanauageChanged() {
            UpdateText();
        }

#if UNITY_EDITOR
        private void Reset() {
            meshText = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}