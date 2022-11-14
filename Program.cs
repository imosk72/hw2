using System;
using System.Diagnostics;
using System.Threading;

namespace Process
{
    public static class Program
    {
        private static void Test1()
        {
            Console.WriteLine("TEST 1");
            var multiLock = new MultiLock();
            var t1 = new Thread(() =>
            {
                Console.WriteLine("Start thread 1");
                var locked = multiLock.AcquireLock("1", "2", "3");
                Console.WriteLine("Lock resources 1, 2, 3");
                Thread.Sleep(1000);
                locked.Dispose();
                Console.WriteLine("Release resources 1, 2, 3");
            }); 
            
            var t2 = new Thread(() =>
            {
                Console.WriteLine("Start thread 2");
                Thread.Sleep(100);
                var locked = multiLock.AcquireLock("2");
                Console.WriteLine("Lock resources 2");
                locked.Dispose();
                Console.WriteLine("Release resources 2");
            }); 
            
            t1.Start();
            t2.Start();
            
            t1.Join();
            t2.Join();
        }

        private static void Test2()
        {
            Console.WriteLine("TEST 2");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var multiLock = new MultiLock();
            var t1 = new Thread(() =>
            {
                var m = multiLock.AcquireLock("1", "2", "3");
                Thread.Sleep(2000);
                m.Dispose();
            }); 
            var t2 = new Thread(() =>
            {
                var m = multiLock.AcquireLock("3", "5", "6");
                Thread.Sleep(2000);
                m.Dispose();
            }); 
            var t3 = new Thread(() =>
            {
                var m = multiLock.AcquireLock("4", "7", "9");
                Thread.Sleep(4000);
                m.Dispose();
            }); 
            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
        
        public static void Main(string[] args)
        {
            Test1();
            Test2();
        }
    }
}