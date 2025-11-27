using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    #region private変数
    [Header("敵の生成範囲指定")]
    [SerializeField] private Vector3 spawnAreaSize; //敵を生成する範囲のサイズ
    [Header("生成したい敵オブジェクトセット")]
    [SerializeField] private GameObject charaKnight;  //騎士生成用
    [SerializeField] private GameObject charaArcher;  //弓兵生成用
    [SerializeField] private GameObject charaPaladin; //パラディン生成用
    [Header("ステージの敵生成上限")]
    [SerializeField] protected int enemyLimit;        //敵の生成上限設定用

    private int point;                                //生成に必要なポイント
    private float count;                              //ポイントアップの為のカウント
    private float randomX;                            //ランダムスポーンポジションX座標用
    private float randomZ;                            //ランダムスポーンポジションZ座標用
    private Vector3 spawnPosition;                    //ランダムスポーンポジション
    #endregion

    #region Unity呼び出し関数
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //生成に必要なポイント
        point = 6;

        //範囲内でランダムな座標を計算
        randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        randomZ = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

        //スポーナーの位置を基準に生成位置を決定
        spawnPosition = transform.position + new Vector3(randomX, 0, randomZ);

        /*        for (int i = 0; i < enemyLimit; i++)
                {
                    //範囲内でランダムな座標を計算
                    float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
                    float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

                    //スポーナーの位置を基準に生成位置を決定
                    Vector3 spawnPosition = transform.position + new Vector3(x, 0, z);

                    //敵をインスタンス化する(生成する)
                    Instantiate(charaKnight, spawnPosition, Quaternion.identity);
                }*/
        StartCoroutine(ApplyLevelNextFrame());
    }

    IEnumerator ApplyLevelNextFrame()
    {
        yield return null; //登録されてから表示するために1フレーム待つ
        GameManager.Instance.ApplyEnemyLevel();
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if(count >= 2) { point += 1; count = 0; }
        if(point >= 12) { point = 12; }
        Debug.Log(point);
    }
    #endregion

    #region 騎士生成が押された時の処理
    public void PushKnigth()
    {
        if(point >= 1)
        {
            //騎士をインスタンス化する(生成する)
            Instantiate(charaKnight, spawnPosition, Quaternion.identity);
            point -= 1;
        }
    }
    #endregion

    #region 弓兵生成が押された時の処理
    public void PushArcher()
    {
        if (point >= 2)
        {
            //弓兵をインスタンス化する(生成する)
            Instantiate(charaArcher, spawnPosition, Quaternion.identity);
            point -= 2;
        }
    }
    #endregion

    #region パラディン生成が押された時の処理
    public void PushPaladin()
    {
        if (point >= 3)
        {
            //弓兵をインスタンス化する(生成する)
            Instantiate(charaPaladin, spawnPosition, Quaternion.identity);
            point -= 3;
        }
    }
    #endregion
}
