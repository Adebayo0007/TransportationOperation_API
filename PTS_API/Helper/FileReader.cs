namespace PTS_API.Helper
{
    public static class FileReader
    {
        public static string ReadFileForOTP(string firstName,string otp)
        {
            string filePath = "./GateWay/Email/OTPTemplate.cshtml";

            if (File.Exists(filePath))
            {
              
                // Dictionary to store variable values
                Dictionary<string, string> variableValues = new Dictionary<string, string>
            {
                { "FirstName", firstName },
                { "Otp", otp },
                // Add more variables as needed
            };

                // Replace placeholders with actual values
                string cshtmlContent = File.ReadAllText(filePath);
                string replacedContent = ReplacePlaceholders(cshtmlContent, variableValues);

                // Do something with replacedContent
                return replacedContent;
            }
            else
            {
                Console.WriteLine("File not found: " + filePath);
                return null;
            }


        }

        static string ReplacePlaceholders(string content, Dictionary<string, string> variableValues)
        {
            foreach (var entry in variableValues)
            {
                string placeholder = $"{{{{{entry.Key}}}}}";
                content = content.Replace(placeholder, entry.Value);
            }

            return content;
        }
    }
}
