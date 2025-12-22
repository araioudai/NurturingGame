using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Udon.Commons;

public class HangOvers : MonoBehaviour
{
    #region 持ち越しデータ
    [HideInInspector ] public Status playerState = new();
    [HideInInspector] public List<Status> mobStatus = new();
    #endregion

    void Awake()
    {
        if(TryGetComponent<EnemySpawn>(out var enemySpawn))
        {
            enemySpawn.charaKnight.GetComponent<NavMeshAgentControllerPlayer>().maxHp = mobStatus[(int)JobType.Knight].hp;
            enemySpawn.charaKnight.GetComponent<NavMeshAgentControllerPlayer>().attackPower = mobStatus[(int)JobType.Knight].attack;
            enemySpawn.charaKnight.GetComponent<NavMeshAgentControllerPlayer>().attackInterval = mobStatus[(int)JobType.Archer].attackInterval;

            enemySpawn.charaArcher.GetComponent<NavMeshAgentControllerPlayer>().maxHp = mobStatus[(int)JobType.Archer].hp;
            enemySpawn.charaArcher.GetComponent<NavMeshAgentControllerPlayer>().attackPower = mobStatus[(int)JobType.Archer].attack;
            enemySpawn.charaArcher.GetComponent<NavMeshAgentControllerPlayer>().attackInterval = mobStatus[(int)JobType.Archer].attackInterval;



        }





    }




    public void SetMobData(List<Status> data)
    {
        mobStatus = data;
    }

    public List<Status> GetMobData()
    {
        return mobStatus;
    }

    public Status GetPlayerData()
    {
        return playerState;
    }

    public void SetPlayerData(Status data)
    {
        playerState = data;
    }

    [ContextMenu("ViewData")]
    void ViewData()
    {
        Debug.Log("HangOverのデータ表示", this);
        for (int i = 0; i < mobStatus.Count; i++)
        {
            Debug.Log("職業タイプ: " + (JobType)i + " 体力: " + mobStatus[i].hp + " 攻撃力: " + mobStatus[i].attack + " 攻撃インターバル: " + mobStatus[i].attackInterval, this);
        }

        Debug.Log("プレイヤーステータス 体力: " + playerState.hp + " 攻撃力: " + playerState.attack + " 攻撃インターバル: " + playerState.attackInterval, this);
    }
}
