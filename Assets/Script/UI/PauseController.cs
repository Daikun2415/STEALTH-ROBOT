using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// ポーズ画面の制御
/// </summary>
public class PauseController : MonoBehaviour
{
    /// <summary>
    /// ポーズ入力のインプットアクション
    /// </summary>
    [SerializeField]
    private InputAction pauseAction;

    /// <summary>
    /// ポーズ画面のパネル
    /// </summary>
    [SerializeField]
    private GameObject pausePanal;

    /// <summary>
    /// リザルト管理クラス
    /// </summary>
    [SerializeField]
    private ResultController resultController;

    /// <summary>
    /// タイムミッション表示テキスト
    /// </summary>
    [SerializeField]
    private Text timeMissionText;

    /// <summary>
    /// ミッションの星Image
    /// </summary>
    [SerializeField]
    private Image[] stars;

    /// <summary>
    /// 星が入っていないSprite
    /// </summary>
    [SerializeField]
    private Sprite emptyStarSprite;

    /// <summary>
    /// 星が入っているSprite
    /// </summary>
    [SerializeField]
    private Sprite starSprite;

    /// <summary>
    /// ポーズ状態確認の真偽型
    /// </summary>
    private bool isPaused;

    /// <summary>
    /// ポーズ禁止状態確認の真偽型
    /// </summary>
    private bool isPausedLooked = false;

    /// <summary>
    /// ゲームシーンの管理クラス
    /// </summary>
    [SerializeField]
    private GameManager gameManager;

    /// <summary>
    /// ポーズ時のSE
    /// </summary>
    [SerializeField]
    private AudioClip pauseSE;

    /// <summary>
    /// インプットアクションの有効化、パネル非表示
    /// </summary>
    private void Start()
    {
        this.pauseAction.Enable();
        this.pausePanal.SetActive(false);
    }

    /// <summary>
    /// キー入力でポーズ状態切り替え
    /// </summary>
    private void Update()
    {
        if (this.pauseAction.WasPressedThisFrame() && !this.isPausedLooked)
        {
            this.TogglePause();
        }
    }

    /// <summary>
    /// ポーズ状態を切り替え処理
    /// </summary>
    private void TogglePause()
    {
        this.isPaused = !this.isPaused;
        this.pausePanal.SetActive(this.isPaused);

        if (this.isPaused)
        {
            this.UpdateMission();
            SEManager.Instance.StopSE();
            SEManager.Instance.PlaySE(this.pauseSE);
        }

        Time.timeScale = this.isPaused ? 0.0f : 1.0f;
    }

    /// <summary>
    /// ポーズできないようにする
    /// </summary>
    public void LockPause()
    {
        this.isPausedLooked = true;
    }

    /// <summary>
    /// ミッション内容を更新
    /// </summary>
    private void UpdateMission()
    {
        // ミッションクリアタイムを表示
        float clearTime = this.gameManager.GetClearTime();
        this.timeMissionText.text = string.Format("{0}秒以内にクリア", clearTime);

        // 現在のステージのミッションデータを取得
        MissionData missionData = MissionSaveManager.Instance.GetMission(this.gameManager.GetStageId());

        if (missionData == null)
        {
            return;
        }

        // 取得済みの星を表示
        for (int i = 0; i < this.stars.Length; i++)
        {
            bool isAchived = missionData.Stars[i];
            this.stars[i].sprite = isAchived ? this.starSprite : this.emptyStarSprite;
        }
    }
}
