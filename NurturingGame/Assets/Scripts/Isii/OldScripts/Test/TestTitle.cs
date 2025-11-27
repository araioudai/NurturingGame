using UnityEngine;
using static Common;

public class TestTitle : SceneManager
{

    [SerializeField] private GameObject titleUI;
    [SerializeField] private DataManager dataManager;


    override public void Start()
    {
        dataManager = FindFirstObjectByType<DataManager>();
        base.Start();
    }

    override public void TitleStart()
    {
        // タイトル表示
        Debug.Log("TitleStart");
    }

    override public void PlayGameStart()
    {
        // プレイゲーム処理
        Debug.Log("PlayGameStart");
        dataManager.InitLoadPlayerData();
    }

    override public void SettingStart()
    {
        // 設定処理
        Debug.Log("SettingStart");
    }

    override public void ExitStart()
    {
        // ゲーム終了処理
        Debug.Log("ExitStart");
        Application.Quit();
    }


    ///////////////////////////////////////////////////////////////////////////////
    
    public void OnClickPlayGame()
    {
        ChangeTitleSceneState(TitleSceneState.PlayGame);
        titleUI.SetActive(false);
    }


}
