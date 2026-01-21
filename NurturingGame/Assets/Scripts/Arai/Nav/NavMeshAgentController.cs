using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

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
    [Header("最終攻撃対象（名前）")]
    [SerializeField] private string castleName;

    Vector3 attackPos;                                              //攻撃場所保存用
    Transform firstPoint;
    Animator animator;

    private List<Transform> targetsInRange = new List<Transform>(); //攻撃対象物の座標格納用リスト
    private List<Transform> targetsPoint = new List<Transform>();

    private int state = MOVE;
    private bool isMove;
    private bool isAttack;
    private float attackCount = 0;

    private bool targetFlg = false;

    #endregion

    #region セット関数
    public void SetState(int state)
    {
        this.state = state;
    }

    public void SetPos(Transform pos)
    {
        target.transform.position = pos.position;
        agent.SetDestination(pos.position);
    }

    #endregion

    #region ゲット関数
    public Transform GetFirstPos()
    {
        // firstPoint = GameObject.Find(castleName).transform;
        return firstPoint;
    }

    public Transform GetTargetPos()
    {
        return target;
    }
    #endregion

    #region Unityイベント関数
    private void Awake()
    {
        isAttack = false;
        isMove = true;
         firstPoint = GameObject.Find(castleName).transform;
        //firstPoint = GameManager.Instance.GetCastlePos();
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        target.transform.position = firstPoint.position;
        attackCount = attackInterval;
        point.SetActive(false);
        agent.SetDestination(target.position);
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // Debug.Log(state);

        // Debug.Log(firstPoint.position);
        switch (state)
        {
            case MOVE:
                //print("移動");
                ResumeAgentMovement();
                break;
            case ATTACK:
                StopAgentMovement();

                AttackAreaFollow();

                AttackInteral();
                //print("こうげき");
                break;
        }
        SetTarget();
        IsDead();
        SetAnimatorAttack();
        SetAnimatorMove();
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

    #region アニメーションセット
    void SetAnimatorAttack()
    {
        animator.SetBool("IsAttack", isAttack);
    }

    void SetAnimatorMove()
    {
        animator.SetBool("IsMove", isMove);
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
        if (targetsInRange.Count == 0)
        {
            state = MOVE;
            isAttack = false;
        }
    }

    /*    IEnumerator AttackCoolTime()
        {
            isAttack = true;
            yield return new WaitForSeconds(1f);
            isAttack = false;
        }*/
    #endregion


    #region 範囲内にいるターゲットのセット
    public void AddTargetPoint(Transform t)
    {
        targetsPoint.Add(t);
    }

    public void RemoveTargetPoint(Transform t)
    {
        target.transform.position = firstPoint.position;
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
        else
        {
            target.transform.position = firstPoint.position;
        }
    }
    #endregion


    #region 移動停止メゾット（NavMesh）
    //移動を停止したいときにこのメソッドを呼び出す
    private void StopAgentMovement()
    {
        if (agent != null && agent.enabled)
        {
            isMove = false;
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
            isMove = true;
            agent.isStopped = false;
        }
    }
    #endregion

    #region 攻撃間隔
    /// <summary>
    /// 攻撃間隔を管理する処理
    /// Update から呼ばれ、一定時間ごとに攻撃を発生させる
    /// </summary>
    void AttackInteral()
    {
        //経過時間を減算
        attackCount -= Time.deltaTime;

        //攻撃クールタイムが終了したら攻撃実行
        if (attackCount <= 0)
        {
            DoAttack();

            //次の攻撃までのクールタイムをリセット
            attackCount = attackInterval;
        }
    }

    /// <summary>
    /// 実際の攻撃処理を開始する
    /// </summary>
    void DoAttack()
    {
        //攻撃中フラグをON（アニメーション制御用）
        isAttack = true;

        //攻撃判定用オブジェクトを表示
        point.SetActive(true);

        //一定時間後に攻撃終了処理を呼び出す
        Invoke(nameof(EndAttack), 0.5f);
    }

    /// <summary>
    /// 攻撃終了時の処理
    /// </summary>
    void EndAttack()
    {
        //攻撃中フラグをOFF
        isAttack = false;

        //攻撃判定用オブジェクトを非表示
        point.SetActive(false);
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
        //破壊されたオブジェクトや null をリストから除外
        targetsInRange.RemoveAll(t => t == null);

        if (targetsInRange.Count == 0) return null; //対象がいなければ null を返す

        Transform closest = null;
        float minDist = float.MaxValue;

        foreach (var t in targetsInRange)
        {
            //null チェック
            if (t == null) continue;

            float dist = Vector3.Distance(transform.position, t.position);

            //より近いターゲットを見つけた場合は更新
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
        //近い攻撃対象を取得
        Transform closest = GetClosestTarget();

        if (closest != null)
        {
            //対象が存在する場合のみ攻撃位置を更新
            attackPos = closest.position;
            point.transform.position = attackPos;
        }
        else
        {
            //対象がいない場合は攻撃ポイント非表示
            point.SetActive(false);
            state = MOVE;

            // 城に戻す
            target.transform.position = firstPoint.position;
            agent.SetDestination(firstPoint.position);
        }
    }
    #endregion

    #region 一番近い攻撃対象物の場所を返す処理(移動場所)
    private Transform GetTargetPoint()
    {
        //破壊済みオブジェクトや null を除外
        targetsPoint.RemoveAll(t => t == null);

        if (targetsPoint.Count == 0) return null;

        Transform closest = null;
        float minDist = float.MaxValue;

        foreach (var t in targetsPoint)
        {
            if (t == null) continue;

            Debug.Log(t.position);
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

