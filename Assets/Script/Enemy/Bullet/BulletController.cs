using UnityEngine;

/// <summary>
/// 弾丸処理
/// </summary>
public class BulletController : MonoBehaviour
{
    /// <summary>
    /// 弾丸速度
    /// </summary>
    [SerializeField]
    private float bulletSpeed = 8.0f;

    /// <summary>
    /// プレイヤーに与えるダメージ
    /// </summary>
    [SerializeField]
    private int damage = 1;

    /// <summary>
    /// プレイヤーのレイヤー
    /// </summary>
    [SerializeField]
    private LayerMask playerLayer;

    /// <summary>
    /// 当たったら弾丸を消すレイヤー
    /// </summary>
    [SerializeField]
    private LayerMask destroyLayer;

    /// <summary>
    /// 弾が飛ぶ方向
    /// </summary>
    private Vector2 direction = Vector2.right;

    /// <summary>
    /// 弾丸を削除するまでの時間
    /// </summary>
    [SerializeField]
    private float destroyTime = 5.0f;

    /// <summary>
    /// プレイヤースクリプトを参照
    /// </summary>
    private PlayerController playerController;

    /// <summary>
    /// 弾丸移動
    /// </summary>
    private void Update()
    {
        this.transform.position += (Vector3)this.direction * this.bulletSpeed * Time.deltaTime;
    }

    /// <summary>
    /// EnemyControllerから飛ぶ方法を設定
    /// </summary>
    /// <param name="direction">移動方向</param>
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;

        // 発射からdestroyTimer後に弾丸を削除
        Destroy(this.gameObject, this.destroyTime);
    }

    /// <summary>
    /// プレイヤースクリプトを読み込む
    /// </summary>
    /// <param name="playerController">シーン内のプレイヤー</param>
    public void SetPlayer(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    /// <summary>
    /// 弾丸がオブジェクトに当たった時の処理
    /// </summary>
    /// <param name="collision">弾丸が当たったオブジェクト</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // レイヤーを取得
        int collisionLayer = 1 << collision.gameObject.layer;

        // プレイヤーに当たったか
        bool hitPlayer = (this.playerLayer.value & collisionLayer) != 0;

        // プレイヤー以外に当たったか
        bool hitDestroyLayer = (this.destroyLayer.value & collisionLayer) != 0;

        // プレイヤーに当たったらダメージを与える
        if (hitPlayer)
        {
            this.playerController.TakeDamage(this.damage);
        }

        // オブジェクトに当たったら削除
        if (hitPlayer || hitDestroyLayer)
        {
            Destroy(this.gameObject);
        }
    }
}