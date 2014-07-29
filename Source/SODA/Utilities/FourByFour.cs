using System;
using System.Text.RegularExpressions;

namespace SODA.Utilities
{
    /// <summary>
    /// Helper class for validating Socrata "4x4" resource identifiers.
    /// </summary>
    public class FourByFour
    {
        static Regex fourByFourRegex = new Regex(@"^[a-z0-9]{4}-[a-z0-9]{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Test whether the specified input string is a valid Socrata "4x4" resource identifier.
        /// </summary>
        /// <param name="testFourByFour">An input string to test.</param>
        /// <returns>True if the specified input is a valid Socrata "4x4" resource identifier. False otherwise.</returns>
        public static bool IsValid(string testFourByFour)
        {
            return !String.IsNullOrEmpty(testFourByFour) && fourByFourRegex.IsMatch(testFourByFour);
        }

        /// <summary>
        /// Test whether the specified input string is an invalid Socrata "4x4" resource identifier.
        /// </summary>
        /// <param name="testFourByFour">An input string to test.</param>
        /// <returns>True if the specified input is an invalid Socrata "4x4" resource identifier. False otherwise.</returns>
        /// <remarks>
        /// This is the logical negation of <see cref="Isvalid"/>
        /// </remarks>
        public static bool IsNotValid(string testFourByFour)
        {
            return !IsValid(testFourByFour);
        }
    }
}
