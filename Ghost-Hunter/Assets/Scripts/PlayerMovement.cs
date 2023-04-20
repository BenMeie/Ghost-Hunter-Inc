using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Transform myTransform;
    private Rigidbody2D myRigidbody;

    public float speed = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        bool horizontalButtonPressed = Input.GetButton("Horizontal");
        bool verticalButtonPressed = Input.GetButton("Vertical");

        if (horizontalButtonPressed || verticalButtonPressed)
        {
            moveCharacter(horizontalButtonPressed,verticalButtonPressed);
           
        }
        else
        {
            myRigidbody.velocity = Vector2.zero;
        }

        
    }

    void moveCharacter(bool moveHorizontal, bool moveVertical)
    {
       
        float direction;
        float rotation;// = 0f;

        if (moveHorizontal && !moveVertical)
        {

            direction = Input.GetAxis("Horizontal");

            myRigidbody.velocity = Vector2.right * (direction * speed);

            if (direction < 0)//a pressed down
            {
                rotation = 90f;
            }
            else //d pressed down
            {
                rotation = 270f;
            }
        }

        else if (moveVertical && !moveHorizontal)
        {

            direction = Input.GetAxis("Vertical");

            myRigidbody.velocity = Vector2.up * (direction * speed);
            
            if (direction < 0)//s pressed down
            {
                rotation = 180f;
            }
            else //w pressed down
            {
                rotation = 0f;
            }
        }
        else //if two buttons are pressed at the same time
        {
            return;
        }
        
        Vector3 newRotation = new Vector3(0, 0,rotation);
        myTransform.rotation = Quaternion.Euler(newRotation);
    }
    
}
