using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲージの基本情報を管理するクラス
/// </summary>
public class Gauge : MonoBehaviour
{
    /// <summary>
    /// ゲージの最大値
    /// </summary>
    [SerializeField]
    private float maxValue = 1.0f;

    /// <summary>
    /// 現在のゲージ値
    /// </summary>
    [SerializeField]
    private float currentValue;

    /// <summary>
    /// ゲージのImage
    /// </summary>
    [SerializeField]
    private Image gaugeImage;

    /// <summary>
    /// ゲージ最大値のプロパティ
    /// </summary>
    protected float MaxValue => this.maxValue;

    /// <summary>
    ///  現在のゲージ値のプロパティ
    /// </summary>
    protected float CurrentValue
    {
        get => this.currentValue;
        set => this.currentValue = value;
    }

    /// <summary>
    /// ゲージImageのプロパティ
    /// </summary>
    protected Image GaugeImage => this.gaugeImage;

    /// <summary>
    /// ゲージを初期化
    /// </summary>
    protected virtual void Start()
    {
        this.currentValue = this.maxValue;
        this.UpdateGauge();
    }

    /// <summary>
    /// ゲージの更新
    /// </summary>
    protected void UpdateGauge()
    {
        this.gaugeImage.fillAmount = this.currentValue / this.maxValue;
    }

    /// <summary>
    /// 現在値の設定
    /// </summary>
    /// <param name="value">設定する値</param>
    public void SetValue(float value)
    {
        this.currentValue = Mathf.Clamp(value, 0.0f, this.maxValue);
        this.UpdateGauge();
    }

    /// <summary>
    /// 現在値を取得
    /// </summary>
    /// <returns>現在値</returns>
    public float GetValue()
    {
        return this.currentValue;
    }

    /// <summary>
    /// 最大値を取得
    /// </summary>
    /// <returns>最大値</returns>
    public float GetMaxValue()
    {
        return this.maxValue;
    }
}
