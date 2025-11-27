using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentController : EnemyStatus
{
    #region 定数
    private const int MOVE = 0;   //移動状態
    private const int ATTACK = 1; //攻撃状態

    #endregion

    #region private変数
    [Header("動かしたいオブジェクトのNavMeshAgent")]
    [SerializeField] private NavMeshAgent agent;
    [Header("ターゲット座標")]
    [SerializeField] private Transform target;
    [Header("攻撃ポイント(場所)")]
    [SerializeField] private GameObject point;
    [Header("攻撃範囲")]
    [SerializeField] private float attackArea;
    [Header("攻撃間隔")]
    [SerializeField] private float attackInterval;

    Vector3 attackPos;                                              //攻撃場所保存用
    Vector3 firstPoint;

    private List<Transform> targetsInRange = new List<Transform>(); //攻撃対象物の座標格納用リスト
    private List<Transform> targetsPoint = new List<Transform>();

    private int state = MOVE; 
    private float count;
    private float attackCount = 0;

    #endregion

    #region Unityイベント関数
    private void Awake()
    {
        firstPoint = target.transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        count = 5;
        attackCount = attackInterval;
        point.SetActive(false);
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        switch (state) {
            case MOVE:
                print("移動");
                ResumeAgentMovement();
                agent.SetDestination(target.position);
                break;
            case ATTACK:
                StopAgentMovement();
                
                AttackAreaFollow();
                
                AttackInteral();

                print("こうげき");
                break;
        }
        //デバッグ用HP減らし
/*        count -= Time.deltaTime;
        if (count <= 0)
        {
            hp -= 25;
            count = 1000;
            SetHp(hp);
        }*/
        //Debug.Log(hp);
        //Debug.Log(attackCount);
    }
    #endregion

    #region 攻撃状態管理
    //範囲内に攻撃対象物がいたら攻撃状態へ
    public void AddAttackTarget(Transform t)
    {
        targetsInRange.Add(t);
        state = ATTACK;
    }

    //いなければ移動状態へ
    public void RemoveAttackTarget(Transform t)
    {
        targetsInRange.Remove(t);
        if (targetsInRange.Count == 0) { 
            state = MOVE;
        }
    }

    #endregion


    #region 範囲内にいるターゲットのセット
    public void AddTargetPoint(Transform t)
    {
        targetsPoint.Add(t);
        SetTarget();
    }

    public void RemoveTargetPoint(Transform t)
    {
        target.transform.position = firstPoint;
        targetsPoint.Remove(t);
    }

    //ターゲットをセット
    private void SetTarget()
    {
        Transform closest = GetTargetPoint();
        if (closest != null)
        {
            //対象の位置に最も近い座標(攻撃場所)
            target.transform.position = closest.position;
        }
    }
    #endregion


    #region 移動停止メゾット（NavMesh）
    //移動を停止したいときにこのメソッドを呼び出す
    private void StopAgentMovement()
    {
        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
        }
    }

    #endregion

    #region 移動再開メゾット（NavMesh）
    //移動を再開したいときにこのメソッドを呼び出す
    private void ResumeAgentMovement()
    {
        if (agent != null && agent.enabled)
        {
            agent.isStopped = false;
        }
    }
    #endregion

    #region 攻撃間隔
    void AttackInteral()
    {
        attackCount -= Time.deltaTime;
        if (attackCount <= 0)
        {
            point.SetActive(true);
            attackCount = attackInterval;
        }
        else
        {
            point.SetActive(false);
        }
    }

    #endregion

    #region 攻撃する場所を返す処理
    //攻撃ポイント
    /*    private Vector3 GetClosestNavMeshPoint(Vector3 sourcePosition, float maxDistance)
        {
            NavMeshHit hit;
            //指定範囲内でNavMesh上の最も近い点をサンプリングする
            if (NavMesh.SamplePosition(sourcePosition, out hit, maxDistance, NavMesh.AllAreas))
            {
                //hit.positionがNavMesh上の最も近い地点
                return hit.position;
            }
            //見つからなかった場合は元の位置を返すか、エラー処理を行う
            return sourcePosition;
        }*/
    #endregion

    #region 一番近い攻撃対象物の場所を返す処理(攻撃対象)
    private Transform GetClosestTarget()
    {
        if (targetsInRange.Count == 0) return null;

        Transform closest = targetsInRange[0];
        float minDist = Vector3.Distance(transform.position, closest.position);

        foreach (var t in targetsInRange)
        {
            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < minDist)
            {
                closest = t;
                minDist = dist;
            }
        }

        return closest;
    }

    #endregion

    #region 攻撃場所にアタックエリアを追従
    void AttackAreaFollow()
    {
        Transform closest = GetClosestTarget();
        if (closest != null)
        {
            //対象の位置に最も近い座標(攻撃場所)
            attackPos = closest.position;/*GetClosestNavMeshPoint(closest.position, attackArea);*/
            point.transform.position = attackPos;
        }
    }
    #endregion

    #region 一番近い攻撃対象物の場所を返す処理(移動場所)
    private Transform GetTargetPoint()
    {
        if (targetsPoint.Count == 0) return null;

        Transform closest = targetsPoint[0];
        float minDist = Vector3.Distance(transform.position, closest.position);

        foreach (var t in targetsPoint)
        {
            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < minDist)
            {
                closest = t;
                minDist = dist;
            }
        }

        return closest;
    }

    #endregion


}
