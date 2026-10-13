using UnityEngine;

/// <summary>
/// 追跡State
/// </summary>
public class ChaseState : IEnemyState
{
    /// <summary>
    /// 敵の元スクリプト
    /// </summary>
    private EnemyController enemyController;

    /// <summary>
    /// 追跡状態の作成
    /// </summary>
    /// <param name="enemyController">敵の処理</param>
    public void OnEnter(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        this.enemyController.ResetLostPlayerTime();
        Debug.Log("追跡状態です");
    }

    /// <summary>
    /// 追跡処理
    /// </summary>
    public void OnUpdate()
    {
        // プレイヤーが視野角にいるか
        if (this.enemyController.CanSeePlayer())
        {
            // いる間はタイマーをリセット
            this.enemyController.ResetLostPlayerTime();
        }
        else
        {
            // プレイヤーを見失った時間を加算
            this.enemyController.AddLostPlayerTime();

            // 一定時間たったら巡回に戻る
            if (this.enemyController.IsLostPlayer())
            {
                this.enemyController.PlayNormalBGM();
                this.enemyController.ChangeToReturnToPatrol();
                return;
            }
        }

        // プレイヤーに向かって移動
        this.enemyController.MoveToPlayer();

        // 射撃
        this.enemyController.Shoot();
    }

    /// <summary>
    /// 追跡状態を抜けるときの処理
    /// </summary>
    public void OnExit()
    {
        Debug.Log("追跡状態から移行");
    }
}
