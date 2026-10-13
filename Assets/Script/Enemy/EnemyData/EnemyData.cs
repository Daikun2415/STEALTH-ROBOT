using System;

/// <summary>
/// 敵のステータス
/// </summary>
[Serializable]
public class EnemyData
{
    /// <summary>
    /// 敵ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 敵の名前
    /// </summary>
    public string EnemyName { get; set; }

    /// <summary>
    /// 巡回時の移動速度
    /// </summary>
    public float MoveSpeed { get; set; }

    /// <summary>
    /// プレイヤー追跡時の移動速度
    /// </summary>
    public float ChaseSpeed { get; set; }

    /// <summary>
    /// プレイヤーを発見できる距離
    /// </summary>
    public float SightDistance { get; set; }

    /// <summary>
    /// プレイヤーを発見できる角度
    /// </summary>
    public float SightAngle { get; set; }

    /// <summary>
    /// 次の弾を発射するまでの時間
    /// </summary>
    public float ShotInterval { get; set; }
}