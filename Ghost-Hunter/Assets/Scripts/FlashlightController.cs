using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightController : MonoBehaviour
{
    //movement code is from a Brackeys video
    public Camera cam;
    Vector2 mousePos;

    //might want to eventually have flashlight in seperate controller
    public Light2D light;
    private bool flashlightOn = true;
    private float battery = 100f;
    public float depleteRate = 1f;
    public float rechargeRate = 5f;

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0)){
            ToggleFlashlight();
        }
    }

    private void FixedUpdate()
    {   
        Vector2 position2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 lookDir = mousePos - position2D;
        float angle = Mathf.Atan2(lookDir.y ,lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

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
        flashlightOn = !flashlightOn;
        light.enabled = flashlightOn;
    }
}
