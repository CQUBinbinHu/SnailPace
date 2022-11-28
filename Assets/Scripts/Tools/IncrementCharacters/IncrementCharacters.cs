using UnityEngine;

namespace DefaultNamespace.Tools.IncrementScoreCharacters
{
    public static class IncrementCharacters
    {
        // Increment and save a string that goes from a to z, then za to zz, zza to zzz etc.
        public static string GetStr()
        {
            // Get the current score string
            string incrementalScoreString = PlayerPrefs.GetString(nameof(incrementalScoreString), "a");

            // Get the current character
            char incrementalCharacter = PlayerPrefs.GetString(nameof(incrementalCharacter), "a")[0];

            // If the previous character we added was 'z', add one more character to the string
            // Otherwise, replace last character of the string with the current incrementalCharacter
            if (incrementalScoreString[incrementalScoreString.Length - 1] == 'z')
            {
                // Add one more character
                incrementalScoreString += incrementalCharacter;
            }
            else
            {
                // Replace character
                incrementalScoreString = incrementalScoreString.Substring(0, incrementalScoreString.Length - 1) + incrementalCharacter.ToString();
            }

            // If the letter int is lower than 'z' add to it otherwise start from 'a' again
            if ((int)incrementalCharacter < 122)
            {
                incrementalCharacter++;
            }
            else
            {
                incrementalCharacter = 'a';
            }

            // Save the current incremental values to PlayerPrefs
            PlayerPrefs.SetString(nameof(incrementalCharacter), incrementalCharacter.ToString());
            PlayerPrefs.SetString(nameof(incrementalScoreString), incrementalScoreString.ToString());

            // Return the updated string
            return incrementalScoreString;
        }
    }
}