using UnityEngine;

public class PlayFabFlags : PersistentManager<PlayFabFlags>
{
    public bool Inventory;
    public bool Currency;
    public bool TitleData;
    public bool Catalog;

    public override void Awake()
    {
        base.Awake();
        Inventory = false;
        Currency = false;
        TitleData = false;
        Catalog = false;
    }

    public bool IsInit()
    {
        return Inventory && Currency && TitleData && Catalog;
    }

    public void Reset(Component sender, object data)
    {
        Inventory = false;
        Currency = false;
        TitleData = false;
        Catalog = false;
    }
}