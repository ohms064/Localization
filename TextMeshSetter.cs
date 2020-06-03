using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
namespace OhmsLibraries.Localization {
    public class TextMeshSetter : LanguageListener {
        [Required, HideInInlineEditors]
        public TextMeshPro meshText;
        public TextData data;

        private void Start() {
            meshText = GetComponent<TextMeshPro>();
            UpdateText();
        }

        public void SetText( TextData newData ) {
            data = newData;
            if ( meshText == null ) {
                meshText = GetComponent<TextMeshPro>();
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
            meshText = GetComponent<TextMeshPro>();
        }
#endif
    }
}
