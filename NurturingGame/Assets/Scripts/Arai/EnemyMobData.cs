using UnityEngine;

/// <summary>
/// プレイヤーモブ用のデータ
/// </summary>
[CreateAssetMenu(
    fileName = "EnemySpawn",
    menuName = "Enemy/Spawn"
)]
public class EnemyMobData : ScriptableObject
{
    [Header("ステージごとの生成上限")]
    [SerializeField] private int spawnMax;           //生成上限
    [Header("一度に生成する数")]
    [SerializeField] private int spawnNumber;        //一度に生成する数
    [Header("モブの生成範囲指定")]
    [SerializeField] private Vector3 spawnAreaSize;  //モブを生成する範囲のサイズ
    [Header("生成したいモブプレファブセット")]
    [SerializeField] private GameObject charaKnight; //騎士生成用
    [SerializeField] private GameObject charaArcher; //弓兵生成用

    /// <summary>
    /// モブを生成する範囲のサイズを返す
    /// </summary>
    public int max
    {
        get { return spawnMax; }
    }

    /// <summary>
    /// 一度に生成する数
    /// </summary>
    public int number
    {
        get { return number; }
    }

    /// <summary>
    /// モブを生成する範囲のサイズを返す
    /// </summary>
    public Vector3 spawnSize
    {
        get { return spawnAreaSize; }
    }

    /// <summary>
    /// モブを生成する範囲のサイズを返す
    /// </summary>
    public GameObject knight
    {
        get { return charaKnight; }
    }

    /// <summary>
    /// モブを生成する範囲のサイズを返す
    /// </summary>
    public GameObject archer
    {
        get { return charaArcher; }
    }

}

