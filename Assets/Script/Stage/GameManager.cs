using UnityEngine;

/// <summary>
/// ゲームシーンの管理クラス
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// ステージ選択画面からのステージID
    /// </summary>
    private static int selectedStageID;

    /// <summary>
    /// ステージ選択画面からのクリアタイム
    /// </summary>
    private static float selectedClearTime;

    /// <summary>
    /// ステージ選択画面からステージ情報が渡されたか
    /// </summary>
    private static bool hasSelectedStage;

    /// <summary>
    /// ステージのプレハブ
    /// </summary>
    [SerializeField]
    private GameObject[] stagePrefabs;

    /// <summary>
    /// ステージのID
    /// </summary>
    [SerializeField]
    private int stageID;

    /// <summary>
    /// プレイヤーの制御クラス
    /// </summary>
    [SerializeField]
    private PlayerController player;

    /// <summary>
    /// 敵データ読み込み
    /// </summary>
    [SerializeField]
    private EnemyDataLoader enemyDataLoader;

    /// <summary>
    /// リザルト制御クラス
    /// </summary>
    [SerializeField]
    private ResultController resultController;

    /// <summary>
    /// タイム制御クラス
    /// </summary>
    [SerializeField]
    private TimeController timeController;

    /// <summary>
    /// 星がもらえるクリアタイム
    /// </summary>
    [SerializeField]
    private float clearTime = 60.0f;

    /// <summary>
    /// ステージBGM管理クラス
    /// </summary>
    [SerializeField]
    private StageBGMManager stageBGMManager;

    /// <summary>
    /// ゲーム開始時の処理
    /// </summary>
    private void Start()
    {
        if (GameManager.hasSelectedStage)
        {
            this.stageID = GameManager.selectedStageID;
            this.clearTime = GameManager.selectedClearTime;
        }

        this.stageBGMManager.PlayBGM(StageBGMManager.BGMType.Normal);

        this.StartStage(this.stageID);
    }

    /// <summary>
    /// ステージ情報を設定する
    /// </summary>
    /// <param name="stageID">ステージID</param>
    /// <param name="clearTime">クリアタイム</param>
    public static void SetStageData(int stageID, float clearTime)
    {
        GameManager.selectedStageID = stageID;
        GameManager.selectedClearTime = clearTime;
        GameManager.hasSelectedStage = true;
    }

    /// <summary>
    /// ステージを用意する
    /// </summary>
    /// <param name="stageID">ステージのID</param>
    public void StartStage(int stageID)
    {
        // 指定されたIDのステージを生成
        GameObject stageObject = Instantiate(this.stagePrefabs[stageID], Vector3.zero, Quaternion.identity);

        // ステージの制御クラスを取得
        StageController stageController = stageObject.GetComponent<StageController>();

        // ステージの制御クラスをデータを渡して初期化
        stageController.InitializeStage(this.player, this.resultController, this.enemyDataLoader, this.timeController, this.clearTime, this.stageBGMManager);

        // リザルト制御クラスにIDを渡して初期化
        this.resultController.Initialize(stageID);
    }

    /// <summary>
    /// ミッションクリアタイムを取得
    /// </summary>
    /// <returns>ミッションクリアタイム</returns>
    public float GetClearTime()
    {
        return this.clearTime;
    }

    /// <summary>
    /// 現在のステージIDを取得する
    /// </summary>
    /// <returns>現在のステージID</returns>
    public int GetStageId()
    {
        return this.stageID;
    }
}
