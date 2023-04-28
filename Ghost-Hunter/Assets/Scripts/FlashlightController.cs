using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    //movement code is from a Brackeys video
    public Camera cam;
    Vector2 mousePos;

    //might want to eventually have flashlight in seperate controller
    public Light2D light;
    public float depleteRate = 1f;
    public float rechargeRate = 5f;
    public Image batteryBar;

    private bool flashlightOn = true;
    private float battery = 100f;
    private Vector2 position2D;
    private Vector2 lookDir;

    private void Start()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        position2D = new Vector2(transform.position.x, transform.position.y);
        lookDir = mousePos - position2D;
        lookDir.Normalize();
        
        StartCoroutine(checkLightCollision(lookDir));
    }

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
        position2D = new Vector2(transform.position.x, transform.position.y);
        lookDir = mousePos - position2D;
        lookDir.Normalize();
        float angle = Mathf.Atan2(lookDir.y ,lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (battery <= 0){
            ToggleFlashlight();
        }

        //can change to make recharging take a bit after or something
        if(flashlightOn)
        {
            battery -= 0.5f;
        } else if(battery < 100) {
            battery += 1f;
        }
        batteryBar.fillAmount = battery/100.0f;
    }

    void ToggleFlashlight(){
        flashlightOn = !flashlightOn;
        light.enabled = flashlightOn;
        StopCoroutine(checkLightCollision(lookDir));
        StartCoroutine(checkLightCollision(lookDir));
    }

    IEnumerator checkLightCollision(Vector2 lookDir)
    {
        while (flashlightOn)
        {
            // print("Checking for Ghost");
            if (Physics.SphereCast(transform.position, 0.5f, new Vector3(lookDir.x, lookDir.y, 0), out var hitInfo, 3.5f))
            {
                if (hitInfo.collider.gameObject.CompareTag("Ghost"))
                {
                    print("Founds Ghost");
                    hitInfo.collider.gameObject.BroadcastMessage("IncreaseAnger", 1);
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(lookDir.x, lookDir.y, 0) * 3.5f, 0.5f);
    }
}
