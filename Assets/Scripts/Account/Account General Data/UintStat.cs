using System;
namespace Account.GeneralData
{

   public class UintStat : Stat<uint>
   {
       public UintStat(uint val) : base(val)
       {

       }
       public override bool AddValue(uint value)
       {
           if (value <= 0)
           {
               throw new Exception("UintStat value inserted is lower or equal to 0");
           }
           _value += value;
           return true;
       }
       public override bool CheckStat(uint value)
       {
           if (value <= 0)
           {
               throw new Exception("UintStat value inserted is lower or equal to 0");
           }
           if (_value >= value)
           {
               return true;
           }
           return false;
       }
       public override bool ReduceValue(uint value)
       {
           if (value <= 0)
           {
               throw new Exception("UintStat value inserted is lower or equal to 0");
           }
           if (_value < value)
           {
               return false;
           }
           else
           {
               _value -= value;
               return true;
           }
       }
   }




}