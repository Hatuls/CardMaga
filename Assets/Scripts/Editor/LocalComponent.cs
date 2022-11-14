using System;
using UnityEngine;
// WIP
public class LocalComponent : PropertyAttribute
{
    private Type _componentType;
    public LocalComponent(Type typeOfComponent) :base()
    {
        _componentType = typeOfComponent;
    }
    

}
