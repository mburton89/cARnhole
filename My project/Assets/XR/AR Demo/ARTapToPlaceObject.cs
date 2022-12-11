using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;

    private ARRaycastManager arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    public GameObject objectToPlace1;
    public GameObject objectToPlace2;
    GameObject placedBoard;

    bool hasPlacedObject = false;
    bool hasSeenHint2;
    bool hasSeenHint3;
    bool hasSeenHint4;

    public Transform handCursor;
    public TextMeshProUGUI tapToPlaceText;

    public Slider distanceSlider;
    public Button nextButton;

    void Start()
    {
        arOrigin = FindObjectOfType<ARRaycastManager>();
        distanceSlider.onValueChanged.AddListener(delegate { HandleDistanceSlider(); });
        nextButton.onClick.AddListener(HandleNextPressed);
        distanceSlider.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (hasPlacedObject)
        {
            placementIndicator.SetActive(false);
        }
        else
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }


        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!hasPlacedObject)
            {
                PlaceObject();
            }
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            if (!hasSeenHint2)
            {
                hasSeenHint2 = true;
                tapToPlaceText.SetText("Tap to place board on ground");
            }

            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            placementIndicator.transform.position = placementPose.position;
        }
        else
        {
            placementIndicator.SetActive(true);
        }
    }

    void PlaceObject()
    {
        if (!hasSeenHint3)
        {
            hasSeenHint3 = true;
            tapToPlaceText.SetText("Adjust distance if needed");
        }

        GameObject object1 = Instantiate(objectToPlace1, placementPose.position, placementPose.rotation);
        placedBoard = GameObject.FindGameObjectWithTag("BoardContainer");
        GameObject object2 = Instantiate(objectToPlace2, new Vector3(Camera.main.transform.position.x, placementPose.position.y, Camera.main.transform.position.z), placementPose.rotation);

        if (Vector3.Distance(object1.transform.position, object2.transform.position) < 1f)
        {
            distanceSlider.minValue = Vector3.Distance(object1.transform.position, object2.transform.position) + .1f;
            distanceSlider.value = Vector3.Distance(object1.transform.position, object2.transform.position) + .1f;
            placedBoard.transform.localPosition = new Vector3(0, 0, Vector3.Distance(object1.transform.position, object2.transform.position) + .1f);
        }

        hasPlacedObject = true;
        SoundManager.Instance.boardPlacedSound.Play();
        distanceSlider.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
    }

    private IEnumerator ThrowMessageCo()
    {
        yield return new WaitForSeconds(5);
        tapToPlaceText.SetText("Move phone and release thumb to throw");
        yield return new WaitForSeconds(5);
        tapToPlaceText.SetText("");
    }

    void HandleDistanceSlider()
    {
        placedBoard.transform.localPosition = new Vector3(0, 0, distanceSlider.value);
    }

    void HandleNextPressed()
    {
        distanceSlider.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        handCursor.localScale = Vector3.one;
        if (!hasSeenHint4)
        {
            hasSeenHint4 = true;
            tapToPlaceText.SetText("Tap and hold to grab bag");
            StartCoroutine(ThrowMessageCo());
        }
    }
}
