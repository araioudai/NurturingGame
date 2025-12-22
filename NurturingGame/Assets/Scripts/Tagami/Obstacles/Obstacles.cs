using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacles : MonoBehaviour
{
    #region private
    [Header(" 最大体力")]
    [SerializeField] int maxHp;
    [Header("HPUIキャンバス")]
    [SerializeField] GameObject HPUI;
    [Header("HPBar（スライダー）")]
    [SerializeField] protected GameObject hpSliderUI;
    [Header("ターゲット座標")]
    [SerializeField] private Transform target;
    [Header("攻撃ポイント(場所)")]
    [SerializeField] private GameObject point;
    [Header("攻撃間隔")]
    [SerializeField] private float attackInterval;

    private int hp;                     // 現在体力
    private float attackCount = 0;      // 攻撃間隔用のカウンタ 
    private Vector3 attackPos;          // 攻撃場所保存用
    private Vector3 firstPoint;

    private List<Transform> targetsInRange = new List<Transform>(); //攻撃対象物の座標格納用リスト
    private List<Transform> targetsPoint = new List<Transform>();
    private Slider hpSlider;
    #endregion

    #region Unityイベント関数
    void Start()
    {
        hp = maxHp;                         // hpを最大hpに初期化
        hpSlider = hpSliderUI.GetComponent<Slider>();   // スライダーの取得
        hpSlider.value = 1f;                            // スライダーの初期化
    }

    void Update()
    {
        if (targetsInRange.Count > 0)   // 敵のリストが0より大きいなら
        {
            AttackAreaFollow();

            AttackInteral();
        }
        SetTarget();

        UpdateHPValue();    // hpのUI更新
    }

    #endregion

    #region HP処理
    public void SetHp(int attack)
    {
        // 攻撃をもらったときにhpが0以下にならないなら
        if (hp - attack > 0)
        {
            hp -= attack;   // hpをダメージ分だけ減らす
        }
        else
        {
            // hpを0にする
            hp = 0;

            // HP表示用UIを非表示にする
            HideStatusUI();

            Destroy(this.gameObject);
        }
    }

    // hp取得用のゲッター
    public int GetHp()
    {
        return hp;
    }
    #endregion

    #region UI
    private void HideStatusUI()
    {
        HPUI.SetActive(false);
    }

    private void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)maxHp;
    }
    #endregion

    #region 範囲内の敵チェック
    //範囲内に攻撃対象物がいたらリストへ追加
    public void AddAttackTarget(Transform t)
    {
        targetsInRange.Add(t);
    }

    //いなければリストから削除
    public void RemoveAttackTarget(Transform t)
    {
        targetsInRange.Remove(t);
    }

    #endregion

    #region 攻撃間隔
    void AttackInteral()
    {
        attackCount -= Time.deltaTime;
        StartCoroutine("AttackTime");
    }

    IEnumerator AttackTime()
    {
        if (attackCount <= 0)
        {
            point.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            attackCount = attackInterval;
            point.SetActive(false);
        }
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
            point.transform.position = closest.transform.position;
        }
        else
        {
            //対象がいない場合は攻撃ポイント非表示
            point.SetActive(false);
        }
    }
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

    #region 範囲内にいるターゲットのセット
    public void AddTargetPoint(Transform t)
    {
        targetsPoint.Add(t);
    }

    public void RemoveTargetPoint(Transform t)
    {
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
            target.transform.position = firstPoint;
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
