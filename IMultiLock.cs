using System;

namespace Process
{
    public interface IMultiLock
    {
        public IDisposable AcquireLock(params string[] keys);
    }
}