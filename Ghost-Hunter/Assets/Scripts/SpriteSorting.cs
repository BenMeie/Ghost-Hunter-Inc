using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteSorting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer[] renderers = FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        foreach (var renderer in renderers)
        {
            renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
        }

        TilemapRenderer[] maps = FindObjectsByType<TilemapRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        foreach (var map in maps)
        {
            map.sortingOrder = (int)(map.transform.position.y * -100);
        }
    }
}
