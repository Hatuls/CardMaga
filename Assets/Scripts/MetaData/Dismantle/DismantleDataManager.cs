using CardMaga.MetaData.AccoutData;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;

namespace CardMaga.MetaData.Dismantle
{
    public class DismantleDataManager : ISequenceOperation<MetaDataManager>
    {
        private DismantleHandler _dismantleHandler;
        private DismantleCurrencyHandler _dismantleCurrencyHandler;
        
        public int Priority => 0;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _dismantleHandler = new DismantleHandler();
            _dismantleCurrencyHandler = new DismantleCurrencyHandler();
        }

        public void AddCardToDismantleList(MetaCardData metaCardData)
        {
            _dismantleCurrencyHandler.AddCardCurrency(metaCardData);
            _dismantleHandler.AddCardToDismantleList(metaCardData);
        }

    }
}