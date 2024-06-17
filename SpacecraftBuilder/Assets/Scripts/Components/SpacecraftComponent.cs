using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.EventSystems;

public enum ComponentState
{
    None = -1,
    Canceled = 0,
    Placed = 1,
}

public abstract class SpacecraftComponent : MonoBehaviour
{
    protected ComponentState componentState = ComponentState.None;
    public IObjectPool<SpacecraftComponent> SpacecraftComponentsPool;

    private bool checkingDrag;

    protected virtual void PlaceComponent() { }

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
            }
            else if(Input.GetMouseButtonUp(0))
            {
                checkingDrag = false;
                SpacecraftComponentsPool.Release(this);
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
