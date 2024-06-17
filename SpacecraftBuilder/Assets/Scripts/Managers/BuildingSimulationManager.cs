using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSimulationManager : MonoBehaviour
{
    [SerializeField] private GridLayout _buildingSpace;
    [SerializeField] private Tilemap _tilemapUsed;
    [SerializeField] private TileBase _blockedSpaceTile;
    [SerializeField] private TileBase _freeSpaceTile;
    public GridLayout BuildingSpace => _buildingSpace;
    public Tilemap TilemapUsed => _tilemapUsed;

    public void PlaceInBuildingArea(BoundsInt v_area, TileBase v_tileToPlace)
    {
        TileBase[] tileBaseArray = new TileBase[v_area.size.x * v_area.size.y];
        FillTilesWithComponentSpace(tileBaseArray, v_tileToPlace);

        _tilemapUsed.SetTilesBlock(v_area, tileBaseArray);
    }

    public void FillTilesWithComponentSpace(TileBase[] v_tilesUsedToFill, TileBase v_tilebaseType)
    {
        for (int i = 0; i < v_tilesUsedToFill.Length; i++)
        {
            v_tilesUsedToFill[i] = v_tilebaseType;
        }
    }

    public void ResetBuildingArea(BoundsInt v_area)
    {
        PlaceInBuildingArea(v_area, null);
    }

    public bool CanUseBuildingSpace(BoundsInt v_area, SpacecraftComponent v_componentToPlace)
    {
        TileBase[] tileBaseArray = _tilemapUsed.GetTilesBlock(v_area);

        foreach (TileBase tileBase in tileBaseArray)
        {
            if (tileBase == _blockedSpaceTile || tileBase != _freeSpaceTile)
            {
                return false;
            }
        }

        return true;
    }
}