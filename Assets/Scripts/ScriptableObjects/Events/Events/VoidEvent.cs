namespace Unity.Events
{
    [UnityEngine.CreateAssetMenu(fileName ="New Void Event" , menuName = "Unity Events/Void Event")]
    public class VoidEvent : BaseGameEvent<Void>
    {
        public void Raise() => Raise(new Void());
    }

}