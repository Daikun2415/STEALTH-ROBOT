using UnityEngine;

/// <summary>
/// ボーナスアイテムを管理するクラス
/// </summary>
public class BonusItem : MonoBehaviour
{
    /// <summary>
    /// ボーナスアイテムを取得したか
    /// </summary>
    private bool isCollected = false;

    /// <summary>
    /// ボーナスアイテムを取得したときのSE
    /// </summary>
    [SerializeField]
    private AudioClip itemGetSE;

    /// <summary>
    /// プレイヤーがボーナスアイテムに触れたときの処理
    /// </summary>
    /// <param name="other">触れたオブジェクト</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.isCollected || !other.CompareTag("Player"))
        {
            return;
        }

        this.isCollected = true;

        // ボーナスアイテム取得時のSEを再生する
        SEManager.Instance.PlaySE(this.itemGetSE);

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// ボーナスアイテムを取得したか取得する
    /// </summary>
    /// <returns>取得済みならtrue、違えばfalse</returns>
    public bool CheckIsCollected()
    {
        return this.isCollected;
    }
}
