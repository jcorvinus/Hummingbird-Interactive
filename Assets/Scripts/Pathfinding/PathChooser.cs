using UnityEngine;
using System.Collections;


// build a path for the bird.  This is a big bezier spline
// that has a few curves for takeoff, a few for flight and a few for landing
//
// if the bird is on, then there is a takeoff spline

public class PathChooser : MonoBehaviour {

    
    public BezierSpline[] takeoff_options;

    public BezierSpline ChoosePath(Transform parent, Vector3 start, Vector3 end)
    {
        // ok. so I can make takeoffs
        int takeoff_index = Random.Range(0, takeoff_options.Length - 1);
        BezierSpline takeoff_choice = (BezierSpline) Instantiate(takeoff_options[takeoff_index], parent.position, parent.rotation);

        Vector3 fin_point = parent.InverseTransformPoint(end) * parent.lossyScale.x;
        takeoff_choice.LerpToFinalPoint(fin_point);

        return takeoff_choice;
    }

    
}
