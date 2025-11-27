using System.Collections;
using UnityEngine;
using static Common;

public class SceneManager : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);
    protected TitleSceneState titleSceneState = TitleSceneState.Title;
    bool isFirst = true;


    virtual public void Start()
    {
        StartCoroutine(TitleSceneCoroutine());
    }

    virtual public void Update()
    {

    }


    IEnumerator TitleSceneCoroutine()
    {
        while (true)
        {
            if (!isFirst)
            {
                yield return new WaitForSeconds(1 / 60f);
                continue;
            }

            switch (titleSceneState)
            {
                case TitleSceneState.Title:
                    // タイトル表示
                    TitleStart();
                    break;
                case TitleSceneState.PlayGame:
                    // プレイゲーム処理
                    PlayGameStart();
                    break;
                case TitleSceneState.Setting:
                    // 設定処理
                    SettingStart();
                    break;
                case TitleSceneState.Exit:
                    // ゲーム終了処理
                    ExitStart();
                    break;
                default:
                    break;
            }
            isFirst = false;
            yield return null;
        }
    }



    virtual public void TitleStart()
    {
        Debug.Log("TitleStart");
    }

    virtual public void PlayGameStart()
    {
        Debug.Log("PlayGameStart");
    }

    virtual public void SettingStart()
    {
        Debug.Log("SettingStart");
    }

    virtual public void ExitStart()
    {
        Application.Quit();
    }

    virtual public void ChangeTitleSceneState(TitleSceneState state)
    {
        titleSceneState = state;
        isFirst = true;
    }

    //////////////////////////////////////////////////////////////////////////////



}
