using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の処理
/// </summary>
public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// 設定された難易度
    /// </summary>
    private DifficultyManager.Difficulty difficulty;

    /// <summary>
    /// 巡回ポイントのリスト
    /// </summary>
    [SerializeField]
    private List<Transform> patrolPoints;

    /// <summary>
    /// 現在の巡回ポイントのインデックス
    /// </summary>
    private int targetPointIndex;

    /// <summary>
    /// リジッドボディ
    /// </summary>
    [SerializeField]
    private Rigidbody2D enemyRb;

    /// <summary>
    /// 巡回ルートから離れたと判断する距離
    /// </summary>
    [SerializeField]
    private float returnDistanse = 3.0f;

    /// <summary>
    /// 追跡するプレイヤーを取得
    /// </summary>
    private Transform player;

    /// <summary>
    /// プレイヤーのスクリプトを取得
    /// </summary>
    private PlayerController playerController;

    /// <summary>
    /// 敵ID
    /// </summary>
    [SerializeField]
    private int enemyId;

    /// <summary>
    /// ステータスデータ読み込むクラス
    /// </summary>
    private EnemyDataLoader enemyDataLoader;

    /// <summary>
    /// 読み込んだステータス
    /// </summary>
    private EnemyData enemyData;

    /// <summary>
    /// 弾丸発射位置のエンプティオブジェクト
    /// </summary>
    [SerializeField]
    private Transform shotPoint;

    /// <summary>
    /// 弾丸を生成するクラス
    /// </summary>
    [SerializeField]
    private BulletSpawner bulletSpawner;

    /// <summary>
    /// 弾丸発射時間を格納する変数
    /// </summary>
    private float shotTime;

    /// <summary>
    /// 視野角クラス
    /// </summary>
    [SerializeField]
    private EnemyVision enemyVision;

    /// <summary>
    /// プレイヤーが視野角を外れても追跡を続ける時間
    /// </summary>
    [SerializeField]
    private float chaseDuration = 3.0f;

    /// <summary>
    /// リザルト制御クラス
    /// </summary>
    private ResultController resultController;

    /// <summary>
    /// プレイヤーを見失ってからの経過時間
    /// </summary>
    private float lostPlayerTime;

    /// <summary>
    /// プレイヤーを発見するまでの残り時間
    /// </summary>
    private float discoveryTime;

    /// <summary>
    /// イージーモードでプレイヤー発見するまでの時間を設定
    /// </summary>
    [SerializeField]
    private float easyDiscoveryDuration = 3.0f;

    /// <summary>
    /// プレイヤーを発見待ち中かどうか
    /// </summary>
    private bool isDiscovering;

    /// <summary>
    /// 目的地到着後の待機時間
    /// </summary>
    [SerializeField]
    private float patrolWaitTime = 1.0f;

    /// <summary>
    /// 射撃SE
    /// </summary>
    [SerializeField]
    private AudioClip shootSE;

    /// <summary>
    /// ステージBGM管理クラス
    /// </summary>
    private StageBGMManager stageBGMManager;

    /// <summary>
    /// 巡回状態
    /// </summary>
    private PatrolState patrolState;

    /// <summary>
    /// 追跡状態
    /// </summary>
    private ChaseState chaseState;

    /// <summary>
    /// 巡回復帰状態
    /// </summary>
    private ReturnToPatrolState returnToPatrolState;

    /// <summary>
    /// 現在の状態
    /// </summary>
    private IEnemyState currentState;

    /// <summary>
    /// 巡回状態の公開用プロパティ
    /// </summary>
    public PatrolState PatrolState => this.patrolState;

    /// <summary>
    /// 追跡状態の公開用プロパティ
    /// </summary>
    public ChaseState ChaseState => this.chaseState;

    /// <summary>
    /// 巡回ポイントの座標
    /// </summary>
    private Transform TargetPoint => this.patrolPoints[this.targetPointIndex];

    /// <summary>
    /// 最初の目的地設定、データ取得
    /// </summary>
    private void Start()
    {
        this.targetPointIndex = 0;

        this.patrolState = new PatrolState();
        this.chaseState = new ChaseState();
        this.returnToPatrolState = new ReturnToPatrolState();

        this.currentState = this.patrolState;
        this.currentState.OnEnter(this);

        // 難易度を取得できなかった場合はノーマルにする
        this.difficulty = DifficultyManager.Difficulty.Normal;

        if (DifficultyManager.Instance != null)
        {
            this.difficulty = DifficultyManager.Instance.GetDifficulty();
        }

        // ハードの場合は視野角表示を消す
        this.enemyVision.SetVisionVisible(this.difficulty != DifficultyManager.Difficulty.Hard);
    }

    /// <summary>
    /// プレイヤー情報などを設定する
    /// </summary>
    /// <param name="playerController">プレイヤー</param>
    /// <param name="resultController">リザルト制御</param>
    /// <param name="enemyDataLoader">敵データ読み込み</param>
    /// <param name="stageBGMManager">ステージBGM管理クラス</param>
    public void Initialize(PlayerController playerController, ResultController resultController, EnemyDataLoader enemyDataLoader, StageBGMManager stageBGMManager)
    {
        this.playerController = playerController;
        this.player = playerController.transform;
        this.resultController = resultController;
        this.enemyDataLoader = enemyDataLoader;
        this.stageBGMManager = stageBGMManager;

        this.enemyData = this.enemyDataLoader.GetEnemyData(this.enemyId);

        this.enemyVision.SetEnemyData(this.enemyData);
    }

    /// <summary>
    /// 今の状態を参照
    /// </summary>
    private void FixedUpdate()
    {
        if (this.player == null)
        {
            return;
        }

        this.currentState.OnUpdate();
    }

    /// <summary>
    /// 敵の状態を変更
    /// </summary>
    /// <param name="state">変更する状態</param>
    public void ChangeState(IEnemyState state)
    {
        this.currentState?.OnExit();
        this.currentState = state;
        this.currentState.OnEnter(this);
    }

    /// <summary>
    /// 巡回ポイントへ移動する
    /// </summary>
    public void MoveToTarget()
    {
        // 移動先の方向を取得
        float direction = this.TargetPoint.position.x > this.transform.position.x ? 1.0f : -1.0f;

        // 巡回ポイントまで移動する
        this.enemyRb.linearVelocity = new Vector2(direction * this.enemyData.MoveSpeed, this.enemyRb.linearVelocity.y);

        // 移動方向に合わせて反転
        this.Flip(direction);
    }

    /// <summary>
    /// プレイヤーへ移動する
    /// </summary>
    public void MoveToPlayer()
    {
        // 移動先の方向を取得
        float direction = this.player.position.x > this.transform.position.x ? 1.0f : -1.0f;

        // プレイヤーまで移動する
        this.enemyRb.linearVelocity = new Vector2(direction * this.enemyData.ChaseSpeed, this.enemyRb.linearVelocity.y);

        // 移動方向に合わせて反転
        this.Flip(direction);
    }

    /// <summary>
    /// 次の巡回ポイントを変更
    /// </summary>
    public void ChangeTarget()
    {
        this.targetPointIndex++;

        if (this.targetPointIndex >= this.patrolPoints.Count)
        {
            this.targetPointIndex = 0;
        }
    }

    /// <summary>
    /// 巡回ポイントについかどうかを判定
    /// </summary>
    /// <returns>目的地に到着したらtrue,そうじゃなければfalse</returns>
    public bool IsArrived()
    {
        return Mathf.Abs(this.transform.position.x - this.TargetPoint.position.x) < 0.1f;
    }

    /// <summary>
    /// 巡回ルートから離れているかどうかの判定
    /// </summary>
    /// <returns>巡回ポイントより離れていたらtrue,違えばfalse</returns>
    public bool IsOutsidePatrolRoute()
    {
        // ２つのポイントのどちらが左にあるか右にあるかを調べる
        float minX = this.patrolPoints[0].position.x;
        float maxX = minX;

        for (int i = 1; i < this.patrolPoints.Count; i++)
        {
            float pointX = this.patrolPoints[i].position.x;

            minX = Mathf.Min(minX, pointX);
            maxX = Mathf.Max(maxX, pointX);
        }

        // 自分の現在位置を取得
        float moveX = this.transform.position.x;

        // 巡回ポイントより左に行き過ぎてるか右に行き過ぎてるかを判定
        if (moveX < minX)
        {
            return minX - moveX > this.returnDistanse;
        }
        else if (moveX > maxX)
        {
            return moveX - maxX > this.returnDistanse;
        }

        return false;
    }

    /// <summary>
    /// 巡回ルートから離れたときに最も近い巡回ポイントを取得する
    /// </summary>
    /// <returns>pointAかpointBのどちらが近いかを返す</returns>
    public int GetNearestPoint()
    {
        int nearestIndex = 0;
        float nearestDistance = Vector2.Distance(this.transform.position, this.patrolPoints[0].position);

        for (int i = 1; i < this.patrolPoints.Count; i++)
        {
            float distance = Vector2.Distance(this.transform.position, this.patrolPoints[i].position);

            if (distance < nearestDistance)
            {
                nearestIndex = i;
                nearestDistance = distance;
            }
        }

        return nearestIndex;
    }

    /// <summary>
    /// 移動方向に合わせて反転
    /// </summary>
    /// <param name="direction">移動方向</param>
    private void Flip(float direction)
    {
        Vector3 scale = this.transform.localScale;

        if (direction > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
        }

        this.transform.localScale = scale;
    }

    /// <summary>
    /// プレイヤーが視野角にいるか判定
    /// </summary>
    /// <returns>視野角にプレイヤーがいればtrue,違えばfalse</returns>
    public bool CanSeePlayer()
    {
        // プレイヤーが透明化中なら発見しない
        if (this.playerController.CheckIsHidden())
        {
            this.ResetDiscovery();
            return false;
        }

        // 敵からプレイヤーへの距離と方向を求める
        Vector2 directionToPlayer = this.player.position - this.transform.position;

        // 視界距離より遠いと見えてない判定
        if (directionToPlayer.magnitude > this.enemyData.SightDistance)
        {
            this.ResetDiscovery();
            return false;
        }

        // 敵が向いている方向
        Vector2 forward = this.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // 敵からプレイヤーへの角度
        float angle = Vector2.Angle(forward, directionToPlayer);

        // 視野角の範囲外なら見えていない
        if (angle > this.enemyData.SightAngle * 0.5f)
        {
            this.ResetDiscovery();
            return false;
        }

        // ノーマル・ハードモードでは即発見
        if (this.difficulty != DifficultyManager.Difficulty.Easy)
        {
            this.resultController.SetDetected();
            return true;
        }

        // イージーは一定時間見えていると発見
        this.isDiscovering = true;
        this.discoveryTime += Time.fixedDeltaTime;

        // 発見までの間は敵を停止
        this.enemyRb.linearVelocity = new Vector2(0.0f, this.enemyRb.linearVelocity.y);

        // 発見時間に到達したら発見する
        if (this.discoveryTime >= this.easyDiscoveryDuration)
        {
            this.resultController.SetDetected();
            return true;
        }

        return false;
    }

    /// <summary>
    /// 射撃処理
    /// </summary>
    public void Shoot()
    {
        // 次の射撃までの時間を計測
        this.shotTime -= Time.fixedDeltaTime;

        if (this.shotTime > 0.0f)
        {
            return;
        }

        // 射撃SEを再生
        SEManager.Instance.PlaySE(this.shootSE);

        // ShotPointからプレイヤーへの方向
        Vector2 directionToPlayer = this.player.position - this.shotPoint.position;

        // プレイヤーの方向を向いた状態で弾を生成
        this.bulletSpawner.Spawn(this.shotPoint.position, directionToPlayer.normalized, this.playerController);

        // 発射タイマーをリセット
        this.shotTime = this.enemyData.ShotInterval;
    }

    /// <summary>
    /// 最寄りの巡回地点へ戻る状態に変更
    /// </summary>
    public void ChangeToReturnToPatrol()
    {
        this.targetPointIndex = this.GetNearestPoint();
        this.ChangeState(this.returnToPatrolState);
    }

    /// <summary>
    /// プレイヤーを見失った時間を加算
    /// </summary>
    public void AddLostPlayerTime()
    {
        this.lostPlayerTime += Time.fixedDeltaTime;
    }

    /// <summary>
    /// プレイヤーを見失った時間をリセット
    /// </summary>
    public void ResetLostPlayerTime()
    {
        this.lostPlayerTime = 0.0f;
    }

    /// <summary>
    /// プレイヤーを見失ったか判定
    /// </summary>
    /// <returns>追跡終了時間になったらtrue、違えばfalse</returns>
    public bool IsLostPlayer()
    {
        return this.lostPlayerTime >= this.chaseDuration;
    }

    /// <summary>
    /// プレイヤーの発見処理をリセット
    /// </summary>
    private void ResetDiscovery()
    {
        this.discoveryTime = 0.0f;
        this.isDiscovering = false;
    }

    /// <summary>
    /// プレイヤーを発見待ちか判定する
    /// </summary>
    /// <returns>発見待ち中ならtrue、違えばfalse</returns>
    public bool CheckIsDiscovering()
    {
        return this.isDiscovering;
    }

    /// <summary>
    /// ゲームオーバー判定用オブジェクトに触れたときの処理
    /// </summary>
    /// <param name="other">接触したコライダー</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GameOver"))
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    /// <summary>
    /// 目的地到着後の待機時間を取得する
    /// </summary>
    /// <returns>待機時間</returns>
    public float GetPatrolWaitTime()
    {
        return this.patrolWaitTime;
    }

    /// <summary>
    /// 水平方向の移動を停止する
    /// </summary>
    public void Stop()
    {
        this.enemyRb.linearVelocity = new Vector2(0.0f, this.enemyRb.linearVelocity.y);
    }

    /// <summary>
    /// ステージBGMを通常に戻す
    /// </summary>
    public void PlayNormalBGM()
    {
        this.stageBGMManager.PlayBGM(StageBGMManager.BGMType.Normal);
    }

    /// <summary>
    /// ステージBGMを発見時に変更する
    /// </summary>
    public void PlayDiscoveredBGM()
    {
        this.stageBGMManager.PlayBGM(StageBGMManager.BGMType.Discovered);
    }
}
