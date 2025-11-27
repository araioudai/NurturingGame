using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region シングルトン（他スクリプトからInstanceでアクセスできるようにする）
    public static GameManager Instance { get; private set; }
    #endregion

    #region private変数
    private List<EnemyBase> enemies = new List<EnemyBase>();

    [Header("lower:ランダムの下限 upper:ランダムの上限")]
    [SerializeField] private int lowerLevel;
    [SerializeField] private int upperLevel;
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
        lowerLevel = 1;
        upperLevel = 50;
        ApplyEnemyLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region 敵登録用関数
    public void RegisterEnemy(EnemyBase enemy)
    {
        enemies.Add(enemy);
    }
    #endregion

    #region 敵のレベル適用／表示
    public void ApplyEnemyLevel()
    {
        foreach (var enemy in enemies)
        {
            enemy.RandomLevel(lowerLevel, upperLevel);
            enemy.ApplyLevelData();
            enemy.PrintEnemyData();
        }
    }
    #endregion
}
