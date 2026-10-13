#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Play開始時のシーンを設定するクラス
/// </summary>
[InitializeOnLoad]
public static class PlayStartSceneSetter
{
    /// <summary>
    /// タイトルシーンのパス
    /// </summary>
    private const string TitleScenePath = "Assets/Scenes/TitleScene.unity";

    /// <summary>
    /// 初期設定
    /// </summary>
    static PlayStartSceneSetter()
    {
        SceneAsset titleScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(TitleScenePath);

        EditorSceneManager.playModeStartScene = titleScene;
    }
}

#endif