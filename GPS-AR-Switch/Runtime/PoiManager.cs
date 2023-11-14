using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class is on each POI, here you can declare what happens if we walk into a poi
/// Also it starts the Distance-Checker once you are in the POI, so if a customizable
/// Distance is reached, you can switch back to the GPS Cam, or prepare the user to switch, etc.
/// </summary>
public class PoiManager : MonoBehaviour
{
    private GameObject arCam;

    //this is for debugging purposes, you can later make the boolean bInPoi private, as we
    //don't need to access it from anywhere
    public bool bInPoi = false;
    float distanceToPoi;
    [SerializeField] float exitFloat;

    CamSwitcher _camSwitcher;

    void Start()
    {
        _camSwitcher = FindObjectOfType<CamSwitcher>();
        arCam = _camSwitcher.arCamera.gameObject;

        if(exitFloat == 0)
        {
            Debug.LogWarning("The exit float cannot be zero. Please define at what distance " +
                "from the center of the poi you want to change back to GPSCam");
        }
    }

    void Update()
    {
        if (bInPoi)
        {
            CheckDistance();
        }
    }

    //This function is to determine when we leave the POI and for demonstrational purposes
    //is switching back to the GPS Cam. Can of course do something else, just make sure that
    //at some point you switch back to the GPSCam/GPSNavigation.
    private void CheckDistance()
    {
        distanceToPoi = Vector3.Distance(gameObject.transform.position, arCam.transform.position);

        if (distanceToPoi >= exitFloat)
        {
            //reset DistanceToPoi if we renter to this particular Poi in the future
            distanceToPoi = 0;

            bInPoi = false;

            _camSwitcher.ShowGPSCamera();
        }
    }

    //This function gets called from the Trigger Checker, if the GPS Cam hits the collider
    //of the POI-GameObject. Here you can define what happens once you enter the POI. For now
    //this is automatically switch to the more specific AR Camera after 1.1 Seconds (this is the 
    //time we need to prepare the ARSession Origin and so on.)
    public IEnumerator DoStuffInPoi()
    {
        yield return new WaitForSeconds(1.1f);
        
        bInPoi = true;
       
        _camSwitcher.InducingChangeToArCam();
    }
    
}
