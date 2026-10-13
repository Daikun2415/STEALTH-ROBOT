using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルシーンを管理するクラス
/// </summary>
public class TitleScene : MonoBehaviour
{
    /// <summary>
    /// タイトルシーンの初期化処理
    /// </summary>
    private void Start()
    {
        AudioVolumeManager.Instance.LoadVolume();
    }

    /// <summary>
    /// ボタンを押してステージセレクトシーンに遷移する
    /// </summary>
    public void OnTapGameStart()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
