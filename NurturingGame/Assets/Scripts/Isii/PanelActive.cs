using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextManager))]
[RequireComponent(typeof(GameDataManager))]
public class PanelSettings : MonoBehaviour
{
    [Header("モブ兵士パネル")]
    [SerializeField] Button jobPanelButton;
    [SerializeField] GameObject jobPanel;

    [Header("プレイヤースキルパネル")]
    [SerializeField] Button skillPanelButton;
    [SerializeField] GameObject skillPanel;

    [System.Obsolete]
    void Start()
    {
        jobPanelButton.onClick.AddListener(() => JobPanelSet());
        skillPanelButton.onClick.AddListener(() => SkillPanelSet());
        skillPanel.SetActive(false);
        jobPanel.SetActive(false);
    }

    [System.Obsolete]
    void JobPanelSet()
    {
        // OnOff切り替え
        jobPanel.SetActive(!jobPanel.activeSelf);

        if (jobPanel.activeSelf)
        {
            GetComponent<TextManager>().ResourcesTextUpdate(GetComponent<GameDataManager>().playerData);
            GetComponent<TextManager>().SkillLevelTextUpdate(0, GetComponent<GameDataManager>().playerData);
        }

        skillPanel.SetActive(false);
    }

    [System.Obsolete]
    void SkillPanelSet()
    {
        // OnOff切り替え
        skillPanel.SetActive(!skillPanel.activeSelf);

        if (skillPanel.activeSelf)
        {
            GetComponent<TextManager>().ResourcesTextUpdate(GetComponent<GameDataManager>().playerData);
            GetComponent<TextManager>().JobLevelTextUpdate(1, GetComponent<GameDataManager>().playerData);
        }

        jobPanel.SetActive(false);
    }





}
