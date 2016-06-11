using UnityEngine;

public class WorldCursor : MonoBehaviour
{
    [SerializeField] Color highlightColor = Color.yellow;
    [SerializeField] Color goToPointColor = Color.magenta;
    [SerializeField] float upDotCutoff = 0.65f;
    private Color defaultColor;
    private MeshRenderer meshRenderer;
    private bool birdCanLand = false;

    public bool BirdCanLand
    {
        get { return birdCanLand; }
    }

    // Use this for initialization
    void Start()
    {
        // Grab the mesh renderer that's on the same object as this script.
        meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            Bird birdCandidate = hitInfo.collider.GetComponent<Bird>();

            if(birdCandidate != null)
            {
                meshRenderer.enabled = true;

                // change our cursor color to be 'select new bird' color.
                meshRenderer.material.color = highlightColor;
            }
            else
            {
                birdCanLand = (Vector3.Dot(Vector3.up, hitInfo.normal) >= upDotCutoff);

                if (BirdManager.Instance.SelectedBird == null)
                {
                    meshRenderer.enabled = false;
                }
                else
                {
                    meshRenderer.enabled = true;
                    meshRenderer.material.color = (birdCanLand) ? goToPointColor : Color.red;
                }
            }            

            // Move thecursor to the point where the raycast hit.
            this.transform.position = hitInfo.point;

            // Rotate the cursor to hug the surface of the hologram.
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
        else
        {
            // If the raycast did not hit a hologram, hide the cursor mesh.
            meshRenderer.enabled = false;
        }
    }
}