
using Battle;
using Managers;

namespace Characters.Stats
{
    public static class CharacterStatsManager {

        // need to remove it and refactor the stats + keywords !
        public static CharacterStatsHandler GetCharacterStatsHandler(bool playerStats)
                => playerStats ? PlayerManager.Instance.StatsHandler : EnemyManager.Instance.StatsHandler; 
    }

}