namespace PLCompliant.Utilities
{
    /// <summary>
    /// This class contains methods for easing the controls of the GUI
    /// </summary>
    public static class StringUtilities
    {
        /// <summary>
        /// This method looks for the previous index of some char in a string when looking from an index
        /// </summary>
        /// <param name="str">The string to search</param>
        /// <param name="ch">The char to look for</param>
        /// <param name="index">The index to look back from</param>
        /// <returns>The index from which the value was found minus 3 </returns>
        public static int PreviousIndexOfAndFixToSeparator(this string str, char ch, int index)
        {
            if (index < 0) return -1;
            if (index > str.Length - 1) index = str.Length - 1;
            for (int i = index; i >= 0; i--)
            {

                if (str[i] == ch) return i - 3;
            }


            return -1;
        }
    }
}
