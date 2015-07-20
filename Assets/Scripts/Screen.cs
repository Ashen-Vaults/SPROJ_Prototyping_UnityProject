using UnityEngine;
using System.Collections;

public class Screen : MonoBehaviour {

    // screen size as angle
    public float currentAngleArea;
    
    // the borders used by the screen
    public Border ccwBorder;
    public Border cwBorder;

    // the mesh of the screen
    public ScreenMesh screenMesh;

    // target vectors following the border points
    private Vector3 ccwTarget;
    private Vector3 cwTarget;

    // DEBUG LineRenderers
    private LineRenderer cwLR;
    private LineRenderer ccwLR;

    // screen manager
    private ScreenManager screenManager;

    // speed that the targets follow the Borders
    private float _screenSpeed = 5f;
    public float screenSpeed
    {
        get
        {
            return _screenSpeed;
        }
        set
        {
            _screenSpeed = value;
        }
    }


	// Use this for initialization
	void Start () {
        // check for borders
        if (cwBorder == null || ccwBorder == null)
        {
            Debug.Log(transform.name + " is missing its border(s)");
            this.enabled = false;
        }

        // DEBUG linerenderer setup
        ccwLR = transform.Find("ccwDebug").GetComponent<LineRenderer>();
        ccwLR.SetVertexCount(2);
        cwLR = transform.Find("cwDebug").GetComponent<LineRenderer>();
        cwLR.SetVertexCount(2);

        // set initial target positions
        ccwTarget = ccwBorder.outerPoint.position;
        cwTarget = cwBorder.outerPoint.position;

        // set initial angle area
        currentAngleArea = ccwBorder.currentAngle - cwBorder.currentAngle;

        // find screen manager
        screenManager = FindObjectOfType<ScreenManager>();
	}
	
	// Update is called once per frame
    void Update()
    {
        // update internal data
        UpdateAngle();

        // DEBUG draw lines
        ccwLR.SetPosition(0, ccwBorder.centerPoint.position);
        ccwLR.SetPosition(1, ccwTarget);
        cwLR.SetPosition(0, cwBorder.centerPoint.position);
        cwLR.SetPosition(1, cwTarget);

        // tell mesh to update
        //screenMesh.UpdatePoints(cwTarget, ccwTarget);
    }

    void UpdateAngle()
    {
        // update targets
        ccwTarget = Vector3.Slerp(ccwTarget, ccwBorder.outerPoint.position, Time.deltaTime * screenSpeed);
        cwTarget = Vector3.Slerp(cwTarget, cwBorder.outerPoint.position, Time.deltaTime * screenSpeed);
        
        // TODO update angle of area
        currentAngleArea = Vector3.Angle(cwTarget, ccwTarget);
        

        // knock out screen if area will be <= 0
        if (currentAngleArea <= 0 && screenManager != null)
        {
            //screenManager.ScreenLose(this);
        }
    }


    void SetBorders(Border ccw, Border cw)
    {
        ccwBorder = ccw;
        cwBorder = cw;
        this.enabled = true;
        this.Start();
    }


    /// <summary>
    /// (DEPRECATED: moved into ScreenManager)
    /// alters the screen size and re-sizes other borders accordingly
    /// </summary>
    /// <param name="deltaSize"> the change in screen size to be applied (in angles) </param>
    public void AlterSize(float deltaSize)
    {
        float moveAmount = deltaSize;

        ccwBorder.Move(moveAmount);
        ccwBorder.ccwBorder.Move(moveAmount - (deltaSize / 3 /*numberOfPlayers - 1 - numberOfLockedScreens*/));

        cwBorder.Move(-moveAmount);
        cwBorder.cwBorder.Move(-(moveAmount - (deltaSize / 3 /*numberOfPlayers - 1 - numberOfLockedScreens*/)));
    }
}
