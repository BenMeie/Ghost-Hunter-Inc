using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Vector2 movement;
    private Camera main;
    

    public float speed = 2f;
    
    //Animations
    private Animator movementAnimation;
    private SpriteRenderer spriteObject;
    private bool[] directions;
    [Header("Sprites")]
    public Sprite idle_down;
    public Sprite idle_up;
    public Sprite idle_left;
    public Sprite idle_right;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        main = Camera.main;
        movementAnimation = GetComponent<Animator>();
        spriteObject = GetComponent<SpriteRenderer>();
        directions = new []{true, false, false, false};
    }

    // Update is called once per frame
    void Update()
    {
        bool horizontalButtonPressed = Input.GetButton("Horizontal");
        bool verticalButtonPressed = Input.GetButton("Vertical");
        
        main.transform.position = new Vector3(myTransform.position.x, myTransform.position.y, -10);
        //code inspired by a Brackeys video
        moveCharacter();
        rotateCharacter(movement.normalized);

        if (myRigidbody.velocity.magnitude == 0)
        {
            movementAnimation.enabled = false;
            switch (Array.IndexOf(directions, true))
            {
                case 0:
                    spriteObject.sprite = idle_down;
                    break;
                case 1:
                    spriteObject.sprite = idle_up;
                    break;
                case 2:
                    spriteObject.sprite = idle_left;
                    break;
                case 3:
                    spriteObject.sprite = idle_right;
                    break;
            }
        }
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
        float xDirection = movement.x;
        float yDirection = movement.y;

        if (xDirection > 0)//if x is positive, we are moving to the right
        {
            movementAnimation.enabled = true;
            movementAnimation.Play("Right");
            directions = new []{false, false, false, true};
        }
        else if (xDirection < 0)//if x is negative, we want to go left
        {
            movementAnimation.enabled = true;
            movementAnimation.Play("Left");
            directions = new []{false, false, true, false};
        }
        else //if x is 0, we want to only use the y value
        {
            if (yDirection > 0) //if y is positive, we want to move up
            {
                movementAnimation.enabled = true;
                movementAnimation.Play("Up");
                directions = new []{false, true, false, false};
            } 
            else//if y is negative, we want to move down
            {
                if (yDirection < 0)
                {
                    movementAnimation.enabled = true;
                    movementAnimation.Play("Down");
                    directions = new []{true, false, false, false};
                }
            }
        }
    }
    
}
