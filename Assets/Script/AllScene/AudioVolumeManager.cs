using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// オーディオの音量を管理するクラス
/// </summary>
public class AudioVolumeManager : MonoBehaviour
{
    /// <summary>
    /// BGMの音量を保存するPlayerPrefsのキー
    /// </summary>
    private const string BGMVolumeKey = "BGMVolume";

    /// <summary>
    /// SEの音量を保存するPlayerPrefsのキー
    /// </summary>
    private const string SEVolumeKey = "SEVolume";

    /// <summary>
    /// BGMの音量を調整するAudioMixerのパラメータ名
    /// </summary>
    private const string BgmVolumeParameter = "BGM_Volume";

    /// <summary>
    /// SEの音量を調整するAudioMixerのパラメータ名
    /// </summary>
    private const string SEVolumeParameter = "SE_Volume";

    /// <summary>
    /// BGMのデフォルト音量
    /// </summary>
    private const float DefaultBGMVolume = 0.4f;

    /// <summary>
    /// SEのデフォルト音量
    /// </summary>
    private const float DefaultSEVolume = 0.6f;

    /// <summary>
    /// 音量調整するAudioMixer
    /// </summary>
    [SerializeField]
    private AudioMixer audioMixer;

    /// <summary>
    /// AudioVolumeManagerのインスタンス
    /// </summary>
    public static AudioVolumeManager Instance { get; private set; }

    /// <summary>
    /// AudioVolumeManagerのシングルトンインスタンスを初期化する
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
    /// BGMの音量を変更する
    /// </summary>
    /// <param name="volume">変更する音量</param>
    public void SetBGMVolume(float volume)
    {
        this.SetMixerVolume(BgmVolumeParameter, volume);

        PlayerPrefs.SetFloat(BGMVolumeKey, volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// SEの音量を変更する
    /// </summary>
    /// <param name="volume">変更する音量</param>
    public void SetSEVolume(float volume)
    {
        this.SetMixerVolume(SEVolumeParameter, volume);

        PlayerPrefs.SetFloat(SEVolumeKey, volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// BGMの音量を取得する
    /// </summary>
    /// <returns>BGMの音量</returns>
    public float GetBGMVolume()
    {
        return PlayerPrefs.GetFloat(BGMVolumeKey, DefaultBGMVolume);
    }

    /// <summary>
    /// SEの音量を取得する
    /// </summary>
    /// <returns>SEの音量</returns>
    public float GetSEVolume()
    {
        return PlayerPrefs.GetFloat(SEVolumeKey, DefaultSEVolume);
    }

    /// <summary>
    /// AudioMixerのパラメータを設定する
    /// </summary>
    /// <param name="parameterName">設定するパラメータ名</param>
    /// <param name="volume">設定する音量</param>
    private void SetMixerVolume(string parameterName, float volume)
    {
        float decibel = volume <= 0.0f ? -80.0f : Mathf.Log10(volume) * 20.0f;

        this.audioMixer.SetFloat(parameterName, decibel);
    }

    /// <summary>
    /// 保存された音量をロードしてAudioMixerに反映する
    /// </summary>
    public void LoadVolume()
    {
        this.SetMixerVolume(BgmVolumeParameter, this.GetBGMVolume());
        this.SetMixerVolume(SEVolumeParameter, this.GetSEVolume());
    }
}
