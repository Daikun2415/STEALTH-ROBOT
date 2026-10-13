using UnityEngine;

/// <summary>
/// 遊び方パネルの制御クラス
/// </summary>
public class HowToPlayPanelController : MonoBehaviour
{
    /// <summary>
    /// 遊び方パネル
    /// </summary>
    [SerializeField]
    private GameObject[] howToPlayPanel;

    /// <summary>
    /// 表示するパネルのインデックス
    /// </summary>
    [SerializeField]
    private int panelIndex = 0;

    /// <summary>
    /// パネルの切り替え方向
    /// </summary>
    private enum PanelDirection
    {
        /// <summary>
        /// 次のパネル
        /// </summary>
        Next,

        /// <summary>
        /// 前のパネル
        /// </summary>
        Back
    }

    /// <summary>
    /// 最初にパネルを非表示にする
    /// </summary>
    private void Start()
    {
        foreach (var panel in this.howToPlayPanel)
        {
            panel.SetActive(false);
        }
    }

    /// <summary>
    /// 遊び方パネルを開く
    /// </summary>
    public void OnTapOpenHowToPlayPanelButton()
    {
        this.SetCurrentPanelActive(true);
    }

    /// <summary>
    /// 遊び方パネルを閉じる
    /// </summary>
    public void OnTapCloseHowToPlayPanelButton()
    {
        this.SetCurrentPanelActive(false);
    }

    /// <summary>
    /// 次の遊び方パネルを開く
    /// </summary>
    public void OnTapNextHowToPlayPanelButton()
    {
        this.ChangePanel(PanelDirection.Next);
    }

    /// <summary>
    /// 前の遊び方パネルを開く
    /// </summary>
    public void OnTapBackHowToPlayPanelButton()
    {
        this.ChangePanel(PanelDirection.Back);
    }

    /// <summary>
    /// 現在のパネルの表示状態を変更する
    /// </summary>
    /// <param name="isActive">表示するかどうか</param>
    private void SetCurrentPanelActive(bool isActive)
    {
        if (this.howToPlayPanel.Length == 0)
        {
            return;
        }

        GameObject panel = this.howToPlayPanel[this.panelIndex];

        if (panel != null)
        {
            panel.SetActive(isActive);
        }
    }

    /// <summary>
    /// 表示するパネルを変更する
    /// </summary>
    /// <param name="direction">変更する方向</param>
    private void ChangePanel(PanelDirection direction)
    {
        if (this.howToPlayPanel.Length == 0)
        {
            return;
        }

        this.SetCurrentPanelActive(false);

        // 移動方向に応じてインデックスを変更する
        int moveAmount = direction == PanelDirection.Next ? 1 : -1;

        // パネルのインデックスを変更し、配列の最後まで行ったら最初に戻るようにする
        this.panelIndex = (this.panelIndex + moveAmount + this.howToPlayPanel.Length) % this.howToPlayPanel.Length;

        this.SetCurrentPanelActive(true);
    }
}
