using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnableManager : MonoBehaviour
{

    [SerializeField]
    ARRaycastManager manager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject prefab;
    
    Camera cam;
    GameObject obj;

    public Material whiteMaterial;
    public Material redMaterial;

    // Start is called before the first frame update
    void Start()
    {
        obj = null;
        cam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 0){
            return;
        }

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);

        if(manager.Raycast(Input.GetTouch(0).position, hits)){
            if(Input.GetTouch(0).phase == TouchPhase.Began && obj == null){
                if(Physics.Raycast(ray, out hit)){
                    if(hit.collider.gameObject.tag == "Spawnable"){
                        obj = hit.collider.gameObject;
                        obj.gameObject.GetComponent<MeshRenderer>().material = redMaterial;
                    } else {
                        SpawnPrefab(hits[0].pose.position);
                    }
                }
            } else if(Input.GetTouch(0).phase == TouchPhase.Moved && obj != null){
                obj.transform.position = hits[0].pose.position;
            }
            if(Input.GetTouch(0).phase ==  TouchPhase.Ended){
                obj.gameObject.GetComponent<MeshRenderer>().material = whiteMaterial;
                obj = null;
            }
        }
    }
    private void SpawnPrefab(Vector3 spawnPos){
        obj = Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
