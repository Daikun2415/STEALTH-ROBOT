using UnityEngine;

/// <summary>
/// ゴール地点の制御クラス
/// </summary>
public class GoalSystem : MonoBehaviour
{
    /// <summary>
    /// リザルト制御クラス
    /// </summary>
    private ResultController resultController;

    /// <summary>
    /// タイム制御クラス
    /// </summary>
    private TimeController timeController;

    /// <summary>
    /// 星がもらえるクリアタイム
    /// </summary>
    private float clearTime;

    /// <summary>
    /// ボーナスアイテム制御クラス
    /// </summary>
    [SerializeField]
    private BonusItem bonusItem;

    /// <summary>
    /// ゴールしてるかどうか
    /// </summary>
    private bool isGoal;

    /// <summary>
    /// 必要な情報を設定する
    /// </summary>
    /// <param name="resultController">リザルト制御クラス</param>
    /// <param name="timeController">タイム制御クラス</param>
    /// <param name="clearTime">星がもらえるクリアタイム</param>
    public void Initialize(ResultController resultController, TimeController timeController, float clearTime)
    {
        this.resultController = resultController;
        this.timeController = timeController;
        this.clearTime = clearTime;
    }

    /// <summary>
    /// プレイヤーがゴール地点に接触したときの処理
    /// </summary>
    /// <param name="other">接触したオブジェクト</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.isGoal || !other.CompareTag("Player"))
        {
            return;
        }

        this.isGoal = true;

        float elapsedTime = this.timeController.GetElapsedTime();

        // 敵に１度でも見つかったかを取得
        bool isUndetected = !this.resultController.CheckWasDetected();

        // クリアタイムは特定の秒数以下だったかどうかを取得
        bool isTimeClear = elapsedTime <= this.clearTime;

        // ボーナスアイテムを取得したかを取得
        bool hasBonusItem = this.bonusItem.CheckIsCollected();

        this.resultController.ShowResult(isUndetected, isTimeClear, hasBonusItem, elapsedTime);

        Time.timeScale = 0.0f;
    }
}
