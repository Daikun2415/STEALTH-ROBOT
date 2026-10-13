using UnityEngine;

/// <summary>
/// 巡回ルートに戻るState
/// </summary>
public class ReturnToPatrolState : IEnemyState
{
    /// <summary>
    /// 敵の元スクリプト
    /// </summary>
    private EnemyController enemyController;

    /// <summary>
    /// 巡回復帰状態の作成
    /// </summary>
    /// <param name="enemyController">敵の処理</param>
    public void OnEnter(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        Debug.Log("巡回復帰状態です");
    }

    /// <summary>
    /// 巡回ルートに戻る処理
    /// </summary>
    public void OnUpdate()
    {
        // 巡回ポイントに移動
        this.enemyController.MoveToTarget();

        // 巡回ポイントに戻ったら通常巡回へ
        if (this.enemyController.IsArrived())
        {
            this.enemyController.ChangeState(this.enemyController.PatrolState);
            this.enemyController.ChangeTarget();
        }
    }

    /// <summary>
    /// 巡回復帰状態を抜けるときの処理
    /// </summary>
    public void OnExit()
    {
        Debug.Log("巡回復帰状態から移行");
    }
}
