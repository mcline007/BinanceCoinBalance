using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceExecute
{
    using System.Threading;

    public sealed class QueuedLock
    {
        private readonly object _innerLock;
        private volatile int _ticketsCount = 0;
        private volatile int _ticketToRide = 1;

        public QueuedLock()
        {
            _innerLock = new Object();
        }

        public void Enter()
        {
            int myTicket = Interlocked.Increment(ref _ticketsCount);
            Monitor.Enter(_innerLock);
            while (true)
            {

                if (myTicket == _ticketToRide)
                {
                    return;
                }
                else
                {
                    Monitor.Wait(_innerLock);
                }
            }
        }

        public void Exit()
        {
            Interlocked.Increment(ref _ticketToRide);
            Monitor.PulseAll(_innerLock);
            Monitor.Exit(_innerLock);
        }
    }
}
