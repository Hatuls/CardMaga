using System;
using System.Threading.Tasks;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountEnergyData : ILoadFirstTime
    {
        #region Fields
 
        public MaxEnergyStat MaxEnergy;
   
        public EnergyStat Energy;
        #endregion

        #region Properties


        public async Task NewLoad()
        {
   
            const int energy = 999;
            MaxEnergy = new MaxEnergyStat((ushort)DefaultVersion._gameVersion.MaxEnergy);
            Energy = new EnergyStat(energy);

            Energy.MaxEnergy = MaxEnergy;
           
        }
        #endregion


        #region Energy Classes
        [System.Serializable]
        public class MaxEnergyStat: UshortStat
        {
        

            public MaxEnergyStat() : base()
            {

            }
            public MaxEnergyStat(ushort value) : base (value)
            {

            }

           
        }
        [System.Serializable]
        public class EnergyStat : UshortStat
        {
            [SerializeField]
            private MaxEnergyStat _maxEnergy;
            public EnergyStat() : base()
            {

            }
            public EnergyStat(ushort _value) : base (_value)
            {
               
            }

            public MaxEnergyStat MaxEnergy { get => _maxEnergy; set => _maxEnergy = value; }

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