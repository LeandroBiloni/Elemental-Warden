using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthElementBody : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animations;
    public GameObject player;
    public SpriteRenderer spRenderer;
    public BoxCollider2D collide;
    public WaterBasicAttack basicAttackPrefab;
    public WaterAirAttack airAttackPrefab;
    public GameObject warden;
    public WardenBody wardenBody;
    public GameObject floorObjectA;
    public GameObject floorObjectB;
    public GameObject ceilingObjectA;
    public GameObject ceilingObjectB;
    public GameObject whipCollider;

    public GameObject manager;
    public Manager gm;
    public int difficulty;

    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip takeDamageSound;
    public AudioClip dissapearSound;
    public AudioClip appearSound;

    public float health;
    public float maxHealth;
    public float damage;
    public float resistance;
    public float hpMultiplier;
    public float dmgMultiplier;
    public string elementType = "Earth";

    public bool isFlipped = false;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool onAir = false;
    public bool onCeiling = false;
    public bool isDamaged = false;
    public bool isAttacking = false;
    public bool isAngry = false;
    public bool canFlip = true;
    public bool usingWhip = false;
    public bool secondWhip = false;
    public bool canJump = true;
    public bool canTeleport = false;
    public bool isTeleporting = false;
    public bool invisible = false;

    public float distance;
    Vector3 damageRecoil = new Vector2(0.1f, 0);

    public float attackTime = 0;
    public float damageTime = 0;
    public float whipAttackTimer = 0;
    public float whipAttackValue;
    public float jumpTimer = 0;
    public float jumpValue;
    public float attackTimer = 0;
    public float cdReset;
    public float whipTime = 0;
    public float secondAttackTime = 0;
    public float randomTeleport;
    public float teleportTimer = 0;
    public float teleportExitTimer = 0;
    public float invisibleTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WARDEN");
        rb = GetComponent<Rigidbody2D>();
        animations = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        collide = GetComponent<BoxCollider2D>();
        warden = GameObject.Find("WARDEN");
        wardenBody = warden.GetComponent<WardenBody>();
        manager = GameObject.Find("GAME MANAGER");
        gm = manager.GetComponent<Manager>();
        floorObjectA = GameObject.Find("FloorTransportA");
        floorObjectB = GameObject.Find("FloorTransportB");
        ceilingObjectA = GameObject.Find("CeilingTransportA");
        ceilingObjectB = GameObject.Find("CeilingTransportB");
        whipCollider = GameObject.Find("EarthWhipCollider");
        whipCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();

        LookPlayer();

        //distance = Mathf.Abs(transform.position.x - wardenBody.transform.position.x);

        Timer();

        AutoAttacks();

        if (isJumping == false && isDamaged == false && isFalling == false && isAttacking == false && secondWhip == false)
        {
            animations.Play("EarthIdle");
            whipCollider.SetActive(false);
        }

        if (difficulty != 1)
            HealthCheck();
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
            isTeleporting = false;
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
        if (collision.gameObject.layer == 12)
        {
            gm.BossDamaged();
            isDamaged = true;
        }

        /*if (collision.gameObject.layer == 10)
        {
            rb.AddForce(transform.up * 100);

            if (isFlipped == false)
                rb.AddForce(transform.right * 100);

            else rb.AddForce(transform.right * -100);
        }*/
    }

    private void OnTriggerExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 16)
        {
            rb.velocity = new Vector3 (0,0,0);
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            onCeiling = true;
        }
    }

    void LookPlayer()
    {
        if (transform.position.x > player.transform.position.x && canFlip == true)
        {
            spRenderer.flipX = false;
            isFlipped = false;
        }

        if (transform.position.x < player.transform.position.x && canFlip == true)
        {
            spRenderer.flipX = true;
            isFlipped = true;
        }
    }

    public void WhipAttack()
    {
        if (spRenderer.flipX == false)
        {
            spRenderer.flipX = true;
        }
        else spRenderer.flipX = false;

        animations.Play("EarthWhip");
        audioSource.clip = attackSound;
        audioSource.Play();
        usingWhip = true;
        isAttacking = true;
        canFlip = false;
    }

    public void SecondAttack()
    {
        if (spRenderer.flipX == true)
        {
            spRenderer.flipX = true;
        }
        else spRenderer.flipX = false;

        animations.Play("EarthMidAttack");
        audioSource.clip = attackSound;
        audioSource.Play();
        secondWhip = true;
        canJump = false;
    }

    void DiagonalJump()
    {
        if (isJumping == false)
        {
            if (isFlipped == false)
            {
                rb.AddForce(transform.up * 250);
                rb.AddForce(transform.right * -100);
                animations.Play("EarthJump");
                isJumping = true;
                isFalling = true;
                onAir = true;
            }
            else if (isFlipped == true)
            {
                rb.AddForce(transform.up * 250);
                rb.AddForce(transform.right * 100);
                animations.Play("EarthJump");
                isJumping = true;
                isFalling = true;
                onAir = true;
            }
            audioSource.clip = jumpSound;
            audioSource.Play();
        }
    }

    void Teleport()
    {
        isTeleporting = true;
        rb.gravityScale = 0;
        randomTeleport = Random.Range(0, 4);

        if (0 <= randomTeleport && randomTeleport < 1)
        {
            randomTeleport = 0;
        }
        else if (1 <= randomTeleport && randomTeleport < 2)
        {
            randomTeleport = 1;
        }
        else if (2 <= randomTeleport && randomTeleport < 3)
        {
            randomTeleport = 2;
        }
        else if (3 <= randomTeleport && randomTeleport <= 4)
        {
             randomTeleport = 3;
        }

        switch (randomTeleport)
        {
            case 0:
                transform.position = floorObjectA.transform.position;
                break;

            case 1:
                transform.position = floorObjectB.transform.position;
                break;
            case 2:
                transform.position = ceilingObjectA.transform.position;
                break;
            case 3:
                transform.position = ceilingObjectB.transform.position;
                break;
        }
        animations.Play("EarthTeleportInverse");
        audioSource.clip = appearSound;
        audioSource.Play();
        canTeleport = false;
    }

    public void DamageCalculator(float damageTaken, string receivedElement)
    {
        if (receivedElement == elementType)
        {
            resistance = 0.5f;
            spRenderer.enabled = false;
            invisible = true;
        }

        else if (receivedElement == "Water")
        {
            resistance = 1f;
            spRenderer.enabled = false;
            invisible = true;
        }

        else if (receivedElement == "Fire")
        {
            resistance = 2;
            if (isFlipped == false)
            {
                animations.Play("EarthTakeDamage");
                transform.position = transform.position + damageRecoil;
            }
            else
            {
                spRenderer.flipX = true;
                animations.Play("EarthTakeDamage");
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

        if (damageTime >= 1) //TIEMPO PARA ANIMACION DAÑO Y VOLVER A RECIBIR DAÑO
        {
            isDamaged = false;
            damageTime = 0;
        }

        if (attackTimer < cdReset && isAttacking)  
        {
            attackTimer += Time.deltaTime;
        }

        if (attackTimer >= cdReset)  //AVISA QUE TERMINE DE ATACAR
        {
            attackTimer = 0;
            isAttacking = false;
        }

        if (usingWhip == true)
        {
            whipTime += Time.deltaTime;
        }

        if (secondWhip)
        {
            secondAttackTime += Time.deltaTime;
        }

        if (secondAttackTime >= 0.3f)
        {
            secondAttackTime = 0;
            secondWhip = false;
            canFlip = true;
            canJump = true;
        }

        if (whipTime >= 0.6f)
        {
            if (isDamaged == false)
            {
                whipCollider.SetActive(true);
            }
        }

        if (whipTime >= 0.7f)
        {
            usingWhip = false;
            whipTime = 0;
            whipCollider.SetActive(false);
            if (isAngry && secondWhip == false)
            {
                SecondAttack();
            }
            else
            {
                canFlip = true;
                canJump = true;
            }
                
        }

        if (canTeleport)
        {
            teleportTimer += Time.deltaTime;
        }

        if (isTeleporting)
        {
            teleportExitTimer += Time.deltaTime;
        }

        if (teleportExitTimer >= 0.6f)
        {
            teleportExitTimer = 0;
            rb.gravityScale = 1;
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

    void CheckDistance()
    {
        distance = Mathf.Abs(transform.position.x - wardenBody.transform.position.x);

        if (isAngry && distance < 1.5f && isDamaged == false && isAttacking == false && secondWhip == false && onAir == false)
        {
            animations.Play("EarthTeleport");
            audioSource.clip = dissapearSound;
            audioSource.Play();
            canTeleport = true;
        }
    }
    public void HealthCheck()
    {
        if ((difficulty == 2 || difficulty == 3) && (health / maxHealth) <= 0.5f && isAngry == false)
        {
            isAngry = true;
            AngryAttacks();
            spRenderer.color = new Color(255, 255, 0);
        }
    }

    public void AngryAttacks()
    {
        whipAttackValue = whipAttackValue / 1.5f;
        jumpValue = jumpValue / 1.5f;
        damage = damage * 3;
    }

    void AutoAttacks()
    {
        if (isAttacking == false && isDamaged == false)
        {
            whipAttackTimer += Time.deltaTime;
            jumpTimer += Time.deltaTime;
        }

        if (whipAttackTimer >= whipAttackValue && isDamaged == false && canTeleport == false && isTeleporting == false)
        {
            WhipAttack();
            whipAttackTimer = 0;
            canJump = false;
        }

        if (jumpTimer >= jumpValue && canJump && isDamaged == false && canTeleport == false && isTeleporting == false)
        {
            DiagonalJump();
            jumpTimer = 0;
        }

        if (isAngry && distance < 1.5f && isDamaged == false && isAttacking == false && secondWhip == false && onAir == false)
        {
            animations.Play("EarthTeleport");
            canTeleport = true;
            isAttacking = true;
            onAir = true;
        }

        if (teleportTimer >= 0.6f)
        {
            teleportTimer = 0;
            Teleport();
            canJump = false;
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



