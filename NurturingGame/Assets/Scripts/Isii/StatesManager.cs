using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static Udon.Commons;

public class StatesManager : MonoBehaviour
{
    [SerializeField] public States playerStates;
    [SerializeField] public List<States> mobStates = new((int)JobType.Count);

    public void PlayerStatesSet(StatesType type, int level)
    {
        playerStates.SetStates(type, level);
    }

    public void MobStatesSet(StatesType type, JobType jobType, int level)
    {
        mobStates[(int)jobType].SetStates(type, jobType, level);
    }

    public void MobStatesInit(SaveData saveData)
    {
        mobStates = new List<States>((int)JobType.Count);
        for (int i = 0; i < (int)JobType.Count; i++)
        {
            States mobState = new();
            mobState.SetStates(StatesType.Mob, (JobType)i, saveData.trainingCentre.tcLevelUp.GetJobLevelText((JobType)i));
            mobStates.Add(mobState);
        }
    }

    public void PlayerStatesInit(SaveData saveData)
    {
        playerStates = new States();
        playerStates.SetStates(StatesType.Player, saveData.playerTC.buildingLevel);
    }


















}
