using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElementBody : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animations;
    public GameObject player;
    public SpriteRenderer spRenderer;
    public WaterBasicAttack basicAttackPrefab;
    public WaterAirAttack airAttackPrefab;
    public GameObject warden;
    public WardenBody wardenBody;


    public GameObject manager;
    public Manager gm;

    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip takeDamageSound;


    public float health;
    public float maxHealth;
    public float damage;
    public float hpMultiplier;
    public float dmgMultiplier;
    public float resistance;
    public string elementType = "Water";
    public int difficulty;

    public bool isFlipped = false;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool onAir = false;
    public bool isDamaged = false;
    public bool isAttacking = false;
    public bool invisible = false;

    public float randomJumpMin;
    public float randomJumpMax;
    public float randomJumpX;
    public float randomJumpY;

    public float distance;
    Vector3 damageRecoil = new Vector2(0.1f, 0);

    public float attackTime = 0;
    public float damageTime = 0;
    public float basicAttackTimer = 0;
    public float basicAttackValue = 0;
    public float jumpTimer = 0;
    public float jumpValue = 0;
    public float invisibleTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WARDEN");
        rb = GetComponent<Rigidbody2D>();
        animations = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        warden = GameObject.Find("WARDEN");
        wardenBody = warden.GetComponent<WardenBody>();
        manager = GameObject.Find("GAME MANAGER");
        gm = manager.GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        LookPlayer();

        distance = Mathf.Abs(transform.position.x - wardenBody.transform.position.x);

        if (distance <= 0.1f && onAir)
        {
            print("Estoy encima");
            AirAttack();
        }

        if (isJumping == false && isDamaged == false && isFalling == false && isAttacking == false)
        {
            animations.Play("WaterIdle");
        }

        if (rb.velocity.y < 0 && isAttacking == false && isDamaged == false && isFalling == true)
        {
            animations.Play("WaterFall");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            DiagonalJump();
        }

        AutoAttacks();
        Timer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 11)
        {
            isJumping = false;
            isFalling = false;
            print("toque el piso");
            onAir = false;
            isAttacking = false;
        }

        if (collision.gameObject.layer == 10)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        if (collision.gameObject.layer == 12)
        {
            gm.BossDamaged();
            isDamaged = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.layer == 10)
        {
            rb.AddForce(transform.up * 200);

            if (isFlipped == false)
                rb.AddForce(transform.right * 200);

            else rb.AddForce(transform.right * -200);
        }*/

        if (collision.gameObject.layer == 12)
        {
            gm.BossDamaged();
            isDamaged = true;
        }
    }

    void LookPlayer()
    {
        if (transform.position.x > player.transform.position.x)
        {
            spRenderer.flipX = false;
            isFlipped = false;
        }

        if (transform.position.x < player.transform.position.x)
        {
            spRenderer.flipX = true;
            isFlipped = true;
        }
    }

    void DiagonalJump()
    {
        if (isJumping == false)
        {
            randomJumpX = Random.Range(randomJumpMin, randomJumpMax);
            randomJumpY = Random.Range(randomJumpMin, randomJumpMax);
            if (isFlipped == false)
            {
                rb.AddForce(transform.up * randomJumpX);
                rb.AddForce(transform.right * randomJumpY * -1);
                animations.Play("WaterJump");
                isJumping = true;
                isFalling = true;
                onAir = true;
            }
            else if (isFlipped == true)
            {
                rb.AddForce(transform.up * randomJumpX);
                rb.AddForce(transform.right * randomJumpY);
                animations.Play("WaterJump");
                isJumping = true;
                isFalling = true;
                onAir = true;
            }
            audioSource.clip = jumpSound;
            audioSource.Play();
        }
    }

    void Attack()
    {
        if (isAttacking == false && isJumping == false && isFalling == false)
        {
            isAttacking = true;
            animations.Play("WaterBasicAttack");
            WaterBasicAttack normalAttack = Instantiate(basicAttackPrefab);
            audioSource.clip = attackSound;
            audioSource.Play();
        }
    }

    void AirAttack()
    {
        if (isAttacking == false)
        {
            isAttacking = true;
            animations.Play("WaterAirAttack");
            WaterAirAttack airAttack = Instantiate(airAttackPrefab);
            print("Ataque en el aire");
            audioSource.clip = attackSound;
            audioSource.Play();
        }  
    }

    public void DamageCalculator(float damageTaken, string receivedElement)
    {
        if (receivedElement == elementType)
        {
            resistance = 0.5f;
            spRenderer.enabled = false;
            invisible = true;
        }

        else if (receivedElement == "Fire")
        {
            resistance = 1f;
            spRenderer.enabled = false;
            invisible = true;
        }

        else if (receivedElement == "Earth")
        {
            resistance = 2;
            if (isFlipped)
            {
                animations.Play("WaterTakeDamage");
                transform.position = transform.position + damageRecoil;
            }
            else
            {
                spRenderer.flipX = false;
                animations.Play("WaterTakeDamage");
                transform.position = transform.position - damageRecoil;
            }
            audioSource.clip = takeDamageSound;
            audioSource.Play();
        }
        health = health - (damageTaken * resistance);
    }

    public void Timer()
    {
        if (isDamaged)
        {
            damageTime += Time.deltaTime;
        }

        if (damageTime >= 1)
        {
            isDamaged = false;
            damageTime = 0;
        }

        if (isAttacking == true)
        {
            attackTime += Time.deltaTime;
        }

        if (attackTime >= 0.3f)
        {
            attackTime = 0;
            isAttacking = false;
        }
    }

    void AutoAttacks()
    {
        if (isAttacking == false && isDamaged == false)
        {
            basicAttackTimer += Time.deltaTime;
            jumpTimer += Time.deltaTime;
        }

        if (basicAttackTimer >= basicAttackValue)
        {
            Attack();
            basicAttackTimer = 0;
        }

        if (jumpTimer >= jumpValue)
        {
            DiagonalJump();
            jumpTimer = 0;
        }

        if (invisible)
        {
            invisibleTime += Time.deltaTime;
        }

        if (invisibleTime >= 0.3f)
        {
            spRenderer.enabled = true;
            invisible = false;
            invisibleTime = 0;
        }
    }

    public void AssignStats(float hpMult, float dmgMult, int diff)
    {
        difficulty = diff;
        damage = damage * dmgMult;
        health = health * hpMult;
        maxHealth = health;
        gm.GetElementalStats(maxHealth, health, damage);
    }
}
