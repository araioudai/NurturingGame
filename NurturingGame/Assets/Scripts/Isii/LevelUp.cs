using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using static Udon.Commons;

[RequireComponent(typeof(TextManager))]
[RequireComponent(typeof(StatesManager))]
public class LevelUp : MonoBehaviour
{
    [SerializeField] SaveData playerData;



    [SerializeField] Button tcBuildingLevelUpButton;
    [SerializeField] List<Button> tcJobLevelUpButton;
    [SerializeField] Button pBuildingLevelUpButton;
    [SerializeField] List<Button> pSkillLevelUpButton;










    [System.Obsolete]
    void Start()
    {
        SaveData playerData = FindObjectOfType<GameDataManager>().playerData;

        tcBuildingLevelUpButton.onClick.AddListener(() => TCBuildingLevelUp());
        for (int i = 0; i < tcJobLevelUpButton.Count; i++)
        {
            int index = i; // ローカル変数にキャプチャ
            tcJobLevelUpButton[i].onClick.AddListener(() => TCJobLevelUp((JobType)index));
        }
        pBuildingLevelUpButton.onClick.AddListener(() => PBuildingLevelUp());
        for (int i = 0; i < pSkillLevelUpButton.Count; i++)
        {
            int index = i; // ローカル変数にキャプチャ
            pSkillLevelUpButton[i].onClick.AddListener(() => PSkillLevelUp((SkillType)index));
        }
    }


    #region モブ

    [ContextMenu("兵舎/建物レベルアップ")]
    [System.Obsolete]
    /// <summary>
    /// 兵舎/建物レベルアップ
    /// </summary>
    public bool TCBuildingLevelUp()
    {
        playerData = FindObjectOfType<GameDataManager>().playerData;
        // 兵舎/建物レベルアップ
        if (playerData.trainingCentre.buildingLevel < (int)JobType.Count)
        {
            int requiredResources = buildingLevelUpResourcesTable[playerData.trainingCentre.buildingLevel];
            if (playerData.resources >= requiredResources)
            {
                playerData.resources -= requiredResources;
                playerData.trainingCentre.buildingLevel++;
                Debug.Log("兵舎/建物レベルアップ成功！ 新しいレベル: " + playerData.trainingCentre.buildingLevel, this);

                GetComponent<StatesManager>().PlayerStatesSet(StatesType.Player, playerData.trainingCentre.buildingLevel);
                GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
                GetComponent<TextManager>().TrainingCenterLevelTextUpdate(playerData.trainingCentre.buildingLevel);

                return true;
            }
            else
            {
                Debug.LogWarning("資源が不足しています。必要な資源: " + requiredResources, this);
                return false;
            }
        }
        else
        {
            Debug.LogWarning("兵舎/建物は既に最大レベルです。", this);
            return false;
        }
    }

    // [ContextMenu("兵舎/職別レベルアップ")]
    [System.Obsolete]
    /// <summary>
    /// 兵舎/職別レベルアップ
    /// </summary>
    public bool TCJobLevelUp(JobType jobType)
    {
        playerData = FindObjectOfType<GameDataManager>().playerData;
        // 兵舎/職別レベルアップ

        // 現在のレベルを取得
        int currentLevel;
        switch (jobType)
        {
            case JobType.Knight:
                currentLevel = playerData.trainingCentre.tcLevelUp.KnightLevel;
                break;
            case JobType.Archer:
                currentLevel = playerData.trainingCentre.tcLevelUp.ArcherLevel;
                break;
            case JobType.Paladin:
                currentLevel = playerData.trainingCentre.tcLevelUp.PaladinLevel;
                break;
            case JobType.Mage:
                currentLevel = playerData.trainingCentre.tcLevelUp.MageLevel;
                break;
            default:
                Debug.LogError("未知の職業タイプ: " + jobType, this);
                return false;
        }

        // レベルアップ処理
        if (currentLevel < mobMaxLevel)
        {
            bool success = false;
            int requiredResources = mobLevelUpResourcesTable[currentLevel];
            if (playerData.resources >= requiredResources)
            {
                playerData.resources -= requiredResources;
                switch (jobType)
                {
                    case JobType.Knight:
                        playerData.trainingCentre.tcLevelUp.KnightLevel++;
                        success = true;
                        break;
                    case JobType.Archer:
                        if(playerData.trainingCentre.buildingLevel >= 2)
                        {
                            playerData.trainingCentre.tcLevelUp.ArcherLevel++;
                            success = true;
                        }
                        break;
                    case JobType.Paladin:
                        if(playerData.trainingCentre.buildingLevel >= 3)
                        {
                            playerData.trainingCentre.tcLevelUp.PaladinLevel++;
                            success = true;
                        }
                        break;
                    case JobType.Mage:
                        if(playerData.trainingCentre.buildingLevel >= 4)
                        {
                            playerData.trainingCentre.tcLevelUp.MageLevel++;
                            success = true;
                        }
                        break;
                }
                if (!success)
                {
                    Debug.LogWarning(jobType.ToString() + "のレベルアップ条件を満たしていません。", this);

                    GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
                    GetComponent<TextManager>().JobLevelTextUpdate(1, playerData);

                    return false;
                }
                Debug.Log(jobType.ToString() + "のレベルアップ成功！ 新しいレベル: " + (currentLevel + 1), this);

                GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
                GetComponent<StatesManager>().MobStatesSet(StatesType.Mob, jobType, playerData.trainingCentre.tcLevelUp.GetJobLevelText(jobType));
                GetComponent<TextManager>().JobLevelTextUpdate(1, playerData);

                return true;
            }
            else
            {
                Debug.LogWarning("資源が不足しています。必要な資源: " + requiredResources, this);

                GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
                GetComponent<TextManager>().JobLevelTextUpdate(1, playerData);

                return false;
            }
        }
        else
        {
            Debug.LogWarning(jobType.ToString() + "は既に最大レベルです。", this);

            GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
            GetComponent<TextManager>().JobLevelTextUpdate(1, playerData);

            return false;
        }
    }
    #endregion

