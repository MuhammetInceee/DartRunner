using System;
using System.Collections.Generic;
using UnityEngine;

namespace MuhammetInce.Helpers.ControlScripts
{
    public class ControlFromTagAndLayer : MonoBehaviour
    {
        [Tooltip("If you want to control from tag, check this")]
        [SerializeField] private bool wannaFromTag;

        [SerializeField] private string tagOrLayer;

        [SerializeField] private GameObject[] tagGameObjects;
        [SerializeField] private List<GameObject> layerGameObjects;


        private void Start()
        {
            if(tagOrLayer == null) return;
            
            ControlInit();


        }

        private void ControlInit()
        {
            if (wannaFromTag)
            {
                tagGameObjects = GameObject.FindGameObjectsWithTag(tagOrLayer);
            }
            else
            {
                foreach (var o in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    var obj = (GameObject) o;
                    if((obj.layer = int.Parse(tagOrLayer)) != 0)
                        layerGameObjects.Add(obj);
                }
            }
        }
    }
}