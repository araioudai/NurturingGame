using UnityEngine;

public class KariBackScene : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] private string titleSceneName = "TitleScene";
    bool isBackToTitle = false;

    void Update()
    {
        // ESCキーでタイトルシーンへ遷移
        if(Input.GetMouseButtonDown(2) && !isBackToTitle)
        {
            isBackToTitle = true;
            BackToTitleScene();
        }
    }

    private void BackToTitleScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(titleSceneName);
    }
}
