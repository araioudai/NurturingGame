using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleManagerkari : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] private List<string> sceneToLoad;

    [Header("遷移先シーン選択ボタン")]
    [SerializeField] private List<Button> sceneLoadButtons;

    void Start()
    {
        for (int i = 0; i < sceneLoadButtons.Count; i++)
        {
            int index = i; // ローカル変数にコピーしてクロージャの問題を回避
            sceneLoadButtons[i].onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad[index]);
            });
        }
    }


// #if UNITY_EDITOR ～ #endif で囲まれた部分はエディタ上でのみ有効になる
#if UNITY_EDITOR
    // インスペクタに表示するためのSceneAsset型変数
    [Header("遷移先シーン選択")] // インスペクタに見出しを表示
    [SerializeField] private List<SceneAsset> sceneAsset; // ここにシーンファイルをD&Dする
    // インスペクタで値が変更された時などに自動で呼ばれるメソッド
    private void OnValidate()
    {
        // sceneAssetフィールドにシーンが設定されたら
        if (sceneAsset != null)
        {
            // そのシーンの名前（文字列）を sceneToLoad 変数にコピーする
            sceneToLoad = new List<string>();
            foreach (var scene in sceneAsset)
            {
                sceneToLoad.Add(scene.name);
            }
        }
        else
        {
            // SceneAssetが未設定なら文字列も空にする
            sceneToLoad = new List<string>();
        }
    }
#endif



}
