using System;

namespace Game.Client
{
    //@TODO:
    [Flags]
    public enum EVFXDespawnType:int
    {
        None =0,
        DestroyWhenReceiverEntityDie = 1 <<0,
        DestroyWhenInstigatorEntityDie = 1 << 1,
        DestroyLifeTime = 1 << 2,
        
        

    }
}