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
        moveCharacter();
        rotateCharacter(movement.normalized);
        
        main.transform.position = new Vector3(myTransform.position.x, myTransform.position.y, -10);
    }

    //uses axis to control what direction the player is going, and then moves the character
    //in that direction
    void moveCharacter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        myRigidbody.velocity = movement.normalized * speed;
    }

    //figures out the general direction that the player is going, and rotates it to face that direction
    //currently this works by literally changing the rotation, but ultimately it should send a number
    //to the animator, which will choose which sprite animation to run
    private void rotateCharacter(Vector2 movement)
    {

        if (movement == Vector2.zero)
        {
            return;
        }
        float xDirection = movement.x;
        float yDirection = movement.y;

        float myRotation = 0;

        if (xDirection > 0)//if x is positive, we are moving to the right
        {
            myRotation = 270;

            if (yDirection > 0)//if y is also positive, we want to go right and up
            {
                myRotation = 315;
            }
            else if (yDirection < 0)//if y is negative, we want to right and down
            {
                myRotation = 225;
            }


        }
        else if (xDirection < 0)//if x is negative, we want to go left
        {
            myRotation = 90;

            if (yDirection > 0)//if y is positive, we want to go left and up
            {
                myRotation = 45;
            }
            else if (yDirection < 0) //if y is negative, we want to go left and down
            {
                myRotation = 135;
            }

        }
        else //if x is 0, we want to only use the y value
        {
            if (yDirection > 0) //if y is positive, we want to move up
            {
                myRotation = 0;
            } 
            else//if y is negative, we want to move down
            {
                myRotation = 180;
            }
        }
        

        // Debug.Log($"Rotation should be {myRotation}");
        
        
        //this is a temporary solution to rotate the object, ultimately what we want to do is send a 
        //number to the animator in order to choose the correct sprite animator
        Vector3 newRotation = new Vector3(0, 0, myRotation);
        myTransform.rotation = Quaternion.Euler(newRotation);
        
    }
    
}
