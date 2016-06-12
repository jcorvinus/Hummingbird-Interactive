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
    [SerializeField] GameObject birdPrefab;
    [SerializeField] bool chasing = false;
   


    #region Speech Stuff
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    #endregion

    #region Debug
    [Header("Debug Commands")]
    [SerializeField] bool returnToUser = false;
    [SerializeField] bool makeBird = false;
    [SerializeField] bool tenBirds = false;
    [SerializeField] bool killAllBirds = false;
    [SerializeField] bool chaseMe = false;
    [SerializeField] bool stopChasingMe = false;
    #endregion

    void Awake()
    {
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

        keywords.Add("Land over there", () =>
        {
            if (BirdManager.Instance.SelectedBird != null)
            {
                LandSelectedBirdAtLocation(cursor.transform.position);
            }
            else
            {
                textMan.SpeakText("No bird selected");
            }
        });

        keywords.Add("Fly over there", () =>
        {
            if (BirdManager.Instance.SelectedBird != null)
            {
                FlyBirdToLocation();
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

        keywords.Add("Sing to me", () =>
        {
            if (BirdManager.Instance.SelectedBird != null)
            {
                BirdManager.Instance.SelectedBird.Sing();
            }
            else
            {
                textMan.SpeakText("No bird selected");
            }
        });

        keywords.Add("Make Bird", () =>
        {
            MakeBird();
        });

        keywords.Add("Make Ten Birds", () =>
        {
            MakeTenBirds();
        });

        keywords.Add("Kill All Birds", () =>
        {
            KillAllBirds();
        });

        keywords.Add("Chase Me", () =>
        {
            ChaseMe();
        });

        keywords.Add("Stop Chasing Me", () =>
        {
            StopChasingMe();
        });



        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    public void ChaseMe()
    {
        chasing = true;
    }

    public void StopChasingMe()
    {
        chasing = false;
    }

    public void KillAllBirds()
    {
        while (BirdManager.Instance.DestroyABird())
        {
            //
        }
    }



    private void MakeBird()
    {
        Bird b = Instantiate(birdPrefab).GetComponent<Bird>();
        b.transform.position = new Vector3(0.0f, 0.2f, 1.4f);
//        BirdManager.Instance.SelectBird(b);
        //BirdManager.Instance.SelectedBird.LandAtLocation(new Vector3(0,0,0));
    }

    private void MakeTenBirds()
    {
        for (int i = 0; i < 10; i++)
        {
            
            Bird b = Instantiate(birdPrefab).GetComponent<Bird>();
            
            b.transform.position = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(1.0f, 2.0f));
        }
    }

    private void SummonBird()
    {
        if (BirdManager.Instance.SelectedBird != null)
        {
            Vector3 pointInFrontOfuser = Camera.main.transform.position + Camera.main.transform.forward * 1.15f + (Vector3.up * -0.04f);
            BirdManager.Instance.SelectedBird.FlyToLocation(pointInFrontOfuser, true);
            BirdManager.Instance.SelectedBird.StateChanged += SelectedBird_ArrivedAtPlayer;
        }
        else
        {
            textMan.SpeakText("No bird selected");
        }
    }

    private void SelectedBird_ArrivedAtPlayer(Bird sender, Bird.BirdAIState currentState, Bird.BirdAIState oldState)
    {
        sender.StateChanged -= SelectedBird_ArrivedAtPlayer;
        sender.FloatInPlace();
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
        if (makeBird)
        {
            MakeBird();
            makeBird = false;
        }
        if (tenBirds)
        {
            MakeTenBirds();
            tenBirds = false;
        }
        if (killAllBirds)
        {
            KillAllBirds();
            killAllBirds = false;
        }
        if (chaseMe)
        {
            ChaseMe();
            chaseMe = false;
        }
        if (stopChasingMe)
        {
            StopChasingMe();
            stopChasingMe = false;
        }

        if (chasing)
        {
            Vector3 pointInFrontOfuser = Camera.main.transform.position + Camera.main.transform.forward + (Vector3.up * -0.1f);
            BirdManager.Instance.SendAllBirdsToLocation(pointInFrontOfuser);
            

        }
    }

    private void LandSelectedBirdAtLocation(Vector3 location)
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

    private void FlyBirdToLocation()
    {
        BirdManager.Instance.SelectedBird.FlyToLocation(Camera.main.transform.position
            + Camera.main.transform.forward * 2.3f, false);
    }

    public void RegisterWorldTap()
    {
        if(BirdManager.Instance.SelectedBird != null)
        {
            LandSelectedBirdAtLocation(cursor.transform.position);
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
