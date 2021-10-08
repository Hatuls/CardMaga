namespace Characters.Stats
{
    public static class CharacterStatsManager {

       private static CharacterStatsHandler _player;
       private static CharacterStatsHandler _opponent;

        public static void RegisterCharacterStats(bool isPlayer, ref CharacterStats stat)
        {
            if (isPlayer)
                _player = new CharacterStatsHandler(isPlayer, ref stat);
            else
                _opponent = new CharacterStatsHandler(isPlayer,ref stat);
        }
        public static CharacterStatsHandler GetCharacterStatsHandler(bool playerStats)
                => playerStats ? _player : _opponent; 
    }

}