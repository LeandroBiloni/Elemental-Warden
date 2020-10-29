using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMultiple : MonoBehaviour
{
    public GameObject source;
    FireElementBody body;
    public SpriteRenderer spRender;
    public GameObject manager;
    public Manager gm;
    public AudioSource audioSource;
    public AudioClip collisionSound;
    public Collider2D coll;
    public float speed;
    public int verticalMove;
    public int counter;
    public string thisElement = "Fire";
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("Elemental");
        body = source.GetComponent<FireElementBody>();
        spRender = GetComponent<SpriteRenderer>();
        manager = GameObject.Find("GAME MANAGER");
        gm = manager.GetComponent<Manager>();
        verticalMove = 1;
        counter = 0;
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
        if (body.spRenderer.flipX == true)
        {
            transform.position = source.transform.position - new Vector3(1, 0, 0);
        }

        if (body.spRenderer.flipX == false)
        {
            transform.position = source.transform.position + new Vector3(1, 0, 0);
        }
    }

    void Move()
    {
        if (spRender.flipX == true)
        {
            transform.position += new Vector3(1, verticalMove, 0) * speed * Time.deltaTime;
            print("Me movi a la izquierda");
            counter++;
        }

        if (spRender.flipX == false)
        {
            transform.position -= new Vector3(1, verticalMove, 0) * speed * Time.deltaTime;
            print("Me movi a la derecha");
            counter++;
        }

        if (counter == 5 && verticalMove == 1)
        {
            verticalMove = -1;
            counter = 0;
        }

        if (counter == 5 && verticalMove == -1)
        {
            verticalMove = 1;
            counter = 0;
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
