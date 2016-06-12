using UnityEngine;
using System.Collections;

/// <summary>
/// Script that handles showing off the bird informational UI
/// </summary>
public class BirdUI : MonoBehaviour
{
    [SerializeField] float WindowScaleSpeed = 5f;
    [SerializeField] Transform HelpWindow;
    [SerializeField] Transform UIWindow;
    Vector3 initialScale;
    Vector3 helpWindowInitialScale;

	// Use this for initialization
	void Start ()
    {
        initialScale = UIWindow.transform.localScale;
        UIWindow.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnHoverStart()
    {

    }

    void OnHoverEnd()
    {

    }
}
