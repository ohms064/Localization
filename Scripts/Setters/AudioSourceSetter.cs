using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OhmsLibraries.Localization.Data;

namespace OhmsLibraries.Localization.Setter {
    public class AudioSourceSetter : MonoBehaviour {
        public AudioData data;
        private AudioSource source;

        private void Start() {
            source = GetComponent<AudioSource>();
            if ( data == null ) {
                return;
            }
            source.clip = data.Data;
        }

        public void SetClip( AudioData newData ) {
            data = newData;
            if ( source == null ) {
                source = GetComponent<AudioSource>();
                return;
            }
            source.clip = data.Data;
        }

        public void Toggle() {
            if ( source.isPlaying ) {
                source.Pause();
            }
            else {
                source.Play();
            }
        }

        public void HideAudio() {
            gameObject.SetActive( false );
        }

        public void Play() {
            source.Play();
        }

        public void Pause() {
            source.Pause();
        }

        public void UnPause() {
            source.UnPause();
        }

        public void Stop() {
            source.Stop();
        }
    }
}