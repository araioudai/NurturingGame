using System.IO;
using UnityEngine;
using static Common;

public class DataManager : MonoBehaviour
{
    [SerializeField] private SaveData playerDataList;           // プレイヤーデータリスト
    [SerializeField] private PlayerData currentPlayerData;      // 現在のプレイヤーデータ

    [Header("設定")]
    [SerializeField, Range(0, 127)] private int playerNoCurrent = 0; // プレイヤー番号を指定するための変数
    [SerializeField] private JobHandle jobHandle; // 初期職業を指定するための変数
    [SerializeField] private string playerName = "UnityChan"; // プレイヤー名を指定するための変数
    [SerializeField] private string saveFileName = "Players.json"; // 保存ファイル名


    void Start()
    {
        //LoadPlayerData();
    }

    void Update()
    {

    }

    /// <summary>
    /// プレイヤーデータの初期化と読込
    /// </summary>
    public void InitLoadPlayerData()
    {
        Debug.Log("InitLoadPlayerData", this);
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(filePath))
        {
            Debug.Log("InitLoadPlayerData: " + filePath, this);

            string json = File.ReadAllText(filePath);
            playerDataList = JsonUtility.FromJson<SaveData>(json);
            if (playerDataList.PlayerNoCount > playerNoCurrent && playerNoCurrent >= 0 && playerDataList.playerData[playerNoCurrent] != null)
            {
                Debug.Log("PlayerNo: " + playerNoCurrent, this);
                LoadPlayerData(playerNoCurrent);
            }
            else if (playerNoCurrent >= 0)
            {
                Debug.LogWarning("プレイヤーデータが存在しません: " + playerNoCurrent, this);

                // for (int i = 0; i < playerNoCurrent; i++)
                // {
                //     if (playerDataList.PlayerNoCount > i) continue;
                //     playerDataList.playerData.Add(null);
                //     Debug.LogWarning("プレイヤーデータが存在しません(NULL): " + i, this);
                // }

                CreatePlayer();       // currentPlayerDataを作成
                playerDataList.playerData.Add(currentPlayerData);
                //SavePlayerData();               // 初期データを保存
            }
            else
            {
                Debug.LogWarning("不明な値です: " + playerNoCurrent, this);
            }
        }
        else
        {
            Debug.LogWarning("プレイヤーデータファイルが見つかりません: " + filePath, this);
            CreatePlayer();     // currentPlayerDataを作成

            // プレイヤーデータリストを初期化
            playerDataList = new SaveData
            {
                playerData = new System.Collections.Generic.List<PlayerData> { currentPlayerData }
            };

            SavePlayerData(); // 初期データを保存
        }


        // string filePath = Path.Combine(Application.persistentDataPath, "Player" + playerNo + ".json");
        // if (File.Exists(filePath))
        // {
        //     Debug.Log("LoadPlayerData: " + filePath);
        //     string json = File.ReadAllText(filePath);
        //     currentPlayerData = JsonUtility.FromJson<PlayerData>(json);
        // }
        // else
        // {
        //     string firstStatusPath = Resources.Load<TextAsset>($"JsonData/{jobHandle}").text;
        //     if (string.IsNullOrEmpty(firstStatusPath))
        //     {
        //         Debug.LogError("ジョブの初期ステータスデータの読み込みに失敗しました: " + jobHandle);
        //         return;
        //     }

