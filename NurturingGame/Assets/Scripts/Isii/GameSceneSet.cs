using UnityEngine;

public class GameSceneSet : MonoBehaviour
{







    public void FightScene()
    {
        var hangOver = FindFirstObjectByType<HangOvers>();
        var statusManager = FindFirstObjectByType<StatusManager>();
        hangOver.SetMobData(statusManager.mobStatus);
        hangOver.SetPlayerData(statusManager.playerStatus);

        // シーン移動
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
