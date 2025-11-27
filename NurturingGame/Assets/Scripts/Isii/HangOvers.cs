using UnityEngine;
using static Udon.Commons;

public class HangOvers : MonoBehaviour
{
    #region 持ち越しデータ
    [SerializeField] public SaveData playerData;
    #endregion


    public void SetData(SaveData data)
    {
        playerData = data;
    }

    public SaveData GetData()
    {
        return playerData;
    }

}
