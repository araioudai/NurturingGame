using UnityEngine;

public class AttackPower : MonoBehaviour
{
    [SerializeField] private int m_Power;

    public int GetPower()
    {
        return m_Power;
    }

    public void SetPower(int power)
    {
        m_Power = power;
    }


}
