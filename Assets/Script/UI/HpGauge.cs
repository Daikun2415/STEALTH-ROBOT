using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HPゲージの管理クラス
/// </summary>
public class HpGauge : MonoBehaviour
{
    /// <summary>
    /// HPグリーンゲージ
    /// </summary>
    [SerializeField]
    private Gauge greenGauge;

    /// <summary>
    /// HPレッドゲージ
    /// </summary>
    [SerializeField]
    private Gauge redGauge;

    /// <summary>
    /// レッドゲージ減少時間
    /// </summary>
    [SerializeField]
    private float redDecreaseDuration = 2.0f;

    /// <summary>
    /// レッドゲージが減少開始までの時間
    /// </summary>
    [SerializeField]
    private float startRedDecrease = 1.0f;

    /// <summary>
    /// 最大HPを格納
    /// </summary>
    private float maxHp;

    /// <summary>
    /// レッドゲージの目標HP
    /// </summary>
    private float targetHp;

    /// <summary>
    /// レッドゲージの減少速度
    /// </summary>
    private float redDecreaseSpeed;

    /// <summary>
    /// レッドゲージ減少コルーチン
    /// </summary>
    private Coroutine redGaugeCoroutine;

    /// <summary>
    /// HPを初期化
    /// </summary>
    protected void Start()
    {
        this.maxHp = this.greenGauge.GetMaxValue();
        this.greenGauge.SetValue(this.maxHp);
        this.redGauge.SetValue(this.maxHp);
        this.targetHp = this.maxHp;
    }

    /// <summary>
    /// HPゲージ変更処理
    /// </summary>
    /// <param name="fillAmount">変更後のHPゲージの割合</param>
    /// <returns>HPゲージの変更処理</returns>
    private IEnumerator UpdateHpGauge()
    {
        // レッドゲージ減少開始まで待つ
        yield return new WaitForSeconds(this.startRedDecrease);

        // レッドゲージを現在HPまで徐々に減らす
        while (this.redGauge.GetValue() > this.targetHp)
        {
            float currentHp = this.redGauge.GetValue();

            currentHp = Mathf.MoveTowards(currentHp, this.targetHp, this.redDecreaseSpeed * Time.unscaledDeltaTime);

            this.redGauge.SetValue(currentHp);

            yield return null;
        }

        this.redGauge.SetValue(this.targetHp);

        this.redGaugeCoroutine = null;
    }

    /// <summary>
    /// ダメージを受けたときの処理
    /// </summary>
    /// <param name="damage">受けたダメージ</param>
    public void TakeDamage(int damage)
    {
        float currentHp = this.greenGauge.GetValue();

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0.0f);

        // グリーンゲージを即座に減らす
        this.greenGauge.SetValue(currentHp);

        // レッドゲージの減少目標値を更新
        this.targetHp = currentHp;

        // レッドゲージと現在HPの差から減少速度を決める
        float redHp = this.redGauge.GetValue();
        float difference = redHp - this.targetHp;

        // 一律でredDecreaseDuration秒でレッドゲージが減るようにする
        if (difference > 0.0f)
        {
            this.redDecreaseSpeed = difference / this.redDecreaseDuration;
        }

        if (this.redGaugeCoroutine == null)
        {
            // 赤ゲージを遅れて減らす
            this.redGaugeCoroutine = this.StartCoroutine(this.UpdateHpGauge());
        }
    }

    /// <summary>
    /// HPが0になっているか取得
    /// </summary>
    /// <returns>HPが0以下ならtrue、違えばfalse</returns>
    public bool IsDead()
    {
        return this.greenGauge.GetValue() <= 0.0f;
    }

    /// <summary>
    /// 現在のHPを取得する
    /// </summary>
    /// <returns>現在のHP</returns>
    public float GetCurrentHp()
    {
        return this.greenGauge.GetValue();
    }
}
