using UnityEngine;

/// <summary>
/// ボタンが押されたときにButtonSEManagerに音源を渡すクラス
/// </summary>
public class ButtonSEController : MonoBehaviour
{
    /// <summary>
    /// このボタンで再生するSE
    /// </summary>
    [SerializeField]
    private AudioClip buttonSE;

    /// <summary>
    /// ボタンのSEを再生する
    /// </summary>
    public void OnTapPlaySE()
    {
        SEManager.Instance.StopSE();
        SEManager.Instance.PlaySE(this.buttonSE);
    }
}
