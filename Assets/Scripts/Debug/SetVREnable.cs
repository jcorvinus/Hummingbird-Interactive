using UnityEngine;
using System.Collections;

public class SetVREnable : MonoBehaviour
{
    public bool Enable = false;

    void Awake()
    {
        UnityEngine.VR.VRSettings.enabled = Enable;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
