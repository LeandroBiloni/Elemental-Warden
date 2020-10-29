using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElementBody : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animations;
    public GameObject player;
    public SpriteRenderer spRenderer;
    public FireBasicAttack basicAttackPrefab;
    public FireAirAttack airAttackPrefab;
    public FireMultiple multiplePrefab;
    public Transform trans;
    public Vector3 direction;
    public GameObject manager;
    public Manager gm;

    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip takeDamageSound;
    public AudioClip kickSound;
    public AudioClip ultimateSound;

    public int difficulty;
    public string elementType = "Fire";

    public float jumpForce;
    float randomMin;
    float randomMax;
    public float gotRandom;
    public bool allowKick = false;
    public bool allowAirAttack = false;
    bool gotDirection = false;
    public float kickSpeed;
    int jumpCounter;
    public float horizontalForce;
    bool onAir = false;
    bool createFire = false;
    int fireCounter;
    public int createdFires;
    public bool isAngry = false;
    public bool enraged = false;
    public float maxHealth;
    public float health;
    public float damage;
    public float resistance;
    public float hpMultiplier;
    public float dmgMultiplier;
    public bool gettingRandom = false;
    Vector3 damageRecoil = new Vector2(0.1f, 0);




    //TIMERS 
    //***********
    public float damageTime = 0;
    public float attackTime = 0;
    public float airTimer = 0;
    public float kickTimer = 0;
    public float fireTimer = 0;
    public float basicAttackTimer = 0;
    public float basicAttackValue = 0;
    public float jumpTimer = 0;
    public float jumpValue = 0;
    public float ultimateAttackTimer = 0;
    public float ultimateAttackValue = 0;
    public float transpTimer = 0;
    public float invisibleTime = 0;


    //CHEQUEAN ACCIONES
    //********
    public bool isJumping = false;
    public bool isFlipped = false;
    public bool isDamaged = false;
    public bool isFalling = false;
    public bool isAttacking = false;
    public bool isKicking = false;
    public bool isFiring = false;
    public bool invisible = false;
    //********

    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WARDEN");
        rb = GetComponent<Rigidbody2D>();
        animations = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
        manager = GameObject.Find("GAME MANAGER");
        gm = manager.GetComponent<Manager>();
        randomMax = 2;
        randomMin = 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (rb.velocity.x == 0 && isJumping == false && isDamaged == false && isFalling == false && isKicking == false && isAttacking == false && isFiring == false)
        {
            animations.Play("FireIdle");
        }

        if (rb.velocity.y < 0 && isKicking == false && isDamaged == false)
        {
            animations.Play("FireFall");
        }
            
        LookPlayer();

        Timer();

        if (createFire)
        {

            FireUltimate();
        }

        AutoAttacks();

        if (difficulty != 1)
            HealthCheck();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 11)
        {
            isJumping = false;
            isKicking = false;
            isFalling = false;
            print("toque el piso");
            gotDirection = false;
            jumpCounter = 0;
            onAir = false;
            gettingRandom = false;
        }

        if (collision.gameObject.layer == 10)
        {
            isKicking = false;
            gotDirection = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10 || collision.gameObject.layer == 11)
        {
            rb.velocity = new Vector2(0, 0);
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
            trans.rotation = new Quaternion(0,0,0,0);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            gm.BossDamaged();
            isDamaged = true;
        }

        /*if (collision.gameObject.layer == 10)
        {
            print("estoy en la cabeza");
            onAir = true;
            isJumping = false;
            rb.AddForce(transform.up * 50);
            if (isFlipped == true)
                transform.position += new Vector3(0.5f, 0, 0);

            else transform.position -= new Vector3(0.5f, 0, 0);
        }*/
    }

    //MOVIMIENTOS / ORIENTACION
    void LookPlayer()
    {
        if (transform.position.x > player.transform.position.x)
        {
            spRenderer.flipX = true;
            isFlipped = true;
        }
        
        if (transform.position.x < player.transform.position.x)
        {
            spRenderer.flipX = false;
            isFlipped = false;
        }
    }

    void Jump()
    {
        if (isJumping == false && jumpCounter < 1 && onAir == false)
        {
            if (isFlipped == false)
            {   
                rb.AddForce(transform.up * jumpForce);
                
            }
            else if (isFlipped == true)
            {
                rb.AddForce(transform.up * jumpForce);
                
            }
            audioSource.clip = jumpSound;
            audioSource.Play();
            animations.Play("FireJump");
            isJumping = true;
            isFalling = true;
            onAir = true;
            jumpCounter++;
        }
    }

    void Attack()
    {
        if (isAttacking == false && isJumping == false && isFalling == false && onAir == false)
        {
            isAttacking = true;
            animations.Play("FireAttack");
            audioSource.clip = attackSound;
            audioSource.Play();
            FireBasicAttack normalAttack = Instantiate(basicAttackPrefab);
        }
    }

    void AirAttack()
    {
        if (isAttacking == false)
        {
            isAttacking = true;
            animations.Play("FireAttack");
            audioSource.clip = attackSound;
            audioSource.Play();
            FireAirAttack airAttack = Instantiate(airAttackPrefab);
        }    
    }



    void FireKick()
    {
        if (isAttacking == false)
        {
            isAttacking = true;
            animations.Play("FireKick");
            
            if (gotDirection == false)
            {
                audioSource.clip = kickSound;
                audioSource.Play();
                direction = transform.position - player.transform.position;
                gotDirection = true;
            }


            if (isFlipped)
            {
                transform.position -= direction.normalized * kickSpeed * Time.deltaTime;
                print("Me movi a la izquierda");
                isAttacking = false;
            }

            if (isFlipped == false)
            {
                transform.position -= direction.normalized * kickSpeed * Time.deltaTime;
                print("Me movi a la derecha");
                isAttacking = false;
            }
        }
        
    }

    void FireUltimate()
    {
        if (onAir == false && isFalling == false)
        {
            isAttacking = true;
            animations.Play("FireUltimate");
            audioSource.clip = ultimateSound;
            audioSource.Play();
            if (fireTimer < 0.1f)
            {
                fireTimer += Time.deltaTime;
            }

            if (fireTimer >= 0.1f)
            {
                FireMultiple multipleAttack = Instantiate(multiplePrefab);
                fireCounter++;
                fireTimer = 0;
            }

            if (fireCounter >= createdFires)
            {
                fireCounter = 0;
                createFire = false;
                ultimateAttackTimer = 0;
            }

        }     
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

        if (attackTime >= 0.5f)
        {
            attackTime = 0;
            isAttacking = false;
        }

        if (onAir && gettingRandom == false)
        {
            if (difficulty == 1)
            {
                allowKick = true;
            }
            else
            {
                gettingRandom = true;
                gotRandom = Random.Range(randomMin, randomMax);
                if (gotRandom <= 1.5f)
                {
                    allowKick = true;
                }
                else if (isAngry)
                {
                    allowAirAttack = true;
                }
            }
            isJumping = false;
        }

        if (allowKick && onAir)
        {
            kickTimer += Time.deltaTime;
        }

        if(kickTimer >= 0.5)
        {
            isKicking = true;
            kickTimer = 0;
        }

        if (allowAirAttack && onAir)
        {
            airTimer += Time.deltaTime;
        }

        if (airTimer >= 0.5)
        {
            isFiring = true;
            airTimer = 0;
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

    void AutoAttacks()
    {
        if (isAttacking == false && isDamaged == false)
        {
            basicAttackTimer += Time.deltaTime;
            jumpTimer += Time.deltaTime;

            if (enraged)
            {
                ultimateAttackTimer += Time.deltaTime;
            }
        }

        if (basicAttackTimer >= basicAttackValue)
        {
            Attack();
            basicAttackTimer = 0;
        }

        if (jumpTimer >= jumpValue)
        {
            Jump();
            jumpTimer = 0;
        }

        if (isKicking)
        {
            FireKick();
            allowKick = false;
        }

        if (isFiring)
        {
            AirAttack();
            allowAirAttack = false;
            isFiring = false;
        }

        if (ultimateAttackTimer >= ultimateAttackValue)
        {
            createFire = true;
        }
    }

    public void HealthCheck()
    {
        if ((difficulty == 2 || difficulty == 3) && (health / maxHealth) <= 0.75f && isAngry == false)
        {
            isAngry = true;
            AngryAttacks();
            if (enraged == false)
            {
                spRenderer.color = new Color(255, 255, 0);
            }
        }

        if (difficulty == 3 && (health / maxHealth) <= 0.5f && enraged == false)
        {
            enraged = true;
            EnragedAttacks();
            spRenderer.color = new Color(255, 0, 0);
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

        else if (receivedElement == "Earth")
        {
            resistance = 1f;
            spRenderer.enabled = false;
            invisible = true;
        }

        else if (receivedElement == "Water")
        {
            resistance = 2;
            if (isFlipped)
            {
                animations.Play("FireTakeDamage");
                transform.position = transform.position + damageRecoil;
            }
            else
            {
                spRenderer.flipX = false;
                animations.Play("FireTakeDamage");
                transform.position = transform.position - damageRecoil;
            }
            audioSource.clip = takeDamageSound;
            audioSource.Play();
        }
        health = health - (damageTaken * resistance);
    }

    public void AngryAttacks()
    {
        basicAttackValue = basicAttackValue / 1.5f;
        jumpValue = jumpValue / 1.5f;
        damage = damage * 3;
    }

    public void EnragedAttacks()
    {
        basicAttackValue = basicAttackValue / 2f;
        jumpValue = jumpValue / 2f;
        damage = damage * 2;
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
