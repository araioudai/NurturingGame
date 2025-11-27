using UnityEngine;

public class EnemyTester : MonoBehaviour
{
    [SerializeField] private EnemyFirst enemyFirst;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //IReceiveLevel levelObj = gameObject.GetComponent<IReceiveLevel>();   //インターフェースを取得する
        if (enemyFirst != null)
        {
            enemyFirst.RandomLevel(1, 50);  //Levelメソッドを呼び出す
            enemyFirst.ApplyLevelData();    //Levelから各値を設定関数を呼び出す
            enemyFirst.PrintEnemyData();    //ここで表示（確実に値が反映された後）
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
