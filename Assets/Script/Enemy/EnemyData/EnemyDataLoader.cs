using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のCSVデータを読み込む処理
/// </summary>
public class EnemyDataLoader : MonoBehaviour
{
    /// <summary>
    /// 敵ステータスのcsvデータファイル
    /// </summary>
    [SerializeField]
    private TextAsset csvFile;

    /// <summary>
    /// データ保存定数
    /// </summary>
    private Dictionary<int, EnemyData> enemyDataList =　new Dictionary<int, EnemyData>();

    /// <summary>
    /// csv読み込み、データを登録
    /// </summary>
    private void Awake()
    {
        string[] lines = this.csvFile.text.Split('\n');

        // 空行を無視
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            // カンマでデータ分割
            string[] values = lines[i].Split(',');

            EnemyData data = new EnemyData();

            data.Id = int.Parse(values[0]);
            data.EnemyName = values[1];
            data.MoveSpeed = float.Parse(values[2]);
            data.ChaseSpeed = float.Parse(values[3]);
            data.SightDistance = float.Parse(values[4]);
            data.SightAngle = float.Parse(values[5]);
            data.ShotInterval = float.Parse(values[6]);

            this.enemyDataList.Add(data.Id, data);
        }
    }

    /// <summary>
    /// 敵IDから敵データを取得
    /// </summary>
    /// <param name="id">敵ID</param>
    /// <returns>敵データ</returns>
    public EnemyData GetEnemyData(int id)
    {
        return this.enemyDataList[id];
    }
}