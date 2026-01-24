using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Udon.Commons;

public class GetResource : MonoBehaviour
{
    [SerializeField] private EndResource endResource;
    [SerializeField] private string saveFileName = "players.json";  // 保存ファイル名
    [SerializeField] private Text getText;



    [SerializeField] bool clear = false;
    [SerializeField] bool over = false;

    void Start()
    {
        Debug.Log("Game Clear Points: " + endResource.gameClearPoints);
        Debug.Log("Game Over Points: " + endResource.gameOverPoints);
        GetResourceData();
    }

    void GetResourceData()
    {
        //ここにデータ読み込み処理
        Debug.Log("データロード", this);
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(filePath))
        {
            Debug.Log("データロード: " + filePath, this);
            string json = File.ReadAllText(filePath);
            var playerData = JsonUtility.FromJson<SaveData>(json);
            // GetComponent<StatusManager>().PlayerStatesSet(StatusType.Player, playerData.playerTC.buildingLevel);


            if(clear)
            {
                Debug.Log("Game Clear Points: " + endResource.gameClearPoints);
                playerData.resources += endResource.gameClearPoints;
                getText.text = "獲得ポイント: " + endResource.gameClearPoints.ToString();
            }
            else if(over)
            {
                Debug.Log("Game Over Points: " + endResource.gameOverPoints);
                playerData.resources += endResource.gameOverPoints;
                getText.text = "獲得ポイント: " + endResource.gameOverPoints.ToString();
            }
            else
            {
                getText.text = "獲得ポイント: 0";
            }


            // セーブデータを更新して保存
            json = JsonUtility.ToJson(playerData);
            File.WriteAllText(filePath, json);
            Debug.Log("データ保存完了", this);

        }
        else
        {
            Debug.LogWarning("データファイルが存在しません: " + filePath, this);

        }

    }










}
