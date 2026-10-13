using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// リトライ処理を管理するクラス
/// </summary>
public class RetryController : MonoBehaviour
{
    /// <summary>
    /// 現在のステージを最初からやり直す
    /// </summary>
    public void Retry()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
