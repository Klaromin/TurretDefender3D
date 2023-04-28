using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInterface : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    [SerializeField] private GameObject _rocketTurret;
    [SerializeField] private GameObject _gattlingTurret;
    [SerializeField] private GameObject _flamingTurret;
    
    [SerializeField] private Button _rocketButton;
    [SerializeField] private Button _gattlingButton;
    [SerializeField] private Button _flamingButton;
    [SerializeField] private Button _closeButton;
    
    [SerializeField] private GameObject _turretMenu;
    private delegate void CreateTurret();
    private GameObject _itemPrefab;
    private GameObject _focusObj;
    

    private void Start()
    {
        AddEvents();
    }

    private void OnDisable()
    {
        RemoveEvents();
    }
    
    void Update()
    {
        //Mobil kontroller için; 
        // 1. if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        // 2. if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        // 3. if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //Input.mousePosition => Input.GetTouch(0).position
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("turret"))
            {
                _turretMenu.transform.position = Input.mousePosition;
               _turretMenu.SetActive(true); 
            }
            
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
                
                _focusObj.GetComponent<Collider>().enabled = true;
                _focusObj.GetComponent<SphereCollider>().enabled = true;
            }
            else
            {
                Destroy(_focusObj);
            }
            _focusObj = null;
        }
    }
    
    private void CreateItemForButton()
    {
        RaycastHit hit;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit))
            return;

        _focusObj = Instantiate(_itemPrefab, hit.point, _itemPrefab.transform.rotation);
        _focusObj.GetComponent<Collider>().enabled = false;
        _focusObj.GetComponent<SphereCollider>().enabled = false;
    }
    
    private void CreateRocket()
    {
        _itemPrefab = _rocketTurret;
        CreateItemForButton();
    }

    private void CreateGattling()
    {
        _itemPrefab = _gattlingTurret;
        CreateItemForButton();
    }
    
    private void CreateFlaming()
    {
        _itemPrefab = _flamingTurret;
        CreateItemForButton();
    }

    private void CloseTurretMenu()
    {
        _turretMenu.SetActive(false);
    }

    private void AddEvents()
    {
        OnPointerDown(_rocketButton, CreateRocket);
        OnPointerDown(_gattlingButton, CreateGattling);
        OnPointerDown(_flamingButton, CreateFlaming);
        _closeButton.onClick.AddListener(CloseTurretMenu);
    }
    
    private void RemoveEvents()
    {
        _gattlingButton.onClick.RemoveAllListeners();;
        _rocketButton.onClick.RemoveAllListeners();
        _closeButton.onClick.RemoveAllListeners();
    }

    private void OnPointerDown(Button button, CreateTurret createTurret)
    {
        
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDown.callback.AddListener(e => createTurret());
        trigger.triggers.Add(pointerDown);
    }
}
