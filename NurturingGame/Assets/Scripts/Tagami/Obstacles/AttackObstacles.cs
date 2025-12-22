using UnityEngine;

public class AttackObstacles : MonoBehaviour
{
    #region private変数
    [SerializeField] private Obstacles controller;
    #endregion

    #region 当たり判定

    #region すり抜けた時
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.AddAttackTarget(other.transform); //アタック状態へ
        }
    }
    #endregion

    #region 離れた時
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.RemoveAttackTarget(other.transform); //移動状態へ
        }
    }
    #endregion

    #endregion
}
