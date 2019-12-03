using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanningFun.Logic
{
    public static class Test
    {
        public static TOut As<TIn, TOut>(this TIn self, Func<TIn, TOut> map) =>
            map(self);

        public static T Do<T>(this T self, Action<T> action)
        {
            if (self != null)
                action(self);

            return self;
        }




        static int a = Convert.ToInt32(5.As(x => x * 2).ToString());
    }
}
