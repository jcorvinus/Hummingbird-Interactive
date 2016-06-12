using UnityEngine;
using System.Collections;

/// <summary>
/// This script will align a transform to face another,
/// however it will keep its Y axis aligned to world Y.
/// </summary>
public class LookatY : MonoBehaviour
{
    [SerializeField] Transform target;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target)
        {
            // throws exception (on spawned birds)
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
	}
}
