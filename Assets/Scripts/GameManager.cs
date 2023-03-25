using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<TamingArea> tamingAreas = new List<TamingArea>();
    [SerializeField] TamingArea _startArea;
    public TamingArea StartArea { get => _startArea; }

    [SerializeField] List<Animal> animals;
    [SerializeField] LayerMask animalLayerMask;
    [SerializeField] GameObject selectionIndicator;
    [SerializeField] Vector3 selectionIndicatorLocalShift;

    public UnityEvent<Animal> OnSelectedAnimalChanged;

    Animal _selectedAnimal = null;
    public Animal SelectedAnimal { get => _selectedAnimal; set
        {
            _selectedAnimal = value;
            UpdateSelectionIndicator();
            OnSelectedAnimalChanged.Invoke(_selectedAnimal);
        } }

    private int _mouseDownButton = -1;
    private Animal _mouseDownAnimal = null;

    void Start()
    {
        for (int i = 0; i < 20; ++i)
        {
            CreateNewAnimal();
        }
        UpdateSelectionIndicator();
    }

    private void Update()
    {
        if (IsMouseOverUI())
        {
            _mouseDownButton = -1;
        }

        bool needHandleMouse = false;
        for (int mouseButton = 0; mouseButton < 2; ++mouseButton)
        {
            if (Input.GetMouseButtonDown(mouseButton) || Input.GetMouseButtonUp(mouseButton))
            {
                needHandleMouse = true;
                break;
            }
        }
        if (needHandleMouse)
        {
            Animal hitAnimal = null;
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cameraRay, out RaycastHit hit, 1000f, animalLayerMask))
            {
                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    hitAnimal = hit.collider.gameObject.CompareTag("Animal") ? hit.collider.gameObject.GetComponent<Animal>() : null;
                }
            }
            for (int mouseButton = 0; mouseButton < 2; ++mouseButton)
            {
                if (Input.GetMouseButtonDown(mouseButton))
                {
                    _mouseDownButton = mouseButton;
                    _mouseDownAnimal = hitAnimal;
                    break;
                }
                if (Input.GetMouseButtonUp(mouseButton))
                {
                    if (mouseButton == _mouseDownButton && hitAnimal == _mouseDownAnimal)
                    { 
                        HandleClick(hitAnimal, mouseButton);
                    }
                    _mouseDownButton = -1;
                }
            }
        }
        
    }

    bool IsMouseOverUI()
    {
        PointerEventData eventData = new(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults.Count > 0;
    }

    void HandleClick(Animal animal, int mouseButton)
    {
        if (mouseButton == 0)
        {
            SelectedAnimal = animal;
        }
        if (animal != null)
        {
            animal.OnMouseClick(mouseButton);
        }
    }

    void UpdateSelectionIndicator()
    {
        if (_selectedAnimal == null && selectionIndicator.transform.parent != null)
        {
            selectionIndicator.transform.parent = null;
            selectionIndicator.SetActive(false);
        } else if (_selectedAnimal != null && selectionIndicator.transform.parent != _selectedAnimal.gameObject.transform)
        {
            selectionIndicator.transform.parent = _selectedAnimal.transform;
            selectionIndicator.transform.localPosition = selectionIndicatorLocalShift;
            selectionIndicator.SetActive(true);
        }
    }

    public void CreateNewAnimal()
    {
        var prefab = animals[Random.Range(0, animals.Count)];
        var point = _startArea.GetRandomPoint();
        var animal = Instantiate(prefab, point, prefab.transform.rotation);
        animal.TamingArea = tamingAreas[Random.Range(0, tamingAreas.Count)];
    }
}
