using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<TamingArea> tamingAreas = new List<TamingArea>();
    [SerializeField] TamingArea startArea;

    [SerializeField] List<Animal> animals;
    [SerializeField] LayerMask animalLayerMask;
    [SerializeField] LayerMask uiLayerMask;

    public UnityEvent<Animal> OnSelectedAnimalChanged;

    Animal _selectedAnimal = null;
    public Animal SelectedAnimal { get => _selectedAnimal; set
        {
            _selectedAnimal = value;
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

    public void CreateNewAnimal()
    {
        var prefab = animals[Random.Range(0, animals.Count)];
        var point = startArea.GetRandomPoint();
        var animal = Instantiate(prefab, point, prefab.transform.rotation);
        animal.TamingArea = tamingAreas[Random.Range(0, tamingAreas.Count)];
    }
}
