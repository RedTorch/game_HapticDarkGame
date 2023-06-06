using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private bool currTagged = false;
    public Material taggedMaterial;
    public GameObject particleSystemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTagged(bool isTagged = true)
    {
        if(isTagged)
        {
            // particlefx, sound
            currTagged = true;
            transform.parent.gameObject.GetComponent<MeshRenderer>().material = taggedMaterial;
            GameObject ips = Instantiate(particleSystemPrefab, transform.position, transform.rotation);
            Destroy(ips,5f);
        }
        else
        {
            // nothing
        }
    }

    public bool isTagged()
    {
        return currTagged;
    }
}
