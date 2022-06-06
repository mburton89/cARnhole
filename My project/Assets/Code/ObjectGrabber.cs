using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectGrabber : MonoBehaviour
{
    private Collider _collider;
    //private GrabbableObject _potentialObject;
    private List<GrabbableObject> _potentialObjects;
    private GrabbableObject _grabbedObject;

    [SerializeField] private List<Sprite> _handSprites;
    [SerializeField] private Image _hand;

    [SerializeField] private HoldButton _holdButton;

    private GameObject _previouslyGrabbedObject;

    void Awake()
    {
        _collider = GetComponent<Collider>();
        _potentialObjects = new List<GrabbableObject>();
    }

    private void OnEnable()
    {
        _holdButton.onPointerDown.AddListener(GrabObject);
        _holdButton.onPointerUp.AddListener(FlingObject);
    }

    private void OnDisable()
    {
        _holdButton.onPointerDown.RemoveListener(GrabObject);
        _holdButton.onPointerUp.RemoveListener(FlingObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            _collider.enabled = false;
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            _collider.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            GrabObject();
        }

        else if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(0))
        {
            FlingObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrabbableObject>())
        {
            //_potentialObject = other.GetComponent<GrabbableObject>();
            _potentialObjects.Add(other.GetComponent<GrabbableObject>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.GetComponent<GrabbableObject>() == _potentialObject)
        //{
        //    //_potentialObject = null;
        //}
        if (_potentialObjects.Contains(other.GetComponent<GrabbableObject>()))
        {
            _potentialObjects.Remove(other.GetComponent<GrabbableObject>());
        }
    }

    void GrabObject()
    {
        //if (_potentialObject != null)
        //{
        //    _grabbedObject = _potentialObject;
        //    _grabbedObject.Grab(this);
        //}
        if (_potentialObjects != null && _potentialObjects.Count > 0)
        {
            GrabbableObject closestPotentialObject = null;
            float closestDistanceFromObject = 0;
            foreach (GrabbableObject potentialObject in _potentialObjects)
            {
                float distanceFromObject;
                distanceFromObject = Mathf.Abs(potentialObject.transform.position.magnitude - transform.position.magnitude);
                if (closestPotentialObject == null)
                {
                    closestDistanceFromObject = distanceFromObject;
                    closestPotentialObject = potentialObject;
                }
                else
                {
                    if (distanceFromObject < closestDistanceFromObject)
                    {
                        closestPotentialObject = potentialObject;
                        closestDistanceFromObject = distanceFromObject;
                    }
                }
            }
            _grabbedObject = closestPotentialObject;
            _grabbedObject.Grab(this);
        }

        _hand.sprite = _handSprites[1];
    }

    void FlingObject()
    {
        if (_grabbedObject != null)
        {
            //if (_previouslyGrabbedObject != null)
            //{
            //    Destroy(_previouslyGrabbedObject);
            //}
            //_previouslyGrabbedObject = _grabbedObject.gameObject;

            _grabbedObject.Fling();
        }
        _hand.sprite = _handSprites[0];
        Reset();
    }

    public void Reset()
    {
        //_potentialObject = null;
        _grabbedObject = null;
        _potentialObjects.Clear();
        _potentialObjects = new List<GrabbableObject>();
        _collider.enabled = false;
        _collider.enabled = true;
    }
}
