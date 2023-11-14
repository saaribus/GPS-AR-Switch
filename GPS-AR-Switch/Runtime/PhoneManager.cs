using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneManager : MonoBehaviour
{
    public static PhoneManager pM;

    [Header("Basic Setup")]
    [Space(7)]
    [Tooltip("Reference the PositionTarget from the Hierarchy, if you have none, create an empty Game Object and reference it")]
    //verbindung mit der Kamera in Unity (world = unity)
    [SerializeField] private GameObject positionTarget;
    private Camera worldCam;

    //Kamera
    private WebCamTexture phoneCameraTexture;
    [Tooltip("Reference the Background Canvas. If its not in your hierarchy put it there, its in the Prefabsfolder")]
    [SerializeField] Canvas BackgroundCanvas;
    private RawImage background;
    private AspectRatioFitter fit;

    //Rotation
    Gyroscope gyro;
    Compass compass;

    //vom Sensor
    private Quaternion gyroRotation;
    private Vector3 worldPosition;
    private float compassHeading;

    float gpsAccuracy = 3f;
    float gpsInterval = 0.1f;
    private double lat, lon;

    [Header("Latitude and Longitude of Point of Reference")]
    [Space(7)]
    [Tooltip("Set the float to the Latitude of your chosen Point of Reference")]
    public float referenceLatitude;
    [Tooltip("Set the float to the Longitude of your chosen Point of Reference")]
    public float referenceLongitude;

    [Space(10)]
    [Header("Show the actual Lat/Lon on UI")]
    [Space(7)]
    //aktuelle lat und lon auf Display anzeigen 
    [SerializeField] TextMeshProUGUI DebugLatitude;
    [SerializeField] TextMeshProUGUI DebugLongitude;

    //DebugingText auf HandyDisplay anzeigen
    //public Text DebugText;

    private void Awake()
    {
        pM = this;

        //Setting Reference to the MainCamera
        worldCam = Camera.main;

        //Setting up Background Canvas
        background = BackgroundCanvas.GetComponentInChildren<RawImage>();
        fit = background.GetComponent<AspectRatioFitter>();
        

    }

    // Use this for initialization
    void Start()
    {
        if(referenceLatitude == 0 || referenceLongitude == 0)
        {
            Debug.LogError("The Latitude and Longitude of the Point of Reference can " +
                "not be Zero. Please set the float of ReferenceLatitude and ReferenceLongitude");
                
        }

        if (positionTarget == null)
        {
            Debug.LogError("The PositionTarget has not been referenced. Create an " +
                "empty Game Object and refrence it to the PositionTarget in the Phone Manager Script.");
        }


        //GPS on
        Input.location.Start(gpsAccuracy, gpsInterval);

        //set up gyro and compass
        gyro = Input.gyro;
        gyro.enabled = true;

        compass = Input.compass;
        compass.enabled = true;

        //ich schaue in den Computer rein und oben ist oben 
        //Quaternion: hier steht drin wie etwas in der Welt hängt
        gyroRotation = Quaternion.LookRotation(new Vector3(0, 0, 1), new Vector3(0, 1, 0));

        //turn on camera
        if (!Application.isEditor)
        {
            phoneCameraTexture = new WebCamTexture(Screen.width, Screen.height, 30);
            phoneCameraTexture.Play();
            background.texture = phoneCameraTexture;

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isEditor)
        {

            //get data from sensor
            GetSensorData();

            //figure out phone back camera scaling
            OrientPhoneCamera();

            //figure out unity virtual camera orientation
            OrientVirtualCamera();

            //transform our position and rotation to Unity coordinates and translate our virtual camera
            TranslateGPSToUnity();

            //move the camera
            MoveCamera();
        }

    }

    void GetSensorData()
    {
        //hier kriegen wir die aktuellen GPS Koordinaten des Telefons
        //werden ca. alle 10 Sekunden geschickt
        lat = Input.location.lastData.latitude;
        lon = Input.location.lastData.longitude;

        SetGPSText();

        //so schnell wie möglich werden die Gyroskopdaten geupdated
        gyro.updateInterval = 0.0f;
        gyroRotation = gyro.attitude;

        //Kompass
        compassHeading = Input.compass.magneticHeading;

    }

    // Das Kamerabild auf dem samrtphone wird immer der Displaygrösse angepasst. (aus Internet)
    void OrientPhoneCamera()
    {
        float ratio = (float)phoneCameraTexture.width / (float)phoneCameraTexture.height;
        fit.aspectRatio = ratio;
        float scaleY = phoneCameraTexture.videoVerticallyMirrored ? -1.0f : 1.0f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
        int orient = -phoneCameraTexture.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    void OrientVirtualCamera()
    {
        //unityCamera wird rotiert, und zwar so wie das Telefon geneigt ist
        if (worldCam)
        {
            worldCam.transform.rotation = gyroRotation;
        }

        //das Telefon braucht zwei extra Drehungen
        if (!Application.isEditor)
        {
            worldCam.transform.Rotate(0, 0, 180, Space.Self);
            worldCam.transform.Rotate(90, 0, 0, Space.World);
        }
    }

    void TranslateGPSToUnity()
    {
        //dlat (latitude distance) ist wie weit ich von StartKoordinate weg bin
        double dlat = referenceLatitude - lat;
        double dlon = referenceLongitude - lon;

        //umrechnung von lat und lon in xy koordinaten, laut formel für mitteleuropa (internet)
        double xPosition = dlon * 75118;
        double zPosition = dlat * 111300;

        //Vector wo ich in Unity tatsächlich bin
        worldPosition = new Vector3((float)xPosition, 2.0f, (float)zPosition);
    }

    void MoveCamera()
    {
        //mit Navmesh (das Target wird bewegt und die kamera folgt) 
        positionTarget.transform.position = worldPosition;
    }

    void SetGPSText()
    {
        if(!DebugLatitude|| !DebugLongitude)
        {
            Debug.LogError("The Variable DebugLatitude and DebugLongitude have not been set. " +
                "Set a reference to two UI Text Objects if you want to see your current GPS Coordinates on the screen");

        }
        else
        {
            DebugLatitude.text = "lat:" + lat;
            DebugLongitude.text = "long:" + lon;
        }

        

    }
}
