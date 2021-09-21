using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ModifiedEvent();
[System.Serializable]
public class ModifiableInt 
{
    [SerializeField]
    private int baseValue;
    public int BaseValue{get {return baseValue;} set{baseValue = value;  UpdateModifiedEvent();}}

    [SerializeField]
    private int modifiedValue;
    public int ModifiedValue {get {return modifiedValue;} private set{modifiedValue = value;} }

    public List<Imodifier> modifiers = new List<Imodifier>();

    public event ModifiedEvent valueModified;
    public ModifiableInt(ModifiedEvent method = null)
    {
        modifiedValue = BaseValue;
        if(method != null)
        {
            valueModified += method;
        }
    }

    public void RegisterModEvent(ModifiedEvent method)
    {
        valueModified += method;
    }

    public void UnregisterModEvent(ModifiedEvent method)
    {
        valueModified -= method;
    }

    public void UpdateModifiedEvent()
    {
        var valueToAdd = 0;
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].AddValue(ref valueToAdd);
        }

        modifiedValue = baseValue + valueToAdd;
        if(valueModified != null)
            valueModified.Invoke();
    }

    public void AddModifier(Imodifier _modifier)
    {
        modifiers.Add(_modifier);
        UpdateModifiedEvent();
    }

    public void RemoveModifier(Imodifier _modifier)
    {
        modifiers.Remove(_modifier);
        UpdateModifiedEvent();
    }
}
