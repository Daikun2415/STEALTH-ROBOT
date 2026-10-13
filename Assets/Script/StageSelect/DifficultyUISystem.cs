using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 難易度表示UIの制御クラス
/// </summary>
public class DifficultyUISystem : MonoBehaviour
{
    /// <summary>
    /// 難易度表示テキスト
    /// </summary>
    [SerializeField]
    private Text difficultyText;

    /// <summary>
    /// ステージ選択ボタン
    /// </summary>
    [SerializeField]
    private StageSelectButton[] stageSelectButtons;

    /// <summary>
    /// 初期設定
    /// </summary>
    private void Start()
    {
        DifficultyManager.Instance.SetDifficultyText(this.difficultyText);
        this.UpdateStageStars();
    }

    /// <summary>
    /// 難易度を変更する
    /// </summary>
    public void ChangeDifficulty()
    {
        DifficultyManager.Instance.ChangeDifficulty();
        this.UpdateStageStars();
    }

    /// <summary>
    /// ステージボタンの星を更新する
    /// </summary>
    private void UpdateStageStars()
    {
        foreach (StageSelectButton button in this.stageSelectButtons)
        {
            button.UpdateStars();
        }
    }
}
