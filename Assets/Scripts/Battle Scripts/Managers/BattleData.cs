
using Characters;

namespace Battles
{

    public static class BattleData
    {
      
        public static MapRewards MapRewards { get; set; }
        public static bool IsFinishedPlaying;
        public static Character Player { get; set; }
        public static Character Opponent { get; set; }
        public static bool PlayerWon;
    }


    public class MapRewards
    {
        public ushort Diamonds { get; set; }
        public ushort EXP { get; set; }
        public ushort Credits { get; set; }
        public ushort Gold { get; set; }
    }
}