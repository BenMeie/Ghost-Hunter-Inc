using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float flashLightRadius = 5f;

    public float pickupRadius = 2f;
    [Range(1, 360)]public float angle = 25f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    private Vector2 lookDir;
    


    [Header("Flashlight")]
    public Light2D flashlight;
    private Transform rotatedTransform;
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
    private Memento interactableMemento;

    public delegate void MementoFound(int id);

    //public static event MementoFound onMementoFound;    
    
    // Start is called before the first frame update
    void Start()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        lightPosition2D = new Vector2(flashlight.transform.position.x, flashlight.transform.position.y);
        lookDir = mousePos - lightPosition2D;
        lookDir.Normalize();
        rotatedTransform =  flashlight.transform;

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
        if(Input.GetKeyDown(KeyCode.E) && interactableMemento != null)
        {
            gameManager.FindMemento(interactableMemento);
            interactableMemento = null;
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
        }
        else if(uvBattery > 15)
        {
            uvOn = true;
            flashlight.color = Color.magenta;
            flashlight.enabled = true;
            flashlightOn = false;
            StartCoroutine(CheckFov());
        }
    }

    IEnumerator CheckFov()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(0.4f);
            gameManager.HideInteractable();
            interactableMemento = null;
            Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(rotatedTransform.position, flashLightRadius, targetLayer);
            foreach (var interactable in rangeCheck)
            {
                Vector2 dirToTarget = interactable.transform.position - rotatedTransform.position;
                float distanceToTarget = Vector2.Distance(interactable.transform.position, rotatedTransform.position);
                
                // Is the target object within our view cone or very close
                if (!(Vector2.Angle(rotatedTransform.up, dirToTarget) < angle / 2) && distanceToTarget > 0.7f) continue;
                
                // Is the target object blocked by anything
                if (Physics2D.Raycast(rotatedTransform.position, dirToTarget, distanceToTarget, obstructionLayer)) continue;
                
                // Check what kind of object we just found
                if (interactable.gameObject.CompareTag("Ghost")) // If it's the ghost, use the flashlight radius
                {
                    if (flashlightOn)
                    {
                        gameManager.AngerGhost(1);

                    } else if (uvOn)
                    {
                        gameManager.StunGhost();
                    }
                    
                } else if (interactable.gameObject.CompareTag("Memento") && distanceToTarget <= pickupRadius) // If it's an item, use the pickup radius
                {
                    gameManager.ShowInteractable(interactable.transform.position);
                    interactableMemento = interactable.gameObject.GetComponent<Memento>();
                } else if (interactable.gameObject.CompareTag("Ritual") && distanceToTarget <= pickupRadius)
                {
                    ritual = interactable.gameObject;
                    gameManager.ShowInteractable(interactable.transform.position);
                }
            }
        }
    }

    void OnDrawGizmos(){
        //Gizmos.DrawWireCube(transform.position, boxSize);
        if (rotatedTransform == null) rotatedTransform = transform;

        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(rotatedTransform.position, Vector3.forward, flashLightRadius);
        UnityEditor.Handles.DrawWireDisc(rotatedTransform.position, Vector3.forward, pickupRadius);

        Vector3 angle1 = DirectionFromAngle(-rotatedTransform.eulerAngles.z, -angle / 2);
        Vector3 angle2 = DirectionFromAngle(-rotatedTransform.eulerAngles.z, angle / 2);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(rotatedTransform.position, rotatedTransform.position + angle1 * flashLightRadius);
        Gizmos.DrawLine(rotatedTransform.position, rotatedTransform.position + angle2 * flashLightRadius);
    }

    private Vector2 DirectionFromAngle(float eulerY, float degreeAngle)
    {
        degreeAngle += eulerY;
        return new Vector2(Mathf.Sin(degreeAngle * Mathf.Deg2Rad), Mathf.Cos(degreeAngle * Mathf.Deg2Rad));
    }
}
