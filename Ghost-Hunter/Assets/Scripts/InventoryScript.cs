using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{

    [Header("Inventory Slots")]
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;

    private InventorySlotScript [] slots ;
    private int currentIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        slots = new InventorySlotScript[4];

        setSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setSlots()
    {
        slots[0] = slot1.GetComponent<InventorySlotScript>();
        slots[1] = slot2.GetComponent<InventorySlotScript>();
        slots[2] = slot3.GetComponent<InventorySlotScript>();
        slots[3] = slot4.GetComponent<InventorySlotScript>();
    }

    public void addMemento(Memento memento)
    {
        slots[currentIndex].acceptImage(memento.mySprite);
        currentIndex++;
    }

    public void failRitual()
    {
        //here, we pass by every slot that is full, and only flash the slots that are empty
        int i = 0;
        while (i < currentIndex)
        {
            i++;
        }

        //have each empty slot flash red
        for ( ; i < slots.Length; i++)
        {
            slots[i].flashRed();
        }
    }
    
}
