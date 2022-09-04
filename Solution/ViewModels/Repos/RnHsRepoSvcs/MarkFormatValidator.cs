using Interfaces;
using Models.Common;
using Models.Errors;
using Models.Results;

namespace ViewModels.Repos.RnHsRepoSvcs
{
    // All the code in this file is included in all platforms.
    public static class MarkFormatValidator
    {
        private const char decimalSign = ','; 

        public static bool Validate(double mark)
        {
            string str = mark.ToString();
            if (!str.Contains(decimalSign))
                return false;
            string[] splitted = str.Split(decimalSign);
            if (splitted.Length != 2)
                return false;
            if (splitted[0].Length != 1)
                return false;
            if (splitted[1].Length != 1)
                return false;
            return true;
        }
    }
}