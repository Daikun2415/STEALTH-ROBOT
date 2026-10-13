using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム経過時間をカウントするクラス
/// </summary>
public class TimeController : MonoBehaviour
{
    /// <summary>
    /// 時間表示用テキスト
    /// </summary>
    [SerializeField]
    private Text timeText;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float elapsedTime;

    /// <summary>
    /// ゲーム経過時間をカウント
    /// </summary>
    private void Update()
    {
        this.elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(this.elapsedTime / 60.0f);
        int seconds = Mathf.FloorToInt(this.elapsedTime % 60.0f);
        int milliseconds = Mathf.FloorToInt((this.elapsedTime * 100.0f) % 100.0f);

        this.timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    /// <summary>
    /// 経過時間を取得
    /// </summary>
    /// <returns>経過時間</returns>
    public float GetElapsedTime()
    {
        return this.elapsedTime;
    }
}
