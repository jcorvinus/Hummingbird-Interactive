using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Anything related to program control flow goes here.
/// </summary>
public class ProgramController : MonoBehaviour
{
    [SerializeField] WorldCursor cursor;
    [SerializeField] SpatialMapping mapping;

    void Awake()
    {
        BirdManager.Instance.BirdSelected += Instance_BirdSelected;
    }

    private void Instance_BirdSelected(BirdManager sender, Bird affectedObject)
    {
        mapping.DrawVisualMeshes = true;
    }

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void RegisterWorldTap()
    {
        if(BirdManager.Instance.SelectedBird != null)
        {
            if(cursor.BirdCanLand)
            {
                BirdManager.Instance.SelectedBird.SendToLocation(cursor.transform.position);
                mapping.DrawVisualMeshes = false;
            }
        }
    }
}
