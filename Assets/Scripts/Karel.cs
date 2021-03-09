/*
 * Author: Zachary Coon
 * Date: 2/27/2017
 * This script contains all of Karel's commands that another script can call
 * Last Updated: 1/25/2018 by Zachary Coon
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum directionFacing
{
    facingNorth,
    facingEast,
    facingSouth,
    facingWest
}

public enum Instructions
{
    move,
    turnLeft,
    putBeeper,
    pickBeeper
}

public class Karel : MonoBehaviour {

    [Tooltip("How many beepers Karel has in his bag. A value of 0 to 101 is acceptable, anything less than 0 will be 0 and 101 or greater will make it so that karel has an infinte number of beepers in his bag")]
    public int beepers_in_bag = 0; //How many beepers karel has in his bag. If this number starts out as 101 or above, then karel will have an infinite ammount of beepers

    [Tooltip("This is where you put the beeper prefab. If you don't put this here, then I will freak out!!")]
    public GameObject beeper;

    [Range(0.0f, .2f)]
    public float speed = .1f;

    private bool infinite_beepers = false;
    private bool front_is_clear = true;
    private bool next_to_a_beeper = false;
    private GameObject beeper_colliding_with = null;
    private directionFacing direction = directionFacing.facingEast;
    public bool custom_karel_mode = false;
    private bool is_karel_dead = false;

    private List<Instructions> instructions;
    private Vector3 start_position;
    private int beepers_in_bag_start = 0;
    private directionFacing start_direction = directionFacing.facingEast;

    private void Start()
    {
        //if there are more than 100 beepers in the bag to start with, then infinite mode is activated
        if (beepers_in_bag >= 101)
        {
            infinite_beepers = true;
        }
        //if the user put less than 0 by accident, set the beepers in bag to equal to 0
        else if (beepers_in_bag < 0)
        {
            beepers_in_bag = 0;
        }

        instructions = new List<Instructions>();
    }

    public void startKarel()
    {
        if (!custom_karel_mode)
        {
            custom_karel_mode = true;
            start_position = transform.position;
            beepers_in_bag_start = beepers_in_bag;
            start_direction = direction;
            is_karel_dead = false;
        }
    }

    /*
     * move
     * move one unit in the direction you are facing
     */

    public void move()
    {
        if(frontIsClear() && !is_karel_dead)
        {
            if (direction == directionFacing.facingEast)
            {
                transform.position = transform.position + Vector3.right;
                if (custom_karel_mode)
                    instructions.Add(Instructions.move);
            }
            else if (direction == directionFacing.facingNorth)
            {
                transform.position = transform.position + Vector3.up;
                if (custom_karel_mode)
                    instructions.Add(Instructions.move);
            }
            else if (direction == directionFacing.facingWest)
            {
                transform.position = transform.position + Vector3.left;
                if (custom_karel_mode)
                    instructions.Add(Instructions.move);
            }
            else if (direction == directionFacing.facingSouth)
            {
                transform.position = transform.position + Vector3.down;
                if (custom_karel_mode)
                    instructions.Add(Instructions.move);
            }
            else
            {
                Debug.LogError("I don't seem to be facing a direction. Karel.cs:move");
            }
        }
        else
        {
            if (!is_karel_dead)
            {
                Debug.LogError("ERROR! Karel ran into a wall and has stopped");
                is_karel_dead = true;
            }
        }
    }

    private IEnumerator move(int item_number)
    {
        yield return new WaitForSeconds(item_number * speed);
        if (direction == directionFacing.facingEast)
        {
            transform.position = transform.position + Vector3.right;
        }
        else if (direction == directionFacing.facingNorth)
        {
            transform.position = transform.position + Vector3.up;
        }
        else if (direction == directionFacing.facingWest)
        {
            transform.position = transform.position + Vector3.left;
        }
        else if (direction == directionFacing.facingSouth)
        {
            transform.position = transform.position + Vector3.down;
        }
        else
        {
            Debug.LogError("I don't seem to be facing a direction. Karel.cs:move");
        }
    }

    

    /*
     * turnLeft
     * rotate Karel 90 degrees then keep track of what direction karel is facing
     */
    public void turnLeft()
    {
        if (!custom_karel_mode && is_karel_dead)
            is_karel_dead = false;

        if (!is_karel_dead)
        {
            transform.Rotate(new Vector3(0, 0, 90));
            if (direction == directionFacing.facingEast)
            {
                direction = directionFacing.facingNorth;
            }
            else if (direction == directionFacing.facingNorth)
            {
                direction = directionFacing.facingWest;
            }
            else if (direction == directionFacing.facingWest)
            {
                direction = directionFacing.facingSouth;
            }
            else if (direction == directionFacing.facingSouth)
            {
                direction = directionFacing.facingEast;
            }
            else
            {
                Debug.LogError("I don't seem to be facing a direction. Karel.cs:turnLeft");
            }
            if (custom_karel_mode)
                instructions.Add(Instructions.turnLeft);
        }
    }

    private IEnumerator turnLeft(int item_number)
    {
        yield return new WaitForSeconds(item_number * speed);
        transform.Rotate(new Vector3(0, 0, 90));
        if (direction == directionFacing.facingEast)
        {
            direction = directionFacing.facingNorth;
        }
        else if (direction == directionFacing.facingNorth)
        {
            direction = directionFacing.facingWest;
        }
        else if (direction == directionFacing.facingWest)
        {
            direction = directionFacing.facingSouth;
        }
        else if (direction == directionFacing.facingSouth)
        {
            direction = directionFacing.facingEast;
        }
        else
        {
            Debug.LogError("I don't seem to be facing a direction. Karel.cs:turnLeft");
        }
    }


    /*
     * finsih
     */ 
    public void finish()
    {
        custom_karel_mode = false;
        transform.position = start_position;

        while(start_direction != direction)
        {
            turnLeft();
        }

        beepers_in_bag = beepers_in_bag_start;

        for(int index = 0; index < instructions.Count; index++)
        {
            if(instructions[index] == Instructions.move)
            {
                StartCoroutine(move(index));
            }
            else if (instructions[index] == Instructions.turnLeft)
            {
                StartCoroutine(turnLeft(index));
            }
            else if(instructions[index] == Instructions.pickBeeper)
            {
                StartCoroutine(pickBeeper(index));
            }
            else if(instructions[index] == Instructions.putBeeper)
            {
                StartCoroutine(putBeeper(index));
            }
            else
            {
                Debug.LogError("Unexpected Error: Not a recognized instruction, karel.cs:finish");
            }
        }

        instructions.Clear();
        is_karel_dead = false;
    }

    /*
     * putBeeper
     * put a beeper down, if there is already a beeper down, update the text to keep track of how many beepers are there
     * if you don't have any beepers, do nothing
     */
    public void putBeeper()
    {
        if (!custom_karel_mode && is_karel_dead)
            is_karel_dead = false;

        if (beepers_in_bag > 0)
        {
            if (!custom_karel_mode)
            {
                beepersPresent();
                beeper_colliding_with.GetComponent<Beeper>().putBeeper(false);
                if(!infinite_beepers)
                    beepers_in_bag--;
            }
            else
            {
                beepersPresent();
                beeper_colliding_with.GetComponent<Beeper>().putBeeper(true);
                if (!infinite_beepers)
                    beepers_in_bag--;
                instructions.Add(Instructions.putBeeper);
            }
        }
    }

    private IEnumerator putBeeper(int item_number)
    {
        yield return new WaitForSeconds(item_number * speed);
        beepersPresent();
        beeper_colliding_with.GetComponent<Beeper>().putBeeper(false);
        if (!infinite_beepers)
            beepers_in_bag--;
    }

    /*
     * pickBeeper
     * if there is a beeper, pick one up
     */
    public void pickBeeper()
    {
        if (!custom_karel_mode && is_karel_dead)
            is_karel_dead = false;

        if (beepersPresent() && !is_karel_dead)
        {
            if (!custom_karel_mode)
            {
                beeper_colliding_with.GetComponent<Beeper>().pickBeeper(false);
                beepers_in_bag++;
            }
            else
            {
                beeper_colliding_with.GetComponent<Beeper>().pickBeeper(true);
                beepers_in_bag++;
                instructions.Add(Instructions.pickBeeper);
            }
        }
        else if (!beepersPresent())
        {
            if (!is_karel_dead)
            {
                Debug.LogError("ERROR! Tried to pick up a non-existant beeper and has stopped");
                is_karel_dead = true;
            }
        }
       
    }

    private IEnumerator pickBeeper(int item_number)
    {
        yield return new WaitForSeconds(item_number * speed);
        beepersPresent();
        beeper_colliding_with.GetComponent<Beeper>().pickBeeper(false);
        beepers_in_bag++;
    }

    /*
     * frontIsClear
     * accessor function
     * Return bool: returns true if the front isn't colliding with a wall, otherwise returns false
     */
    public bool frontIsClear()
    {
        if (direction == directionFacing.facingEast)
        {
            if (Physics.Raycast(transform.position, Vector3.right, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (direction == directionFacing.facingNorth)
        {
            if (Physics.Raycast(transform.position, Vector3.up, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (direction == directionFacing.facingWest)
        {
            if (Physics.Raycast(transform.position, Vector3.left, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (direction == directionFacing.facingSouth)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("I don't seem to be facing a direction. Karel.cs:frontIsClear");
            return false;
        }
        
    }

    public bool rightIsClear()
    {
        if (direction == directionFacing.facingEast)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (direction == directionFacing.facingNorth)
        {
            if (Physics.Raycast(transform.position, Vector3.right, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (direction == directionFacing.facingWest)
        {
            if (Physics.Raycast(transform.position, Vector3.up, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (direction == directionFacing.facingSouth)
        {
            if (Physics.Raycast(transform.position, Vector3.left, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("I don't seem to be facing a direction. Karel.cs:frontIsClear");
            return false;
        }
    }

    public bool leftIsClear()
    {
        if (direction == directionFacing.facingEast)
        {
            if (Physics.Raycast(transform.position, Vector3.up, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (direction == directionFacing.facingNorth)
        {
            if (Physics.Raycast(transform.position, Vector3.left, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (direction == directionFacing.facingWest)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (direction == directionFacing.facingSouth)
        {
            if (Physics.Raycast(transform.position, Vector3.right, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("I don't seem to be facing a direction. Karel.cs:frontIsClear");
            return false;
        }
    }

    /*
     * facingNorth
     * accessor function
     * Return bool: returns true if karel is facing north, otherwise returns false
     */ 
    public bool facingNorth()
    {
        return direction == directionFacing.facingNorth;
    }

    /*
     * facingEast
     * accessor function
     * Return bool: returns true if karel is facing east, otherwise returns false
     */
    public bool facingEast()
    {
        return direction == directionFacing.facingEast;
    }

    /*
     * facingSouth
     * accessor function
     * Return bool: returns true if karel is facing south, otherwise returns false
     */
    public bool facingSouth()
    {
        return direction == directionFacing.facingSouth;
    }

    /*
     * facingWest
     * accessor function
     * Return bool: returns true if karel is facing west, otherwise returns false
     */
    public bool facingWest()
    {
        return direction == directionFacing.facingWest;
    }

    /*
     * nextToABeeper
     * accessor function
     * Return bool: returns true if karel is ontop of a beeper, otherwise returns false 
     */
    public bool beepersPresent()
    {
        RaycastHit hit;
        if (!custom_karel_mode)
        {
            if (Physics.Raycast(transform.position, Vector3.forward, out hit))
            {
                beeper_colliding_with = hit.transform.gameObject;
                if (hit.transform.gameObject.GetComponent<Beeper>().number_of_beepers > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.LogError("Missing Beeper");
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.forward, out hit))
            {
                beeper_colliding_with = hit.transform.gameObject;
                if (hit.transform.gameObject.GetComponent<Beeper>().GetInvisibleBeepers() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.LogError("Missing Beeper");
            }
        }
        return false;
    }

    public bool beepersInBag()
    {
        return beepers_in_bag > 0;
    }

    //opposite functions
    public bool frontIsBlocked()
    {
        return !frontIsClear();
    }

    public bool leftIsBlocked()
    {
        return !leftIsClear();
    }

    public bool rightIsBlocked()
    {
        return !rightIsClear();
    }

    public bool noBeepersPresent()
    {
        return !beepersPresent();
    }

    public bool noBeepersInBag()
    {
        return !beepersInBag();
    }

    public bool notFacingNorth()
    {
        return direction != directionFacing.facingNorth;
    }

    public bool notFacingEast()
    {
        return direction != directionFacing.facingEast;
    }

    public bool notFacingSouth()
    {
        return direction != directionFacing.facingSouth;
    }

    public bool notFacingWest()
    {
        return direction != directionFacing.facingWest;
    }



}
