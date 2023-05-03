using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectController : MonoBehaviour
{
    public void CharacterSelect(string name)
    {
        PlayerPrefs.SetString("Character", name);
    }
}
