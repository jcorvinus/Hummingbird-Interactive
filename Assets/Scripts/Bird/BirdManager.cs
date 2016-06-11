using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdManager : MonoBehaviour
{
    public static BirdManager Instance;

    List<Bird> birdList;
    Bird selectedBird;

    bool hasInitialized = false;

    void Awake()
    {
        if (hasInitialized) return;
        Instance = this;
        birdList = new List<Bird>();

        hasInitialized = true;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SelectBird(Bird newSelection)
    {
        

    }

    public void AddBird(Bird newBird)
    {
        Awake();
        birdList.Add(newBird);
    }

    public void RemoveBird(Bird bird)
    {
        birdList.Remove(bird);
    }
}
