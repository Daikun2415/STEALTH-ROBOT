using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 音量スライダーを制御するクラス
/// </summary>
public class VolumeSliderController : MonoBehaviour
{
    /// <summary>
    /// BGMの音量スライダー
    /// </summary>
    [SerializeField]
    private Slider bgmSlider;

    /// <summary>
    /// SEの音量スライダー
    /// </summary>
    [SerializeField]
    private Slider seSlider;

    /// <summary>
    /// BGMとSEの初期化処理
    /// </summary>
    private void Start()
    {
        this.bgmSlider.value = AudioVolumeManager.Instance.GetBGMVolume();
        this.seSlider.value = AudioVolumeManager.Instance.GetSEVolume();
    }

    /// <summary>
    /// BGMの音量を変更する
    /// </summary>
    public void ChangeBGMVolume()
    {
        AudioVolumeManager.Instance.SetBGMVolume(this.bgmSlider.value);
    }

    /// <summary>
    /// SEの音量を変更する
    /// </summary>
    public void ChangeSEVolume()
    {
        AudioVolumeManager.Instance.SetSEVolume(this.seSlider.value);
    }
}