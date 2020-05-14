using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionLevel : MonoBehaviour
{
    const int MAX_INFECTION = 100;
    public const int MIN_INFECTION = 0;
    public const int INFECTION_DMG = 10;
    public int currentInfection;
    public bool attacked;
    public GameObject enemy;

    public player_MOVEMENT_christos movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<player_MOVEMENT_christos>();

        //set infection to MIN_INFECTION (0)
        currentInfection = MIN_INFECTION;
        CheckMutation();
        attacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if enemy collides with player, increase infection by INFECTION_DMG (10)
        if (attacked)
        {
            //add infection damage, check to see if mutation level has changed
            currentInfection += INFECTION_DMG;
            CheckMutation();
            attacked = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //if you collide with the seeker, be attacked
        if(other.gameObject.CompareTag("Enemy"))
        {
            attacked = true;
        }
    }

    void CheckMutation()
    {
        //TODO Add mutation effects (could be speed, visibility etc)
        //changes stats depending on infection level
        if(currentInfection <= 25)
        {
            //set to default speed and height
            movement.speed = 12f;
            movement.jumpHeight = 3f;
        }
        else if(currentInfection > 25 && currentInfection <= 50)
        {
            //speed increased by 4f
            movement.speed = 16f;
        }
        else if(currentInfection > 50 && currentInfection <= 75)
        {
            //jump height increased by 2f
            movement.jumpHeight = 5f;
        }
        else if(currentInfection > 75)
        {
            //speed increased by another 4f
            //jump height increased by another 2f
            movement.speed = 20f;
            movement.jumpHeight = 7f;
        }
    }
}
