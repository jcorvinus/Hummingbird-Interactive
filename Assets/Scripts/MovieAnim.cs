using UnityEngine;
using System.Collections;

public class MovieAnim : MonoBehaviour
{
    MovieTexture movie;
    public bool PlayMovie = false;

	// Use this for initialization
	void Start ()
    {
        movie = (MovieTexture)GetComponent<MeshRenderer>().material.mainTexture;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!movie.isPlaying) movie.Play();
	    if(PlayMovie)
        {
            movie.Play();
            PlayMovie = false;
        }
	}
}
