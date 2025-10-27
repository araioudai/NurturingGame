using UnityEngine;
using static Common;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] public PlayerData playerData;


    public virtual void Init(PlayerData data)
    {
        playerData = data;
    }



    virtual public void Start()
    {

    }

    virtual public void Update()
    {

    }

    /// <summary>
    /// モンスター討伐処理
    /// </summary>
    void DefeatMonsters(int getExp)
    {
        playerData.exp += getExp;
        Debug.Log("Got Exp: " + getExp + ", Current Exp: " + playerData.exp, this);

        // // レベルアップ判定
        // while (playerData.exp >= playerData.nextLevelExp)
        // {
        //     playerData.exp -= playerData.nextLevelExp;
        //     LevelUp();
        // }

    }

    bool CheckLevelUp()
    {
        int nextLevelExp = playerData.level * 100; // 次のレベルに必要な経験値の計算例


        return playerData.exp >= nextLevelExp;
    }













    /// <summary>
    /// レベルアップ処理
    /// </summary>
    void LevelUp()
    {
        playerData.level += 1;
        // ステータスアップ処理
        Debug.Log("Level Up! New Level: " + playerData.level, this);
        playerData.firstStatus.HP   += levelUpStatusTable[0];
        playerData.firstStatus.MP   += levelUpStatusTable[1];
        playerData.statusPoints     += levelUpStatusTable[2];
    }












}
