using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    #region protected•Ï”
    [SerializeField] protected int level;       //ƒŒƒxƒ‹
    [SerializeField] protected int hp;          //‘Ì—Í
    [SerializeField] protected int mp;          //–‚—Í—Ê
    [SerializeField] protected int atk;         //UŒ‚—Í
    [SerializeField] protected int intell;      //–‚–@UŒ‚—Í
    [SerializeField] protected int def;         //–hŒä—Í
    [SerializeField] protected int agi;         //‘f‘‚³
    [SerializeField] protected int rack;        //K‰^’l

    #endregion

    #region UnityƒCƒxƒ“ƒgŠÖ”
    protected virtual void Awake()
    {
        //GameManager.Awake() ‚ÌŒã‚ÉŒÄ‚Î‚ê‚é‚©‚çŠmÀ‚É“o˜^
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

    #region StartŒÄ‚Ño‚µŠÖ”
    
    #region ‰Šú‰»
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


    #region UpdateŒÄ‚Ño‚µŠÖ”
    protected virtual void Move()
    {

    } 

    protected virtual void Attack()
    {

    }
    #endregion
}
