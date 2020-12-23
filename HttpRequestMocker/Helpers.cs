using System;

namespace RepairsApi.Tests.ApiMocking
{
    public static class Helpers
    {
        public static void Assert(bool check)
        {
            if (!check) throw new Exception("Assert failed check call stack");
        }
    }
}
