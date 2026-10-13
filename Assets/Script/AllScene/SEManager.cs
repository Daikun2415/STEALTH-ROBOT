using UnityEngine;

/// <summary>
/// ボタンのSEを再生するクラス
/// </summary>
public class SEManager : MonoBehaviour
{
    /// <summary>
    /// ボタンSE再生用のAudioSource
    /// </summary>
    [SerializeField]
    private AudioSource seAudioSource;

    /// <summary>
    /// 直前の再生SE
    /// </summary>
    private AudioClip currentSE;

    /// <summary>
    /// 唯一のButtonSEManager
    /// </summary>
    public static SEManager Instance { get; private set; }

    /// <summary>
    /// ButtonSEManagerのシングルトンインスタンスを初期化する
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 指定したSEを再生する
    /// </summary>
    /// <param name="se">再生するSE</param>
    public void PlaySE(AudioClip se)
    {
        // 連打で音が大きくならないように直前のSEと同じ場合は一度止める
        if (this.currentSE == se)
        {
            this.seAudioSource.Stop();
        }

        this.currentSE = se;
        this.seAudioSource.PlayOneShot(se);
    }

    /// <summary>
    /// SEの再生を停止する
    /// </summary>
    public void StopSE()
    {
        this.seAudioSource.Stop();
    }
}