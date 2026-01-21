using UnityEngine;

public class AttackRangeTriggerPlayer : MonoBehaviour
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
            controller.AddAttackTarget(other.transform); //アタック状態へ
        }
    }
    #endregion

    #region 離れた時
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacles"))
        {
            //controller.SetState(0);
            controller.SetPos(controller.GetFirstPos());
            controller.RemoveAttackTarget(other.transform); //移動状態へ
        }
    }
    #endregion

    #endregion
}
