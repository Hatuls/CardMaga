using System;
using ReiTools.TokenMachine;

namespace CardMaga.Server.Request
{
    public abstract class BaseServerRequest : IServerRequest
    {
        private IDisposable _token;

        protected abstract void ServerLogic();//temp, will move to the server side
        
        protected void ReceiveResult()
        {
            _token.Dispose();
        }

        public virtual void SendRequest(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            ServerLogic();
        }
    }
    
    public interface IServerRequest
    {
        void SendRequest(ITokenReciever tokenReciever);
    }
}

