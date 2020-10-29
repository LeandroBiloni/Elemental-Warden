using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    public GameObject player;
    public WardenBody flip;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WARDEN");
        flip = player.GetComponent<WardenBody>();
    }

    // Update is called once per frame
    void Update()
    {
        Position();
    }

    void Position()
    {
        if (flip.isFlipped == false)
        {
            transform.position = player.transform.position + new Vector3(0.35f, 0, 0);
        }

        if (flip.isFlipped == true)
        {
            transform.position = player.transform.position - new Vector3(0.35f, 0, 0);
        }
    }
}
