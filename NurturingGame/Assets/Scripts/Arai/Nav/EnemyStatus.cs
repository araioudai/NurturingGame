using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    #region private�ϐ�
    [Header("�ő�HP")]
    [SerializeField] protected int maxHp = 100;
    [Header("HP(�o�[�p)")]
    [SerializeField] protected int hp;
    [Header("�U����")]
    [SerializeField] protected int attackPower = 1;
    [Header("HPUI�L�����o�X")]
    [SerializeField] protected GameObject HPUI;
    [Header("HPBar�i�X���C�_�[�j")]
    [SerializeField] protected GameObject hpSliderUI;
    protected Slider hpSlider;

    #endregion

    #region Unity�C�x���g�֐�
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

    #region hp�����������̃o�[���f����(�U�������炤)
    public void SetHp(int attack)
    {
        hp -= attack;

        //HP�\���pUI�̃A�b�v�f�[�g
        UpdateHPValue();

        //���S���Ă���
        if (hp <= 0)
        {
            //HP�\���pUI���\���ɂ���
            HideStatusUI();
        }
    }
    #endregion

    #region Get�֐�
    protected int GetHp()
    {
        return hp;
    }

    protected int GetMaxHp()
    {
        return maxHp;
    }

    #endregion

    #region ���S��
    protected void IsDead()
    {
        if(hp <= 0)
        {
            //�Ǐ]���Ă���v���C���[�ɓo�^����
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

    #region ���񂾂�UI���\������
    protected void HideStatusUI()
    {
        HPUI.SetActive(false);
    }
    #endregion

    #region HP�\���pUI�̃A�b�v�f�[�g����
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
