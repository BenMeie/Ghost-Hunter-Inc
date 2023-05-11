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
    [SerializeField] private CharacterSprites Daisy;
    [SerializeField] private CharacterSprites Daniel;
    [SerializeField] private CharacterSprites Abi;
    [SerializeField] private CharacterSprites Ben;
    [SerializeField] private CharacterSprites Brielle;
    [SerializeField] private CharacterSprites Chris;
    [SerializeField] private CharacterSprites Joseph;
    [SerializeField] private CharacterSprites Noel;
    [SerializeField] private CharacterSprites Guerrero;

    private CharacterSprites character;
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        main = Camera.main;
        movementAnimation = GetComponent<Animator>();
        spriteObject = GetComponent<SpriteRenderer>();
        directions = new []{true, false, false, false};
        name = PlayerPrefs.GetString("Character");
        switch (name)
        {
            case "Daisy":
                character = Daisy;
                break;
            case "Daniel":
                character = Daniel;
                break;
            case "Abigaelle":
                character = Abi;
                break;
            case "Benjamin":
                character = Ben;
                break;
            case "Brielle":
                character = Brielle;
                break;
            case "Chris":
                character = Chris;
                break;
            case "Joseph":
                character = Joseph;
                break;
            case "Noel":
                character = Noel;
                break;
            case "Mr. Guerrero":
                character = Guerrero;
                name = "Teacher";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        //code inspired by a Brackeys video
        moveCharacter();
        rotateCharacter(movement.normalized);

        if (myRigidbody.velocity.magnitude == 0)
        {
            movementAnimation.enabled = false;
            switch (Array.IndexOf(directions, true))
            {
                case 0:
                    spriteObject.sprite = character.down;
                    break;
                case 1:
                    spriteObject.sprite = character.up;
                    break;
                case 2:
                    spriteObject.sprite = character.left;
                    break;
                case 3:
                    spriteObject.sprite = character.right;
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
            movementAnimation.Play(name + "_right");
            directions = new []{false, false, false, true};
        }
        else if (xDirection < 0)//if x is negative, we want to go left
        {
            movementAnimation.enabled = true;
            movementAnimation.Play(name + "_left");
            directions = new []{false, false, true, false};
        }
        else //if x is 0, we want to only use the y value
        {
            if (yDirection > 0) //if y is positive, we want to move up
            {
                movementAnimation.enabled = true;
                movementAnimation.Play(name + "_up");
                directions = new []{false, true, false, false};
            } 
            else//if y is negative, we want to move down
            {
                if (yDirection < 0)
                {
                    movementAnimation.enabled = true;
                    movementAnimation.Play(name + "_down");
                    directions = new []{true, false, false, false};
                }
            }
        }
    }
    
}
