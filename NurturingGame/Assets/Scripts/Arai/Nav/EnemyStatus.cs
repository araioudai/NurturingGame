using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    #region 定数
    public enum Stage
    {
        STAGE_1,
        STAGE_2,
        STAGE_3,
        STAGE_4
    }
    #endregion

    #region private変数
    [Header("ステージごとの敵データ")]
    [SerializeField, EnumIndex(typeof(Stage))] 
    private EnemyState[] status;

    [Header("HP（バー用）")]
    [SerializeField] protected int hp;

    [Header("HPUIキャンバス")]
    [SerializeField] protected GameObject HPUI;

    [Header("HPバー（スライダー）")]
    [SerializeField] protected GameObject hpSliderUI;

    protected Slider hpSlider;
    private int maxHp;
    private int attackPower;
    #endregion

    #region Unityイベント関数
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //hp関連
        maxHp = status[StageIndex.Instance.GetIndex()].max;
        hp = maxHp;
        hpSlider = hpSliderUI.GetComponent<Slider>();
        hpSlider.value = 1f;

        //攻撃関連
        attackPower = status[StageIndex.Instance.GetIndex()].power;
    }

    protected virtual void Update()
    {
    }
    #endregion

    #region HPを減少させる処理（攻撃を受ける）
    /// <summary>
    /// HPを減少させる処理
    /// </summary>
    /// <param name="attack">HPの減らす量</param>
    public void SetHp(int attack)
    {
        hp -= attack;

        // HP表示用UIの更新
        UpdateHPValue();

        // 死亡している場合
        if (hp <= 0)
        {
            // HP表示用UIを非表示にする
            HideStatusUI();
        }
    }
    #endregion

    #region Get関数
    /// <summary>
    /// 現在のhp取得
    /// </summary>
    /// <returns>現在のhp</returns>
    protected int GetHp()
    {
        return hp;
    }

    /// <summary>
    /// 最大hp取得
    /// </summary>
    /// <returns>最大hp</returns>
    protected int GetMaxHp()
    {
        return maxHp;
    }
    #endregion

    #region 死亡判定
    /// <summary>
    /// 死亡判定 オブジェクト削除
    /// </summary>
    protected void IsDead()
    {
        if (hp <= 0)
        {
            // 追従しているプレイヤーから登録解除
            /*
            var player = FindObjectOfType<NavMeshAgentControllerPlayer>();
            if (player != null)
            {
                player.RemoveAttackTarget(this.transform);
                player.RemoveTargetPoint(this.transform);
            }
            */

            Destroy(gameObject);
        }
    }
    #endregion

    #region HPが0になったらUIを非表示にする
    /// <summary>
    /// hpバー非表示処理
    /// </summary>
    protected void HideStatusUI()
    {
        HPUI.SetActive(false);
    }
    #endregion

    #region HP表示用UIの更新処理
    /// <summary>
    /// hpバー更新処理
    /// </summary>
    protected void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxHp();
    }
    #endregion

    #region ��U�����ւƂȂ�
    public void SetHPInit(int hpValue)
    {
        maxHp = hpValue;
        hp = maxHp;
    }

    public void SetAttackPower(int attackValue)
    {
        attackPower = attackValue;
    }
    #endregion



}
