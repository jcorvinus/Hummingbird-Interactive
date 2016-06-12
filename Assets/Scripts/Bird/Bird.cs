using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
    public delegate void BirdStateChangeHandler(Bird sender, BirdAIState currentState, BirdAIState oldState);
    public event BirdStateChangeHandler StateChanged;

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
    bool landAtDestination = false;
    Vector3 flightGoal;
    [SerializeField] float turnSpeed = 4f;
    [SerializeField] float forwardSpeed = 0.35f;

    // audio selection methods
    float minimumTimeBetweenChirps = 4;
    float maximumTimeBetweenChirps = 20f;
    float chirpTimer;

    bool hasSnapped = false;
    #endregion

    #region Debug
    [Header("Debug Variables")]
    public Color birdMeshSelectedColor = Color.red;
    public Transform debugGoal;

    [Header("Debug Commands")]
    public bool BirdSing = false;
    public bool BirdChirp = false;
    public bool LandBird = false;
    #endregion

    // Use this for initialization
    void Awake ()
    {
        sphereCollider = GetComponent<SphereCollider>();
        rigidBody = GetComponent<Rigidbody>();
        characterController = GetComponentInChildren<HummingbirdCharacterScript>();
	}

    void Start()
    {
        characterController.Soar();
        characterController.ForwardSpeedSet(0f);
    }

    public bool SnapToGround()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, float.PositiveInfinity))
        {
            transform.position = hitInfo.point + (Vector3.up * sphereCollider.radius);
            return true;
        }
        else return false;
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
                if (landAtDestination)
                {
                    if (Vector3.Distance(transform.position, flightGoal) < 0.01f)
                    {
                        characterController.Landing();
                        currentState = BirdAIState.Sitting;
                        if (StateChanged != null) StateChanged(this, BirdAIState.Sitting, BirdAIState.FlyingToGoal);
                        break;
                    }
                }

                Vector3 directionToGoal = (flightGoal - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToGoal, Vector3.up), turnSpeed * Time.deltaTime);
                break;

            default:
                break;
        }

        ProcessDebugCommands();
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

        if(LandBird)
        {
            LandAtLocation(debugGoal.position);
            LandBird = false;
        }
    }

    /// <summary>
    /// The bird will fly to a specified location and circle around it once it
    /// arrives.
    /// </summary>
    /// <param name="direction"></param>
    public void FlyToLocation(Vector3 location)
    {
        BirdAIState oldState = currentState;
        flightGoal = location;
        currentState = BirdAIState.FlyingToGoal;
        characterController.Soar();

        wingsSource.volume = 1;
        characterController.ForwardSpeedSet(forwardSpeed);

        landAtDestination = false;

        if (StateChanged != null) StateChanged(this, currentState, oldState);
    }

    /// <summary>
    /// Tells the bird to stop whatever it's doing and hover in place.
    /// </summary>
    public void FloatInPlace()
    {
        characterController.forwardSpeed = 0;
    }

    /// <summary>
    /// The bird will try to land at the specified location.
    /// </summary>
    /// <param name="location"></param>
    public void LandAtLocation(Vector3 location)
    {
        BirdAIState oldState = currentState;
        flightGoal = location;
        currentState = BirdAIState.FlyingToGoal;
        characterController.Soar();

        wingsSource.volume = 1;
        characterController.ForwardSpeedSet(forwardSpeed);

        landAtDestination = true;

        if(StateChanged != null) StateChanged(this, currentState, oldState);
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
