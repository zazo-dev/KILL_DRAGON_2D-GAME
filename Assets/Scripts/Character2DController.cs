using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character2DController : MonoState<Character2DController>
{
    [Header("Move")]
    [SerializeField]
    float moveSpeed = 100.0F;

    [SerializeField]
    float maxSpeed = 7.0F;

    [SerializeField]
    float linearDrag = 4.0F;

    [SerializeField]
    bool isFacingRight = true;

    [Header("jump")]
    [SerializeField]
    float jumpForce = 140.0F;

    [SerializeField]
    float fallMultiplier = 3.0F;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    float jumpGraceTime = 0.20F;

    [Header("Attack")]
    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    LayerMask enemyMask;

    [SerializeField]
    float reboundY = 5.0F;

    [Header("Attack Parameters")]
    [SerializeField]
    float attackRadius = 0.2F;

    [SerializeField]
    float attackDelay = 0.1F;

    [Header("Animation")]
    [SerializeField]
    public Animator animator;


    Rigidbody2D _rb;

    Vector2 _direction;

    bool _isMoving;
    bool _isJumping;
    bool _isJumpPressed;
    bool _isAttacking;

    float _gravityY;
    float _lastTimeJumpPressed;
    float _meleeDamage;

    private AudioSource _jump;

    [HideInInspector]
    public bool _canMove = true;

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();
        _gravityY = -Physics2D.gravity.y;
        _jump = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleInputs();
    }

    void FixedUpdate()
    {
        HandleJump();
        HandleFlipX();
        HandleMove();
    }

    void HandleInputs()
    {
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0.0F);
        _isMoving = _direction.x != 0.0F;

        _isJumpPressed = Input.GetButtonDown("Jump");

        if (_isJumpPressed)
        {
            _lastTimeJumpPressed = Time.time;
        }
    }

    void HandleMove()
    {
        if (!_canMove)
        {
            return;
        }

        bool isMoving = animator.GetFloat("speed") > 0.01F;
        if (_isMoving != isMoving && !_isJumping)
        {
            animator.SetFloat("speed", Mathf.Abs(_direction.x));
        }

        Vector2 velocity = _direction * moveSpeed * Time.fixedDeltaTime;
        if (Mathf.Abs(velocity.x) > maxSpeed)
        {
            velocity.x = Math.Sign(velocity.x) * maxSpeed;
        }

        velocity.y = _rb.velocity.y;
        _rb.velocity = velocity;
    }

    void HandleFlipX()
    {
        if (!_isMoving)
        {
            return;
        }

        bool facingRight = _direction.x > 0.0F;
        if (isFacingRight != facingRight)
        {
            isFacingRight = facingRight;
            transform.Rotate(0.0F, 180.0F, 0.0F);

            _rb.drag = linearDrag;
        }
        else
        {
            _rb.drag = 0.0F;
        }
    }

    void HandleJump()
    {
        if (_lastTimeJumpPressed > 0.0F && Time.time - _lastTimeJumpPressed <= jumpGraceTime)
        {
            _isJumpPressed = true;
            _jump.Play();
        }
        else
        {
            _lastTimeJumpPressed = 0.0F;
        }

        if (_isJumpPressed)
        {
            bool isGrounded = IsGrounded();
            if (isGrounded)
            {
                _rb.velocity += Vector2.up * jumpForce * Time.fixedDeltaTime;
            }
        }

        if (_rb.velocity.y < -0.01F)
        {
            _rb.velocity -= Vector2.up * _gravityY * fallMultiplier * Time.fixedDeltaTime;
        }

        _isJumping = !IsGrounded();

        bool isJumping = animator.GetBool("isJumping");
        if (_isJumping != isJumping)
        {
            animator.SetBool("isJumping", _isJumping);
        }

        bool isFalling = animator.GetBool("isFalling");
        bool isNegativeVelocityY = _rb.velocity.y < -0.01F;

        if (isNegativeVelocityY != isFalling)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", isNegativeVelocityY);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCapsule
            (groundCheck.position, new Vector2(1.25F, 0.65F),
            CapsuleDirection2D.Horizontal, 0.0F, groundMask);
    }

    public void Attack(float damage)
    {
        if (_isAttacking)
        {
            return;
        }

        _isAttacking = true;
        _meleeDamage = damage;

        animator.SetBool("attack", true);

        // Se hace una corutina de retraso antes de verificar colisiones para dar tiempo a la animación de ataque
        StartCoroutine(DelayedAttackCheck());

        StartCoroutine(ResetAttackState());
    }

    IEnumerator DelayedAttackCheck()
    {
        yield return new WaitForSeconds(attackDelay);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);
        foreach (Collider2D collider in enemies)
        {
            // Se verifica si el objeto con el que se colisiona es un enemigo
            EnemyController enemy = collider.GetComponent<EnemyController>();

            if (enemy != null)
            {
                // Se aplicar daño al enemigo
                enemy.TakeDamage(_meleeDamage);
            }
            else
            {
                // Se verifica si es un jefe o un enemigo normal
                BossController boss = collider.GetComponent<BossController>();

                if (boss != null)
                {
                    // Aplicar daño al jefe
                    boss.TakeDamage(_meleeDamage);
                }
            }
        }
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.5f); 

        _isAttacking = false;
        animator.SetBool("attack", false);
    }

    public void Rebound()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, reboundY);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
            
        }
    }
}
