using UnityEngine;

public class OreArgs : PoolArgs
{
    public Vector3 worldPosition;
    public OreType oreType;

    public OreArgs(Vector3 worldPosition, OreType oreType)
    {
        this.worldPosition = worldPosition;
        this.oreType = oreType;
    }
}
public class OrePooler : Pooler<OrePresentor>
{

}
