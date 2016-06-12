using UnityEngine;
using System.Collections;

/// <summary>
/// Script that handles showing off the bird informational UI
/// </summary>
public class BirdUI : MonoBehaviour
{
    private delegate void TweenFinishedHandler(BirdUI sennder);
    private event TweenFinishedHandler HideTweenFinished;
    private event TweenFinishedHandler ShowTweenFinished;

    [SerializeField] float WindowScaleSpeed = 5f;
    [SerializeField] Transform HelpWindow;
    [SerializeField] Transform UIWindow;
    [SerializeField] Transform VideoWindow;
    [SerializeField] float windowTweenTime = 0.45f;
    Vector3 initialScale;
    Vector3 helpWindowInitialScale;

    Coroutine showWindowCoroutine;
    Coroutine hideWindowCoroutine;

    Coroutine showVideoCoroutine;
    Coroutine hideVideoCoroutine;

    private bool hovering = false;

    #region Debug
    [Header("Debug Commands")]
    [SerializeField] bool ShowVid = false;
    [SerializeField] bool HideVid = false;
    [SerializeField] bool ShowInfo = false;
    [SerializeField] bool ClearAll = false;
    #endregion

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

        ProcessDebugCommands();
    }

    void ProcessDebugCommands()
    {
        if (ShowVid)
        {
            ShowVideo();
            ShowVid = false;
        }

        if (HideVid)
        {
            HideVideo();
            HideVid = false;
        }

        if (ShowInfo)
        {
            ShowInfoDisplay();
            ShowInfo = false;
        }

        if (ClearAll)
        {
            CloseAll();
            ClearAll = false;
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

    #region Video Methods
    public void ShowVideo()
    {
        // todo: we need to hook into the hide info display event
        // and only show this once the hiding of the info display is done
        HideTweenFinished += Info_HideTweenFinished;
        hideWindowCoroutine = StartCoroutine(HideWindowCoroutine(UIWindow));

        UIAudio.Instance.PlayClickSound();
    }

    private void Info_HideTweenFinished(BirdUI sender)
    {
        HideTweenFinished -= Info_HideTweenFinished;
        if (hideVideoCoroutine != null) StopCoroutine(hideVideoCoroutine);
        showVideoCoroutine = StartCoroutine(ShowWindowCoroutine(VideoWindow));
    }

    public void HideVideo()
    {
        HideTweenFinished += Video_HideTweenFinished;
        hideVideoCoroutine = StartCoroutine(HideWindowCoroutine(VideoWindow));

        UIAudio.Instance.PlayClickSound();
    }

    private void Video_HideTweenFinished(BirdUI sender)
    {
        sender.HideTweenFinished -= Video_HideTweenFinished;
        ShowInfoDisplay();
    }
    #endregion

    #region Info window show / hide
    IEnumerator ShowWindowCoroutine(Transform target)
    {
        float timer = 0;
        float tValue = 0;

        target.gameObject.SetActive(true);

        while(timer < windowTweenTime)
        {
            timer += Time.deltaTime;
            tValue = Mathf.InverseLerp(0, windowTweenTime, timer);

            target.transform.localScale = MathSupplement.Exerp(Vector3.zero, initialScale, tValue);

            yield return null;
        }

        target.transform.localScale = initialScale;
        if (ShowTweenFinished != null) ShowTweenFinished(this);
        yield break;
    }

    IEnumerator HideWindowCoroutine(Transform target)
    {
        float timer = 0;
        float tValue = 0;

        while (timer < windowTweenTime)
        {
            timer += Time.deltaTime;
            tValue = Mathf.InverseLerp(0, windowTweenTime, timer);

            target.transform.localScale = MathSupplement.Exerp(initialScale, Vector3.zero, tValue);

            yield return null;
        }

        target.gameObject.SetActive(false);
        target.transform.localScale = Vector3.zero;
        if (HideTweenFinished != null) HideTweenFinished(this);
        yield break;
    }

    public void ShowInfoDisplay()
    {
        if (hideWindowCoroutine != null) StopCoroutine(hideWindowCoroutine);

        HelpWindow.gameObject.SetActive(false);
        showWindowCoroutine = StartCoroutine(ShowWindowCoroutine(UIWindow));

        UIAudio.Instance.PlayClickSound();
    }
    #endregion

    public void CloseAll()
    {
        if (showWindowCoroutine != null) StopCoroutine(showWindowCoroutine);
        if (UIWindow.gameObject.activeInHierarchy) hideWindowCoroutine = StartCoroutine(HideWindowCoroutine(UIWindow));

        if (showVideoCoroutine != null) StopCoroutine(showVideoCoroutine);
        if (VideoWindow.gameObject.activeInHierarchy) hideVideoCoroutine = StartCoroutine(HideWindowCoroutine(VideoWindow));

        HideTweenFinished = null;
        ShowTweenFinished = null;

        HelpWindow.gameObject.SetActive(true);
        hovering = false;
    }
}
