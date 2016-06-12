using UnityEngine;
using System.Collections;

public class UIAudio : MonoBehaviour
{
    public static UIAudio Instance;

    private AudioSource source;
    [SerializeField] AudioClip clickSound;

	// Use this for initialization
	void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
	}
	
    public void PlayClickSound()
    {
        source.clip = clickSound;
        source.Play();
    }
}
