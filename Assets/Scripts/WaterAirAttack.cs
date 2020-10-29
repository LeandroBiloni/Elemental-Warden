using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAirAttack : MonoBehaviour
{
    public GameObject source;
    WaterElementBody body;
    SpriteRenderer spRender;
    public Collider2D coll;
    public GameObject manager;
    public Manager gm;
    public AudioSource audioSource;
    public AudioClip collisionSound;
    public string thisElement = "Water";
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("Elemental");
        body = source.GetComponent<WaterElementBody>();
        manager = GameObject.Find("GAME MANAGER");
        gm = manager.GetComponent<Manager>();
        Position();
        audioSource.clip = collisionSound;
        spRender = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    void Position()
    {
        transform.position = body.transform.position - new Vector3(0, 0.5f, 0);
    }
}
