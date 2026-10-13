using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ステージ情報パネルの制御クラス
/// </summary>
public class StagePanelController : MonoBehaviour
{
    /// <summary>
    /// ステージ情報パネル
    /// </summary>
    [SerializeField]
    private GameObject stageInformationPanel;

    /// <summary>
    /// ステージID表示テキスト
    /// </summary>
    [SerializeField]
    private Text stageIdText;

    /// <summary>
    /// クリアタイム表示テキスト
    /// </summary>
    [SerializeField]
    private Text clearTimeText;

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
    /// 選択中のステージID
    /// </summary>
    private int selectedStageId;

    /// <summary>
    /// 選択中のクリアタイム
    /// </summary>
    private float selectedClearTime;

    /// <summary>
    /// 最初にパネルを非表示にする
    /// </summary>
    private void Start()
    {
        this.stageInformationPanel.SetActive(false);
    }

    /// <summary>
    /// ステージ情報パネルを開く
    /// </summary>
    /// <param name="stageId">ステージID</param>
    /// <param name="clearTime">クリアタイム</param>
    public void OpenPanel(int stageId, float clearTime)
    {
        this.selectedStageId = stageId;
        this.selectedClearTime = clearTime;

        if (stageId == 0)
        {
            // IDが0ならチュートリアルステージ
            this.stageIdText.text = "TutorialStage";
        }
        else
        {
            // IDが0以外なら通常ステージ
            this.stageIdText.text = $"Stage {stageId}";
        }

        // クリアタイムを表示
        this.clearTimeText.text = string.Format("{0}秒以内にクリア", clearTime);

        // ミッションデータを取得
        MissionData missionData = MissionSaveManager.Instance.GetMission(stageId);

        // 取得済みの星を表示
        for (int i = 0; i < this.stars.Length; i++)
        {
            bool isAchieved = missionData.Stars[i];
            this.stars[i].sprite = isAchieved ? this.starSprite : this.emptyStarSprite;
        }

        // パネルを表示
        this.stageInformationPanel.SetActive(true);
    }

    /// <summary>
    /// ステージ情報パネルを閉じる
    /// </summary>
    public void ClosePanel()
    {
        this.stageInformationPanel.SetActive(false);
    }

    /// <summary>
    /// GameManagerにステージ情報を渡してステージシーンに遷移する
    /// </summary>
    public void StartStage()
    {
        GameManager.SetStageData(this.selectedStageId,　this.selectedClearTime);

        SceneManager.LoadScene("StageScene");
    }
}
