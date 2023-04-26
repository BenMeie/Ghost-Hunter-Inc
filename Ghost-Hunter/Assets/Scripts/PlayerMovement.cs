using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Transform myTransform;
    private Rigidbody2D myRigidbody;
    private Vector2 movement;
    private Camera main;
    

    public float speed = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        bool horizontalButtonPressed = Input.GetButton("Horizontal");
        bool verticalButtonPressed = Input.GetButton("Vertical");

        //code inspired by a Brackeys video
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        myRigidbody.velocity = movement.normalized * speed;
        rotateCharacter(movement.normalized);
        /*
        if (horizontalButtonPressed || verticalButtonPressed)
        {
            moveCharacter(horizontalButtonPressed,verticalButtonPressed);
           
        }
        else
        {
            myRigidbody.velocity = Vector2.zero;
        }

        */
        main.transform.position = new Vector3(myTransform.position.x, myTransform.position.y, -10);
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

    private void rotateCharacter(Vector2 movement)
    {

        if (movement == Vector2.zero)
        {
            return;
        }
        float xDirection = movement.x;
        float yDirection = movement.y;

        float myRotation = 0;

        if (xDirection > 0)//if x is positive, we want to start at 0 degrees
        {
            myRotation = 270;

            if (yDirection > 0)//if y is also positive, we want to go up a little
            {
                myRotation = 315;
            }
            else if (yDirection < 0)//if y is negative, we want to go down to 315 degrees
            {
                myRotation = 225;
            }


        }
        else if (xDirection < 0)//if x is negative, we want to go leftish, starting at 180 degrees
        {
            myRotation = 90;

            if (yDirection > 0)//if y is positive, we want to go left and up
            {
                myRotation = 45;
            }
            else if (yDirection < 0)
            {
                myRotation = 135;
            }

        }
        else //if x is 0, we want to only use the y value
        {
            if (yDirection > 0)
            {
                myRotation = 0;
            }
            else
            {
                myRotation = 180;
            }
        }
        

        // Debug.Log($"Rotation should be {myRotation}");
        
        //TODO figure out how to set the rotation to the value I want
        Vector3 newRotation = new Vector3(0, 0, myRotation);
        myTransform.rotation = Quaternion.Euler(newRotation);
        //myTransform.Rotate(Vector3.zero);
        //myTransform.Rotate(new Vector3(0,0,myRotation));
    }
    
}
