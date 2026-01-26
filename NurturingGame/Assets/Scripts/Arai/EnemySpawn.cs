using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
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

    [Header("敵の生成データ")]
    [SerializeField, EnumIndex(typeof(Stage))]
    private EnemyMobData[] data;                           //敵モブ用データ

    private float randomX;                                 //ランダムスポーンポジションX座標用
    private float randomZ;                                 //ランダムスポーンポジションZ座標用
    private Vector3 spawnPosition;                         //ランダムスポーンポジション
    private int random;                                    //ランダム生成用

    private float coolTimer;                               //クールダウン計測用
    private const float coolTime = 10;                     //クールダウンセット用
    private int cnt;
    #endregion

    #region Unity呼び出し関数
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coolTimer = 5; 
        cnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTime();
        RandomSpawn();
    }
    #endregion

    #region ランダムの値を返す
    /// <summary>
    /// ランダムの値を返す
    /// </summary>
    /// <returns>生成するキャラ</returns>
    private int GetRandom()
    {
        Random.Range(0, 2);
        return random;
    }
    #endregion

    #region 生成計測処理
    /// <summary>
    /// 生成計測処理
    /// </summary>
    private void SpawnTime()
    {
        if (coolTimer > 0)
        {
            coolTimer -= Time.deltaTime;
        }
    }

    #endregion

    #region ランダム生成処理
    /// <summary>
    /// ランダム生成処理
    /// </summary>
    private void RandomSpawn()
    {
        if(cnt >= data[StageIndex.Instance.GetIndex()].max)
        {
            return;
        }

        if (coolTimer <= 0)
        {
            switch (GetRandom())
            {
                case 0:
                    for(int i = 0; i < data[StageIndex.Instance.GetIndex()].number; i++)
                    {
                        SpawnKnigth();
                        cnt++;
                    }
                    break;
                case 1:
                    for (int i = 0; i < data[StageIndex.Instance.GetIndex()].number; i++)
                    {
                        SpawnArcher();
                        cnt++;
                    }
                    break;
            }
            coolTimer = coolTime;
        }
    }
    #endregion

    #region ランダム生成場所
    /// <summary>
    /// ランダム生成場所
    /// </summary>
    /// <returns>生成する場所</returns>
    private Vector3 GetRandomPos()
    {


        //範囲内でランダムな座標を計算
        randomX = Random.Range(-data[StageIndex.Instance.GetIndex()].spawnSize.x / 2, data[StageIndex.Instance.GetIndex()].spawnSize.x / 2);
        randomZ = Random.Range(-data[StageIndex.Instance.GetIndex()].spawnSize.z / 2, data[StageIndex.Instance.GetIndex()].spawnSize.z / 2);

        //スポーナーの位置を基準に生成位置を決定
        spawnPosition = transform.position + new Vector3(randomX, 0, randomZ);

        return spawnPosition;
    }

    #endregion

    #region 騎士生成処理
    /// <summary>
    /// 騎士生成処理
    /// </summary>
    private void SpawnKnigth()
    {
        //ランダムの場所を取得
        GetRandomPos();

        //騎士をインスタンス化する(生成する)
        Instantiate(data[StageIndex.Instance.GetIndex()].knight, spawnPosition, Quaternion.identity);
    }
    #endregion

    #region 弓兵生成処理
    /// <summary>
    /// 弓兵生成処理
    /// </summary>
    private void SpawnArcher()
    {
        //ランダムの場所を取得
        GetRandomPos();

        //弓兵をインスタンス化する(生成する)
        Instantiate(data[StageIndex.Instance.GetIndex()].archer, spawnPosition, Quaternion.identity);
    }
    #endregion
}
