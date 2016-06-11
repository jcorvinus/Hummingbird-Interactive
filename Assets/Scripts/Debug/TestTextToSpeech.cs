using UnityEngine;
using System.Collections;

using HoloToolkit.Unity;

public class TestTextToSpeech : MonoBehaviour
{
    [SerializeField] string textToSay;
    [SerializeField] bool speak = false;

    public TextToSpeechManager textMan;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(speak)
        {
            textMan.SpeakText(textToSay);
            Debug.Log(textToSay);
            speak = false;
        }
	}

    void OnSelect()
    {
        speak = true;
    }
}
