using System;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountEnergyData
    {
        #region Fields
        private MaxEnergyStat _maxEnergy;
        private EnergyStat  _energy;
        #endregion

        #region Properties
        public EnergyStat Energy { get => _energy; private set => _energy = value; }
        public MaxEnergyStat MaxEnergy { get => _maxEnergy; private set => _maxEnergy = value; }
        #endregion



        public AccountEnergyData (ushort maxEnergy, ushort energy)
        {
            MaxEnergy = new MaxEnergyStat(maxEnergy);
            Energy = new EnergyStat(energy);
            Energy.MaxEnergy = MaxEnergy;
            MaxEnergy.Energy = Energy;
        }



        #region Energy Classes
        public class MaxEnergyStat: Stat<uint>
        {
            public EnergyStat Energy { get; set; }
            public MaxEnergyStat(uint value) : base (value)
            {

            }
        }
        public class EnergyStat : Stat<uint>
        {
          public MaxEnergyStat MaxEnergy { get; set; }
            public EnergyStat(uint _value) : base (_value)
            {

            }
        }
        #endregion

    }
}