using System;
using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;

namespace Romanify
{

    [System.Runtime.InteropServices.Guid("a3f2b887-d973-4842-ad0c-80ebd7450768")]
    public class RomanifyCommand : Command

    {
        private string[] ThouLetters = { "", "M", "MM", "MMM" };
        private string[] HundLetters =
            { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
        private string[] TensLetters =
            { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        private string[] OnesLetters =
            { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

        // Convert Roman numerals to an integer.
        private string ArabicToRoman(int arabic)
        {
            // See if it's >= 4000.
            if (arabic >= 4000)
            {
                // Use parentheses.
                int thou = arabic / 1000;
                arabic %= 1000;
                return "(" + ArabicToRoman(thou) + ")" +
                    ArabicToRoman(arabic);
            }

            // Otherwise process the letters.
            string result = "";

            // Pull out thousands.
            int num;
            num = arabic / 1000;
            result += ThouLetters[num];
            arabic %= 1000;

            // Handle hundreds.
            num = arabic / 100;
            result += HundLetters[num];
            arabic %= 100;

            // Handle tens.
            num = arabic / 10;
            result += TensLetters[num];
            arabic %= 10;

            // Handle ones.
            result += OnesLetters[arabic];

            return result;
        }
        public RomanifyCommand()
        { Instance = this; }
        public static RomanifyCommand Instance
        { get; private set; }
        public override string EnglishName
        { get { return "NumberToRoman"; } }
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            GetInteger getUserInteger = new GetInteger();
            getUserInteger.SetCommandPrompt("Enter a valid Integer only (no decimal points):");
            getUserInteger.SetDefaultInteger(1984);
            GetResult get_rn = getUserInteger.Get();
            RhinoApp.WriteLine("The Roman Numeral version of " + getUserInteger.Number() + " is " + ArabicToRoman(getUserInteger.Number()));
            return Result.Success;
        }
    }
}
