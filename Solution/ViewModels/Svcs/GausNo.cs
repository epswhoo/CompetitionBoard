

namespace ViewModels.Svcs
{
    public static class GausNo
    {
        public static int Get(int dividend, int divisor)
        {
            int result = dividend / divisor;
            int rest = dividend % divisor;
            if (rest > 0)
                return result + 1;
            else
                return result;
        }
    }
}
