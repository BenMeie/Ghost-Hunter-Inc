using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //movement code is from a Brackeys video
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;
    Vector2 movement;
    Vector2 mousePos;

    //might want to eventually have flashlight in seperate controller
    public GameObject flashlight;
    private bool flashlightOn = true;
    private float battery = 100f;
    public float depleteRate = 1f;
    public float rechargeRate = 5f;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0)){
            ToggleFlashlight();
        }
    }

    private void FixedUpdate()
    {   
        //movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y ,lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        if (battery <= 0){
            ToggleFlashlight();
        }

        //can change to make recharging take a bit after or something
        if(flashlightOn){
            battery -= 1f;
        } else {
            battery += 5f;
        }
    }

    void ToggleFlashlight(){
        Debug.Log("Toggling FLashlight");
        flashlightOn = !flashlightOn;
        flashlight.SetActive(flashlightOn);
    }
}
