using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージセレクトシーンに戻るボタンの制御クラス
/// </summary>
public class BackStageSelectSceneController : MonoBehaviour
{
    /// <summary>
    /// ステージセレクトシーンに戻る
    /// </summary>
    public void BackStageSelectScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StageSelectScene");
    }
}
