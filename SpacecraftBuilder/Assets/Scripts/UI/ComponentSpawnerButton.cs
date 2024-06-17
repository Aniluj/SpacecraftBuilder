using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentSpawnerButton : MonoBehaviour, IPointerDownHandler
{
    private SpacecraftComponent spawnedComponent;
    [SerializeField] private ComponentType componentToSpawn;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            SpawnComponent();
        }
    }

    private void SpawnComponent()
    {
        spawnedComponent = SpacecraftComponentsFactory.Instance.GetSpacecraftComponent(componentToSpawn);
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnedComponent.transform.position = newPosition;
    }
}
