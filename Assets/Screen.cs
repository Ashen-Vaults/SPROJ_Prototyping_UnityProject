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
    public Transform ccwDebug;
    public Transform cwDebug;

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
        if (cwBorder == null || ccwBorder == null)
        {
            Debug.Log(transform.name + " is missing its border(s)");
            this.enabled = false;
        }

        ccwTarget = ccwBorder.outerPoint.position;
        cwTarget = cwBorder.outerPoint.position;
        ccwDebug.position = ccwTarget;
        cwDebug.position = cwTarget;

        currentAngleArea = ccwBorder.currentAngle - cwBorder.currentAngle;
	}
	
	// Update is called once per frame
    void Update()
    {
        // update internal data
        UpdateAngle();

        // tell mesh to update
        //screenMesh.UpdatePoints(cwTarget, ccwTarget);
    }

    void UpdateAngle()
    {
        // update targets
        ccwTarget = Vector3.Slerp(ccwTarget, ccwBorder.outerPoint.position, Time.deltaTime * screenSpeed);
        cwTarget = Vector3.Slerp(cwTarget, cwBorder.outerPoint.position, Time.deltaTime * screenSpeed);

        //debug
        ccwDebug.position = ccwTarget;
        cwDebug.position = cwTarget;

        currentAngleArea = Vector3.Angle(cwTarget, ccwTarget);
    }


    /// <summary>
    /// alters the screen size and re-sizes other borders accordingly
    /// </summary>
    /// <param name="deltaSize"> the change in screen size to be applied (in angles) </param>
    public void AlterSize(float deltaSize)
    {
        float moveAmount = deltaSize;

        if (cwBorder.locked && ccwBorder.locked)
        {
            return;
        }
        else if (!cwBorder.locked && !ccwBorder.locked)
        {
            moveAmount /= 2;
        }

        ccwBorder.Move(moveAmount);
        ccwBorder.ccwBorder.Move(moveAmount - (deltaSize / 3 /*numberOfPlayers - 1 - numberOfLockedScreens*/));

        cwBorder.Move(-moveAmount);
        cwBorder.cwBorder.Move(-(moveAmount - (deltaSize / 3 /*numberOfPlayers - 1 - numberOfLockedScreens*/)));
    }
}
