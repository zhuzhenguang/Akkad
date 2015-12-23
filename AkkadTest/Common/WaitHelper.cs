using System;
using System.Threading;

namespace AkkadTest
{
    public class WaitHelper
    {
        public static void WaitUntil(Func<bool> shouldBeTrue)
        {
            var count = 30;
            while (!shouldBeTrue() && count-- > 0)
            {
                Thread.Sleep(100);
            }

            if (count <= 0)
                throw new Exception("Wait too long");
        }
    }
}