// Generic parent class for data stored in a Module (i.e. Talents, Augments)
using UnityEngine;

public abstract class Module : ScriptableObject
{
    protected byte moduleID;
    protected string moduleName;
}
