using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{

    // defines
    public enum BirdAIState { Sitting, Floating, FlyingToGoal }

    #region Other Components
    private SphereCollider sphereCollider;
    private Rigidbody rigidBody;
    [SerializeField] MeshRenderer[] renderers;
    [SerializeField] AudioSource birdMouth;
    [SerializeField] AudioSource wingsSource;
    [SerializeField] AudioClip song;
    [SerializeField] AudioClip[] chirpSounds;

    HummingbirdCharacterScript characterController;
    #endregion

    #region State Variables
    [SerializeField] BirdAIState currentState = BirdAIState.Sitting;
    /// <summary>If true, bird is flying in an arc.
    /// If false, bird is doing a quick jump to its destination.</summary>
    bool smoothFlight = false;
    Vector3 flightGoal;

    // audio selection methods
    float minimumTimeBetweenChirps = 4;
    float maximumTimeBetweenChirps = 20f;
    float chirpTimer;
    #endregion

    #region Debug
    [Header("Debug Variables")]
    public Color birdMeshSelectedColor = Color.red;

    [Header("Debug Commands")]
    public bool BirdSing = false;
    public bool BirdChirp = false;
    #endregion

    // Use this for initialization
    void Awake ()
    {
        sphereCollider = GetComponent<SphereCollider>();
        rigidBody = GetComponent<Rigidbody>();
        characterController = GetComponentInChildren<HummingbirdCharacterScript>();

        
	}

    void OnEnable()
    {
        BirdManager.Instance.AddBird(this);
    }

    void OnDisable()
    {
        BirdManager.Instance.RemoveBird(this);
    }

    void OnDestroy()
    {
        BirdManager.Instance.RemoveBird(this);
    }

    void GetNewChirpTime()
    {
        chirpTimer = Random.Range(minimumTimeBetweenChirps,
            maximumTimeBetweenChirps);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(chirpTimer > 0)
        {
            chirpTimer -= Time.deltaTime;

            if(chirpTimer < 0)
            {
                Chirp();
                GetNewChirpTime();
            }
        }

        switch (currentState)
        {
            case BirdAIState.Sitting:
                break;

            case BirdAIState.Floating:
                break;

            case BirdAIState.FlyingToGoal:
                if(Vector3.Distance(transform.position, flightGoal) < 0.01f)
                {
                    characterController.Landing();
                    currentState = BirdAIState.Sitting;
                    break;
                }

                if(smoothFlight)
                {

                }
                else
                {

                }
                break;

            default:
                break;
        }
    }

    private void Sing()
    {
        birdMouth.clip = song;
        birdMouth.Play();
    }

    private void Chirp()
    {
        birdMouth.clip = chirpSounds[(int)Random.Range(0, chirpSounds.Length - 1)];
        birdMouth.Play();
    }

    private void ProcessDebugCommands()
    {
        if(BirdSing)
        {
            BirdSing = false;
        }

        if(BirdChirp)
        {
            BirdSing = false;
        }
    }

    public void SendToLocation(Vector3 location)
    {
        flightGoal = location;
        currentState = BirdAIState.FlyingToGoal;
        characterController.Soar();

        wingsSource.volume = 1;

        smoothFlight = true;
    }

    public void Deselect()
    {

    }

    void OnSelect()
    {
        BirdManager.Instance.SelectBird(this);
        Debug.Log("Bird selected.");
    }
}
