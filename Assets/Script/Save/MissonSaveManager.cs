using System.IO;
using UnityEngine;

/// <summary>
/// ミッション進捗のセーブ・ロードを管理するクラス
/// </summary>
public class MissionSaveManager : MonoBehaviour
{
    /// <summary>
    /// ステージ数
    /// </summary>
    [SerializeField]
    private int stageCount = 3;

    /// <summary>
    /// MissonSaveManagerをシングルトンインスタンス化
    /// </summary>
    public static MissionSaveManager Instance { get; private set; }

    /// <summary>
    /// MissonSaveManagerのシングルトンインスタンスを初期化する
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
    /// ミッション進捗を保存する
    /// </summary>
    /// <param name="stageId">ステージID</param>
    /// <param name="cleared">クリア状態</param>
    /// <param name="stars">星の獲得状態</param>
    public void SaveMission(int stageId, bool cleared, bool[] stars)
    {
        // ミッション進捗データをロードする
        MissionSaveData data = this.LoadData();

        // ステージIDに対応するミッションデータを更新する
        for (int i = 0; i < data.Missions.Length; i++)
        {
            if (data.Missions[i].StageId != stageId)
            {
                continue;
            }

            data.Missions[i].UpdateData(cleared, stars);

            this.SaveData(data);
            return;
        }
    }

    /// <summary>
    /// ミッション進捗を読み込む
    /// </summary>
    /// <param name="stageId">ステージID</param>
    /// <returns>ミッションデータ</returns>
    public MissionData GetMission(int stageId)
    {
        // ミッション進捗データをロードする
        MissionSaveData data = this.LoadData();

        // ステージIDに対応するミッションデータを返す
        for (int i = 0; i < data.Missions.Length; i++)
        {
            if (data.Missions[i].StageId == stageId)
            {
                return data.Missions[i];
            }
        }

        return null;
    }

    /// <summary>
    /// ミッション進捗データをロードする
    /// </summary>
    /// <returns>ミッション進捗データ</returns>
    private MissionSaveData LoadData()
    {
        // セーブファイルのパスを取得する
        string path = this.GetSavePath();

        // セーブファイルがあるか確認
        if (!File.Exists(path))
        {
            // なければ新しく作る
            return this.CreateDefaultData();
        }

        // あれば読み込む
        string json = File.ReadAllText(path);

        // jsonをC#のオブジェクトに変換
        MissionSaveData data = JsonUtility.FromJson<MissionSaveData>(json);

        // 不足しているステージのミッションデータを追加する
        this.AddMissingMissions(data);

        // ミッション進捗データを返す
        return data;
    }

    /// <summary>
    /// ミッション進捗データを保存する
    /// </summary>
    /// <param name="data">保存するミッション進捗データ</param>
    private void SaveData(MissionSaveData data)
    {
        // C#のオブジェクトをjsonに変換する
        string json = JsonUtility.ToJson(data, true);

        // jsonをセーブファイルに書き込む
        File.WriteAllText(this.GetSavePath(), json);
    }

    /// <summary>
    /// デフォルトのミッション進捗データを作成する
    /// </summary>
    /// <returns>デフォルトのミッション進捗データ</returns>
    private MissionSaveData CreateDefaultData()
    {
        MissionSaveData data = new MissionSaveData();

        MissionData[] missions = new MissionData[this.stageCount];

        for (int i = 0; i < this.stageCount; i++)
        {
            missions[i] = this.CreateMissionData(i);
        }

        data.SetMissions(missions);

        return data;
    }

    /// <summary>
    /// ミッションデータを作成する
    /// </summary>
    /// <param name="stageId">ステージID</param>
    /// <returns>初期状態のミッションデータ</returns>
    private MissionData CreateMissionData(int stageId)
    {
        MissionData missionData = new MissionData();

        missionData.Initialize(stageId, false, new bool[3]);

        return missionData;
    }

    /// <summary>
    /// 不足しているステージのミッションデータを追加する
    /// </summary>
    /// <param name="data">ミッション進捗データ</param>
    private void AddMissingMissions(MissionSaveData data)
    {
        int currentStageCount = data.Missions.Length;

        if (currentStageCount >= this.stageCount)
        {
            return;
        }

        MissionData[] missions =
            new MissionData[this.stageCount];

        for (int i = 0; i < currentStageCount; i++)
        {
            missions[i] = data.Missions[i];
        }

        for (int i = currentStageCount; i < this.stageCount; i++)
        {
            missions[i] = this.CreateMissionData(i);
        }

        data.SetMissions(missions);

        this.SaveData(data);
    }

    /// <summary>
    /// セーブファイルのパスを取得する
    /// </summary>
    /// <returns>セーブファイルのパス</returns>
    private string GetSavePath()
    {
        string difficulty = DifficultyManager.Instance.GetDifficulty().ToString();

        return Path.Combine(Application.persistentDataPath, difficulty + ".json");
    }
}