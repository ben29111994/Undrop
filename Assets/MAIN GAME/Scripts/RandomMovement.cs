using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public List<float> timeMovement = new List<float>();
    public List<float> timeRotate = new List<float>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator Movement()
    {
        for(int i = 0; i < timeMovement.Count - 1; i++)
        {
            yield return new WaitForSeconds(0.5f);       
        }
    }

    IEnumerator Rotate()
    {
        for(int i = 0; i < timeRotate.Count - 1; i++)
        {
            yield return new WaitForSeconds(0.5f);       
        }
    }
}
