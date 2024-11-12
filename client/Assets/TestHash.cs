using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HashSet<int> set = new HashSet<int>(10);
        set.Add(1);
        set.Add(2);
        set.Add(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
