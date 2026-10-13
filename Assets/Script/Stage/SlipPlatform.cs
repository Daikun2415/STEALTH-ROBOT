using System.Collections;
using UnityEngine;

/// <summary>
/// すり抜け床の制御
/// </summary>
public class SlipPlatform : MonoBehaviour
{
    /// <summary>
    /// すり抜け床のコライダー
    /// </summary>
    [SerializeField]
    private Collider2D platformCollider;

    /// <summary>
    /// プレイヤーの制御クラス
    /// </summary>
    private PlayerController playerController;

    /// <summary>
    /// 落下中かどうか
    /// </summary>
    private bool isDropping;

    /// <summary>
    /// しゃがみジャンプしたときに床判定を復活させる指標
    /// </summary>
    [SerializeField]
    private float upperLimit;

    /// <summary>
    /// プレイヤーが落下判定範囲に入ったときの処理
    /// </summary>
    /// <param name="other">侵入したコライダー</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerTrigger"))
        {
            return;
        }

        this.playerController = other.GetComponentInParent<PlayerController>();

        if (this.playerController == null)
        {
            return;
        }

        this.playerController.SetCurrentSlipPlatform(this);
    }

    /// <summary>
    /// プレイヤーが落下判定範囲から出たときの処理
    /// </summary>
    /// <param name="other">退出したコライダー</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerTrigger"))
        {
            return;
        }

        if (this.playerController == null)
        {
            return;
        }

        this.playerController.ClearCurrentSlipPlatform(this);
        this.playerController = null;
    }

    /// <summary>
    /// プレイヤーをすり抜け床から落下させる
    /// </summary>
    public void DropPlayer()
    {
        if (this.isDropping || this.playerController == null)
        {
            return;
        }

        this.StartCoroutine(this.DropCoroutine());
    }

    /// <summary>
    /// プレイヤーをすり抜け床から落下させるコルーチン
    /// </summary>
    /// <returns>処理待機</returns>
    private IEnumerator DropCoroutine()
    {
        this.isDropping = true;
        PlayerController player = this.playerController;
        player.SetPassingSlipPlatform(true);
        Collider2D playerCollider = player.GetCurrentCollider();

        // すり抜け床とプレイヤーのコライダーの衝突を一時的に無視する
        Physics2D.IgnoreCollision(playerCollider, this.platformCollider, true);

        yield return new WaitForFixedUpdate();

        while (playerCollider.bounds.max.y > this.platformCollider.bounds.min.y && playerCollider.bounds.min.y < this.platformCollider.bounds.max.y + this.upperLimit)
        {
            yield return new WaitForFixedUpdate();
        }

        // 衝突判定を復活
        Physics2D.IgnoreCollision(playerCollider, this.platformCollider, false);
        player.SetPassingSlipPlatform(false);
        this.isDropping = false;
    }
}
