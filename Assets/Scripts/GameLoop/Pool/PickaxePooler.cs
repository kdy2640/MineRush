using UnityEngine;

public class PickaxeArgs : PoolArgs
{

}
public class PickaxePooler : Pooler<PickaxeActor>
{ 
    public override PickaxeActor Get(PoolArgs args)
    {
        return base.Get(args);
    }
}
