using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Process
{
    public class MultiLock : IMultiLock, IDisposable
    {
        private static readonly Dictionary<string, object> Locks = new();
        private readonly string[] keys;

        public MultiLock()
        {
            
        }

        private MultiLock(params string[] keys)
        {
            this.keys = keys;
            
            lock (Locks)
            {
                foreach (var key in keys.Where(k => !Locks.ContainsKey(k)))
                {
                    Locks[key] = new object();   
                }
            }
            
            foreach (var key in keys)
            {
                Monitor.Enter(Locks[key]);   
            }
        }

        public void Dispose()
        {
            Array.Reverse(keys);
            foreach (var key in keys)
            {
                Monitor.Exit(Locks[key]);    
            }
        }

        public IDisposable AcquireLock(params string[] keys)
        {
            Array.Sort(keys);
            // Сортировка должна исключать возможность дедлоков:
            // Допустим в потоке 1 мы смогли зааблокировать какие-то ресурсы и сейчас хотим заблокировать ресурс X
            // Но ресурс X уже заблокирован в потоке 2
            // Но из-за сортировки поток 2 уже никогда не захочет обратиться к заблокированным ресурсам потока 1
            // Потому что поток 1 заблокировал все ресурсы меньше X, а второй поток будет дальше пытаться блокировать только ресурсы больше X  
            return new MultiLock(keys);   
        }
    }
}