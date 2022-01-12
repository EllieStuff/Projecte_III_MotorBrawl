using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpots : MonoBehaviour
{
    [SerializeField] private ModifiersManager modifiers;
    [SerializeField] private bool placed;

    [SerializeField] private LayerMask layerMask;

    private void Awake()
    {
        placed = false;
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        transform.localScale = new Vector3(1,1,1);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            
            if (!placed)
            {

                transform.position = raycastHit.transform.position;
                transform.localScale = raycastHit.transform.lossyScale;
                transform.rotation = raycastHit.transform.rotation;

                if (Input.GetMouseButtonDown(0) && raycastHit.transform.childCount == 0)    //Instantiate Object
                {
                    GameObject clone = Instantiate(gameObject, raycastHit.transform).gameObject;

                    clone.transform.localScale = clone.transform.parent.parent.localScale;
                    clone.transform.localRotation = clone.transform.parent.parent.localRotation;

                    clone.transform.position = Vector3.zero;
                    clone.transform.localPosition = Vector3.zero;

                    clone.GetComponent<BuildingSpots>().SetPlaced();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(1))                                            //Remove Object
                {
                    if(transform.parent != null)
                    {
                        if(raycastHit.transform == transform.parent)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
        else
        {
            if(!placed)
            {
                Vector3 newPos = ray.origin + ray.direction * (modifiers.transform.position.z + Mathf.Abs(Camera.main.transform.position.z));
                //newPos.z = modifiers.transform.parent.transform.localPosition.z;

                Debug.Log(Camera.main.nearClipPlane);
                transform.position = newPos;
            }
        }
        Debug.DrawLine(ray.origin, transform.position, Color.red);
    }

    public void SetPlaced()
    {
        placed = true;
    }

    public void ChangeGameObject(GameObject obj)
    {
        if(transform.childCount > 0)
        {
            GameObject currentChild = transform.GetChild(0).gameObject;
            if (obj.name == currentChild.name)
            {
                return;
            }
            Destroy(currentChild);
        }
        
        Instantiate(obj, transform);
    }
}
