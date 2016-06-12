using UnityEngine;
using System.Collections;

/// <summary>
/// Script that handles showing off the bird informational UI
/// </summary>
public class BirdUI : MonoBehaviour
{
    [SerializeField] float WindowScaleSpeed = 5f;
    [SerializeField] Transform HelpWindow;
    [SerializeField] Transform UIWindow;
    [SerializeField] float windowTweenTime = 0.45f;
    Vector3 initialScale;
    Vector3 helpWindowInitialScale;

    Coroutine showWindowCoroutine;
    Coroutine hideWindowCoroutine;

    private bool hovering = false;

	// Use this for initialization
	void Start ()
    {
        initialScale = UIWindow.transform.localScale;
        UIWindow.transform.localScale = Vector3.zero;

        helpWindowInitialScale = HelpWindow.transform.localScale;
        HelpWindow.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        HelpWindow.transform.localScale = (!hovering) ? Vector3.Lerp(Vector3.zero, helpWindowInitialScale, WindowScaleSpeed * Time.deltaTime) :
            Vector3.Lerp(helpWindowInitialScale, Vector3.zero, WindowScaleSpeed * Time.deltaTime);
    }

    void OnHoverStart()
    {
        hovering = true;
    }

    void OnHoverEnd()
    {
        hovering = false;
    }

    public void ShowVideo()
    {

    }

    IEnumerator ShowWindowCoroutine()
    {
        float timer = windowTweenTime;
        float tValue = 0;

        UIWindow.gameObject.SetActive(true);

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            tValue = Mathf.InverseLerp(0, timer, windowTweenTime);

            UIWindow.transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, tValue);

            yield return null;
        }

        UIWindow.transform.localScale = initialScale;
        yield break;
    }

    IEnumerator HideWindowCoroutine()
    {
        float timer = windowTweenTime;
        float tValue = 0;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            tValue = Mathf.InverseLerp(0, timer, windowTweenTime);

            UIWindow.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, tValue);

            yield return null;
        }

        UIWindow.gameObject.SetActive(false);
        UIWindow.transform.localScale = Vector3.zero;
        yield break;
    }

    public void ShowInfoDisplay()
    {
        HelpWindow.gameObject.SetActive(false);
        showWindowCoroutine = StartCoroutine(ShowWindowCoroutine());
    }

    public void CloseAll()
    {
        if (showWindowCoroutine != null) StopCoroutine(showWindowCoroutine);
        hideWindowCoroutine = StartCoroutine(HideWindowCoroutine());
        HelpWindow.gameObject.SetActive(true);
        hovering = false;
    }
}
