using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class LanguageModule : ModuleBase<SocketCommandContext>
    {
        #region MorseCode Dictionary
        string morseOutput = string.Empty;
        static readonly Dictionary<char, string> MorseCodeDictionary = new Dictionary<char, string>
        {
            {'A', ".-"}, {'B', "-..."}, {'C', "-.-."}, {'D', "-.."}, {'E', "."}, {'F', "..-."},
            {'G', "--."}, {'H', "...."}, {'I', ".."}, {'J', ".---"}, {'K', "-.-"}, {'L', ".-.."},
            {'M', "--"}, {'N', "-."}, {'O', "---"}, {'P', ".--."}, {'Q', "--.-"}, {'R', ".-."},
            {'S', "..."}, {'T', "-"}, {'U', "..-"}, {'V', "...-"}, {'W', ".--"}, {'X', "-..-"},
            {'Y', "-.--"}, {'Z', "--.."},
            {'1', ".----"}, {'2', "..---"}, {'3', "...--"}, {'4', "....-"}, {'5', "....."},
            {'6', "-...."}, {'7', "--..."}, {'8', "---.."}, {'9', "----."}, {'0', "-----"},
            {'Ä', ".-.-"}, {'Ö', "---."}, {'Ü', "..--"},
            {'?', "..--.."}, {',', "--..--"}, {'.', ".-.-.-"}, {':', "---..."}, {'-', "-....-"},
            {'_', "..--.-"}, {';', "-.-.-."}, {'!', "-.-.--"}, {'=', "-...-"}, {'+', ".-.-."}
        };
        #endregion MorseCodeDictionary
        #region ConvertAlphabetToMorseCode
        private string ConvertAlphabetToMorseCode(string text)
        {
            StringBuilder morseBuilder = new StringBuilder();

            foreach (char c in text.ToUpper())
            {
                if (c == ' ')
                {
                    morseBuilder.Append(" ");
                }
                else if (MorseCodeDictionary.ContainsKey(c))
                {
                    morseBuilder.AppendFormat("{0} ", MorseCodeDictionary[c]);
                }
            }

            return morseOutput = morseBuilder.ToString();
        }
        #endregion ConvertAlphabetToMorseCode

        #region Alphabet Dictionary
        string alphabetOutput = string.Empty;
        static readonly Dictionary<string, char> AlphabetDictionary = new Dictionary<string, char>
        {
            {".-", 'A'}, {"-...", 'B'}, {"-.-.", 'C'}, {"-..", 'D'}, {".", 'E'}, {"..-.", 'F'},
            {"--.", 'G'}, {"....", 'H'}, {"..", 'I'}, {".---", 'J'}, {"-.-", 'K'}, {".-..", 'L'},
            {"--", 'M'}, {"-.", 'N'}, {"---", 'O'}, {".--.", 'P'}, {"--.-", 'Q'}, {".-.", 'R'},
            {"...", 'S'}, {"-", 'T'}, {"..-", 'U'}, {"...-", 'V'}, {".--", 'W'}, {"-..-", 'X'},
            {"-.--", 'Y'}, {"--..", 'Z'},
            {".----", '1'}, {"..---", '2'}, {"...--", '3'}, {"....-", '4'}, {".....", '5'},
            {"-....", '6'}, {"--...", '7'}, {"---..", '8'}, {"----.", '9'}, {"-----", '0'},
            {".-.-", 'Ä'}, {"---.", 'Ö'}, {"..--", 'Ü'},
            {"..--..", '?'}, {"--..--", ','}, {".-.-.-", '.'}, {"---...", ':'}, {"-....-", '-'},
            {"..--.-", '_'}, {"-.-.-.", ';'}, {"-.-.--", '!'}, {"-...-", '='}, {".-.-.", '+'}
        };
        #endregion Alphabet Dictionary
        #region ConvertMorseToAlphabet
        private string ConvertMorseToAlphabetCode(string text)
        {
            string morseCode = text;
            string[] morseWords = morseCode.Split(' ');

            StringBuilder alphabetBuilder = new StringBuilder();

            foreach (string morseWord in morseWords)
            {
                if (AlphabetDictionary.TryGetValue(morseWord, out char letter))
                {
                    alphabetBuilder.Append(letter);
                }
                else if (morseWord == "")
                {
                    alphabetBuilder.Append(" ");
                }
            }

            return alphabetOutput = alphabetBuilder.ToString();
        }
        #endregion convertMorseToAlphabet

        [Command("Morse")]
        [Alias("morse")]
        public async Task ConvertToMorse([Remainder] string input)
        {
            string convertedMorseCode = ConvertAlphabetToMorseCode(input);
            await Context.Channel.SendMessageAsync(morseOutput);

            Console.WriteLine($"Server: {Context.Guild}");
            Console.WriteLine($"(Morse) {Context.User.ToString()}: {Context.Message}");
            Console.WriteLine($"Text: {input}");
            Console.WriteLine($"Converted: {morseOutput}");
            Console.WriteLine("-----------------------------------------------------------------------------");
        }

        [Command("ReMorse")]
        [Alias("remorse", "Remorse", "reMorse")]
        public async Task ConvertMorseToAlphabet([Remainder] string input)
        {
            string convertedMorseCode = ConvertMorseToAlphabetCode(input);
            await Context.Channel.SendMessageAsync(alphabetOutput);

            Console.WriteLine($"Server: {Context.Guild}");
            Console.WriteLine($"(Remorse) {Context.User.ToString()}: {Context.Message}");
            Console.WriteLine($"Text: {input}");
            Console.WriteLine($"Converted: {alphabetOutput}");
            Console.WriteLine("-----------------------------------------------------------------------------");
        }



    }
}
