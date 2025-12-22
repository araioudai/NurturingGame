using UnityEngine;

public class PlayerHitJudgment : MonoBehaviour
{
    [SerializeField] private Player controller;

    #region 当たり判定

    #region すり抜け時
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackArea"))
        {
            //衝突した相手のGameObjectからAttackPowerコンポーネントを取得
            AttackPower attack = other.gameObject.GetComponent<AttackPower>();

            if (attack != null)
            {
                //attackが取得できた場合の処理
                Debug.Log("AttackPowerを取得しました: " + attack.GetPower());
                controller.SetHp(attack.GetPower());
            }
            else
            {
                Debug.Log("AttackPowerコンポーネントがありません");
            }
        }
    }
    #endregion

    #endregion
}
