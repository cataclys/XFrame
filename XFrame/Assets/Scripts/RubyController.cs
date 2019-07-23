using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : BaseBehaviour
{
    #region 公有属性
    public int Health { get { return currentHealth; } }
    //最大生命
    public int MaxHealth { get; } = 5;
    //速度
    public float Speed { get; set; } = 3f;
    //无敌时间
    public float TimeInvincible { get; set; } = 2f;

    #endregion


    #region 私有属性
    //角色朝向
    Vector2 lookDirection = new Vector2(1, 0);
    Rigidbody2D rigidbody2d;
    //当前血量
    int currentHealth;
    //是否无敌
    bool isInvincible;
    //无敌事件
    float invincibleTime;
    Animator animator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("MoveX", lookDirection.x);
        animator.SetFloat("MoveY", lookDirection.y);

        animator.SetFloat("Speed", move.magnitude);


        Vector2 position = rigidbody2d.position;
        position = position + move * Speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (isInvincible)
        {
            invincibleTime -= Time.deltaTime;
            if (invincibleTime <= 0)
                isInvincible = false;
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            isInvincible = true;
            invincibleTime = TimeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, MaxHealth);
    }
}
