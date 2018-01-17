/////////////////////////////////////////////////////////////////////
// Messages.cs - defines communication messages                    //
// ver 1.0                                                         //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2016 //
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * Messages provides helper code for building and parsing XML messages.
 *
 * Required files:
 * ---------------
 * - Messages.cs
 * 
 * Maintanence History:
 * --------------------
 * ver 1.0 : 16 Oct 2016
 * - first release
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

namespace SharedCode
{
    public class BlockingQueue<T>
    {
        private Queue blockingQ;
        object locker_ = new object();

        //----< constructor >--------------------------------------------

        public BlockingQueue()
        {
            blockingQ = new Queue();
        }
        //----< enqueue a string >---------------------------------------

        public void enQ(T msg)
        {
            lock (locker_)  // uses Monitor
            {
                blockingQ.Enqueue(msg);
                Monitor.Pulse(locker_);
            }
        }
        //----< dequeue a T >---------------------------------------
        //
        // Note that the entire deQ operation occurs inside lock.
        // You need a Monitor (or condition variable) to do this.

        public T deQ()
        {
            T msg = default(T);
            lock (locker_)
            {
                while (this.size() == 0)
                {
                    Monitor.Wait(locker_);
                }
                msg = (T)blockingQ.Dequeue();
                return msg;
            }
        }
        //
        //----< return number of elements in queue >---------------------

        public int size()
        {
            int count;
            lock (locker_) { count = blockingQ.Count; }
            return count;
        }
        //----< purge elements from queue >------------------------------

        public void clear()
        {
            lock (locker_) { blockingQ.Clear(); }
        }

    }

#if (TEST_BLOCKINGQUEUE)
    class TestStub
    {
        static void Main(string[] args)
        {
            Console.Write("\n  Testing Monitor-Based Blocking Queue");
            Console.Write("\n ======================================");

            BlockingQueue<string> q = new BlockingQueue<string>();
            Thread t = new Thread(() =>
            {
                string msg;
                while (true)
                {
                    msg = q.deQ(); Console.Write("\n  child thread received {0}", msg);
                    if (msg == "quit") break;
                }
            });
            t.Start();
            string sendMsg = "msg #";
            for (int i = 0; i < 20; ++i)
            {
                string temp = sendMsg + i.ToString();
                Console.Write("\n  main thread sending {0}", temp);
                q.enQ(temp);
            }
            q.enQ("quit");
            t.Join();
            Console.Write("\n\n");
        }
#endif
}
