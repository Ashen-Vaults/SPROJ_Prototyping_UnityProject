using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {

    public int playerCount = 4;

    public GameObject screenPrefab;
    public GameObject borderPrefab;

    public List<Screen> allScreens = new List<Screen>();
    public List<Border> allBorders = new List<Border>();

    // TEMP debug keys
    private KeyCode[] key = new KeyCode[10] {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0,
    };

	void Start () {
        // TEMP? setup screens based on playerCount
        #region setup borders
        float borderAngle = 180f;
        GameObject borderObj;
        Border newBorder;
        // instantiate Border objects
        for (int i = 0; i < playerCount; i++)
        {
            borderObj = Instantiate(borderPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            borderObj.name = "Border_" + i;
            newBorder = borderObj.GetComponent<Border>();
            newBorder.Move(borderAngle);
            allBorders.Add(newBorder);
            borderObj.transform.parent = transform.Find("borders");
            borderAngle += 360f / playerCount;
        }
        // hook up adjacent borders
        for (int i = 0; i < allBorders.Count; i++)
        {
            allBorders[i].ccwBorder = allBorders[(i + 1 + allBorders.Count) % allBorders.Count];
            allBorders[i].cwBorder = allBorders[(i - 1 + allBorders.Count) % allBorders.Count];
        }
        #endregion

        #region setup screens
        GameObject screenObj;
        Screen newScreen;
        // instantiate Screen objects
        for (int i = 0; i < playerCount; i++)
        {
            screenObj = Instantiate(screenPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            screenObj.name = "Screen_" + i;
            newScreen = screenObj.GetComponent<Screen>();
            allScreens.Add(newScreen);
            screenObj.transform.parent = transform.Find("screens");
        }
        // hook up screen borders
        for (int i = 0; i < allBorders.Count; i++)
        {
            allScreens[i].ccwBorder = allBorders[i];
            allScreens[i].cwBorder = allBorders[(i - 1 + allBorders.Count) % allBorders.Count];
        }
        #endregion
    }

    
    void Update()
    {
        // TEMP debug key commands
        for (int i = 0; i < Mathf.Clamp(playerCount, 0f, key.Length); i++)
        {
            if (Input.GetKeyDown(key[i]))
            {
                ChangeScreen(allScreens[i]);
            }
        }
    }

    public void ChangeScreen(Screen screen/*, float deltaSize*/)
    {
        float deltaSize = -10f;
        float cwDelta = deltaSize / 2;
        float ccwDelta = deltaSize / 2;

        Border currentCWB = screen.cwBorder;
        Border currentCCWB = screen.ccwBorder;

        // run through borders based on number of players
        for (int i = 0; i < Mathf.CeilToInt(playerCount / 2); i++)
        {
            // move borders
            currentCWB.Move(-cwDelta);
            currentCCWB.Move(ccwDelta);

            // find next and update moveAmount
            currentCWB = currentCWB.cwBorder;
            cwDelta = (cwDelta - (deltaSize / (playerCount - 1/* - NumScreensLocked()*/)));

            currentCCWB = currentCCWB.ccwBorder;
            ccwDelta = ccwDelta - (deltaSize / (playerCount - 1/* - NumScreensLocked()*/));
        }
    }


    public void ScreenLose(Screen loser)
    {
        allScreens.Remove(loser);
        loser.gameObject.SetActive(false);

        MergeBorders(loser.ccwBorder, loser.cwBorder);

        playerCount--;
    }

    /// <summary>
    /// merges two borders together
    /// the ccwBorder is kept, all connections to the cwBorder are re-set
    /// </summary>
    /// <param name="ccwBorder">the ccw border of the merge, this border remains</param>
    /// <param name="cwBorder">the cw border of the merge, this border is knocked out</param>
    public void MergeBorders(Border ccwBorder, Border cwBorder)
    {
        Debug.Log("MERGING: " + ccwBorder.transform + " and " + cwBorder.transform);

        // set to the angle between the merged borders
        ccwBorder.SetAngle(ccwBorder.currentAngle - ((ccwBorder.currentAngle - cwBorder.currentAngle) / 2));

        // update screen border for missing one
        foreach (Screen screen in allScreens)
        {
            if (screen.ccwBorder == cwBorder)
            {
                screen.ccwBorder = ccwBorder;
            }
        }

        // update border list
        allBorders.Remove(cwBorder);
        
        // fix border neighbors for the one that's been merged
        ccwBorder.cwBorder = cwBorder.cwBorder;
        cwBorder.cwBorder.ccwBorder = ccwBorder;
        
        // turn off merged border
        cwBorder.gameObject.SetActive(false);
    }


    /*private int NumScreensLocked()
    {
        int numLocked = 0;
        foreach (Screen screen in allScreens)
        {
            if (screen.locked)
            {
                numLocked++;
            }
        }
        return numLocked;
    }*/
}
