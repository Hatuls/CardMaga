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
        public class MaxEnergyStat: UshortStat
        {
            public EnergyStat Energy { get; set; }
            public MaxEnergyStat(ushort value) : base (value)
            {

            }
        }
        public class EnergyStat : UshortStat
        {

          public MaxEnergyStat MaxEnergy { get; set; }
            public EnergyStat(ushort _value) : base (_value)
            {
               
            }
            public bool CheckAndAddValue(ushort value)
            {
                if (value <= 0)
                {
                    throw new Exception("AccountEnergyData added value is negative or 0");
                }
                if (Value >= MaxEnergy.Value)
                {
                    return false;
                }
                else
                {
                    if(Value + value > MaxEnergy.Value)
                    {
                        ushort dif = (ushort)(MaxEnergy.Value - Value);//////////
                        _value += dif;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        #endregion

    }
}