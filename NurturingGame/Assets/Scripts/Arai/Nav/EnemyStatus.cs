using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    #region private変数
    [Header("最大HP")]
    [SerializeField] protected int maxHp = 100;
    [Header("HP(バー用)")]
    [SerializeField] protected int hp;
    [Header("攻撃力")]
    [SerializeField] protected int attackPower = 1;
    [Header("HPUIキャンバス")]
    [SerializeField] protected GameObject HPUI;
    [Header("HPBar（スライダー）")]
    [SerializeField] protected GameObject hpSliderUI;
    protected Slider hpSlider;

    #endregion

    #region Unityイベント関数
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        hp = maxHp;
        hpSlider = hpSliderUI.GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    protected virtual void Update()
    {

    }
    #endregion

    #region hpが減った時のバー反映処理(攻撃をくらう)
    public void SetHp(int attack)
    {
        hp -= attack;

        //HP表示用UIのアップデート
        UpdateHPValue();

        //死亡してたら
        if (hp <= 0)
        {
            //HP表示用UIを非表示にする
            HideStatusUI();
        }
    }
    #endregion

    #region Get関数
    protected int GetHp()
    {
        return hp;
    }

    protected int GetMaxHp()
    {
        return maxHp;
    }

    #endregion

    #region 死亡時
    protected void IsDead()
    {
        if(hp <= 0)
        {
            //追従しているプレイヤーに登録解除
/*            var player = FindObjectOfType<NavMeshAgentControllerPlayer>();
            if (player != null)
            {
                player.RemoveAttackTarget(this.transform);
                player.RemoveTargetPoint(this.transform);
            }*/

            Destroy(gameObject);
        }
    }
    #endregion

    #region 死んだらUIを非表示処理
    protected void HideStatusUI()
    {
        HPUI.SetActive(false);
    }
    #endregion

    #region HP表示用UIのアップデート処理
    protected void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxHp();
    }
    #endregion
}