    #region プレイヤー
    [ContextMenu("プレイヤー/建物レベルアップ")]
    [System.Obsolete]
    public bool PBuildingLevelUp()
    {
        playerData = FindObjectOfType<GameDataManager>().playerData;
        // プレイヤーレベルアップ
        if (playerData.playerTC.buildingLevel < buildingMaxLevel)
        {
            int requiredResources = buildingLevelUpResourcesTable[playerData.playerTC.buildingLevel];
            if (playerData.resources >= requiredResources)
            {
                playerData.resources -= requiredResources;
                playerData.playerTC.buildingLevel++;
                Debug.Log("プレイヤーレベルアップ成功！ 新しいレベル: " + playerData.playerTC.buildingLevel, this);

                GetComponent<StatesManager>().PlayerStatesSet(StatesType.Player, playerData.playerTC.buildingLevel);
                GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
                GetComponent<TextManager>().PlayerLevelTextUpdate(playerData.playerTC.buildingLevel);

                return true;
            }
            else
            {
                Debug.LogWarning("資源が不足しています。必要な資源: " + requiredResources, this);
                return false;
            }
        }
        else
        {
            Debug.LogWarning("プレイヤーは既に最大レベルです。", this);
            return false;
        }
    }

    // [ContextMenu("プレイヤー/レベルアップ")]
    [System.Obsolete]
    public bool PSkillLevelUp(SkillType skillType)
    {
        playerData = FindObjectOfType<GameDataManager>().playerData;
        // プレイヤー/スキル別レベルアップ

        // 現在のレベルを取得
        int currentLevel;
        switch (skillType)
        {
            case SkillType.Hummer:
                currentLevel = playerData.playerTC.ptcLevelUp.HummerLevel;
                break;
            case SkillType.Toxic:
                currentLevel = playerData.playerTC.ptcLevelUp.ToxicLevel;
                break;
            case SkillType.Stan:
                currentLevel = playerData.playerTC.ptcLevelUp.StanLevel;
                break;
            case SkillType.FireStorm:
                currentLevel = playerData.playerTC.ptcLevelUp.FireStormLevel;
                break;
            case SkillType.SwordRain:
                currentLevel = playerData.playerTC.ptcLevelUp.SwordRainLevel;
                break;
            case SkillType.Heal:
                currentLevel = playerData.playerTC.ptcLevelUp.HealLevel;
                break;
            default:
                Debug.LogError("未知のスキルタイプ: " + skillType, this);
                return false;
        }

        // レベルアップ処理
        if (currentLevel < mobMaxLevel)
        {
            bool success = false;
            int requiredResources = mobLevelUpResourcesTable[currentLevel];
            if (playerData.resources >= requiredResources)
            {
                playerData.resources -= requiredResources;
                switch (skillType)
                {
                    case SkillType.Hummer:
                        playerData.playerTC.ptcLevelUp.HummerLevel++;
                        break;
                    case SkillType.Toxic:
                        if (playerData.playerTC.buildingLevel >= 2)
                        {
                            playerData.playerTC.ptcLevelUp.ToxicLevel++;
                            success = true;
                        }
                        break;
                    case SkillType.Stan:
                        if (playerData.playerTC.buildingLevel >= 3)
                        {
                            playerData.playerTC.ptcLevelUp.StanLevel++;
                            success = true;
                        }
                        break;
                    case SkillType.FireStorm:
                        if (playerData.playerTC.buildingLevel >= 4)
                        {
                            playerData.playerTC.ptcLevelUp.FireStormLevel++;
                            success = true;
                        }
                        break;
                    case SkillType.SwordRain:
                        if (playerData.playerTC.buildingLevel >= 5)
                        {
                            playerData.playerTC.ptcLevelUp.SwordRainLevel++;
                            success = true;
                        }
                        break;
                    case SkillType.Heal:
                        if (playerData.playerTC.buildingLevel >= 6)
                        {
                            playerData.playerTC.ptcLevelUp.HealLevel++;
                            success = true;
                        }
                        break;
                }
                if (!success)
                {
                    Debug.LogWarning(skillType.ToString() + "のレベルアップ条件を満たしていません。", this);

                    GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
                    GetComponent<TextManager>().SkillLevelTextUpdate(0, playerData);

                    return false;
                }
                Debug.Log(skillType.ToString() + "のレベルアップ成功！ 新しいレベル: " + (currentLevel + 1), this);

                GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
                GetComponent<TextManager>().SkillLevelTextUpdate(0, playerData);

                return true;
            }
            else
            {
                Debug.LogWarning("資源が不足しています。必要な資源: " + requiredResources, this);

                GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
                GetComponent<TextManager>().SkillLevelTextUpdate(0, playerData);

                return false;
            }
        }
        else
        {
            Debug.LogWarning(skillType.ToString() + "は既に最大レベルです。", this);

            GetComponent<TextManager>().ResourcesTextUpdate(playerData.resources);
            GetComponent<TextManager>().SkillLevelTextUpdate(0, playerData);

            return false;
        }
    }
    #endregion


}
