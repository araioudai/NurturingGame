using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    [Header("資源追加設定")]
    [SerializeField] private int resourcesToAdd = 10000000;
    [SerializeField] private Button resourcesAddButton;
    [SerializeField] private GameObject managerObject;

    private void Start()
    {
        if (resourcesAddButton != null)
        {
            resourcesAddButton.onClick.AddListener(() => AddResources());
        }
    }

    public void AddResources()
    {
        var gameDataManager = managerObject.GetComponent<GameDataManager>();
        gameDataManager.playerData.resources += resourcesToAdd;
        Debug.Log("資源を " + resourcesToAdd + " 増加させました。現在の資源: " + gameDataManager.playerData.resources, this);

        // テキストマネージャーを使って資源テキストを更新
        if (managerObject.TryGetComponent<TextManager>(out var textManager))
        {
            textManager.ResourcesTextUpdate(gameDataManager.playerData.resources);
        }
    }
}
