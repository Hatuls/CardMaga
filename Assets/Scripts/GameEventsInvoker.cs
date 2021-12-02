using Keywords;
using UnityEngine.Events;

public class GameEventsInvoker : MonoSingleton<GameEventsInvoker>
{
    public UnityEvent OnStartTurn;
    public UnityEvent OnEndTurn;

    public UnityEvent OnSelectCard;
    public UnityEvent OnZoomCard;

    public UnityEvent OnHeal;

    public UnityEvent OnShield;

    public UnityEvent OnRecieveDamage;

    public override void Init()
    {
    
    }

    public void OnKeywordActivate (KeywordAbst keywordData )
    {
        switch (keywordData.Keyword)
        {
   
         
            case KeywordTypeEnum.Attack:
                OnRecieveDamage?.Invoke();
                break;
            case KeywordTypeEnum.Shield:
                OnShield?.Invoke();
                break;
            case KeywordTypeEnum.Heal:
                OnHeal?.Invoke();
                break;
            case KeywordTypeEnum.Strength:
                break;
            case KeywordTypeEnum.Bleed:
                break;
            case KeywordTypeEnum.MaxHealth:
                break;
            case KeywordTypeEnum.Interupt:
                break;
            case KeywordTypeEnum.Weak:
                break;
            case KeywordTypeEnum.Vulnerable:
                break;
            case KeywordTypeEnum.Fatigue:
                break;
            case KeywordTypeEnum.Regeneration:
                break;
            case KeywordTypeEnum.Dexterity:
                break;
            case KeywordTypeEnum.Draw:
                break;
            case KeywordTypeEnum.MaxStamina:
                break;
            case KeywordTypeEnum.LifeSteal:
                break;
            case KeywordTypeEnum.Remove:
                break;
            case KeywordTypeEnum.Counter:
                break;
            case KeywordTypeEnum.Coins:
                break;
            case KeywordTypeEnum.StaminaShards:
                break;
            case KeywordTypeEnum.Discard:
                break;
            case KeywordTypeEnum.Double:
                break;
            case KeywordTypeEnum.Stamina:
                break;
            case KeywordTypeEnum.Fast:
                break;
            case KeywordTypeEnum.Freeze:
                break;
            case KeywordTypeEnum.Burn:
                break;
            case KeywordTypeEnum.Lock:
                break;
            case KeywordTypeEnum.Empower:
                break;
            case KeywordTypeEnum.Reinforce:
                break;
            case KeywordTypeEnum.Clear:
                break;
            case KeywordTypeEnum.Find:
                break;
            case KeywordTypeEnum.Shuffle:
                break;
            case KeywordTypeEnum.Push:
                break;
            case KeywordTypeEnum.Stun:
                break;
            case KeywordTypeEnum.StunShard:
                break;
            case KeywordTypeEnum.RageShard:
                break;
            case KeywordTypeEnum.Rage:
                break;
            case KeywordTypeEnum.ProtectionShard:
                break;
            case KeywordTypeEnum.Protected:
                break;
            case KeywordTypeEnum.Reset:
                break;
            case KeywordTypeEnum.SpiritLoss:
                break;
            case KeywordTypeEnum.Brused:
                break;
            case KeywordTypeEnum.Frail:
                break;
            case KeywordTypeEnum.Confuse:
                break;
            case KeywordTypeEnum.Deny:
                break;
            case KeywordTypeEnum.Intimidate:
                break;
            case KeywordTypeEnum.Taunt:
                break;
            case KeywordTypeEnum.Disable:
                break;
            case KeywordTypeEnum.Limit:
                break;
            case KeywordTypeEnum.BloodLoss:
                break;
            case KeywordTypeEnum.None:
            default:

                break;
        }
    }


}
