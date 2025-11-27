using UnityEngine;
using UnityEngine.Splines;

public class EnemyMovePas : MonoBehaviour
{
    #region private変数
    [Header("移動スピード")]
    [SerializeField] private float speed;          //移動スピード
    [Header("パスへの吸着強さ")]
    [SerializeField] private float stickStrength;  //パスに吸着させる強さ

    float t = 0f;                                  //スプライン上の現在位置（0.0～1.0 の範囲で進行度を表す）

    Rigidbody rb;

    SplineContainer splineContainer;               //使用するスプライン（パス）を保持するコンテナ

    Spline spline;                                 //実際に参照して使うスプラインデータ（splineContainer から取得）
    #endregion

    [System.Obsolete]
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        splineContainer = FindObjectOfType<SplineContainer>(); //シーン内のSplineContainerを取得
        spline = splineContainer.Splines[0];
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        //スプライン上の距離の割合を更新
        t += (speed * Time.fixedDeltaTime) / spline.GetLength();
        //if (t > 1f) t = 0f;

        //パス上の位置/スプライン上の位置
        Vector3 targetPos = (Vector3)spline.EvaluatePosition(t);

        //パスの進行方向/スプラインの進行方向
        Vector3 forward = ((Vector3)spline.EvaluateTangent(t)).normalized;

        //進む方向（X,Z）
        Vector3 move = forward * speed;

        //パスに吸着補正（ズレ防止）
        Vector3 correction = (targetPos - transform.position) * stickStrength;

        //高さだけはスプライン位置を使う
        correction.y = 0;

        //最終的な速度
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z) + correction;

        //向きをパス方向へ
        transform.forward = forward;
    }
}
