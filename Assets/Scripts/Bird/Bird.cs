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
    [SerializeField] float turnSpeed = 4f;

    // audio selection methods
    float minimumTimeBetweenChirps = 4;
    float maximumTimeBetweenChirps = 20f;
    float chirpTimer;
    #endregion

    #region Debug
    [Header("Debug Variables")]
    public Color birdMeshSelectedColor = Color.red;
    public Transform debugGoal;

    [Header("Debug Commands")]
    public bool BirdSing = false;
    public bool BirdChirp = false;
    public bool SendBird = false;
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
        Invoke("SnapToGround", Time.deltaTime);
    }

    void SnapToGround()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, float.PositiveInfinity, ~SpatialMapping.PhysicsRaycastMask))
        {
            transform.position = hitInfo.point + (Vector3.up * sphereCollider.radius);
        }
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

        if(SendBird)
        {
            SendToLocation(debugGoal.position);
            SendBird = false;
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
