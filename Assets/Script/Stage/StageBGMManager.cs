using UnityEngine;

/// <summary>
/// ステージシーンのBGMを制御するクラス
/// </summary>
public class StageBGMManager : MonoBehaviour
{
    /// <summary>
    /// BGM再生用のAudioSource
    /// </summary>
    [SerializeField]
    private AudioSource bgmAudioSource;

    /// <summary>
    /// 通常時のBGM
    /// </summary>
    [SerializeField]
    private AudioClip normalBGM;

    /// <summary>
    /// 発見時のBGM
    /// </summary>
    [SerializeField]
    private AudioClip discoveredBGM;

    /// <summary>
    /// ゲームオーバー時のBGM
    /// </summary>
    [SerializeField]
    private AudioClip gameOverBGM;

    /// <summary>
    /// リザルト時のBGM
    /// </summary>
    [SerializeField]
    private AudioClip resultBGM;

    /// <summary>
    /// BGMの種類
    /// </summary>
    public enum BGMType
    {
        /// <summary>
        /// 通常時のBGM
        /// </summary>
        Normal,

        /// <summary>
        /// 発見時のBGM
        /// </summary>
        Discovered,

        /// <summary>
        /// ゲームオーバー時のBGM
        /// </summary>
        GameOver,

        /// <summary>
        /// リザルト時のBGM
        /// </summary>
        Result
    }

    /// <summary>
    /// BGMを切り替える
    /// </summary>
    /// <param name="bgmType">再生するBGMの種類</param>
    public void PlayBGM(BGMType bgmType)
    {
        // 指定されたBGMの種類に応じてAudioClipを取得
        AudioClip bgm = bgmType switch
        {
            BGMType.Normal => this.normalBGM,
            BGMType.Discovered => this.discoveredBGM,
            BGMType.GameOver => this.gameOverBGM,
            BGMType.Result => this.resultBGM,
            _ => null
        };

        // すでに同じBGMが再生されている場合は何もしない
        if (this.bgmAudioSource.clip == bgm)
        {
            return;
        }

        this.bgmAudioSource.Stop();
        this.bgmAudioSource.clip = bgm;
        this.bgmAudioSource.Play();
    }
}
