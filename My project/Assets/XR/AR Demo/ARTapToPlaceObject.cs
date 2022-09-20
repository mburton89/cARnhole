using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;

    private ARRaycastManager arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    public GameObject objectToPlace1;
    public GameObject objectToPlace2;

    bool hasPlacedObject = false;

    public Transform handCursor;
    public Transform tapToPlaceText;

    void Start()
    {
        arOrigin = FindObjectOfType<ARRaycastManager>();
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
        Instantiate(objectToPlace1, placementPose.position, placementPose.rotation);
        Instantiate(objectToPlace2, new Vector3(Camera.main.transform.position.x, placementPose.position.y, Camera.main.transform.position.z), placementPose.rotation);
        hasPlacedObject = true;
        SoundManager.Instance.boardPlacedSound.Play();
        handCursor.localScale = Vector3.one;
        tapToPlaceText.localScale = Vector3.zero;
    }
}
