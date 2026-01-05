using System.IO;
using UnityEngine;
using static Udon.Commons;

[RequireComponent(typeof(TextManager))]
[RequireComponent(typeof(StatusManager))]
public class GameDataManager : MonoBehaviour
{
    [SerializeField] public SaveData playerData;

    [SerializeField] private string playerName; // プレイヤー名を指定するための変数
[SerializeField] private string saveFileName = "players.json";  // 保存ファイル名

    void Start()
    {
        LoadGameData();
    }











    [ContextMenu("LoadGameData")]
    public void LoadGameData()
    {
        //ここにデータ読み込み処理
        Debug.Log("データロード", this);
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(filePath))
        {
            Debug.Log("データロード: " + filePath, this);
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<SaveData>(json);
            GetComponent<StatusManager>().PlayerStatesSet(StatusType.Player, playerData.playerTC.buildingLevel);

            // モブ兵士のステータス設定
            for (int i = 0; i < (int)JobType.Count; i++)
            {
                JobType jobType = (JobType)i;
                int jobLevel = playerData.trainingCentre.tcLevelUp.GetJobLevelText(jobType);
                GetComponent<StatusManager>().MobStatesSet(StatusType.Mob, jobType, jobLevel);
            }
        }
        else
        {
            Debug.LogWarning("データファイルが存在しません: " + filePath, this);
            CreateData();
            SaveData();
            GetComponent<StatusManager>().PlayerStatesInit(playerData);
            GetComponent<StatusManager>().MobStatesInit(playerData);
        }

        GetComponent<TextManager>().InitTextUpdate(playerData);
    }


    /// <summary>
    /// データ作成
    /// </summary>
    void CreateData()
    {
        //ここにデータ作成処理
        Debug.Log("データ作成", this);
        playerData = new InitData().CreateInitialPlayerData(playerName);
    }

    /// <summary>
    /// データ保存
    /// </summary>
    [ContextMenu("SaveData")]
    public void SaveData()
    {
        //ここにデータ保存処理
        Debug.Log("データ保存", this);
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("データ保存: " + filePath, this);
    }

    /// <summary>
    /// データ削除
    /// </summary>
    [ContextMenu("DataDelete")]
    public void DataDelete()
    {
        //ここにデータ削除処理
        Debug.Log("データ削除", this);
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("データ削除: " + filePath, this);
            CreateData();
            Debug.Log("新規データ作成", this);
            SaveData();
        }
        else
        {
            Debug.LogWarning("データファイルが存在しません: " + filePath, this);
        }
    }
}
