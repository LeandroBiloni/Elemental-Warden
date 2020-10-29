using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBasicAttack : MonoBehaviour
{
    public GameObject source;
    public WaterElementBody body;
    public SpriteRenderer spRender;
    public Rigidbody2D rb;
    public GameObject manager;
    public Manager gm;
    public AudioSource audioSource;
    public AudioClip collisionSound;
    public Collider2D coll;
    public float speed;
    public string thisElement = "Water";
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("Elemental");
        body = source.GetComponent<WaterElementBody>();
        spRender = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        manager = GameObject.Find("GAME MANAGER");
        gm = manager.GetComponent<Manager>();
        Flip();
        Position();
        audioSource.clip = collisionSound;
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (spRender.flipX == false)
        {
            rb.velocity = new Vector2(-speed, 0);
        }
        else rb.velocity = new Vector2(speed, 0);
    }

    void Flip() //DETERMINA PARA DONDE VA A MIRAR EL SPRITE DEL ATAQUE
    {
        if (body.isFlipped == false)
        {
            spRender.flipX = false;
        }

        if (body.isFlipped == true)
        {
            spRender.flipX = true;
        }
    }

    void Position()
    {
        if (body.spRenderer.flipX == true)
        {
            transform.position = body.transform.position + new Vector3(0.3f, 0.3f, 0);
        }

        if (body.spRenderer.flipX == false)
        {
            transform.position = body.transform.position - new Vector3(0.3f, -0.3f, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 8 || collision.gameObject.layer == 10 || collision.gameObject.layer == 9)
        {
            audioSource.Play();
            spRender.enabled = false;
            coll.enabled = false;
            Invoke("Destroy", 0.3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 8 || collision.gameObject.layer == 10 || collision.gameObject.layer == 9)
        {
            audioSource.Play();
            spRender.enabled = false;
            coll.enabled = false;
            Invoke("Destroy", 0.3f);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
