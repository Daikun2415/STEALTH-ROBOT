using UnityEngine;

/// <summary>
/// プレイヤーを追従するカメラを制御
/// </summary>
public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// 追尾対象のプレイヤーを取得
    /// </summary>
    [SerializeField]
    private Transform player;

    /// <summary>
    /// プレイヤーに合わせてカメラを追従
    /// </summary>
    private void LateUpdate()
    {
        if (this.player == null)
        {
            return;
        }

        this.transform.position = new Vector3(this.player.position.x, this.player.position.y, this.transform.position.z);
    }
}
