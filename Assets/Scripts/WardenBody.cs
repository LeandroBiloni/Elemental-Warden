using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenBody : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spRenderer;
    public GameObject swordHitbox;
    public GameObject manager;
    public Manager gm;
    Animator animations;

    //PROPIEDADES / STATS
    public float maxHealth = 100;
    public float health;
    public float speed;
    public float jumpForce;
    public float damage;
    public int activeElement;
    public string elementType;
    public float receivedDamageMultiplier;

    //SONIDO
    AudioSource audioSource;
    public AudioClip runSound;
    public AudioClip screamSound;
    public AudioClip attackSound;
    public AudioClip takeDamageSound;
    public AudioClip changeElementSound;
    public bool doRunSound = true;
    public float runSoundTimer = 0;

    //TIMERS ATAQUES / DAÑO
    float attackTimer = 0;
    float cdReset = 0.5f;
    public float damageTime = 0;
    public float cooldown;

    //OTROS
    Vector3 holdPosition;
    Vector3 damageRecoil = new Vector2(0.2f, 0);
    int isJumping = 0;
    public bool isAttacking = false;
    public bool isDamaged = false;
    public bool isFlipped = false;
    public bool canAttack = true;
    public float horizontalForce;
    bool godMode = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animations = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        swordHitbox = GameObject.Find("SwordCollider");
        swordHitbox.SetActive(false);
        manager = GameObject.Find("GAME MANAGER");
        gm = manager.GetComponent<Manager>();
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
        activeElement = 1;
        elementType = "Fire";
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y < 0 && isAttacking == false && isDamaged == false)
        {
            animations.Play("Fall");
        }

        Timers();

        if (rb.velocity.x == 0 && isAttacking == false && isJumping == 0 && isDamaged == false)
        {
            animations.Play("Idle");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isJumping = 0;
        }

        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 13 || collision.gameObject.layer == 14) //RECIBO DAÑO
        {
            if (isFlipped == false)
            {
                animations.Play("TakeDamage");
                transform.position = transform.position - damageRecoil;
            }
            else if (isFlipped == true)
            {
                spRenderer.flipX = true;
                animations.Play("TakeDamage");
                transform.position = transform.position + damageRecoil;
            }
            audioSource.clip = takeDamageSound;
            audioSource.Play();
            gm.PlayerDamaged();
            isDamaged = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 13 || collision.gameObject.layer == 14) //RECIBO DAÑO
        {
            if (isFlipped == false)
            {
                animations.Play("TakeDamage");
                transform.position = transform.position - damageRecoil;
            }
            else if (isFlipped == true)
            {
                spRenderer.flipX = true;
                animations.Play("TakeDamage");
                transform.position = transform.position + damageRecoil;
            }
            audioSource.clip = takeDamageSound;
            audioSource.Play();
            gm.PlayerDamaged();
            isDamaged = true;
        }
    }

    //MOVIMIENTOS / POSICION
    public void Move(float horizontal)
    {
        if (isDamaged == false)
        {
            if (isAttacking == true) //ME QUEDO EN EL LUGAR SI ATACO
            {
                rb.velocity = new Vector2(0, 0);
            }
            else if (isJumping > 0) //ME MUEVO EN EL AIRE
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
            else if ((isAttacking == false) && isJumping == 0) //CORRER EN EL PISO
            {
                rb.velocity = new Vector2(horizontal * speed, 0);

                if (horizontal != 0)
                {
                    animations.Play("Run");

                    if (doRunSound)
                    {
                        doRunSound = false;
                        audioSource.clip = runSound;
                        audioSource.Play();
                    } 
                }
                //animations.SetFloat("Speed", Mathf.Abs(horizontal));
            }
        }  
    }

    public void Flip(char key) //CAMBIA LA ROTACION PARA PODER GIRAR TAMBIEN EL COLLIDER
    {
        if ((key == 'D' || key == 'd') && isAttacking == false && isDamaged == false)
        {
            spRenderer.flipX = false;
            isFlipped = false;
        }

        if ((key == 'A' || key == 'a') && isAttacking == false && isDamaged == false)
        {
            spRenderer.flipX = true;
            isFlipped = true;
        }
    }

    public void Jump()
    {
        if (isJumping < 2 && isDamaged == false) //HASTA 2 SALTOS
        {
            audioSource.clip = screamSound;
            audioSource.Play();
            rb.AddForce(transform.up * jumpForce);
            animations.Play("Jump");
            isJumping++;
        }
    }

    public void ChangeElement(char key)
    {
        if (key == 'E' || key == 'e')
        {
            NextElement();
        }

        if (key == 'Q' || key == 'q')
        {
            PreviousElement();
        }

        audioSource.clip = changeElementSound;
        audioSource.Play();
        print(activeElement);
        Element();
    }

    void NextElement()
    {
        activeElement++;

        if (activeElement > 3)
        {
            activeElement = 1;
        }
    }

    void PreviousElement()
    {
        activeElement--;

        if (activeElement < 1)
        {
            activeElement = 3;
        }
    }

    private void Element()
    {
        switch (activeElement)
        {
            case 1:
                spRenderer.color = new Color(255f, 255f, 255f);
                print("Estoy de fuego");
                elementType = "Fire";
                break;

            case 2:
                spRenderer.color = Color.blue;
                print("Estoy de agua");
                elementType = "Water";
                break;

            case 3:
                spRenderer.color = Color.yellow;
                print("Estoy de tierra");
                elementType = "Earth";
                break;
        }
    }


    //ATAQUES
    public void Attack()
    {
        if (canAttack)
        {
            audioSource.clip = attackSound;
            audioSource.Play();
            if (isAttacking == false && isJumping == 0 && isDamaged == false) //ATAQUE NORMAL
            {
                swordHitbox.SetActive(true);
                isAttacking = true;
                animations.Play("Attack");
                canAttack = false;
            }
            else if (isJumping > 0 && isDamaged == false) //ATAQUE AEREO
            {
                animations.Play("AirAttack");
                swordHitbox.SetActive(true);
                isAttacking = true;
                canAttack = false;
            }
        }
    }

    public void Timers()
    {
        if (canAttack == false)
        {
            cooldown += Time.deltaTime;
        }

        if (cooldown >= 1f)
        {
            canAttack = true;
            cooldown = 0;
        }

        if (attackTimer < cdReset && isAttacking)  //AVISA QUE TERMINE DE ATACAR
        {
            attackTimer += Time.deltaTime;
        }

        if (attackTimer >= cdReset)
        {
            swordHitbox.SetActive(false);
            attackTimer = 0;
            isAttacking = false;
        }

        if (isDamaged) //TIEMPO DE ANIMACION DE DAÑO
        {
            damageTime += Time.deltaTime;
        }

        if (damageTime >= 0.6) //AVISA QUE TERMINE DE RECIBIR DAÑO
        {
            isDamaged = false;
            damageTime = 0;
        }

        if (doRunSound == false)
        {
            runSoundTimer += Time.deltaTime;
        }

        if (runSoundTimer >= 0.3f)
        {
            doRunSound = true;
            runSoundTimer = 0;
        }
    }

    public void DamageCalculator(float damageTaken, string receivedElement)
    {
        if (receivedElement == elementType)
        {
            receivedDamageMultiplier = 0.5f;
        }

        else if ((receivedElement == "Fire" && elementType == "Water") || (receivedElement == "Water" && elementType == "Earth") || (receivedElement == "Earth" && elementType == "Fire"))
        {
            receivedDamageMultiplier = 1f;
        }

        else if ((receivedElement == "Fire" && elementType == "Earth") || (receivedElement == "Water" && elementType == "Fire") || (receivedElement == "Earth" && elementType == "Water"))
        {
            receivedDamageMultiplier = 2;
        }
        health = health - (damageTaken * receivedDamageMultiplier);
        print("recibi daño");
    }

    public void PseudoGodMode()
    {
        if(godMode == false)
        {
            health = 99999;
            damage = 20;
            godMode = true;
        }
        else
        {
            health = 100;
            damage = 5;
            godMode = false;
        }

    }
}
