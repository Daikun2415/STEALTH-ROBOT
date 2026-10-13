using UnityEngine;

/// <summary>
/// 弾丸を生成するクラス
/// </summary>
public class BulletSpawner : MonoBehaviour
{
    /// <summary>
    /// 弾丸のプレハブ
    /// </summary>
    [SerializeField]
    private BulletController bulletPrefab;

    /// <summary>
    /// 弾丸を生成する処理
    /// </summary>
    /// <param name="position">生成位置</param>
    /// <param name="direction">飛ぶ方向</param>
    /// <param name="playerController">プレイヤー</param>
    public void Spawn(Vector3 position, Vector2 direction, PlayerController playerController)
    {
        BulletController bullet = Instantiate(this.bulletPrefab, position, Quaternion.identity);

        bullet.SetDirection(direction);
        bullet.SetPlayer(playerController);
    }
}
