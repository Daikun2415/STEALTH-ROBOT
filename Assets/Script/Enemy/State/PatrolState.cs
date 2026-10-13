using UnityEngine;

/// <summary>
/// 巡回State
/// </summary>
public class PatrolState : IEnemyState
{
    /// <summary>
    /// 敵の元スクリプト
    /// </summary>
    private EnemyController enemyController;

    /// <summary>
    /// 目的地到着後の経過時間
    /// </summary>
    private float waitTimer;

    /// <summary>
    /// 巡回状態に入った時の処理
    /// </summary>
    /// <param name="enemyController">敵の処理</param>
    public void OnEnter(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        this.waitTimer = 0.0f;
        Debug.Log("巡回状態です");
    }

    /// <summary>
    /// 巡回処理
    /// </summary>
    public void OnUpdate()
    {
        // プレイヤーを発見したら追跡
        if (this.enemyController.CanSeePlayer())
        {
            this.enemyController.PlayDiscoveredBGM();
            this.enemyController.ChangeState(this.enemyController.ChaseState);
            return;
        }

        // イージーで発見待ち中なら停止
        if (this.enemyController.CheckIsDiscovering())
        {
            return;
        }

        // 巡回ルートから離れたら復帰
        if (this.enemyController.IsOutsidePatrolRoute())
        {
            this.enemyController.ChangeToReturnToPatrol();
            return;
        }

        // 目的地に到着したら少し待ってからポイントを変える
        if (this.enemyController.IsArrived())
        {
            // 待機中はその場で停止
            this.enemyController.Stop();

            this.waitTimer += Time.fixedDeltaTime;

            if (this.waitTimer >= this.enemyController.GetPatrolWaitTime())
            {
                this.waitTimer = 0.0f;
                this.enemyController.ChangeTarget();
            }

            return;
        }

        // 目的地に移動
        this.enemyController.MoveToTarget();
    }

    /// <summary>
    /// 巡回状態を抜けるときの処理
    /// </summary>
    public void OnExit()
    {
        Debug.Log("巡回状態から移行");
    }
}
