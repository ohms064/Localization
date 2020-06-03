using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using OhmsLibraries.Localization;
using Sirenix.Utilities.Editor;
using System.Linq;
using TMPro;

public class LanguageDataCreatorWindow : OdinEditorWindow
{
    [MenuItem("Tools/Language/Data Creator")]
    public static void OpenLanguageWindow() {
        GetWindow<LanguageDataCreatorWindow>().Show();
    }

    [FolderPath]
    public string defaultPath = "Assets/";

    [TableList]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, OnTitleBarGUI = "Localization_TitleBar" )]
    public List<LanguageCreatorItem_TMP> localization = new List<LanguageCreatorItem_TMP>();

    private void Localization_TitleBar() {
        if ( SirenixEditorGUI.ToolbarButton( EditorIcons.MagnifyingGlass ) ) {
            var result = FindObjectsOfType<TextMeshUISetter>();
            localization = (from r in result select new LanguageCreatorItem_TMP { setter = r , defaultPath = defaultPath }).ToList();
        }
    }

    [ListDrawerSettings( HideAddButton = true, HideRemoveButton = true, OnTitleBarGUI = "Tmp_TitleBar",
        OnBeginListElementGUI = "Tmp_Begin", OnEndListElementGUI = "Tmp_End" )]
    public List<TextMeshProUGUI> tmp = new List<TextMeshProUGUI>();

    private void Tmp_TitleBar() {
        if ( SirenixEditorGUI.ToolbarButton( EditorIcons.MagnifyingGlass ) ) {
            var result = FindObjectsOfType<TextMeshProUGUI>();
            tmp = (from r in result where !r.GetComponent<TextMeshUISetter>() select r).ToList();
        }
    }

    private void Tmp_Begin( int index ) {
        GUILayout.BeginHorizontal( GUILayout.MaxWidth( 700f ), GUILayout.MinWidth( 300f ) );
    }

    private void Tmp_End( int index ) {
        GUILayout.Label( tmp[index].text, GUILayout.MinWidth(300f) );
        if ( SirenixEditorGUI.ToolbarButton( "Make Localizable" ) ) {
            tmp[index].gameObject.AddComponent<TextMeshUISetter>();
            tmp.RemoveAt( index );
            SearchLocalization();
            EditorSceneManager.MarkSceneDirty( EditorSceneManager.GetActiveScene() );
        }
        GUILayout.EndHorizontal();
    }

    private void SearchLocalization() {
        var result = FindObjectsOfType<TextMeshUISetter>();
        localization = (from r in result select new LanguageCreatorItem_TMP { setter = r }).ToList();
    }

}


public class LanguageCreatorItem_TMP {
    [DisableInEditorMode, DisableInPlayMode]
    public TextMeshUISetter setter;
    [ShowInInspector]
    public string currentText => setter.meshText?.text ?? "" ;
    [ShowInInspector]
    public TextData assignedData {
        get => setter.data;
        set {
            setter.data = value;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            //SerializedObject serText = new SerializedObject( setter );
            //serText.FindProperty( "data" ).objectReferenceValue = value;
        }
    }
    [HideInInspector]
    public string defaultPath = "";
    [HideInInspector]
    public string defaultName => currentText.Split( ' ' ).FirstOrDefault() ?? "";

    [DisableIf( "assignedData" ), Button, TableColumnWidth(2)]
    public void CreateData() {
        var path = EditorUtility.SaveFilePanelInProject( "Data path", defaultName, "asset", "", defaultPath );
        if ( string.IsNullOrEmpty( path ) ) return;
        var scriptable = TextData.CreateInstance<TextData>();
        AssetDatabase.CreateAsset( scriptable, path );
        AssetDatabase.SaveAssets();
        assignedData = scriptable;
    }
}
