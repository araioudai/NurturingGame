using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    #region private変数
    [Header("最大HP")]
    [SerializeField] private int maxHp = 100;
    [Header("HP(バー用)")]
    [SerializeField] private int hp;
    [Header("HPUIキャンバス")]
    [SerializeField] private GameObject HPUI;
    [Header("HPBar（スライダー）")]
    [SerializeField] private GameObject hpSliderUI;
    private Slider hpSlider;

    #endregion

    #region Unityイベント関数
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        hp = maxHp;
        hpSlider = hpSliderUI.GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    private void Update()
    {
        IsDead();
        //Debug.Log(hpSlider.value);
    }
    #endregion

    #region hpが減った時のバー反映処理(攻撃をくらう)
    private void SetHp(int attack)
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

    #region 死亡時
    private void IsDead()
    {
        if (hp <= 0)
        {
            //追従しているプレイヤーに登録解除
            /*            var player = FindObjectOfType<NavMeshAgentControllerPlayer>();
                        if (player != null)
                        {
                            player.RemoveAttackTarget(this.transform);
                            player.RemoveTargetPoint(this.transform);
                        }*/
            SceneManager.LoadScene("resultTest011");

            Destroy(gameObject);
        }
    }
    #endregion

    #region 死んだらUIを非表示処理
    private void HideStatusUI()
    {
        HPUI.SetActive(false);
    }
    #endregion

    #region HP表示用UIのアップデート処理
    private void UpdateHPValue()
    {
        hpSlider.value = (float)hp / (float)maxHp;
    }
    #endregion

    #region 当たり判定

    #region すり抜け時
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackArea"))
        {
            //衝突した相手のGameObjectからAttackPowerコンポーネントを取得
            AttackPower attack = other.gameObject.GetComponent<AttackPower>();

            if (attack != null)
            {
                //attackが取得できた場合の処理
                Debug.Log("AttackPowerを取得しました: " + attack.GetPower());
                SetHp(attack.GetPower());
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
