using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerState : MonoBehaviour
{

    // checking to see if drawer is open/opening (true) or closed/closing (false)
    private bool drawerState = false;
    public bool hasChecked = false;
    public string drawerSide;

    // Opens or closes drawers
    public void ToggleDrawers() 
    {
        if (!hasChecked)
        {
            if (drawerState)
            {
                drawerState = false;
            }
            else
            {
                drawerState = true;
            }
            hasChecked = true;
        }
    }

    // Checks the state of the opening bool and then checks to see if the drawer is side A or B and then moves them forward or back in local space. 
    // You can create a more complete version by setting the input from the scenecontroller to be a specifically named drawer instead of all drawers on side A or B as I have. 
    // That way you would not need to check the drawerSide name variable as I have done here. This is just done this way for the example scene. 
    void Update()
    {
        // Opening the drawers
        if (drawerState)
        {
            if (transform.localPosition.z < 0.5f && drawerSide == "A")
            {
                transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
            }
            if (transform.localPosition.z > -0.5f && drawerSide == "B")
            {
                transform.Translate(Vector3.back * Time.deltaTime, Space.Self);
            }
        } else // Closing the Drawers. Check is done not at 0.0 because then it will have them moving after the 0.0 check and will move them too far in
        {
            if (transform.localPosition.z > 0.1f && drawerSide == "A")
            {
                transform.Translate(Vector3.back * Time.deltaTime, Space.Self);
            }
            if (transform.localPosition.z < -0.1f && drawerSide == "B")
            {
                transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
            }
        }
    }
}
