using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoor : MonoBehaviour
{

    public GameObject trigger;
    public GameObject door;

    Animator doorAnim;

    // Start is called before the first frame update
    void Start()
    {
        doorAnim = door.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player"|| coll.gameObject.tag == "Enemy")
        {
            slideDoor(true);
        }
    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Enemy")
        {
            slideDoor(false);
        }
    }

    void slideDoor(bool state)
    {
        doorAnim.SetBool("slide", state);
    }
}