        //     Debug.LogWarning("プレイヤーデータファイルが見つかりません: " + filePath);
        //     // 初期データを設定するなどの処理を行う
        //     currentPlayerData = new PlayerData
        //     {
        //         name = playerName,
        //         job = jobHandle,
        //         level = 1,
        //         exp = 0,
        //         firstStatus = JsonUtility.FromJson<Status>(firstStatusPath),
        //         addStatus = new Status(),
        //         statusPoints = 0
        //         //skills = new List<string>()
        //     };
        //     SavePlayerData(); // 初期データを保存
        // }
    }

    /// <summary>
    /// プレイヤーデータ作成
    /// </summary>
    void PlayerDataCreate()
    {
        string firstStatusPath = Resources.Load<TextAsset>($"JsonData/{jobHandle}").text;
        if (string.IsNullOrEmpty(firstStatusPath))
        {
            Debug.LogError("ジョブの初期ステータスデータの読み込みに失敗しました: " + jobHandle, this);
            return;
        }

        // 初期データを設定するなどの処理を行う
        currentPlayerData = new PlayerData
        {
            name = playerName,
            job = jobHandle,
            level = 1,
            exp = 0,
            firstStatus = JsonUtility.FromJson<Status>(firstStatusPath),
            addStatus = new Status(),
            statusPoints = 0
            //skills = new List<string>()
        };
    }

    /// <summary>
    /// プレイヤーデータ保存
    /// </summary>
    [ContextMenu("SavePlayerData")]
    public void SavePlayerData()
    {
        Debug.Log("SavePlayerData", this);

        // 現在のプレイヤーデータをリストに反映
        playerDataList.playerData[playerNoCurrent] = currentPlayerData;
        playerDataList.currentPlayerNo = playerNoCurrent;
        playerDataList.PlayerNoCount = playerDataList.playerData.Count;

        // 保存先のファイルパスを生成
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        string json = JsonUtility.ToJson(playerDataList, true);
        File.WriteAllText(filePath, json);

        //string filePath = Path.Combine(Application.persistentDataPath, "Player" + playerNo + ".json");
        // string json = JsonUtility.ToJson(currentPlayerData, true);
        // File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// プレイヤーデータのロード
    /// </summary>
    /// <param name="playerNo">プレイヤー番号</param>
    public void LoadPlayerData(int playerNo)
    {
        playerNoCurrent = playerNo;
        currentPlayerData = playerDataList.playerData[playerNoCurrent];
    }

    /// <summary>
    /// 全データ削除
    /// </summary>
    public void AllDeleteData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("DeleteData: " + filePath, this);
        }
        else
        {
            Debug.LogWarning("プレイヤーデータファイルが見つかりません: " + filePath, this);
        }
    }

    /// <summary>
    /// プレイヤーデータ削除
    /// </summary>
    /// <param name="playerNo">プレイヤー番号</param>
    public void PlayerDataDelete(int playerNo)
    {
        playerNo = playerNoCurrent;

        Debug.Log("PlayerDataDelete: " + playerNo, this);

        if (playerNo < 0 || playerNo >= playerDataList.PlayerNoCount)
        {
            Debug.LogWarning("無効なプレイヤー番号です: " + playerNo, this);
            return;
        }

        playerDataList.playerData.RemoveAt(playerNo);
        playerDataList.PlayerNoCount = playerDataList.playerData.Count;

        // 現在のプレイヤー番号が削除された番号以上の場合、-1する
        if (playerNoCurrent >= playerNo && playerNoCurrent > 0)
        {
            playerNoCurrent--;
        }

        SavePlayerData();
    }

    /// <summary>
    /// プレイヤー作成
    /// </summary>
    public void CreatePlayer()
    {
        Debug.Log("CreatePlayer: " + jobHandle, this);

        string firstStatusPath = Resources.Load<TextAsset>($"JsonData/{jobHandle}").text;
        if (string.IsNullOrEmpty(firstStatusPath))
        {
            Debug.LogError("ジョブの初期ステータスデータの読み込みに失敗しました: " + jobHandle, this);
            return;
        }

        // 初期データを設定するなどの処理を行う
        currentPlayerData = new PlayerData
        {
            name = playerName,
            job = jobHandle,
            level = 1,
            exp = 0,
            firstStatus = JsonUtility.FromJson<Status>(firstStatusPath),
            addStatus = new Status(),
            statusPoints = 0
            //skills = new List<string>()
        };

        playerDataList.playerData.Add(currentPlayerData);
        playerDataList.PlayerNoCount = playerDataList.playerData.Count;
        playerNoCurrent = playerDataList.PlayerNoCount - 1; // 新しいプレイヤーを現在のプレイヤーに設定

        SavePlayerData();
    }




}
