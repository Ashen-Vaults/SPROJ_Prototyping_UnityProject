using UnityEngine;
using System.Collections;

public class Border : MonoBehaviour
{
    // TEMP: debug auto move
    public float deltaOverTime;

    // angle data
    public float startingAngle;
    public float currentAngle;

    // border points
    public Transform centerPoint;
    public Transform outerPoint;

    // adjacent borders
    public Border cwBorder;
    public Border ccwBorder;

    // DEBUG LineRenderer 
    //private LineRenderer lr;


    public Border(float startingAngle)
    {
        this.startingAngle = startingAngle;
    }


    // Use this for initialization
    void Awake()
    {
        // find the border points
        centerPoint = transform.Find("centerPoint");
        outerPoint = transform.Find("outerPoint");

        // DEBUG linerenderer
        //lr = gameObject.GetComponent<LineRenderer>();
        //lr.SetVertexCount(2);

        // set initial rotation
        currentAngle = startingAngle;
        transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
    }


    public void SetAngle(float newAngle)
    {
        currentAngle = newAngle;
        transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
    }


    // Update is called once per frame
    void Update()
    {
        // DEBUG line renderer
        //lr.SetPosition(0, centerPoint.position);
        //lr.SetPosition(1, outerPoint.position);

        // DEBUG auto move
        //Move(deltaOverTime);
    }


    /// <summary>
    ///  rotates the outerPoint around centerPoint by deltaAngle 
    /// </summary>
    /// <param name="deltaAngle"> the amount to move the angle by </param>
    public void Move(float deltaAngle)
    {
        //Debug.Log(transform.name + " moving " + deltaAngle);

        currentAngle += deltaAngle;
        if (currentAngle < 0)
        {
            currentAngle += 360;
        }
        else if (currentAngle >= 360)
        {
            currentAngle -= 360;
        }
        
        transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
    }
}