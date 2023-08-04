using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePlane : MonoBehaviour
{
    // public GameController gameController;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Target") && tag == "Lose")
        {
            Vector3 pos = new Vector3(other.contacts[0].point.x, other.contacts[0].point.y + 0.2f, other.contacts[0].point.z);
            GameController.instance.Lose();
            //GameController.instance.PlayZoneGround(pos, other.gameObject);
            other.gameObject.tag = "Untagged";
        }
        //if (other.gameObject.CompareTag("Target") && tag == "Stop")
        //{
        //    GameController.instance.NextChance();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target") && tag == "Stop")
        {
            GameController.instance.NextChance();
        }
    }
}
