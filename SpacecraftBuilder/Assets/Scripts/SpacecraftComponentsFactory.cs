using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public enum ComponentType
{
    engineComponent = 0,
    shieldComponent = 1,
    livingAreaComponent = 2,
    cannonComponent = 3
}

public class SpacecraftComponentsFactory : MonoBehaviour
{
    public static SpacecraftComponentsFactory Instance;

    [Header("Factory Settings")]
    [SerializeField] private int _poolMaxSize = 30;
    [SerializeField] private GameObject _poolParent;

    [Header("Factory References")]
    [SerializeField] private EngineComponent _enginePrefab;
    [SerializeField] private ShieldComponent _shieldPrefab;
    [SerializeField] private LivingAreaComponent _livingAreaPrefab;
    [SerializeField] private CannonComponent _cannonPrefab;

    private Dictionary<ComponentType, SpacecraftComponent> componentsDictionary = new Dictionary<ComponentType, SpacecraftComponent>();
    private Dictionary<ComponentType, ObjectPool<SpacecraftComponent>> componentPoolsDictionary = new Dictionary<ComponentType, ObjectPool<SpacecraftComponent>>();

    private ComponentType currentlySelectedComponentType = ComponentType.livingAreaComponent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        componentsDictionary.Add(ComponentType.engineComponent, _enginePrefab);
        componentsDictionary.Add(ComponentType.shieldComponent, _shieldPrefab);
        componentsDictionary.Add(ComponentType.livingAreaComponent, _livingAreaPrefab);
        componentsDictionary.Add(ComponentType.cannonComponent, _cannonPrefab);

        AddComponentPool<EngineComponent>();
        AddComponentPool<ShieldComponent>();
        AddComponentPool<LivingAreaComponent>();
        AddComponentPool<CannonComponent>();
    }

    private void AddComponentPool<T>() where T : SpacecraftComponent
    {
        ComponentType componentTypeAssigned = componentsDictionary.FirstOrDefault(x => x.Value.GetType() == typeof(T)).Key;

        componentPoolsDictionary.Add(componentTypeAssigned, new ObjectPool<SpacecraftComponent>(CreateComponent<T>, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, true, 10, _poolMaxSize));
    }

    public SpacecraftComponent GetSpacecraftComponent(ComponentType v_componentType)
    {
        currentlySelectedComponentType = v_componentType;
        return componentPoolsDictionary[currentlySelectedComponentType].Get();
    }

    private SpacecraftComponent CreateComponent<T>() where T : SpacecraftComponent
    {
        T v_spaceCraftComponent = Instantiate((T)componentsDictionary[currentlySelectedComponentType], _poolParent != null ? _poolParent.transform : this.transform);
        v_spaceCraftComponent.SpacecraftComponentsPool = componentPoolsDictionary[currentlySelectedComponentType];
        return v_spaceCraftComponent;
    }

    private void OnGetFromPool(SpacecraftComponent pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(SpacecraftComponent pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(SpacecraftComponent pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}