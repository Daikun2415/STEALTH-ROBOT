using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 透明化ゲージの管理クラス
/// </summary>
public class HideGaugeController : Gauge
{
    /// <summary>
    /// 再使用できなくなるゲージの割合
    /// </summary>
    private const float LowGaugeLimit = 0.25f;

    /// <summary>
    /// 透明化ゲージ減少速度
    /// </summary>
    [SerializeField]
    private float hideGaugeDecreaseSpeed = 1.0f;

    /// <summary>
    /// 透明化ゲージ回復速度
    /// </summary>
    [SerializeField]
    private float hideGaugeRecoverySpeed = 0.5f;

    /// <summary>
    /// 透明化が再使用可能かどうか
    /// </summary>
    private bool canUseHide = true;

    /// <summary>
    /// 通常時の色
    /// </summary>
    [SerializeField]
    private Color normalColor = Color.blue;

    /// <summary>
    /// ゲージが1/4未満になった時の色
    /// </summary>
    [SerializeField]
    private Color lowGaugeColor = Color.red;

    /// <summary>
    /// 透明化ゲージ減少処理
    /// </summary>
    public void Decrease()
    {
        // 透明化中はゲージを減らす
        this.CurrentValue -= this.hideGaugeDecreaseSpeed * Time.deltaTime;

        // UIの透明化ゲージを更新
        this.UpdateHideGauge();

        // ゲージが1/4未満になったら再使用不可
        if (this.CurrentValue < this.MaxValue * LowGaugeLimit)
        {
            this.canUseHide = false;
        }

        // ゲージがなくなったら透明化解除
        if (this.CurrentValue <= 0.0f)
        {
            this.CurrentValue = 0.0f;
        }
    }

    /// <summary>
    /// 透明化ゲージ回復処理
    /// </summary>
    public void Recover()
    {
        // ゲージ満タンなら処理しない
        if (this.CurrentValue >= this.MaxValue)
        {
            return;
        }

        // スペースキーを離している間はゲージ回復
        this.CurrentValue += this.hideGaugeRecoverySpeed * Time.deltaTime;

        // UIの透明化ゲージを更新
        this.UpdateHideGauge();

        // 回復が最大値を超えないように
        this.CurrentValue = Mathf.Min(this.CurrentValue, this.MaxValue);

        // ゲージ満タンで再使用可能に
        if (this.CurrentValue >= this.MaxValue)
        {
            this.canUseHide = true;
        }
    }

    /// <summary>
    /// 透明化を使用できるか取得
    /// </summary>
    /// <returns>使用できるならtrue、違えばfalse</returns>
    public bool CanHide()
    {
        return this.canUseHide;
    }

    /// <summary>
    /// ゲージが残ってるか取得
    /// </summary>
    /// <returns>ゲージが残っていればtrue、違えばfalse</returns>
    public bool HasGauge()
    {
        return this.CurrentValue > 0.0f;
    }

    /// <summary>
    /// UIの透明化ゲージ更新処理
    /// </summary>
    private void UpdateHideGauge()
    {
        this.UpdateGauge();

        // ゲージが1/4未満になったらゲージが赤くなる
        if (this.CurrentValue < this.MaxValue * LowGaugeLimit)
        {
            this.GaugeImage.color = this.lowGaugeColor;
        }

        // Maxまで回復したら青に戻す
        else if (this.CurrentValue >= this.MaxValue)
        {
            this.GaugeImage.color = this.normalColor;
        }
    }
}
