using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DetectRangeTriggerPlayer : MonoBehaviour
{
    #region private変数
    [SerializeField] private NavMeshAgentController controller;
    #endregion

    #region 当たり判定

    #region すり抜けた時
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + "1");
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacles"))
        {
            Debug.Log(gameObject.name + "2");
            controller.SetPos(other.transform);
            controller.AddTargetPoint(other.transform); //リストに範囲内ターゲットを追加
        }
    }
    #endregion

    #region 離れた時
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacles"))
        {
            controller.RemoveTargetPoint(controller.GetTargetPos()); //リストの範囲外ターゲットを削除
        }
    }
    #endregion

    #endregion
}
