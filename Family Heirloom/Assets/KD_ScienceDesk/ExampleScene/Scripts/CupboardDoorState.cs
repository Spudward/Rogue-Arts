using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardDoorState : MonoBehaviour
{
    //  // checking to see if drawer is open/opening (true) or closed/closing (false)
    private bool cupBoardState = false;
    public bool hasChecked = false;
    public bool isMoving = false;
    public string cupBoardSide;

    private float y; // Axis to rotate
    public float rotSpeed; // Speed to open and close

    // Opens or closes drawers
    public void ToggleCupBoardDoors()
    {
        if (!hasChecked)
        {
            if (cupBoardState)
            {
                cupBoardState = false;
            }
            else
            {
                cupBoardState = true;
            }
            hasChecked = true;
        }
    }

    // Checks to see if the cupBoard is moving and if so checks to see how much open or close it is and currently clamps it at 90 degrees open. 
    void Update()
    {
        if (cupBoardState && isMoving)
        {
            y += Time.deltaTime * rotSpeed;
            if(y < 90)
            {
                transform.localRotation = Quaternion.Euler(0, y, 0);
            } else
            {
                isMoving = false;
            }
        }
        else if (!cupBoardState && isMoving)
        {
            y -= Time.deltaTime * rotSpeed;
            if(y > 0)
            {
                transform.localRotation = Quaternion.Euler(0, y, 0);
            }
            else
            {
                isMoving = false;
            }
        }
    }
}
