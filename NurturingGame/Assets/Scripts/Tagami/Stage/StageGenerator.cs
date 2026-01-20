using System.Linq;
using System.Collections;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class StageGenerator : MonoBehaviour
{
    #region　定数
    private static Vector3 TOWER_DOWN = new Vector3(0, 2.7f, 2);
    private static Vector3 TOWER_UP = new Vector3(0, 2.7f, -2);
    private static Vector3 TOWER_LEFT = new Vector3(-2, 2.7f, 0);
    private static Vector3 TOWER_RIGHT = new Vector3(2, 2.7f, 0);

    private const float rote = 90;  // 回転角度
    #endregion

    #region public変数
    public StageInfo[] stageInfo;   // ステージ用のスクリプタブルオブジェクト
    public Transform stage;         // 生成する親オブジェクト
    public GameObject player;

    #endregion

    #region private変数
    [Header("NavMesh")]
    [SerializeField] private NavMeshSurface surface;
    [Header("プレイヤーモブ生成場所")]
    [SerializeField] private Transform spawn;
    [Header("敵の城プレファブ")]
    [SerializeField] private GameObject enemyCastle;
    [SerializeField] float objInterval = 2;     // オブジェクト生成の間隔
    private int stageIndex = 0;      // 生成するステージ
    private Vector3 spownPos;       // playerの生成座標

    List<GameObject> cubes = new List<GameObject>();

    private Vector3 casPos;
    #endregion

    #region　Unityイベント関数
    void Awake()
    {
        

        if (surface == null)
        {
            surface = GetComponentInParent<NavMeshSurface>();
        }

        stageIndex = StageIndex.Instance.GetIndex();

        if(stageIndex < 1 ||  stageIndex >= stageInfo.Length) stageIndex = 1;

        // スクリプタブルオブジェクトがnullじゃないなら
        if (stageInfo != null)
        {
            ImportCSV(stageInfo[stageIndex]);   // CSV読み込み
            Generator();                        // 生成処理
        }

        player.transform.position = spownPos;   // 生成座標にplayerをセット
        spawn.transform.position = spownPos;    //生成座標にspwan場所をセット
    }

    void Start()
    {
        StartCoroutine(BakeDelay());
    }

    #region ベイク処理
    /// <summary>
    /// 時間を置いて確実にBakeする
    /// </summary>
    /// <returns></returns>
    IEnumerator BakeDelay()
    {
        yield return null; //1フレーム待つ

        Bake();
    }

    /// <summary>
    /// BakeのNullチェック
    /// </summary>
    private void Bake()
    {
        if (surface == null)
        {
            Debug.LogError("NavMeshSurface が null");
            return;
        }

        Debug.Log("NavMesh Bake Start");
        surface.BuildNavMesh();
        GameManager.Instance.SetCastlePos(casPos);
    }

    #endregion

    #endregion



    #region　CSV関数
    private static void ImportCSV(StageInfo info)
    {
        string[] lines = info.csv.text.Replace("\r", "").Split('\n').
            Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        if (lines.Length == 0) return;

        int row = lines.Length;
        int col = lines[0].Split(',').Length;

        info.lines = row;
        info.columns = col;
        info.map = new int[row][];

        for (int y = 0; y < row; y++)
        {
            string[] cols = lines[y].Split(',');
            info.map[y] = new int[col];
            for (int x = 0; x < col; x++)
            {
                int.TryParse(cols[x], out info.map[y][x]);
            }
        }
    }
    #endregion

    #region 生成処理
    private void Generator()
    {
        StageInfo currentStage = stageInfo[stageIndex];

        for (int i = 0; i < currentStage.lines; i++)
        {
            for (int j = 0; j < currentStage.columns; j++)
            {
                float x = j * objInterval;
                float z = -i * objInterval;

                int index = currentStage.map[i][j];
                if (index >= 0 && index < currentStage.stageObj.Length)
                {
                    if (currentStage.stageObj[index] != null)
                    {
                        if (index == (int)Info.START)
                        {
                            spownPos = new Vector3(x, 1, z);
                        }
                        
                        if(index == (int)Info.GOAL)
                        {
                            casPos = new Vector3(x, 1, z);


                        }

                        if (index == (int)Info.TOWER)
                        {
                            GameObject obj = Instantiate(currentStage.stageObj[index], new Vector3(x, 0, z), Quaternion.identity, stage);
                            TowerMove(i, j, obj);
                        }
                        else
                        {
                            var obj = Instantiate(currentStage.stageObj[index], new Vector3(x, 0, z), Quaternion.identity, stage);
                        
                            if(index == (int)Info.GROUND || index == (int)Info.ROAD || index == (int)Info.START)
                            {
                                cubes.Add(obj);
                            }
                        }
                    }
                }

            }
        }

       //  CombineMeshesForCollider();
    }

    private void TowerMove(int y, int x, GameObject obj)
    {

        StageInfo currentStage = stageInfo[stageIndex];
        GameObject tower = obj.transform.GetChild(1).gameObject;

        if (y - 1 >= 0)
        {
            if (currentStage.map[y - 1][x] == (int)Info.ROAD)
            {
                tower.transform.position = obj.transform.position + TOWER_DOWN;
                Debug.Log("UP");
                tower.transform.rotation = Quaternion.Euler(-rote, 0, rote * 2);
                return;
            }
        }

        if (y + 1 < currentStage.lines)
        {
            if (currentStage.map[y + 1][x] == (int)Info.ROAD)
            {
                tower.transform.position = obj.transform.position + TOWER_UP;

                tower.transform.rotation = Quaternion.Euler(-rote, 0, rote * 2);
                Debug.Log("DOWN");
                return;
            }
        }

        if (x - 1 >= 0)
        {
            if (currentStage.map[y][x - 1] == (int)Info.ROAD)
            {
                tower.transform.position = obj.transform.position + TOWER_LEFT;
                tower.transform.rotation = Quaternion.Euler(-rote, 0, -rote);
                Debug.Log("LEFT");
                return;
            }
        }

        if (x + 1 < currentStage.columns)
        {
            if (currentStage.map[y][x + 1] == (int)Info.ROAD)
            {
                tower.transform.position = obj.transform.position + TOWER_RIGHT;
                tower.transform.rotation = Quaternion.Euler(-rote, 0, rote);
                Debug.Log("RIGHT");
                return;
            }
        }
    }
    #endregion

    #region tanaka
    /// <summary>
    /// 当たり判定を親オブジェクトへ結合
    /// </summary>
    private void CombineMeshesForCollider()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combine = new List<CombineInstance>();

        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.gameObject == this.gameObject) continue;

            CombineInstance ci = new CombineInstance();
            ci.mesh = mf.sharedMesh;
            ci.transform = mf.transform.localToWorldMatrix * transform.worldToLocalMatrix;
            combine.Add(ci);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combine.ToArray(), true, true);

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedMesh;

        foreach (GameObject cube in cubes)
        {
            Destroy(cube.GetComponent<Collider>());
        }
    }
    #endregion
}
