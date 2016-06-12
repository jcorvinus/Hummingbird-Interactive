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
    [SerializeField] int numBirdsSpawned = 1;
    [SerializeField] Stack<GameObject> spawnedBirds = new Stack< GameObject >();
    [SerializeField] bool chasing = false;

    #region Speech Stuff
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    #endregion

    #region Debug
    [Header("Debug Commands")]
    [SerializeField] bool returnToUser = false;
    [SerializeField] bool bigBird = false;
    [SerializeField] bool littleBird = false;
    [SerializeField] bool tenBirds = false;
    [SerializeField] bool deadBird = false;
    [SerializeField] bool killAllBirds = false;
    [SerializeField] bool chaseMe = false;
    [SerializeField] bool stopChasingMe = false;

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

        keywords.Add("Big bird", () =>
        {
            BigBird();
        });
        keywords.Add("Little bird", () =>
        {
            LittleBird();
        });

        keywords.Add("Ten birds", () =>
        {
            TenBirds();
        });


        keywords.Add("Dead bird", () =>
        {
            DeadBird();
        });

        keywords.Add("Kill all birds", () =>
        {
            KillAllBirds();
        });

        keywords.Add("Chase me", () =>
        {
            ChaseMe();
        });

        keywords.Add("Stop Chase", () =>
        {
            StopChasingMe();
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
            BirdManager.Instance.SelectedBird.SendToLocation(pointInFrontOfuser);
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
        if (bigBird)
        {
            BigBird();
            bigBird = false;
        }
        if (littleBird)
        {
            LittleBird();
            littleBird = false;
        }
        if (deadBird)
        {
            DeadBird();
            deadBird = false;
        }
        if (tenBirds)
        {
            TenBirds();
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
            stopChasingMe = true;
        }

        if (chasing)
        {
            Vector3 pointInFrontOfuser = Camera.main.transform.position + Camera.main.transform.forward + (Vector3.up * -0.1f);
            foreach (GameObject bird in spawnedBirds)
            {
                bird.GetComponent<Bird>().SendToLocation(pointInFrontOfuser);
            }
        }

    }

    private void SendSelectedBirdToLocation(Vector3 location)
    {
        if (cursor.BirdCanLand)
        {
            BirdManager.Instance.SelectedBird.SendToLocation(location);
            mapping.DrawVisualMeshes = false;
        }
        else
        {
            textMan.SpeakText("Bird cannot land there.");
        }
    }

    private void SpawnBird(Vector3 scale)
    {
        GameObject b = Instantiate(birdPrefab);
        int x = numBirdsSpawned % 3;
        int y = numBirdsSpawned / 3;
        numBirdsSpawned++;
        b.transform.Translate(x * 0.25f, y *0.25f, 0);
        b.transform.localScale = scale;
        spawnedBirds.Push(b);
    }
    public void BigBird()
    {
        SpawnBird(new Vector3(2, 2, 2));      
    }

    public void LittleBird()
    {
        SpawnBird(new Vector3(0.5f, 0.5f, 0.5f));
    }

    public void TenBirds()
    {
        for (int i = 0; i < 10; i++)
        {
            SpawnBird(new Vector3(1.0f, 1.0f, 1.0f));
        }
    }

    public void ChaseMe()
    {
        chasing = true;
        
    }

    public void StopChasingMe()
    {
        chasing = false;
    }

    public void DeadBird()
    {
        if (spawnedBirds.Count > 0)
        {
            GameObject kill_this_one = spawnedBirds.Pop();
            DestroyObject(kill_this_one);
            numBirdsSpawned--;
        }
    }
    public void KillAllBirds()
    {
        while (spawnedBirds.Count > 0)
        {
            GameObject kill_this_one = spawnedBirds.Pop();
            DestroyObject(kill_this_one);
            numBirdsSpawned--;
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
