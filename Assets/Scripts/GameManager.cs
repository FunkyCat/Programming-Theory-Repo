using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<TamingArea> tamingAreas = new List<TamingArea>();
    [SerializeField] TamingArea startArea;

    [SerializeField] List<Animal> animals;

    private void Start()
    {
        for (int i = 0; i < 20; ++i)
        {
            CreateNewAnimal();
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
