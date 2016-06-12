using UnityEngine;

public class SplineWalker : MonoBehaviour {
    public bool started = false;
	public BezierSpline spline;
    public float progress;
    public float duration;
	public bool lookForward;
	public SplineWalkerMode mode;
    public Vector3 last_position;
	
	private bool goingForward = true;

    public void ResetPath(BezierSpline new_path)
    {
        progress = 0;
        spline = new_path;
    }


	private void Update () {
        if (!started)
        {
            return;
        }
		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				if (mode == SplineWalkerMode.Once) {
					progress = 1f;
				}
				else if (mode == SplineWalkerMode.Loop) {
					progress -= 1f;
				}
				else {
					progress = 2f - progress;
					goingForward = false;
				}
			}
		}
		else {
			progress -= Time.deltaTime / duration;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}

		last_position = spline.GetPoint(progress);
		transform.localPosition = last_position;
		if (lookForward) {
			transform.LookAt(last_position + spline.GetDirection(progress));
		}
	}
}