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
    [SerializeField] AudioClip chirp;
    #endregion

    #region State Variables
    [SerializeField] BirdAIState currentState = BirdAIState.Sitting;
    /// <summary>If true, bird is flying in an arc.
    /// If false, bird is doing a quick jump to its destination.</summary>
    bool smoothFlight = false;

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
	
	// Update is called once per frame
	void Update ()
    {
        switch (currentState)
        {
            case BirdAIState.Sitting:
                break;

            case BirdAIState.Floating:
                break;

            case BirdAIState.FlyingToGoal:
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
        birdMouth.clip = chirp;
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

    void OnDeselect()
    {

    }

    void OnSelect()
    {
        BirdManager.Instance.SelectBird(this);
    }
}
