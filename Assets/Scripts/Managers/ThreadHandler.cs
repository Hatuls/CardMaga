using System.Collections.Generic;
using UnityEngine;
using System.Threading;


namespace ThreadsHandler
{
    public static class ThreadHandler
    {
        static List<ThreadList> _threads = new List<ThreadList>();

        public static byte GetNewID
        {
            get
            {
                byte idCache = 0;
                bool containID;

                do
                {
                    idCache = (byte)Random.Range(0, sizeof(byte));
                    containID = false;

                    for (int i = 0; i < _threads.Count; i++)
                    {
                        if (_threads[i].ID == idCache)
                        {
                            containID = true;
                            break;
                        }
                    }

                } while (containID);


                return idCache;
            }
        }
        // return the ThreadList
        public static void StartThread(ThreadList thread)
        {
            if (thread == null)
                return;

            if (_threads.Contains(thread))
            {
                ThreadList threadCache = GetThreadFromList(thread);
                threadCache.AbortThread();
            }

            if (thread.IsThreadAlive)
                thread.AbortThread();

            _threads.Add(thread);

            thread.thread = new Thread(
                new ThreadStart(thread.actionOnStartThread)
                );

            thread.thread.Start();
        }
        public static void TickThread()
        {
            if (_threads.Count == 0)
                return;

            for (int i = 0; i < _threads.Count; i++)
            {
                if (_threads[i].IsThreadAlive)
                    continue;
                else
                {
                    _threads[i].AbortThread();
                    _threads[i].actionOnFinishThread?.Invoke();
                    RemoveFromList(_threads[i].ID);
                }
            }
        }

        private static ThreadList GetThreadFromList(ThreadList thread)
         => GetThreadFromList(thread);

        private static ThreadList GetThreadFromList(byte id)
        {
            for (int i = 0; i < _threads.Count; i++)
            {
                if (_threads[i].ID == id)
                    return _threads[i];
            }
            return null;
        }
        private static void RemoveFromList(ThreadList thread)
        {
            if (_threads.Contains(thread))
            {
                thread.AbortThread();
                _threads.Remove(thread);
            }
        }
        private static void RemoveFromList(byte id)
        {
            for (int i = 0; i < _threads.Count; i++)
            {
                if (_threads[i].ID == id)
                    _threads.Remove(_threads[i]);
            }
        }
        public static void ResetList()
        {

            if (_threads.Count > 0)
            {
                for (int i = 0; i < _threads.Count; i++)
                    _threads[i].AbortThread();
            }

            _threads.Clear();
        }
    }


    public class ThreadList
    {
        public byte ID;
        public bool _threadFinished;
        public Thread thread;
        public System.Action actionOnStartThread;
        public System.Action actionOnFinishThread;
        public ThreadList(byte id, System.Action ActionInThread, System.Action _actionOnFinishThread = null)
        {
            ID = id;
            actionOnStartThread = ActionInThread;
            actionOnFinishThread = _actionOnFinishThread;
        }
        public void AbortThread() => thread?.Abort();
        public bool IsThreadAlive
         => (thread != null) ? thread.IsAlive : false;
    }
}

