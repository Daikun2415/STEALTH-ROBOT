using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームの難易度を管理するクラス
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    /// <summary>
    /// 難易度表示用テキスト
    /// </summary>
    private Text difficultyText;

    /// <summary>
    /// 現在の難易度
    /// </summary>
    [SerializeField]
    private Difficulty currentDifficulty = Difficulty.Easy;

    /// <summary>
    /// 難易度
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// イージーモード
        /// </summary>
        Easy,

        /// <summary>
        /// ノーマルモード
        /// </summary>
        Normal,

        /// <summary>
        /// ハードモード
        /// </summary>
        Hard
    }

    /// <summary>
    /// 唯一のDifficultyManager
    /// </summary>
    public static DifficultyManager Instance { get; private set; }

    /// <summary>
    /// 初期設定
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
    /// 難易度表示用テキストを設定する
    /// </summary>
    /// <param name="text">難易度表示用テキスト</param>
    public void SetDifficultyText(Text text)
    {
        this.difficultyText = text;
        this.UpdateDifficultyText();
    }

    /// <summary>
    /// 難易度を切り替える
    /// </summary>
    public void ChangeDifficulty()
    {
        switch (this.currentDifficulty)
        {
            case Difficulty.Easy:
                this.currentDifficulty = Difficulty.Normal;
                break;

            case Difficulty.Normal:
                this.currentDifficulty = Difficulty.Hard;
                break;

            case Difficulty.Hard:
                this.currentDifficulty = Difficulty.Easy;
                break;
        }

        this.UpdateDifficultyText();
    }

    /// <summary>
    /// 現在の難易度を取得する
    /// </summary>
    /// <returns>現在の難易度</returns>
    public Difficulty GetDifficulty()
    {
        return this.currentDifficulty;
    }

    /// <summary>
    /// 難易度の表示を更新する
    /// </summary>
    private void UpdateDifficultyText()
    {
        switch (this.currentDifficulty)
        {
            case Difficulty.Easy:
                this.difficultyText.text = "イージー";
                break;

            case Difficulty.Normal:
                this.difficultyText.text = "ノーマル";
                break;

            case Difficulty.Hard:
                this.difficultyText.text = "ハード";
                break;
        }
    }
}
