using System.Globalization;

public object ConvertUserInputSafe(string input, int Atyp)
{
    try
    {
        switch (Atyp)
        {
            case 1: // string
                return input;

            case 2: // int
                return int.Parse(input);

            case 3: // long
                return long.Parse(input);

            case 4: // real (double)
                return double.Parse(input, CultureInfo.InvariantCulture);

            case 5: // decimal
                return decimal.Parse(input, CultureInfo.InvariantCulture);

            case 6: // bool
                return bool.Parse(input);

            case 7: // date
                return DateTime.Parse(input, CultureInfo.InvariantCulture);

            default:
                return input;
        }
    }
    catch (FormatException)
    {
        // Return input string if parsing failed
        return input;
    }
}
