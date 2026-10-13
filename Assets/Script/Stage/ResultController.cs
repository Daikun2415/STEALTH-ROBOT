using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト画面の管理クラス
/// </summary>
public class ResultController : MonoBehaviour
{
    /// <summary>
    /// リザルトパネル
    /// </summary>
    [SerializeField]
    private GameObject resultPanel;

    /// <summary>
    /// 星のImage
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
    /// タイムを表示するテキスト
    /// </summary>
    [SerializeField]
    private Text timeText;

    /// <summary>
    /// ポーズを管理するクラス
    /// </summary>
    [SerializeField]
    private PauseController pauseController;

    /// <summary>
    /// プレイヤーが一度でも敵に発見されたか
    /// </summary>
    private bool wasDetected = false;

    /// <summary>
    /// ステージID
    /// </summary>
    private int stageId;

    /// <summary>
    /// ステージBGM管理クラス
    /// </summary>
    [SerializeField]
    private StageBGMManager stageBGMManager;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        foreach (Image star in this.stars)
        {
            star.sprite = this.emptyStarSprite;
        }

        this.resultPanel.SetActive(false);
    }

    /// <summary>
    /// ステージIDを設定する
    /// </summary>
    /// <param name="stageId">ステージID</param>
    public void Initialize(int stageId)
    {
        this.stageId = stageId;
    }

    /// <summary>
    /// プレイヤーが発見されたことを記録
    /// </summary>
    public void SetDetected()
    {
        this.wasDetected = true;
    }

    /// <summary>
    /// プレイヤーが発見されたかを取得
    /// </summary>
    /// <returns>発見されていればtrue、違えばfalse</returns>
    public bool CheckWasDetected()
    {
        return this.wasDetected;
    }

    /// <summary>
    /// リザルト画面の表示と星の表示を更新とクリアタイムの表示
    /// </summary>
    /// <param name="isUndetected">一度も発見されなかったか</param>
    /// <param name="isTimeClear">特定のタイム以下か</param>
    /// <param name="hasBonusItem">ボーナスアイテムを取得したか</param>
    /// <param name="clearTime">クリアタイム</param>
    public void ShowResult(bool isUndetected, bool isTimeClear, bool hasBonusItem, float clearTime)
    {
        // ポーズできないようにする
        this.pauseController.LockPause();

        // リザルトBGMを再生する
        this.stageBGMManager.PlayBGM(StageBGMManager.BGMType.Result);

        this.resultPanel.SetActive(true);

        // 今回獲得した星を配列にまとめる
        bool[] achievedStars =
        {
            isUndetected,
            isTimeClear,
            hasBonusItem
        };

        // 星を保存する
        MissionSaveManager.Instance.SaveMission(this.stageId, true, achievedStars);

        // 保存されている星を読み込んで表示する
        MissionData missionData = MissionSaveManager.Instance.GetMission(this.stageId);

        for (int i = 0; i < this.stars.Length; i++)
        {
            this.SetStar(i, missionData != null && missionData.Stars[i]);
        }

        // タイムを取得して、リザルト画面に表示する
        int minutes = Mathf.FloorToInt(clearTime / 60.0f);
        int seconds = Mathf.FloorToInt(clearTime % 60.0f);
        int milliseconds = Mathf.FloorToInt((clearTime * 100.0f) % 100.0f);

        this.timeText.text = string.Format("Time : {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    /// <summary>
    /// 星の表示を切り替える
    /// </summary>
    /// <param name="index">星の番号</param>
    /// <param name="isAchieved">条件を達成したか</param>
    private void SetStar(int index, bool isAchieved)
    {
        this.stars[index].sprite = isAchieved ? this.starSprite : this.emptyStarSprite;
    }
}
