using UnityEngine;
using static Udon.Commons;

public class HangOvers : MonoBehaviour
{
    #region 持ち越しデータ
    [SerializeField] public States states;
    #endregion


    public void SetData(States data)
    {
        states = data;
    }

    public States GetData()
    {
        return states;
    }

}
