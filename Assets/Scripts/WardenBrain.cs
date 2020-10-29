using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenBrain : MonoBehaviour
{
    public WardenBody body;
    float horizontal;

    private void Awake()
    {
        body = this.GetComponent<WardenBody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeys();

        horizontal = Input.GetAxis("Horizontal");
    }

    public void CheckKeys()
    {
        if (horizontal != 0)
        {
            body.Move(horizontal);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.Jump();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            body.ChangeElement('Q');
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            body.ChangeElement('E');
        }
            
        if (Input.GetMouseButton(0))
        {
            body.Attack();
        }

        if (Input.GetKey(KeyCode.A))
        {
            body.Flip('A');
        }

        if (Input.GetKey(KeyCode.D))
        {
            body.Flip('D');
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            body.PseudoGodMode();
        }
    }
}
