#if XNATOUCH
using XnaTouch.Framework;
using XnaTouch.Framework.Audio;
using XnaTouch.Framework.Content;
using XnaTouch.Framework.GamerServices;
using XnaTouch.Framework.Graphics;
using XnaTouch.Framework.Input;
using XnaTouch.Framework.Media;
using XnaTouch.Framework.Net;
using XnaTouch.Framework.Storage;
#else
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceCream.Debug.Console
{
    /// <summary>
    /// Defines the key modifier state for a keyboard key
    /// </summary>
    public enum KeyModifier : int
    {
        None,
        Shift,
    }

    /// <summary>
    /// ConsoleKeyMap maps a set of keys and key modifier combinations to ASCII char values
    /// </summary>
    public static class ConsoleKeyMap
    {
        private static Dictionary<Keys, Dictionary<KeyModifier, char>> mMap;

        /// <summary>
        /// Retrieves the char value for the specified key and key modifier combination.
        /// If a char is registered for said combination, then the char is assigned to
        /// 'ch' and true is returned. False is returned otherwise and 'ch' is unassigned.
        /// </summary>
        /// <param name="key">The key for which to retrieve the corresponding char value</param>
        /// <param name="mod">The key modifier for the specified key</param>
        /// <param name="ch">Will contain the requested char value for the specified key
        /// and key modifier if such a value exists in the key map, or will be unassigned
        /// if no such char value is registered</param>
        /// <returns>True if the char value for the specified key and key modifier combination
        /// was retrieved and stored in 'ch', false otherwise</returns>
        public static bool GetCharacter(Keys key, KeyModifier mod, ref char ch)
        {
            if (!mMap.ContainsKey(key))
                return false;

            if (!mMap[key].ContainsKey(mod))
                return false;

            ch = mMap[key][mod];

            return true;
        }

        /// <summary>
        /// Sets the char value for the specified key and key modifier combination
        /// </summary>
        /// <param name="key">The key for which to set the corresponding char value</param>
        /// <param name="mod">The key modifier for which to set the corresponding char value</param>
        /// <param name="ch">The char value to set for the specified key and key modifier</param>
        public static void SetCharacter(Keys key, KeyModifier mod, char ch)
        {
            if (!mMap.ContainsKey(key))
                mMap.Add(key, new Dictionary<KeyModifier, char>());

            mMap[key][mod] = ch;
        }

        /// <summary>
        /// Unsets the char value for the specified key and key modifier combination
        /// </summary>
        /// <param name="key">The key for which to unset the corresponding char value</param>
        /// <param name="mod">The key modifier for which to unset the corresponding char value</param>
        /// <returns>The char value previously set for the specified key and key modifier, or the
        /// null character ('\0') if no char value was set for the combination</returns>
        public static char UnsetCharacter(Keys key, KeyModifier mod)
        {
            if (!mMap.ContainsKey(key))
                return '\0';

            if (!mMap[key].ContainsKey(mod))
                return '\0';

            char ch = mMap[key][mod];

            mMap[key].Remove(mod);

            if (mMap[key].Count == 0)
                mMap.Remove(key);

            return ch;
        }

        /// <summary>
        /// Gets the unique set of char values registered to the ConsoleKeyMap
        /// </summary>
        /// <returns>The unique set of char values registered to the ConsoleKeyMaps</returns>
        public static List<char> GetRegisteredCharacters()
        {
            List<char> chars = new List<char>();

            foreach (Keys key in mMap.Keys)
            {
                foreach (KeyModifier mod in mMap[key].Keys)
                    if (!chars.Contains(mMap[key][mod]))
                        chars.Add(mMap[key][mod]);
            }

            return chars;
        }

        /// <summary>
        /// Static constructor for ConsoleKeyMap that initializes the internal Keys to char
        /// dictionary set
        /// </summary>
        static ConsoleKeyMap()
        {
            mMap = new Dictionary<Keys, Dictionary<KeyModifier, char>>();

            mMap[Keys.Space] = new Dictionary<KeyModifier, char>();
            mMap[Keys.Space][KeyModifier.None] = ' ';

            char[] numMods = new char[]
            {
                '!', '@', '#', '$', '%', '^', '&', '*', '(', ')'
            };

            for (char ch = '0'; ch <= '9'; ++ch)
            {
                mMap[(Keys)ch] = new Dictionary<KeyModifier, char>();
                mMap[(Keys)ch][KeyModifier.None] = ch;
                mMap[(Keys)ch][KeyModifier.Shift] = numMods[(int)ch - 48];
            }

            for (char ch = 'A'; ch <= 'Z'; ++ch)
            {
                mMap[(Keys)ch] = new Dictionary<KeyModifier, char>();
                mMap[(Keys)ch][KeyModifier.None] = (char)(ch + 32);
                mMap[(Keys)ch][KeyModifier.Shift] = ch;
            }

            mMap[Keys.OemPipe] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemPipe][KeyModifier.None] = '\\';
            mMap[Keys.OemPipe][KeyModifier.Shift] = '|';

            mMap[Keys.OemOpenBrackets] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemOpenBrackets][KeyModifier.None] = '[';
            mMap[Keys.OemOpenBrackets][KeyModifier.Shift] = '{';

            mMap[Keys.OemCloseBrackets] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemCloseBrackets][KeyModifier.None] = ']';
            mMap[Keys.OemCloseBrackets][KeyModifier.Shift] = '}';

            mMap[Keys.OemComma] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemComma][KeyModifier.None] = ',';
            mMap[Keys.OemComma][KeyModifier.Shift] = '<';

            mMap[Keys.OemPeriod] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemPeriod][KeyModifier.None] = '.';
            mMap[Keys.OemPeriod][KeyModifier.Shift] = '>';

            mMap[Keys.OemSemicolon] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemSemicolon][KeyModifier.None] = ';';
            mMap[Keys.OemSemicolon][KeyModifier.Shift] = ':';

            mMap[Keys.OemQuestion] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemQuestion][KeyModifier.None] = '/';
            mMap[Keys.OemQuestion][KeyModifier.Shift] = '?';

            mMap[Keys.OemQuotes] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemQuotes][KeyModifier.None] = '\'';
            mMap[Keys.OemQuotes][KeyModifier.Shift] = '"';

            mMap[Keys.OemMinus] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemMinus][KeyModifier.None] = '-';
            mMap[Keys.OemMinus][KeyModifier.Shift] = '_';

            mMap[Keys.OemPlus] = new Dictionary<KeyModifier, char>();
            mMap[Keys.OemPlus][KeyModifier.None] = '=';
            mMap[Keys.OemPlus][KeyModifier.Shift] = '+';
        }
    }
}
