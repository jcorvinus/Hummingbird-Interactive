using UnityEngine;
using System.Collections;

public class WorldTap : MonoBehaviour
{
    [SerializeField] WorldCursor cursor;

    void OnSelect()
    {
        Debug.Log("Tap selected");
        Debug.Break();
    }
}
