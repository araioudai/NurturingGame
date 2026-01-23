using UnityEngine;

/// <summary>
/// エネミー用のデータ
/// </summary>
[CreateAssetMenu(
    fileName = "EnemyState",
    menuName = "Enemy/State"
)]

public class EnemyState : ScriptableObject
{
    [Header("最大HP")]
    [SerializeField] private int maxHp;
    [Header("攻撃力")]
    [SerializeField] private int attackPower;

    /// <summary>
    /// 最大hpを返す
    /// </summary>
    public int max 
    { 
        get { return maxHp; } 
    }

    /// <summary>
    /// 攻撃力を返す
    /// </summary>
    public int power
    {
        get { return attackPower; }
    }

}
