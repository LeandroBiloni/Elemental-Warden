using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Animator animations;
    public GameObject manager;
    public Manager gm;
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    public float distance;
    public float interactionRange;
    public bool isOpen = false;
    public bool playOpen = false;
    public bool playClose = false;
    // Start is called before the first frame update
    void Start()
    {
        animations = GetComponent<Animator>();
        manager = GameObject.Find("GAME MANAGER");
        gm = manager.GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        gm.PortalPlayerDistance();

        if (distance > interactionRange)
        {
            ClosePortal();
        }

        if (distance < interactionRange)
        {
            OpenPortal();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            gm.NextScreen();
        }
    }
    
    void ClosePortal()
    {
        if (playClose == false)
        {
            audioSource.clip = closeSound;
            audioSource.Play();
            playOpen = false;
            playClose = true;
        }
        animations.Play("Closed");
    }

    void OpenPortal()
    {
        if (playOpen == false)
        {
            audioSource.clip = openSound;
            audioSource.Play();
            playOpen = true;
            playClose = false;
        }
        animations.Play("Opened");
    }
}
