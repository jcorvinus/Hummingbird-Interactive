using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    public UnityEvent ButtonFired;
    Image buttonImage;
    [SerializeField] Color hoverColor = Color.cyan;
    [SerializeField] float colorPhaseTime = 0.45f;
    Vector3 buttonStartScale;
    private bool hovering = false;
    float hoverTime;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        buttonStartScale = buttonImage.transform.localScale;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(hovering)
        {
            hoverTime += Time.deltaTime;
            buttonImage.color = Color.Lerp(Color.white, hoverColor, Mathf.PingPong(hoverTime, colorPhaseTime));
            transform.localScale = Vector3.Lerp(buttonStartScale, buttonStartScale * 1.125f, Mathf.PingPong(hoverTime, colorPhaseTime));
        }
        else
        {
            buttonImage.color = Color.Lerp(buttonImage.color, Color.white, colorPhaseTime);
            transform.localScale = Vector3.Lerp(transform.localScale, buttonStartScale, colorPhaseTime);
        }
	}

    void OnHoverStart()
    {
        hovering = true;
    }

    void OnHoverEnd()
    {
        hovering = false;
    }

    void OnSelect()
    {
        ButtonFired.Invoke();
    }
}
