using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステージ選択ボタンの制御クラス
/// </summary>
public class StageSelectButton : MonoBehaviour
{
    /// <summary>
    /// ステージID
    /// </summary>
    [SerializeField]
    private int stageId;

    /// <summary>
    /// クリアタイム
    /// </summary>
    [SerializeField]
    private float clearTime;

    /// <summary>
    /// ステージ情報パネル
    /// </summary>
    [SerializeField]
    private StagePanelController stagePanelController;

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
    /// 獲得済みの星を表示する
    /// </summary>
    public void UpdateStars()
    {
        MissionData missionData = MissionSaveManager.Instance.GetMission(this.stageId);

        if (missionData == null)
        {
            return;
        }

        for (int i = 0; i < this.stars.Length; i++)
        {
            bool isAchieved = missionData.Stars[i];
            this.stars[i].sprite = isAchieved ? this.starSprite : this.emptyStarSprite;
        }
    }

    /// <summary>
    /// ステージ情報パネルを開く
    /// </summary>
    public void OpenStagePanel()
    {
        this.stagePanelController.OpenPanel(this.stageId, this.clearTime);
    }
}
