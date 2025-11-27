using UnityEngine;
using UnityEngine.UI;
using static Udon.Commons;

public class TextManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private Text resourcesText;

    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject jobPanel;

    /// <summary>
    /// 資源テキスト更新
    /// </summary>
    public void ResourcesTextUpdate(SaveData data)
    {
        resourcesText.text = data.resources.ToString();
    }


    public void ResourcesTextUpdate(int resources)
    {
        resourcesText.text = resources.ToString();
    }

    /// <summary>
    /// 初期テキスト更新
    /// </summary>
    public void InitTextUpdate(SaveData data)
    {
        ResourcesTextUpdate(data.resources);

        skillPanel.SetActive(true);
        jobPanel.SetActive(true);

        SkillLevelTextUpdate(0, data);
        JobLevelTextUpdate(1, data);

        skillPanel.SetActive(false);
        jobPanel.SetActive(false);
    }





    /// <summary>
    /// プレイヤースキルレベルテキスト更新
    /// </summary>
    /// <param name="panelType">0: skillPanel, 1: jobPanel</param>
    public void SkillLevelTextUpdate(int panelType, SaveData data)
    {
        if (panelType != 0 && panelType != 1) return;

        GameObject Panel = panelType == 0 ? skillPanel : jobPanel;
        for (int i = 0; i < (int)SkillType.Count; i++)
        {                                       // PList      ImgScroll   Skill1      LvUpBtn     LvText
            Text skillLevelText = Panel.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>();
            skillLevelText.text = "Level " + data.magiciansTower.mtLevelUp.GetSkillLevelText((SkillType)i).ToString();
        }
    }

    /// <summary>
    /// モブジョブレベルテキスト更新
    /// </summary>
    /// <param name="panelType">0: skillPanel, 1: jobPanel</param>
    public void JobLevelTextUpdate(int panelType, SaveData data)
    {
        if (panelType != 0 && panelType != 1) return;

        GameObject Panel = panelType == 0 ? skillPanel : jobPanel;
        for (int i = 0; i < (int)JobType.Count; i++)
        {                                       // PList    ImgScroll   Job1        LvUpBtn     LvText
            Text jobLevelText = Panel.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>();
            jobLevelText.text = "Level " + data.trainingCentre.tcLevelUp.GetJobLevelText((JobType)i).ToString();
        }
    }





}
