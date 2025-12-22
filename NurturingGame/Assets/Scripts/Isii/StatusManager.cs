using System.Collections.Generic;
using UnityEngine;
using static Udon.Commons;

public class StatusManager : MonoBehaviour
{
    [SerializeField] public Status playerStatus;
    [SerializeField] public List<Status> mobStatus = new((int)JobType.Count);

    public void PlayerStatesSet(StatusType type, int level)
    {
        playerStatus.SetStates(type, level);
    }

    public void MobStatesSet(StatusType type, JobType jobType, int level)
    {
        mobStatus[(int)jobType].SetStates(type, jobType, level);
    }

    public void MobStatesInit(SaveData saveData)
    {
        mobStatus = new List<Status>((int)JobType.Count);
        for (int i = 0; i < (int)JobType.Count; i++)
        {
            Status mobState = new();
            mobState.SetStates(StatusType.Mob, (JobType)i, saveData.trainingCentre.tcLevelUp.GetJobLevelText((JobType)i));
            mobStatus.Add(mobState);
        }
    }

    public void PlayerStatesInit(SaveData saveData)
    {
        playerStatus = new Status();
        playerStatus.SetStates(StatusType.Player, saveData.playerTC.buildingLevel);
    }
}
