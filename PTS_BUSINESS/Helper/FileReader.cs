using sib_api_v3_sdk.Model;

namespace PTS_BUSINESS.Helper
{
    public static class FileReader
    {
        public static string ReadFileForBirthday(string firstName)
        {
            var filePath = "../PTS_BUSINESS/Email/BirthdayTemplate.cshtml";
            if (File.Exists(filePath))
            {
              
                // Dictionary to store variable values
                Dictionary<string, string> variableValues = new Dictionary<string, string>
            {
                { "Name", firstName }
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
