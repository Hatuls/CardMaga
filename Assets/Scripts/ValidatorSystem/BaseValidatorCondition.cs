using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using UnityEngine;

namespace CardMaga.ValidatorSystem
{
    public abstract class BaseValidatorCondition<T> : ScriptableObject , IValid<T>
    {
        public abstract bool Valid(T obj);
    }
    
    public abstract class MetaCardDataValidatorCondition : BaseValidatorCondition<MetaCardData>
    {
    }
    
    public abstract class MetaCollectionCardDataValidatorCondition : BaseValidatorCondition<MetaCollectionCardData>
    {
    }
}