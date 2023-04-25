using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInterface : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private GameObject _cubeTurret;
    private GameObject _focusObj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Mobil kontroller için; 
        // 1. if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        // 2. if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        // 3. if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //Input.mousePosition => Input.GetTouch(0).position
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit))
            return;

            _focusObj = Instantiate(_cubeTurret, hit.point, _cubeTurret.transform.rotation);
            _focusObj.GetComponent<Collider>().enabled = false;
        }
        else if (_focusObj && Input.GetMouseButton(0))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit))
                return;

            _focusObj.transform.position = hit.point ;
        }
        else if (_focusObj && Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("platform") && hit.normal.Equals(new Vector3(0,1,0)))
            {
                hit.collider.gameObject.tag = "occupied";
                var transformPosition = _focusObj.transform.position;
                transformPosition.x = hit.collider.gameObject.transform.position.x;
                transformPosition.z = hit.collider.gameObject.transform.position.z;
                _focusObj.transform.position = transformPosition;
            }
            else
            {
                Destroy(_focusObj);
                
            }
            _focusObj = null;
            
        }
    }
}
