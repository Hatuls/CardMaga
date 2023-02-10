using System;
using ReiTools.TokenMachine;

namespace CardMaga.Server.Request
{
    public abstract class BaseServerRequest : IServerRequest
    {
        private IDisposable _token;

        protected abstract void ServerLogic();//temp, will move to the server side
        
        protected virtual void ReceiveResult()
        {
            _token.Dispose();
        }

        public virtual void SendRequest(ITokenReceiver tokenReciever)
        {
            _token = tokenReciever.GetToken();
            ServerLogic();
        }
    }
    
    public interface IServerRequest
    {
        void SendRequest(ITokenReceiver tokenReciever);
    }
}

