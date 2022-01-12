
using System;
using System.Collections.Generic;

namespace TokenFactory
{

    // Token Machine
    public sealed class TokenMachine
    {
        private Action OnActivatateTask;
        private Action OnFinishTask;

        private List<WeakReference<Token>> _registerTokens;
        public TokenMachine(Action onActivation = null, Action onFinish = null)
        {
            _registerTokens = new List<WeakReference<Token>>();
            OnActivatateTask += onActivation;
            OnFinishTask += onFinish;

        }

        ~TokenMachine()
        {
            OnActivatateTask = null;
            OnFinishTask = null;
        }
        private void RegisterToken(Token token)
        {

            if (token == null)
                return;

            WeakReference<Token> weakRefToken = new WeakReference<Token>(token);

            if (_registerTokens.Contains(weakRefToken))
            {
                Console.WriteLine("Already Has this token");
                return;
            }


            //first Token
            if (_registerTokens.Count == 0)
                OnActivatateTask?.Invoke();

            Console.WriteLine("Registering new Token");
            _registerTokens.Add(weakRefToken);
            Console.WriteLine("Token Register Count " + _registerTokens.Count);
        }
        public void CheckReferences()
        {
            Token token;
            for (int i = 0; i < _registerTokens.Count; i++)
            {
                if (!_registerTokens[i].TryGetTarget(out token))
                {
                    _registerTokens.RemoveAt(i);
                }

            }
            if (_registerTokens.Count == 0)
            {
                Console.WriteLine("Activating On Finish Task");
                OnFinishTask?.Invoke();
            }
        }
        private Token TryGetValue(WeakReference<Token> weakRef)
        {
            if (weakRef != null && weakRef.TryGetTarget(out Token target))
                return target;
            return null;
        }
        private void ReleaseToken(Token t)
        {
            if (t == null)
                return;
            Token token;
            for (int i = 0; i < _registerTokens.Count; i++)
            {
                token = TryGetValue(_registerTokens[i]);
                if (token != null)
                {
                    if (token == t)
                    {
                        _registerTokens.RemoveAt(i);
                        break;
                    }
                }
                else
                {
                    _registerTokens.RemoveAt(i);
                }

            }
            Console.WriteLine($"Removing Token\nRemaining {_registerTokens.Count}");

            if (_registerTokens.Count == 0)
            {
                Console.WriteLine("Activating On Finish Task");
                OnFinishTask?.Invoke();
            }
        }
        public void ForceRelease()
        {
            for (int i = 0; i < _registerTokens.Count; i++)
                TryGetValue(_registerTokens[i])?.Dispose();
        }
        public Token CreateToken()
        {
            Token t = new Token(ReleaseToken);
            RegisterToken(t);
            return t;
        }

    }
        public sealed class Token : IDisposable
        {
            private Action<Token> OnRelease;
            public Token(Action<Token> onRelease)
            {
                OnRelease += onRelease;
            }
            public void Dispose()
            {
                OnRelease.Invoke(this);
            }

            ~Token() => Dispose();
        }
}