using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

//This 
public class CamSwitcher : MonoBehaviour
{
    [Header("Reference the two Cameras you want to switch between!")]
    [Space(5)]
    [Tooltip("Set a reference to the gpsCamera")]
    [SerializeField] Camera gpsCamera;
    [Tooltip("Set a reference to the Ar Camera (ARFoundation)")]
    public Camera arCamera;

    ARSessionOrigin _arSessionOrigin;
    ARSession _arSession;


    private void Awake()
    {
        if(gpsCamera == null || arCamera == null)
        {
            Debug.LogError("There is no reference to the GPS and/or AR Camera! Please refrence them" +
                "in the CamSwitcher Script in the editor");
        }
    }

    void Start()
    {
        _arSessionOrigin = FindAnyObjectByType<ARSessionOrigin>();
        _arSession = FindObjectOfType<ARSession>();


        //when the scene is startin I want people to walk around with their GPS Cam
        //so I turn of ARCam Elements
        arCamera.enabled = false;
        arCamera.GetComponent<AudioListener>().enabled = false;

        gpsCamera.GetComponent<BoxCollider>().isTrigger = true;
    }

    public void InducingChangeToArCam()
    {
        //Reset the Session to remove all trackables and start fresh
        _arSession.Reset();

        SetAROriginToGPSLocation();
        AlignView();

        Invoke("ShowArCamera", 1);

    }

    //here we make sure that in the virtual world the ARCam (that is a child of the Session Origin
    //has its origin at the position where we are at the moment. We indirectly move the ARCam.
    private void SetAROriginToGPSLocation()
    {
        _arSessionOrigin.transform.position = gpsCamera.transform.position;
    }

    //we adjust the rotation of the cam, so our view is synced.
    private void AlignView()
    {
        float absolutenum = gpsCamera.transform.rotation.eulerAngles.y;

        _arSessionOrigin.transform.rotation = Quaternion.identity;
        _arSessionOrigin.transform.Rotate(0, absolutenum, 0);
    }


    public void ShowGPSCamera()
    {
        //we only disable the camera component, don't set the whole object inactive as the
        //tracking still needs to carry on for the google ARFoundation Code to work.
        arCamera.enabled = false;
        

        gpsCamera.gameObject.SetActive(true);
        gpsCamera.enabled = true;

        //we want to know when we hit the triggerbox of a poi, so trigger of the Collider need
        //to be true
        gpsCamera.gameObject.GetComponent<BoxCollider>().isTrigger = true;

        //we cannot have to Audiolisteners active at the same time, so here we switch to the
        //one the GPS cam
        arCamera.GetComponent<AudioListener>().enabled = false;
        gpsCamera.GetComponent<AudioListener>().enabled = true;

    }

    public void ShowArCamera()
    {
        //for now we are looking through our ARCamera, so we dont want the GPS Cam to
        //trigger anything
        gpsCamera.GetComponent<BoxCollider>().isTrigger = false;

        //Dont turn of the whole GPSCam Object as in the background the NavMesh Agent still
        //needs to do its work, so we can smoothly switch back to the GPS View
        gpsCamera.enabled = false;
        arCamera.enabled = true;

        arCamera.GetComponent<AudioListener>().enabled = true;
        gpsCamera.GetComponent<AudioListener>().enabled = false;

    }
}
