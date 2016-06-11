using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    [SerializeField]float rotationSpeed = 15f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
	}
}
