using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region シングルトン（他スクリプトからInstanceでアクセスできるようにする）
    public static GameManager Instance { get; private set; }
    #endregion

    #region private変数
    private List<EnemyBase> enemies = new List<EnemyBase>();

    /*    [Header("lower:ランダムの下限 upper:ランダムの上限")]
        [SerializeField] private int lowerLevel = 1;
        [SerializeField] private int upperLevel = 50;*/

    [Header("最大SP(ポイント)")]
    [SerializeField] private float maxSp = 12;
    [Header("SPBar（スライダー）")]
    [SerializeField] private GameObject spSliderUI;
    private Slider spSlider;

    //内部用SP（論理値：一定時間ごとに+1）
    private float spLogical = 6f;    //生成に必要なポイントv

    //表示用SP（スライダーはこれを徐々に追いかける）
    private float sp = 6f;

    [Header("論理SPが増える間隔(秒)")]
    [SerializeField] private float increaseInterval = 0.1f;

    private float timer;

    private Vector3 castlePos;

    #endregion

    #region ゲット関数
    public float GetPoint()
    {
        return (int)spLogical;
    }

    public Vector3 GetCastlePos()
    {
        return castlePos;
    }
    #endregion

    #region セット関数
    public void SetMinusPoint(int point)
    {
        sp -= point;
        spLogical -= point;
    }

    public void SetCastlePos(Vector3 point)
    {
        castlePos = point;
    }
    #endregion

    #region Unity呼び出し関数
    void Awake()
    {
        //シングルトン管理
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); //既にInstanceがあれば破棄
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0;
        //生成に必要なポイント初期値
        spLogical = 6;
        sp = 6;
        spSlider = spSliderUI.GetComponent<Slider>();
        spSlider.value = 1f;
        //Debug.Log(StageIndex.Instance.GetIndex());
        //ApplyEnemyLevel();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(1f / Time.deltaTime);
        UpdateLogicalSP(); //内部SPの増加（一定間隔）
        UpdateDisplaySP(); //スライダーを徐々に近づける
    }
    #endregion

    #region ポイント加算処理

    void UpdateLogicalSP()
    {
        timer += Time.deltaTime;

        if (timer >= increaseInterval)
        {
            spLogical += 1f;
            spLogical = Mathf.Clamp(spLogical, 0f, maxSp);
            timer = 0f;
        }
    }
    #endregion

    #region HP表示用UIのアップデート処理
    void UpdateDisplaySP()
    {
        //表示SPを論理SPに向けて徐々に近づける
        sp = Mathf.Lerp(sp, spLogical, Time.deltaTime);

        spSlider.value = sp / maxSp;
    }
    #endregion

    #region 敵登録用関数
    public void RegisterEnemy(EnemyBase enemy)
    {
        //enemies.Add(enemy);
    }
    #endregion

    #region 敵のレベル適用／表示
    private void ApplyEnemyLevel()
    {
/*        foreach (var enemy in enemies)
        {
            enemy.RandomLevel(lowerLevel, upperLevel);
            enemy.ApplyLevelData();
            enemy.PrintEnemyData();
        }*/
    }
    #endregion
}
