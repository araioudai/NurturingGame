using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    #region private変数
    [Header("タイトルパネル")]
    [SerializeField] private GameObject titlePanel;     //タイトルパネル
    [Header("ステージセレクト")]
    [SerializeField] private GameObject stagePanel;     //ステージパネル
    [Header("育成パネル")]
    [SerializeField] private GameObject nurturingPanel; //育成パネル

    private GameObject objctName;                       //オブジェクト名
    #endregion

    #region Unityイベント関数
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titlePanel.SetActive(true);
        stagePanel.SetActive(false);
        nurturingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region タイトル

    #region 育成が押された
    /// <summary>
    /// 育成モードにする
    /// </summary>
    public void PushNurturing()
    {
        titlePanel.SetActive(false);
        stagePanel.SetActive(false);
        nurturingPanel.SetActive(true);
    }
    #endregion

    #region ステージセレクトが押された
    /// <summary>
    /// ステージセレクトパネルを有効
    /// </summary>
    public void PushStage()
    {
        titlePanel.SetActive(false);
        nurturingPanel.SetActive(false);
        stagePanel.SetActive(true);
    }
    #endregion

    #endregion

    #region ステージセレクト

    #region 何かのステージが押された
    /// <summary>
    /// ステージ番号に応じたステージのゲームスタート
    /// </summary>
    public void GameStart()
    {
        objctName = EventSystem.current.currentSelectedGameObject;
        string name = objctName.name;

        //ステージ番号に変換
        if (name.StartsWith("Stage"))
        {
            string numberPart = name.Replace("Stage", "");

            //TryParseで安全に整数に変換（失敗してもクラッシュしない）
            if (int.TryParse(numberPart, out int number))
            {
                StageIndex.Instance.SetIndex(number); //選択されたステージ番号を保存
                StartCoroutine(StageLoad());
            }
            else
            {
                Debug.LogWarning("ステージ名に数値が含まれていません: " + name);
                StartCoroutine(TextCountDown());
            }
        }
    }

    IEnumerator StageLoad()
    {
        yield return new WaitForSeconds(0.5f);
        //SceneManager.LoadScene("TestGame");
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator TextCountDown()
    {
        //TextRock.SetActive(true);

        yield return new WaitForSeconds(1.0f); //1秒待つ

        //TextRock.SetActive(false);
    }

    #endregion

    #endregion

    #region 戻るが押された
    /// <summary>
    /// タイトルパネルへ戻る
    /// </summary>
    public void PushExit()
    {
        titlePanel.SetActive(true);
        stagePanel.SetActive(false);
        nurturingPanel.SetActive(false);
    }
    #endregion
}
