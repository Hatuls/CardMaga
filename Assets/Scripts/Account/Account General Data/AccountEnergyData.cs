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

        public bool IsCorrupted()
            => Energy.Value <= 0&& MaxEnergy.Value<=0;

        #endregion

        #region Properties


        public void NewLoad()
        {
   
            const int energy = 999;
            MaxEnergy = new MaxEnergyStat((ushort)DefaultVersion._gameVersion.MaxEnergy);
            Energy = new EnergyStat(energy);

            Energy.MaxEnergy = MaxEnergy;
           
        }
        #endregion


        #region Energy Classes
        [System.Serializable]
        public class MaxEnergyStat: IntStat
        {
        

         
            public MaxEnergyStat(int value) : base (value)
            {

            }

           
        }
        [System.Serializable]
        public class EnergyStat : IntStat
        {
            [SerializeField]
            private MaxEnergyStat _maxEnergy;
         
            public EnergyStat(int _value) : base (_value)
            {
               
            }

            public MaxEnergyStat MaxEnergy { get => _maxEnergy; set => _maxEnergy = value; }

            public bool CheckAndAddValue(int value)
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
                        int dif = (MaxEnergy.Value - Value);//////////
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