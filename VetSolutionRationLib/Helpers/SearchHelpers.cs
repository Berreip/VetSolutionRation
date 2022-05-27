using System;
using System.Linq;

namespace VetSolutionRationLib.Helpers
{
    public static class SearchHelpers
    {
        public static string[] SplitByWhitspaceAndSpecificSymbols(string input)
        {
            return input
                .Trim()
                // split by space, comma and parenthesis
                .Split(
                    new[] {',' , ' ', '(', ')' }, 
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        } 
    }
}