using System;

namespace VetSolutionRationLib.Helpers
{
    public static class SearchHelpers
    {
        public static string[] SplitByWhitspace(string input)
        {
            return input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        } 
    }
}