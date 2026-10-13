using System.IO;
using UnityEngine;

/// <summary>
/// セーブデータを削除するテスト用スクリプト
/// </summary>
public class ResetData : MonoBehaviour
{
    /// <summary>
    /// JSONのセーブデータを削除する
    /// </summary>
    public void ResetSaveData()
    {
        this.DeleteSaveData("Easy");
        this.DeleteSaveData("Normal");
        this.DeleteSaveData("Hard");

        PlayerPrefs.DeleteKey("BGMVolume");
        PlayerPrefs.DeleteKey("SEVolume");
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 指定した難易度のセーブデータを削除する
    /// </summary>
    /// <param name="difficulty">難易度名</param>
    private void DeleteSaveData(string difficulty)
    {
        string path = Path.Combine(Application.persistentDataPath, difficulty + ".json");

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
