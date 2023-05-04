using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("General")]

    public GameManager gameManager;
    public Camera cam;
    
    [Header("Look Detection")]
    public float radius = 5f;
    [Range(1, 360)]public float angle = 15f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
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
    Vector2 mousePos;
    public Image uvBar;

    private Vector2 lightPosition2D;
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
        
        StartCoroutine(CheckFov());
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
        // if (Physics.SphereCast(transform.position, 0.5f, new Vector3(lookDir.x, lookDir.y, 0), out var hitInfo, 1f))
        // {
        //     if (hitInfo.collider.gameObject.CompareTag("Memento"))
        //     {
        //         gameManager.ShowInteractable(hitInfo.transform.position);
        //
        //         if (Input.GetKeyDown("e"))
        //         {
        //             //might not be the best way to do this
        //             Memento memento = hitInfo.collider.GetComponent<Memento>();
        //             gameManager.FindMemento(memento);
        //         }
        //     }
        //     
        //     //can add check for ritual or other interactable objects here
        //     
        // } else {
        //     gameManager.HideInteractable();
        // }
        
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
            uvBattery += rechargeRate / 16.0f;
        }

        batteryBar.fillAmount = battery / 100.0f;
        uvBar.fillAmount = uvBattery / 50.0f;
    }

    void ToggleFlashlight(){
        if (uvOn)
        {
            uvOn = false;
            flashlight.color = Color.white;
            flashlight.enabled = false;
            StopCoroutine(CheckLightCollision(lookDir));
        }
        flashlightOn = !flashlightOn;
        flashlight.enabled = flashlightOn;
        StopCoroutine(CheckFov());
        StartCoroutine(CheckFov());
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
        else if(uvBattery > 15)
        {
            uvOn = true;
            flashlight.color = Color.magenta;
            flashlight.enabled = true;
            flashlightOn = false;
            StartCoroutine(CheckLightCollision(lookDir));
        }
    }

    IEnumerator CheckFov()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
            foreach (var interactable in rangeCheck)
            {
                print(interactable.gameObject.name+ "Is in view radius");
                Vector2 dirToTarget = interactable.transform.position - transform.position;
                // Is the target object within our view cone
                if (!(Vector2.Angle(transform.up, dirToTarget) < angle / 2)) continue;
                print(interactable.gameObject.name+ "Is in view cone");
                float distanceToTarget = Vector2.Distance(interactable.transform.position, transform.position);
                // Is the target object blocked by anything
                if (Physics2D.Raycast(transform.position, dirToTarget, distanceToTarget, obstructionLayer)) continue;
                print(interactable.gameObject.name+ "Is not blocked");
                if (interactable.gameObject.CompareTag("Ghost"))
                {
                    interactable.gameObject.BroadcastMessage("IncreaseAnger", 1);
                } else if (interactable.gameObject.CompareTag("Memento"))
                {
                    if (!Input.GetKeyDown(KeyCode.E)) continue;
                    Memento memento = interactable.gameObject.GetComponent<Memento>();
                    gameManager.FindMemento(memento);
                }
            }
        }
    }

    void OnDrawGizmos(){
        //Gizmos.DrawWireCube(transform.position, boxSize);

        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        Vector3 angle1 = DirectionFromAngle(-transform.eulerAngles.z, -angle / 2);
        Vector3 angle2 = DirectionFromAngle(-transform.eulerAngles.z, angle / 2);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + angle1 * radius);
        Gizmos.DrawLine(transform.position, transform.position + angle2 * radius);
    }

    private Vector2 DirectionFromAngle(float eulerY, float degreeAngle)
    {
        degreeAngle += eulerY;
        return new Vector2(Mathf.Sin(degreeAngle * Mathf.Deg2Rad), Mathf.Cos(degreeAngle * Mathf.Deg2Rad));
    }
}
