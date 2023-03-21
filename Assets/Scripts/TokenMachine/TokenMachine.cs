using System;
using System.Collections.Generic;
namespace ReiTools.TokenMachine
{
    [Serializable]
    public class UnityTokenMachineEvent : UnityEngine.Events.UnityEvent<ITokenReceiver> { }
    public interface ITokenReceiver
    {
        Token GetToken();
    }
    public sealed class TokenMachine : ITokenReceiver, IDisposable
    {
        public event Action OnLock = null;
        public event Action OnRelease = null;

        private readonly List<WeakReference<Token>> _tokens = new List<WeakReference<Token>>();
        public int TokenCount => _tokens.Count;
        public bool Locked => TokenCount > 0;

        public bool Released => TokenCount == 0;

        public TokenMachine(Action OnLock = null, Action OnRelease = null) : this(OnRelease)
        {
            if (OnLock != null)
                this.OnLock += OnLock;
        }
        public TokenMachine(Action OnRelease = null)
        {
            if (OnRelease != null)
                this.OnRelease += OnRelease;
        }
        public Token GetToken()
        {
            Token newToken = new Token();
            AddToken(newToken);
            return newToken;
        }
        public void ForceRelease()
        {
            int count = _tokens.Count;
            for (int i = 0; i < count; i++)
                GetTokenFromReference(_tokens[i]).Dispose();

        }
        private Token GetTokenFromReference(WeakReference<Token> reference)
        {
            if (reference != null && reference.TryGetTarget(out var token))
                return token;
            return null;
        }

        private static bool IsValid(WeakReference<Token> tokenRef)
            => tokenRef != null && tokenRef.TryGetTarget(out Token token) && IsValid(token);

        private static bool IsValid(Token token)
            => token != null && !token.Released;

        private void AddToken(Token newToken)
        {
            if (_tokens.Find((x) => x.TryGetTarget(out Token token) && token == newToken) != null)
                return;

            _tokens.Add(new WeakReference<Token>(newToken));

            if (_tokens.Count == 1)
                OnLock?.Invoke();

            newToken.OnRelease += TokenReleased;

            void TokenReleased(Token releasedToken)
            {
                int index = _tokens.FindIndex((x) => x.TryGetTarget(out Token token) && token == releasedToken);

                if (index == -1)
                    return;

                _tokens.RemoveAt(index);
                releasedToken.OnRelease -= TokenReleased;
                if (_tokens.Count == 0)
                    OnRelease?.Invoke();
            }
        }

        public void Dispose()
        {
            ForceRelease();
        }
    }
    public sealed class Token  :IDisposable
    {
        private bool _released = false;
        public bool Released => _released;
        /// <summary>
        /// When the Check-In count reaches 0.
        /// </summary>
        public event Action<Token> OnRelease = default;
        ~Token()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_released)
                return;

            _released = true;
            OnRelease?.Invoke(this);
        }
    }
}