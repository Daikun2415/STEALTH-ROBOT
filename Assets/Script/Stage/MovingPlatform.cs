using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動床の制御クラス
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    /// <summary>
    /// 移動先のポイント
    /// </summary>
    [SerializeField]
    private List<Transform> movePoints;

    /// <summary>
    /// 現在の移動先のインデックス
    /// </summary>
    private int movePointsIndex;

    /// <summary>
    /// リジッドボディ
    /// </summary>
    [SerializeField]
    private Rigidbody2D platformRb;

    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField]
    private float moveSpeed = 2.0f;

    /// <summary>
    /// プレイヤーレイヤー
    /// </summary>
    [SerializeField]
    private LayerMask playerLayer;

    /// <summary>
    /// プレイヤーのリジッドボディ
    /// </summary>
    private Rigidbody2D playerRb;

    /// <summary>
    /// プレイヤーの制御クラス
    /// </summary>
    private PlayerController playerController;

    /// <summary>
    /// 移動開始地点
    /// </summary>
    private Vector2 startPosition;

    /// <summary>
    /// 現在の移動経過時間
    /// </summary>
    private float moveTime;

    /// <summary>
    /// 現在の移動にかかる時間
    /// </summary>
    private float moveDuration;

    /// <summary>
    /// プレイヤーが乗っているか
    /// </summary>
    private bool isPlayerOnPlatform;

    /// <summary>
    /// 天井判定用のレイヤー
    /// </summary>
    [SerializeField]
    private LayerMask ceilingLayer;

    /// <summary>
    /// プレイヤーの上方向を確認する距離
    /// </summary>
    [SerializeField]
    private float ceilingCheckDistance = 0.05f;

    /// <summary>
    /// 移動床のコライダー
    /// </summary>
    [SerializeField]
    private Collider2D platformCollider;

    /// <summary>
    /// プレイヤーの下方向を確認するレイヤー
    /// </summary>
    [SerializeField]
    private LayerMask groundLayer;

    /// <summary>
    /// 現在の移動先
    /// </summary>
    private Transform TargetPoint => this.movePoints[this.movePointsIndex];

    /// <summary>
    /// 初期設定
    /// </summary>
    private void Start()
    {
        if (this.movePoints == null || this.movePoints.Count == 0)
        {
            return;
        }

        this.movePointsIndex = 0;

        // 現在位置を移動開始地点にする
        this.startPosition = this.platformRb.position;
        this.SetMoveDuration();
    }

    /// <summary>
    /// 常時移動するようにする
    /// </summary>
    private void FixedUpdate()
    {
        if (this.movePoints == null || this.movePoints.Count == 0)
        {
            return;
        }

        // 移動床の位置と目的地の位置を取得
        Vector2 currentPosition = this.platformRb.position;
        Vector2 targetPosition = this.TargetPoint.position;

        // 移動時間を進める
        this.moveTime += Time.fixedDeltaTime;

        // 移動の進行度を計算
        float progress = this.moveDuration > 0.0f ? Mathf.Clamp01(this.moveTime / this.moveDuration) : 1.0f;

        // 移動床を滑らかに動かす
        float smoothProgress = Mathf.SmoothStep(0.0f, 1.0f, progress);

        // 次に移動する座標を計算
        Vector2 nextPosition = Vector2.Lerp(this.startPosition, targetPosition, smoothProgress);

        // どれだけ移動したかを計算
        Vector2 movement = nextPosition - currentPosition;

        // 移動を中止する必要があるか確認
        bool isMovementBlocked = this.IsMovementBlocked(movement);

        if (isMovementBlocked)
        {
            // 次の移動先へ変更
            this.ChangePoint();

            // 現在の上昇を中止
            this.startPosition = currentPosition;

            // 新しい移動先までの移動時間を設定
            this.SetMoveDuration();

            // 今回の移動量を無効にする
            nextPosition = currentPosition;
            movement = Vector2.zero;
        }

        // 移動床を徐々に動かす
        this.platformRb.MovePosition(nextPosition);

        // プレイヤーが乗っていれば、移動床の速度をプレイヤーに渡す
        if (this.isPlayerOnPlatform)
        {
            Vector2 velocity = movement / Time.fixedDeltaTime;
            this.playerController.SetPlatformVelocity(velocity);
        }

        // 目的地についたら次の目的地に移動する
        if (progress >= 1.0f)
        {
            this.startPosition = targetPosition;
            this.ChangePoint();
            this.SetMoveDuration();
        }
    }

    /// <summary>
    /// 移動先までの移動時間を設定する
    /// </summary>
    private void SetMoveDuration()
    {
        float distance = Vector2.Distance(this.startPosition, this.TargetPoint.position);

        if (this.moveSpeed <= 0.0f)
        {
            this.moveDuration = 0.0f;
            this.moveTime = 0.0f;
            return;
        }

        this.moveDuration = distance / this.moveSpeed;
        this.moveTime = 0.0f;
    }

    /// <summary>
    /// 移動床にプレイヤーが入った時の処理
    /// </summary>
    /// <param name="other">オブジェクトのコライダー</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & this.playerLayer) == 0)
        {
            return;
        }

        this.playerController = other.GetComponentInParent<PlayerController>();

        if (this.playerController == null)
        {
            return;
        }

        this.isPlayerOnPlatform = true;
    }

    /// <summary>
    /// 移動床にプレイヤーが入り続けているときの処理
    /// </summary>
    /// <param name="other">オブジェクトのコライダー</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (this.playerController == null || (((1 << other.gameObject.layer) & this.playerLayer) == 0))
        {
            return;
        }

        this.isPlayerOnPlatform = true;
    }

    /// <summary>
    /// 移動床からプレイヤーが離れたときの処理
    /// </summary>
    /// <param name="other">オブジェクトのコライダー</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & this.playerLayer) == 0)
        {
            return;
        }

        this.isPlayerOnPlatform = false;

        if (this.playerController != null)
        {
            this.playerController.SetPlatformVelocity(Vector2.zero);
            this.playerController = null;
        }
    }

    /// <summary>
    /// 次の移動先へ変更する
    /// </summary>
    private void ChangePoint()
    {
        this.movePointsIndex++;

        if (this.movePointsIndex >= this.movePoints.Count)
        {
            this.movePointsIndex = 0;
        }
    }

    /// <summary>
    /// 移動床の移動を中止する必要があるか確認
    /// </summary>
    /// <param name="movement">移動床の移動量</param>
    /// <returns>移動を中止する場合はtrue</returns>
    private bool IsMovementBlocked(Vector2 movement)
    {
        // 上昇中でプレイヤーが乗っている場合
        if (movement.y > 0.0f && this.isPlayerOnPlatform)
        {
            return this.IsPlayerHeadBlocked();
        }

        // 下降中でプレイヤーが下にいる場合
        if (movement.y < 0.0f)
        {
            return this.IsPlayerBlockedFromAbove(movement);
        }

        return false;
    }

    /// <summary>
    /// プレイヤーの頭が天井に当たっているか確認する
    /// </summary>
    /// <returns>天井に当たっている場合はtrue</returns>
    private bool IsPlayerHeadBlocked()
    {
        // プレイヤーの現在のコライダーを取得
        Collider2D playerCollider = this.playerController.GetCurrentCollider();

        if (playerCollider == null)
        {
            return false;
        }

        // コライダーの大きさを取得
        Bounds bounds = playerCollider.bounds;

        // プレイヤーの頭上を横長の範囲で検知する
        Vector2 checkSize = new Vector2(bounds.size.x, this.ceilingCheckDistance);

        // ceilingLayerで指定したレイヤー持ちのオブジェクトを検知
        Collider2D hit = Physics2D.OverlapBox(new Vector2(bounds.center.x, bounds.max.y + (this.ceilingCheckDistance * 0.5f)), checkSize, 0.0f, this.ceilingLayer);

        return hit != null;
    }

    /// <summary>
    /// 移動床の下降によってプレイヤーが挟まるか判定する。
    /// </summary>
    /// <param name="movement">移動床の移動量</param>
    /// <returns>挟まる場合はtrue、それ以外はfalse</returns>
    private bool IsPlayerBlockedFromAbove(Vector2 movement)
    {
        if (this.platformCollider == null)
        {
            return false;
        }

        Bounds platformBounds = this.platformCollider.bounds;

        // 移動床が今回移動する範囲を検知する
        float checkHeight = Mathf.Abs(movement.y) + 0.1f;

        Vector2 checkCenter = new Vector2(platformBounds.center.x + (movement.x * 0.5f), platformBounds.min.y + (movement.y * 0.5f));

        Vector2 checkSize = new Vector2(platformBounds.size.x, checkHeight);

        Collider2D playerHit = Physics2D.OverlapBox(checkCenter, checkSize, 0.0f, this.playerLayer);

        if (playerHit == null)
        {
            return false;
        }

        PlayerController player = playerHit.GetComponentInParent<PlayerController>();

        if (player == null)
        {
            return false;
        }

        Collider2D playerCollider = player.GetCurrentCollider();

        if (playerCollider == null)
        {
            return false;
        }

        Bounds playerBounds = playerCollider.bounds;

        // 移動床がプレイヤーの頭に到達するか確認
        bool willHitHead = platformBounds.min.y + movement.y <= playerBounds.max.y && platformBounds.min.y > playerBounds.max.y;

        if (!willHitHead)
        {
            return false;
        }

        // プレイヤーの下に床があるか確認
        Vector2 groundCheckPosition = new Vector2(playerBounds.center.x, playerBounds.min.y - 0.05f);

        Vector2 groundCheckSize = new Vector2(playerBounds.size.x, 0.1f);

        Collider2D ground = Physics2D.OverlapBox(groundCheckPosition, groundCheckSize, 0.0f, this.groundLayer);

        return ground != null;
    }
}