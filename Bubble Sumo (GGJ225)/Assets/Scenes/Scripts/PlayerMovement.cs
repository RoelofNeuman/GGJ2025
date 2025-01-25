using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private float movement = 0f;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        if(movement > 0f)
        {
            body.linearVelocity = new Vector2 (movement * speed, body.linearVelocity.y);

        }
        else if(movement < 0f)
        {
            body.linearVelocity = new Vector2(movement * speed, body.linearVelocity.y);
        }
        else
        {
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);
        }
        
    }
}
