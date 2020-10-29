using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWhipCollider : MonoBehaviour
{
    public GameObject earthBody;
    public EarthElementBody body;

    // Start is called before the first frame update
    void Start()
    {
        earthBody = GameObject.Find("Elemental");
        body = earthBody.GetComponent<EarthElementBody>();
    }

    // Update is called once per frame
    void Update()
    {
        Position();
    }

    void Position()
    {
        if (body.secondWhip == false)
        {
            if (body.isFlipped == true)
            {
                transform.position = earthBody.transform.position + new Vector3(0.7f, -0.05f, 0);
            }

            if (body.isFlipped == false)
            {
                transform.position = earthBody.transform.position - new Vector3(1.4f, 0.05f, 0);
            }
        }
    }
}
