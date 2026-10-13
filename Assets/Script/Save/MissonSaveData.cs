using System;
using UnityEngine;

/// <summary>
/// ミッションのセーブデータ
/// </summary>
[Serializable]
public class MissionSaveData
{
    /// <summary>
    /// ミッションのデータ
    /// </summary>
    [SerializeField]
    private MissionData[] missions;

    /// <summary>
    /// ミッションのデータを取得する
    /// </summary>
    public MissionData[] Missions
    {
        get
        {
            return this.missions;
        }
    }

    /// <summary>
    /// ミッションのデータを設定する
    /// </summary>
    /// <param name="missions">ミッションのデータ</param>
    public void SetMissions(MissionData[] missions)
    {
        this.missions = missions;
    }
}

/// <summary>
/// ミッション1つ分のデータ
/// </summary>
[Serializable]
public class MissionData
{
    /// <summary>
    /// ステージID
    /// </summary>
    [SerializeField]
    private int stageId;

    /// <summary>
    /// クリア済みか
    /// </summary>
    [SerializeField]
    private bool cleared;

    /// <summary>
    /// 星の獲得状態
    /// </summary>
    [SerializeField]
    private bool[] stars;

    /// <summary>
    /// ステージIDを取得する
    /// </summary>
    public int StageId
    {
        get
        {
            return this.stageId;
        }
    }

    /// <summary>
    /// クリア状態を取得する
    /// </summary>
    public bool Cleared
    {
        get
        {
            return this.cleared;
        }
    }

    /// <summary>
    /// 星の獲得状態を取得する
    /// </summary>
    public bool[] Stars
    {
        get
        {
            return this.stars;
        }
    }

    /// <summary>
    /// ミッションデータを初期化する
    /// </summary>
    /// <param name="stageId">ステージID</param>
    /// <param name="cleared">クリア状態</param>
    /// <param name="stars">星の獲得状態</param>
    public void Initialize(int stageId, bool cleared, bool[] stars)
    {
        this.stageId = stageId;
        this.cleared = cleared;
        this.stars = stars;
    }

    /// <summary>
    /// ミッションのクリア状態と星を更新する
    /// </summary>
    /// <param name="cleared">クリア状態</param>
    /// <param name="newStars">今回獲得した星</param>
    public void UpdateData(bool cleared, bool[] newStars)
    {
        this.cleared = this.cleared || cleared;

        for (int i = 0; i < newStars.Length; i++)
        {
            if (newStars[i])
            {
                this.stars[i] = true;
            }
        }
    }
}