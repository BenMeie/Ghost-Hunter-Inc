using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("General")]

    public GameManager gameManager;

    public Camera cam;
    Vector2 mousePos;

    private Vector2 lightPosition2D;
    private Vector2 lookDir;

    [Header("Flashlight")]
    public Light2D flashlight;
    private bool flashlightOn = true;
    private bool uvOn = false;
    private float battery = 100f;
    private float uvBattery = 50f;
    public float depleteRate = 0.5f;
    public float rechargeRate = 2f;
    public Image batteryBar;
    public Image uvBar;

    //[Header("Mementos")]
    //public float mementoCheckingDistance = 1f;
    //public GameObject[] mementos;

    private GameObject ritual;
    private Ghost ghost;

    public delegate void MementoFound(int id);

    //public static event MementoFound onMementoFound;    
    
    // Start is called before the first frame update
    void Start()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        lightPosition2D = new Vector2(flashlight.transform.position.x, flashlight.transform.position.y);
        lookDir = mousePos - lightPosition2D;
        lookDir.Normalize();
        
        StartCoroutine(CheckLightCollision(lookDir));
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0)){
            ToggleFlashlight();
        }

        if (Input.GetMouseButtonDown(1))
        {
            ToggleUVLight();
        }

        //checking if there's anything interactable where the player is looking
        if (Physics.SphereCast(transform.position, 0.5f, new Vector3(lookDir.x, lookDir.y, 0), out var hitInfo, 1f))
        {
            if (hitInfo.collider.gameObject.CompareTag("Memento"))
            {
                gameManager.ShowInteractable(hitInfo.transform.position);

                if (Input.GetKeyDown("e"))
                {
                    //might not be the best way to do this
                    Memento memento = hitInfo.collider.GetComponent<Memento>();
                    gameManager.FindMemento(memento);
                }
            }
            
            //can add check for ritual or other interactable objects here
            
        } else {
            gameManager.HideInteractable();
        }
    }

    private void FixedUpdate()
    {   
        //finding light direction facing
        lightPosition2D = new Vector2(flashlight.transform.position.x, flashlight.transform.position.y);
        lookDir = mousePos - lightPosition2D;
        lookDir.Normalize();
        float angle = Mathf.Atan2(lookDir.y ,lookDir.x) * Mathf.Rad2Deg - 90f;
        flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (battery <= 0){
            ToggleFlashlight();
        }

        if (uvBattery <= 0)
        {
            ToggleUVLight();
        }

        //can change to make recharging take a bit after or something
        if(flashlightOn){
            battery -= depleteRate;
        } else if(battery < 100) {
            battery += rechargeRate;
        }

        if (uvOn) {
            uvBattery -= depleteRate * 2;
        } else if (uvBattery < 50)
        {
            uvBattery += rechargeRate / 2.0f;
        }

        batteryBar.fillAmount = battery / 100.0f;
        uvBar.fillAmount = uvBattery / 50.0f;
    }

    void ToggleFlashlight(){
        if (uvOn)
        {
            ToggleUVLight();
        }
        flashlightOn = !flashlightOn;
        flashlight.enabled = flashlightOn;
        StopCoroutine(CheckLightCollision(lookDir));
        StartCoroutine(CheckLightCollision(lookDir));
    }

    void ToggleUVLight()
    {
        if (uvOn)
        {
            uvOn = false;
            flashlight.color = Color.white;
            flashlight.enabled = false;
            StopCoroutine(CheckLightCollision(lookDir));
        }
        else
        {
            uvOn = true;
            flashlight.color = Color.magenta;
            flashlight.enabled = true;
            flashlightOn = false;
            StartCoroutine(CheckLightCollision(lookDir));
        }
    }

    IEnumerator CheckLightCollision(Vector2 lookDir)
    {
        while (flashlightOn || uvOn)
        {
            if (Physics.SphereCast(transform.position, 0.5f, new Vector3(lookDir.x, lookDir.y, 0), out var hitInfo, 3.5f))
            {
                if (hitInfo.collider.gameObject.CompareTag("Ghost"))
                {
                    if (flashlightOn)
                    {
                        hitInfo.collider.gameObject.BroadcastMessage("IncreaseAnger", 1);
                    } else if (uvOn)
                    {
                        hitInfo.collider.gameObject.BroadcastMessage("Stun");
                    }
                    
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    void OnDrawGizmos(){
        //Gizmos.DrawWireCube(transform.position, boxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(lookDir.x, lookDir.y, 0) * 3.5f, 0.5f);
    }
}
