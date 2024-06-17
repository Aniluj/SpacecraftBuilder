using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

public enum ComponentState
{
    None = -1,
    Canceled = 0,
    Placed = 1,
}

public abstract class SpacecraftComponent : MonoBehaviour
{
    [SerializeField] protected BoundsInt _componentSizeInBounds;
    [SerializeField] protected TileBase _tileAssigned;
    protected ComponentState componentState = ComponentState.None;
    private bool checkingDrag;
    public IObjectPool<SpacecraftComponent> SpacecraftComponentsPool;
    public BoundsInt ComponentSizeInBounds => _componentSizeInBounds;

    protected virtual void PlaceComponent()
    {
        Vector3Int componentPositionInCell = GameManager.Instance.BuildingSimulationManager.BuildingSpace.LocalToCell(transform.position);
        BoundsInt auxComponentSize = _componentSizeInBounds;
        auxComponentSize.position = componentPositionInCell;

        GameManager.Instance.BuildingSimulationManager.PlaceInBuildingArea(auxComponentSize, _tileAssigned);

        componentState = ComponentState.Placed;

        checkingDrag = false;
    }

    protected virtual bool CanPlaceComponent()
    {
        Vector3Int componentPositionInCell = GameManager.Instance.BuildingSimulationManager.BuildingSpace.LocalToCell(transform.position);
        BoundsInt auxComponentSize = _componentSizeInBounds;
        auxComponentSize.position = componentPositionInCell;

        return GameManager.Instance.BuildingSimulationManager.CanUseBuildingSpace(auxComponentSize, this);
    }

    protected virtual void CheckComponentPlacement()
    {
        if (CanPlaceComponent())
        {
            PlaceComponent();
        }
        else
        {
            checkingDrag = false;
            SpacecraftComponentsPool.Release(this);
        }
    }

    private void OnMouseEnter()
    {
        checkingDrag = true;
    }

    private void Update()
    {
        if(checkingDrag)
        {
            if(Input.GetMouseButton(0))
            {
                transform.position = GetMousePosition();

                Vector3Int cellPos = GameManager.Instance.BuildingSimulationManager.BuildingSpace.WorldToCell(GetMousePosition());
                transform.position = GameManager.Instance.BuildingSimulationManager.BuildingSpace.CellToLocalInterpolated(cellPos);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                CheckComponentPlacement();
            }
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 updatedMousePosition = new Vector3(mousePosition.x, mousePosition.y, 1);

        return updatedMousePosition;
    }
}
