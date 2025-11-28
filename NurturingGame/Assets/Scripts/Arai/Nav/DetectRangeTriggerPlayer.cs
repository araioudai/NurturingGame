using UnityEngine;

public class DetectRangeTriggerPlayer : MonoBehaviour
{
    #region private変数
    [SerializeField] private NavMeshAgentControllerPlayer controller;
    #endregion

    #region 当たり判定

    #region すり抜けた時
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacles"))
        {
            controller.AddTargetPoint(other.transform); //リストに範囲内ターゲットを追加
        }
    }
    #endregion

    #region 離れた時
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacles"))
        {
            controller.RemoveTargetPoint(other.transform); //リストの範囲外ターゲットを削除
        }
    }
    #endregion

    #endregion
}
