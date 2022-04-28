using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private static readonly int Property = Shader.PropertyToID("_CutOffHeight");
    
    // Start is called before the first frame update
    void Start()
    {
        print(gameObject.GetComponent<MeshRenderer>().material.GetVector(Property));
        gameObject.GetComponent<MeshRenderer>().material.SetVector(Property, Vector4.zero);
        
    }
    
}
