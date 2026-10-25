namespace double_pendulum.CLI;

public static class ConsolePrompt
{
    /// <summary>
    /// Asks a yes/no question until a valid answer is given.
    /// </summary>
    /// <param name="question">Question shown to the user.</param>
    /// <returns><c>true</c> for yes, <c>false</c> for no.</returns>
    public static bool AskYesNo(string question)
    {
        string answer;
        bool IsInvalid;

        do
        {
            Console.WriteLine("\n-------------------------------------------------------------------------------");
            Console.WriteLine(question);
            answer = (Console.ReadLine() ?? string.Empty).ToLower();

            IsInvalid = (answer != "y" && answer != "yes" && answer != "n" && answer != "no");

            if (IsInvalid)
            {
                Console.WriteLine("Invalid input!");
            }
        }
        while (IsInvalid);

        return answer == "y" || answer == "yes";
    }
}
