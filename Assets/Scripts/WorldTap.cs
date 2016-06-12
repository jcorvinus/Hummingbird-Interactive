using UnityEngine;
using System.Collections;

public class WorldTap : MonoBehaviour
{
    ProgramController programController;

    void Awake()
    {
        programController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ProgramController>();
    }

    void OnSelect()
    {
        programController.RegisterWorldTap();
        Debug.Log("World Tap occured.");
    }
}
