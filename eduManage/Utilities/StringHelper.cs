using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace eduManage.Utilities
{
    public class StringHelper
    {
        public static string ToCodeFriendly(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // Bỏ dấu tiếng Việt
            string normalized = input.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var ch in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(ch);
                }
            }

            string noDiacritics = builder.ToString().Normalize(NormalizationForm.FormC);

            // Xóa ký tự đặc biệt và khoảng trắng
            string cleaned = Regex.Replace(noDiacritics, @"[^a-zA-Z0-9]", "");

            return cleaned;
        }


        public static string ToAbbreviation(string name)
            {
                var noDiacritics = ToCodeFriendly(name);
                var words = Regex.Split(noDiacritics, @"(?<!^)(?=[A-Z])|[^A-Za-z]+");
                return string.Concat(words.Where(w => !string.IsNullOrEmpty(w))
                                          .Select(w => w[0]))
                                          .ToUpper();
            }

        }
    }
