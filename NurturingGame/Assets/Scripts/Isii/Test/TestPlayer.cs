using System.Collections;
using UnityEngine;

public class TestPlayer : PlayerBase
{
    private static WaitForSeconds _waitForSeconds2 = new WaitForSeconds(2f);
    private Animator animator;
    // アニメーションの状態
    // ジャンプ
    [SerializeField] bool isJumping = false;
    [SerializeField] bool isBigJump = false;
    // 移動
    [SerializeField] bool isWalking = false;
    [SerializeField] bool isBacking = false;
    // 攻撃
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isPunching = false;







    [SerializeField] private float speed = 5f;



    public override void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        Movement();
        MotionUpdate();
    }


    void Movement()
    {
        // 移動
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += speed * Time.deltaTime * transform.forward;
            isWalking = true;
            isBacking = false;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= speed * Time.deltaTime * transform.forward;
            isBacking = true;
            isWalking = false;
        }
        else
        {
            isWalking = false;
            isBacking = false;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0.2f, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -0.2f, 0);
        }

        // ジャンプ
        if (Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        // 大ジャンプの切り替え
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isBigJump = !isBigJump;
        }

        // 攻撃
        if (Input.GetKey(KeyCode.Q))
        {
            isPunching = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                isAttacking = true;
                StartCoroutine(AttackCoroutine());
            }
        }
        else
        {
            isPunching = false;
        }


    }

    /// <summary>
    /// アニメーションの更新
    /// </summary>
    void MotionUpdate()
    {
        // 移動
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isBacking", isBacking);
        // ジャンプ
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isBigJump", isBigJump);
        // 攻撃
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isPunching", isPunching);
    }

    IEnumerator AttackCoroutine()
    {
        yield return _waitForSeconds2;
        isAttacking = false;
    }





}
