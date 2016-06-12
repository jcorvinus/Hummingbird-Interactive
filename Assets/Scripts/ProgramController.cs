using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;

/// <summary>
/// Anything related to program control flow goes here.
/// </summary>
public class ProgramController : MonoBehaviour
{
    [SerializeField] WorldCursor cursor;
    [SerializeField] SpatialMapping mapping;
    [SerializeField] HoloToolkit.Unity.TextToSpeechManager textMan;

    #region Speech Stuff
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    #endregion

    #region Debug
    [Header("Debug Commands")]
    [SerializeField] bool returnToUser = false;
    #endregion

    void Awake()
    {
        BirdManager.Instance.BirdSelected += Instance_BirdSelected;

        keywords.Add("Select this bird", () =>
        {
            var focusObject = GazeGestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                Bird birdCandidate = focusObject.GetComponent<Bird>();

                if(birdCandidate != null)
                {
                    BirdManager.Instance.SelectBird(birdCandidate);
                }
                else
                {
                    textMan.SpeakText("That is not a bird.");
                }
            }
        });

        keywords.Add("Fly over there", () =>
        {
            if (BirdManager.Instance.SelectedBird != null)
            {
                SendSelectedBirdToLocation(cursor.transform.position);
            }
            else
            {
                textMan.SpeakText("No bird selected");
            }
        });

        keywords.Add("Come to me", () =>
        {
            SummonBird();
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void SummonBird()
    {
        if (BirdManager.Instance.SelectedBird != null)
        {
            Vector3 pointInFrontOfuser = Camera.main.transform.position + Camera.main.transform.forward + (Vector3.up * -0.1f);
            BirdManager.Instance.SelectedBird.LandAtLocation(pointInFrontOfuser);
        }
        else
        {
            textMan.SpeakText("No bird selected");
        }
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
	    if(returnToUser)
        {
            SummonBird();
            returnToUser = false;
        }
	}

    private void SendSelectedBirdToLocation(Vector3 location)
    {
        if (cursor.BirdCanLand)
        {
            BirdManager.Instance.SelectedBird.LandAtLocation(location);
            mapping.DrawVisualMeshes = false;
        }
        else
        {
            textMan.SpeakText("Bird cannot land there.");
        }
    }

    public void RegisterWorldTap()
    {
        if(BirdManager.Instance.SelectedBird != null)
        {
            SendSelectedBirdToLocation(cursor.transform.position);
        }
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
