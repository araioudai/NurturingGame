using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    #region protected�ϐ�
    [SerializeField] protected int level;       //���x��
    [SerializeField] protected int hp;          //�̗�
    [SerializeField] protected int mp;          //���͗�
    [SerializeField] protected int atk;         //�U����
    [SerializeField] protected int intell;      //���@�U����
    [SerializeField] protected int def;         //�h���
    [SerializeField] protected int agi;         //�f����
    [SerializeField] protected int rack;        //�K�^�l

    #endregion

    #region Unity�C�x���g�֐�
    protected virtual void Awake()
    {
        //GameManager.Awake() �̌�ɌĂ΂�邩��m���ɓo�^
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterEnemy(this);
        }
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Update()
    {

    }
    #endregion

    #region Start�Ăяo���֐�
    
    #region ������
    protected virtual void Init()
    {
        
    }

    public void RandomLevel(int lower, int upper)
    {
        level = Random.Range(lower, upper + 1);
    }

    public void ApplyLevelData()
    {
        hp = level * 10;
        mp = level * 2;
        atk = level * 4;
        intell = level * 3;
        def = level;
        agi = level * 2;
        rack = level;
    }

    public void PrintEnemyData()
    {
       Debug.Log("level:" + level);
       Debug.Log("hp:" + hp);
       Debug.Log("atk:" + atk);
       Debug.Log("int:" + intell);
       Debug.Log("def:" + def);
       Debug.Log("agi:" + agi); 
       Debug.Log("rack:" + rack);
    }

    #endregion

    #endregion


    #region Update�Ăяo���֐�
    protected virtual void Move()
    {

    } 

    protected virtual void Attack()
    {

    }
    #endregion
}
