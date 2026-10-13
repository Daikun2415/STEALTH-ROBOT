using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの制御
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 移動用のInputAction
    /// </summary>
    [SerializeField]
    private InputAction moveAction;

    /// <summary>
    /// しゃがみ用のInputAction
    /// </summary>
    [SerializeField]
    private InputAction downAction;

    /// <summary>
    /// ジャンプ用InputAction
    /// </summary>
    [SerializeField]
    private InputAction jumpAction;

    /// <summary>
    /// 透明化用InputAction
    /// </summary>
    [SerializeField]
    private InputAction hideAction;

    /// <summary>
    /// プレイヤーの見た目
    /// </summary>
    [SerializeField]
    private SpriteRenderer playerSprite;

    /// <summary>
    /// リジッドボディ
    /// </summary>
    [SerializeField]
    private Rigidbody2D playerRb;

    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    private float moveSpeed = 5.0f;

    /// <summary>
    /// ジャンプの強さ
    /// </summary>
    [SerializeField]
    private float jumpPower = 10.0f;

    /// <summary>
    /// InputActionの値を格納
    /// </summary>
    [SerializeField]
    private float moveInput;

    /// <summary>
    /// 地面チェック用エンプティオブジェクト
    /// </summary>
    [SerializeField]
    private Transform groundCheck;

    /// <summary>
    /// 地面チェック範囲
    /// </summary>
    [SerializeField]
    private Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);

    /// <summary>
    /// 地面レイヤー
    /// </summary>
    [SerializeField]
    private LayerMask groundLayer;

    /// <summary>
    /// 立ち上がれるかをチェックするエンプティオブジェクト
    /// </summary>
    [SerializeField]
    private Transform standCheck;

    /// <summary>
    /// 立ち上がれるかをチェックする範囲
    /// </summary>
    [SerializeField]
    private Vector2 standCheckSize = new Vector2(0.8f, 1.8f);

    /// <summary>
    /// 周辺のオブジェクトのチェック対象のレイヤー
    /// </summary>
    [SerializeField]
    private LayerMask standCheckLayer;

    /// <summary>
    /// 通常状態のプレイヤーオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject normalObject;

    /// <summary>
    /// しゃがみ状態のプレイヤーオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject downObject;

    /// <summary>
    /// しゃがみ状態の真偽型
    /// </summary>
    private bool isDown;

    /// <summary>
    /// 前フレームのしゃがみ状態
    /// </summary>
    private bool previousIsDown;

    /// <summary>
    /// プレイヤーUIのスクリプトを取得
    /// </summary>
    [SerializeField]
    private HpGauge hpGauge;

    /// <summary>
    /// 今、透明中かどうか
    /// </summary>
    private bool isHidden;

    /// <summary>
    /// 直前のフレームが透明中かどうか
    /// </summary>
    private bool previousIsHidden;

    /// <summary>
    /// 透明化ゲージを管理するクラス
    /// </summary>
    [SerializeField]
    private HideGaugeController hideGaugeController;

    /// <summary>
    /// ポーズを管理するクラス
    /// </summary>
    [SerializeField]
    private PauseController pauseController;

    /// <summary>
    /// ゲームオーバー時に表示するパネル
    /// </summary>
    [SerializeField]
    private GameObject gameOverPanel;

    /// <summary>
    /// 移動床による移動量
    /// </summary>
    private Vector2 platformVelocity;

    /// <summary>
    /// すり抜け床のクラス
    /// </summary>
    private SlipPlatform currentSlipPlatform;

    /// <summary>
    /// 立ち状態のコライダー
    /// </summary>
    [SerializeField]
    private Collider2D normalCollider;

    /// <summary>
    /// しゃがみ状態のコライダー
    /// </summary>
    [SerializeField]
    private Collider2D downCollider;

    /// <summary>
    /// すり抜け床を通過中か
    /// </summary>
    private bool isPassingSlipPlatform;

    /// <summary>
    /// ジャンプ中かどうか
    /// </summary>
    private bool isJumping;

    /// <summary>
    /// ステージBGM管理クラス
    /// </summary>
    [SerializeField]
    private StageBGMManager stageBGMManager;

    /// <summary>
    /// ジャンプのSE
    /// </summary>
    [SerializeField]
    private AudioClip jumpSE;

    /// <summary>
    /// 透明化のSE
    /// </summary>
    [SerializeField]
    private AudioClip hideSE;

    /// <summary>
    /// ダメージを受けたときのSE
    /// </summary>
    [SerializeField]
    private AudioClip damageSE;

    /// <summary>
    /// 死亡時のSE
    /// </summary>
    [SerializeField]
    private AudioClip dieSE;

    /// <summary>
    /// 各InputActionの有効化、HPをセット
    /// </summary>
    private void Start()
    {
        this.moveAction.Enable();
        this.downAction.Enable();
        this.jumpAction.Enable();
        this.hideAction.Enable();

        this.gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// しゃがみ状態、ジャンプの検知
    /// </summary>
    private void Update()
    {
        // ポーズ中やリザルト表示中は操作を受け付けない
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        bool isGrounded = this.CheckGrounded();

        this.UpdateHide();

        if (isGrounded && !this.isHidden && !this.isJumping)
        {
            // しゃがみ入力中
            if (this.downAction.IsPressed())
            {
                this.isDown = true;
            }

            // しゃがみ入力を離して頭上に何もなければしゃがみ状態を解除
            else if (this.isDown && this.CanStand())
            {
                this.isDown = false;
            }
        }

        // しゃがみ中のみ、オブジェクトを切り替え、すり抜け床を通過する
        if (this.isDown != this.previousIsDown)
        {
            this.normalObject.SetActive(!this.isDown);
            this.downObject.SetActive(this.isDown);

            if (this.isDown && this.currentSlipPlatform != null)
            {
                this.currentSlipPlatform.DropPlayer();
            }

            this.previousIsDown = this.isDown;
        }

        // A・Dの入力を取得
        this.moveInput = this.moveAction.ReadValue<float>();

        // 移動方向に合わせて向きを変更
        this.Flip();

        // Wでジャンプ
        if (this.jumpAction.WasPressedThisFrame() && isGrounded && !this.isHidden && !this.CheckIsPassingSlipPlatForm())
        {
            this.PlaySE(this.jumpSE);
            this.playerRb.linearVelocity = new Vector2(this.playerRb.linearVelocity.x, this.jumpPower);
            this.isJumping = true;
        }

        // 着地したらジャンプ中を解除
        if (this.isJumping && isGrounded && this.playerRb.linearVelocity.y <= 0.0f)
        {
            this.isJumping = false;
        }
    }

    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    private void FixedUpdate()
    {
        // 透明中は自身の移動速度を0にする
        if (this.isHidden)
        {
            this.playerRb.linearVelocity = new Vector2(this.platformVelocity.x, this.playerRb.linearVelocity.y);
            return;
        }

        // しゃがみ中は移動スピードを半分に
        float currentMoveSpeed = this.isDown ? this.moveSpeed * 0.5f : this.moveSpeed;

        // プレイヤー入力の移動速度を算出
        float horizontalVelocity = this.moveInput * currentMoveSpeed;

        // 移動床の速度を加える
        horizontalVelocity += this.platformVelocity.x;

        // リジッドボディを動かす
        this.playerRb.linearVelocity = new Vector2(horizontalVelocity, this.playerRb.linearVelocity.y);
    }

    /// <summary>
    /// 地面にいるかどうかを判定する処理
    /// </summary>
    /// <returns>地面に触れている場合はtrue、それ以外はfalseを返します。</returns>
    private bool CheckGrounded()
    {
        return Physics2D.OverlapBox(this.groundCheck.position, this.groundCheckSize, 0.0f, this.groundLayer);
    }

    /// <summary>
    /// しゃがんだのち、立ち上がれるかどうかを判定する処理
    /// </summary>
    /// <returns>頭上に障害物がないなら true、あるなら false を返す</returns>
    private bool CanStand()
    {
        return !Physics2D.OverlapBox(this.standCheck.position, this.standCheckSize, 0f, this.standCheckLayer);
    }

    /// <summary>
    /// 移動方向に合わせて反転
    /// </summary>
    private void Flip()
    {
        if (this.moveInput == 0.0f)
        {
            return;
        }

        Vector3 scale = this.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(this.moveInput);
        this.transform.localScale = scale;
    }

    /// <summary>
    /// ダメージを受けたときの処理
    /// </summary>
    /// <param name="damage">受けたダメージ</param>
    public void TakeDamage(int damage)
    {
        this.hpGauge.TakeDamage(damage);

        if (this.hpGauge.IsDead())
        {
            this.Die();
        }
        else
        {
            this.PlaySE(this.damageSE);
        }
    }

    /// <summary>
    /// プレイヤー死亡時の処理
    /// </summary>
    private void Die()
    {
        // ポーズできないようにする
        this.pauseController.LockPause();

        // 死亡時のSEを再生
        this.PlaySE(this.dieSE);

        // プレイヤーオブジェクトを非表示にする
        this.gameObject.SetActive(false);

        // ゲームオーバーBGMを再生
        this.stageBGMManager.PlayBGM(StageBGMManager.BGMType.GameOver);

        // ゲームオーバーパネルを表示
        this.gameOverPanel.SetActive(true);

        // ゲームオーバー時は時間を止める
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// 透明中かどうかを取得
    /// </summary>
    /// <returns>透明中ならtrue、違えばfalse</returns>
    public bool CheckIsHidden()
    {
        return this.isHidden;
    }

    /// <summary>
    /// 透明化処理
    /// </summary>
    private void UpdateHide()
    {
        // 透明化はスペースキーを押していて、しゃがんでなくて、ゲージが残っていて、再使用可能なら有効
        if (this.hideAction.IsPressed() && !this.isDown && this.hideGaugeController.CanHide() && this.hideGaugeController.HasGauge())
        {
            this.isHidden = true;
        }

        // スペースキーを離したら透明化解除
        else if (!this.hideAction.IsPressed())
        {
            this.isHidden = false;
        }

        // 透明中は半透明表示にする
        if (this.isHidden != this.previousIsHidden)
        {
            Color color = this.playerSprite.color;
            color.a = this.isHidden ? 0.5f : 1.0f;
            this.playerSprite.color = color;

            if (this.isHidden)
            {
                this.PlaySE(this.hideSE);
            }

            this.previousIsHidden = this.isHidden;
        }

        if (this.isHidden)
        {
            // 透明中はゲージを減らす
            this.hideGaugeController.Decrease();

            // ゲージがなくなったら透明化解除
            if (!this.hideGaugeController.HasGauge())
            {
                this.isHidden = false;
            }
        }
        else
        {
            // 透明化してない間はゲージを回復
            this.hideGaugeController.Recover();
        }
    }

    /// <summary>
    /// 移動床の移動量を設定
    /// </summary>
    /// <param name="velocity">移動床の移動量</param>
    public void SetPlatformVelocity(Vector2 velocity)
    {
        this.platformVelocity = velocity;
    }

    /// <summary>
    /// 現在のすり抜け床を設定する
    /// </summary>
    /// <param name="platform">すり抜け床</param>
    public void SetCurrentSlipPlatform(SlipPlatform platform)
    {
        this.currentSlipPlatform = platform;
    }

    /// <summary>
    /// 現在のすり抜け床を解除する
    /// </summary>
    /// <param name="platform">すり抜け床</param>
    public void ClearCurrentSlipPlatform(SlipPlatform platform)
    {
        if (this.currentSlipPlatform == platform)
        {
            this.currentSlipPlatform = null;
        }
    }

    /// <summary>
    /// 現在のコライダーを渡す
    /// </summary>
    /// <returns>現在のコライダー</returns>
    public Collider2D GetCurrentCollider()
    {
        return this.isDown ? this.downCollider : this.normalCollider;
    }

    /// <summary>
    /// すり抜け床の通過状態を設定する
    /// </summary>
    /// <param name="isPassing">通過中ならtrue、違えばfalse</param>
    public void SetPassingSlipPlatform(bool isPassing)
    {
        this.isPassingSlipPlatform = isPassing;
    }

    /// <summary>
    /// すり抜け床を上から下、下から上問わず通過中かどうか
    /// </summary>
    /// <returns>通過中ならtrue、違えばfalse</returns>
    private bool CheckIsPassingSlipPlatForm()
    {
        return this.isPassingSlipPlatform || (this.currentSlipPlatform != null && this.playerRb.linearVelocity.y > 0.0f);
    }

    /// <summary>
    /// ゲームオーバー判定用オブジェクトに触れたときの処理
    /// </summary>
    /// <param name="other">接触したコライダー</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GameOver"))
        {
            this.TakeDamage((int)this.hpGauge.GetCurrentHp());
        }
    }

    /// <summary>
    /// 指定したSEを再生する
    /// </summary>
    /// <param name="se">再生するSE</param>
    private void PlaySE(AudioClip se)
    {
        SEManager.Instance.PlaySE(se);
    }
}
