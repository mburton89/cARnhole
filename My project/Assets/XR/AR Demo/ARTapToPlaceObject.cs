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

    bool hasPlacedObject1 = false;
    bool hasPlacedObject2 = false;

    void Start()
    {
        arOrigin = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (!hasPlacedObject1 && !hasPlacedObject2)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }
        else
        {
            placementIndicator.SetActive(false);
        }


        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!hasPlacedObject1)
            {
                PlaceObject1();
            }
            else if (!hasPlacedObject2)
            {
                PlaceObject2();
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

    void PlaceObject1()
    {
        Instantiate(objectToPlace1, placementPose.position, placementPose.rotation);
        hasPlacedObject1 = true;
    }

    void PlaceObject2()
    {
        Instantiate(objectToPlace2, placementPose.position, placementPose.rotation);
        hasPlacedObject2 = true;
    }
}
