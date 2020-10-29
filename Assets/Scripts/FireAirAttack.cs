using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAirAttack : MonoBehaviour
{
    public GameObject source;
    public GameObject target;
    public GameObject manager;
    public Manager gm;
    public AudioSource audioSource;
    public AudioClip collisionSound;
    public Collider2D coll;
    FireElementBody body;
    Vector3 direction;
    public float speed;
    SpriteRenderer spRender;
    public string thisElement = "Fire";

    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("Elemental");
        body = source.GetComponent<FireElementBody>();
        target = GameObject.Find("WARDEN");
        spRender = GetComponent<SpriteRenderer>();
        manager = GameObject.Find("GAME MANAGER");
        gm = manager.GetComponent<Manager>();
        coll = GetComponent<Collider2D>();
        Flip();
        Position();
        Direction();
        audioSource.clip = collisionSound;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Direction()
    {
        direction = target.transform.position - transform.position;
    }

    void Move()
    {
        if (spRender.flipX == false)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            print("Me movi a la izquierda");
        }

        if (spRender.flipX)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            print("Me movi a la derecha");
        }
    }

    void Flip() //DETERMINA PARA DONDE VA A MIRAR EL SPRITE DEL ATAQUE
    {
        if (body.isFlipped == false)
        {
            spRender.flipX = true;
            print("flip true");
        }

        if (body.isFlipped == true)
        {
            spRender.flipX = false;
            print("flip fallse");
        }
    }

    void Position()
    {
        if (spRender.flipX == true)
        {
            transform.position = source.transform.position + new Vector3(1, 0, 0);
        }

        if (spRender.flipX == false)
        {
            transform.position = source.transform.position - new Vector3(1, 0, 0);
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
