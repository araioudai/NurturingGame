using UnityEngine;

public class AttackRangeTrigger : MonoBehaviour
{
    #region private変数
    [SerializeField] private NavMeshAgentController controller;
    #endregion

    #region 当たり判定

    #region すり抜けた時
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + "1");

        if (other.CompareTag("Player"))
        {
            Debug.Log(gameObject.name + "2");
            controller.AddAttackTarget(other.transform); //アタック状態へ
        }
    }
    #endregion

    #region 離れた時
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.SetState(0);
            controller.RemoveAttackTarget(other.transform); //移動状態へ
        }
    }
    #endregion

    #endregion
}
