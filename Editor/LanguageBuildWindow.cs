using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using System.Linq;
using System.IO;
using OhmsLibraries.Localization;

public class LanguageBuildWindow : OdinEditorWindow {

    private const string AUDIO = "Audio", TEXT = "Text", GLOBAL = "global";

    [MenuItem( "Tools/Language/LanguageData Garbage Collector" )]
    private static void OpenWindow() {
        GetWindow<LanguageBuildWindow>().Show();
    }

    [InfoBox( "Este asset sirve para borrar datos innecesarios del idioma ANTES de hacer el Build. Se debe tener el proyecto en un versionador (git) para poder restaurar posteriormente los datos borrados." )]
    public SystemLanguage language;
    
    [HorizontalGroup( GLOBAL, width: 0.5f )]
    [AssetsOnly]
    public LanguageManager managerPrefab;

    [Button( ButtonSizes.Medium )]
    [HorizontalGroup( GLOBAL, width:0.5f )]
    public void FindPrefab() {
        var candidates = Resources.FindObjectsOfTypeAll<LanguageManager>();
        managerPrefab = (from c in candidates where (c.gameObject.scene == null) select c).FirstOrDefault();
    }
    #region AUDIO
    [TabGroup( AUDIO )]
    [TableList]
    public AudioInfo[] audios;

    [TabGroup( AUDIO )]
    [Button( "Find AudioData", ButtonSizes.Medium )]
    public void FindAudioData() {
        var aux = Resources.FindObjectsOfTypeAll<AudioData>();
        audios = new AudioInfo[aux.Length];
        for ( int i = 0; i < audios.Length; i++ ) {
            var info = new AudioInfo( aux[i] );
            audios[i] = info;
        }
    }

    [TabGroup( AUDIO )]
    public SystemLanguage audioLanguage;

    [TabGroup( AUDIO)]
    [GUIColor(0.8f, 0, 0)]
    [Button( "Delete Audios", ButtonSizes.Large )]
    public void DeleteAudio() {
        int count = audios.Length;
        for ( int i = 0; i < count; i++ ) {
            var progress = (float) i / (count - 1);
            EditorUtility.DisplayProgressBar( "Borrando", string.Format( "Borrando: {0} {1}", audios[i].data.name, progress ), progress );
            audios[i].data.Delete( audioLanguage );
            audios[i].UpdateLanguages();
        }
        EditorUtility.ClearProgressBar();
    }

    [TabGroup( AUDIO )]
    [GUIColor( 0.5f, 0.8f, 0 )]
    [Button( "Unreference Audios", ButtonSizes.Large )]
    public void UnreferenceAudios() {
        int count = audios.Length;
        for ( int i = 0; i < count; i++ ) {
            EditorUtility.DisplayProgressBar( "Borrando", string.Format( "Quitando referencia: {0}", audios[i].data.name ), i / ( count - 1 ) );
            audios[i].data.Unreference( audioLanguage );
            audios[i].UpdateLanguages();
        }
        EditorUtility.ClearProgressBar();
    }

    #endregion

    #region TEXT
    [TabGroup( TEXT )]
    [TableList]
    public TextInfo[] texts;

    [TabGroup( TEXT )]
    [Button( "Find TextData", ButtonSizes.Medium )]
    public void FindTextData() {
        var aux = Resources.FindObjectsOfTypeAll<TextData>();
        texts = new TextInfo[aux.Length];
        for ( int i = 0; i < texts.Length; i++ ) {
            var info = new TextInfo( aux[i] );
            texts[i] = info;
        }
    }

    [TabGroup( TEXT )]
    public SystemLanguage textLanguage;

    [TabGroup( TEXT )]
    [GUIColor( 0.8f, 0, 0 )]
    [Button( "Delete Text", ButtonSizes.Large )]
    public void DeleteText() {
        int count = texts.Length;
        for ( int i = 0; i < count; i++ ) {
            EditorUtility.DisplayProgressBar( "Borrando", string.Format( "Borrando: {0}", texts[i].data.name ), i / count - 1 );
            texts[i].data.Delete( textLanguage );
            texts[i].UpdateLanguages();
        }
        EditorUtility.ClearProgressBar();
    }
    #endregion

    [TabGroup(TEXT)]
    [Button("Dump to File", ButtonSizes.Medium)]
    public void DumpToText() {
        int count = texts.Length;
        Debug.LogFormat( "About to dump {0} TextData", count );
        using ( StreamWriter writer = new StreamWriter( "Assets/LanguageData/dump.txt", false ) ) {
            for ( int i = 0; i < count; i++ ) {
                EditorUtility.DisplayProgressBar( "Dumping", string.Format( "Dumping: {0} {1}", texts[i].data.name, (i / count - 1) *100 ), i / count - 1 );
                writer.WriteLine( texts[i].data.GetData( textLanguage ) );
            }
            AssetDatabase.ImportAsset( "Assets/LanguageData/dump.txt" );
        }
        EditorUtility.ClearProgressBar();
    }
}

public class AudioInfo {
    public AudioData data;
    public string languages;

    public AudioInfo( AudioData data ) {
        this.data = data;
        UpdateLanguages();
    }

    public void UpdateLanguages() {
        languages = data.GetSavedLanguages();
    }
}

public class TextInfo {
    public TextData data;
    public string languages;

    public TextInfo( TextData data ) {
        this.data = data;
        UpdateLanguages();
    }

    public void UpdateLanguages() {
        languages = data.GetSavedLanguages();
    }
}
