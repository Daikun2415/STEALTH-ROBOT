using UnityEngine;

/// <summary>
/// ステージ内部の管理
/// </summary>
public class StageController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのスポーン位置
    /// </summary>
    [SerializeField]
    private Transform playerSpawnPoint;

    /// <summary>
    /// ステージ内の敵
    /// </summary>
    [SerializeField]
    private EnemyController[] enemies;

    /// <summary>
    /// ゴール制御クラス
    /// </summary>
    [SerializeField]
    private GoalSystem goalSystem;

    /// <summary>
    /// ステージ開始時の処理
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <param name="resultController">リザルト制御</param>
    /// <param name="enemyDataLoader">敵データ読み込み</param>
    /// <param name="timeController">タイム制御クラス</param>
    /// <param name="clearTime">星がもらえるクリアタイム</param>
    /// <param name="stageBGMManager">ステージBGM管理クラス</param>
    public void InitializeStage(PlayerController player, ResultController resultController, EnemyDataLoader enemyDataLoader, TimeController timeController, float clearTime, StageBGMManager stageBGMManager)
    {
        // プレイヤーをスポーン位置に移動
        player.transform.position = this.playerSpawnPoint.position;

        foreach (EnemyController enemy in this.enemies)
        {
            enemy.Initialize(player, resultController, enemyDataLoader, stageBGMManager);
        }

        this.goalSystem.Initialize(resultController, timeController, clearTime);
    }
}
