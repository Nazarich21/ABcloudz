using System;
using System.Text.RegularExpressions;

namespace E_mail_search
{
    public class Search
    {
        public event EventHandler<CustomEventArgs> NewResult;
        public string Text { get; private set; }
        public Regex Pattern { get; private set; }
        public int Length { get; private set; }
        public string Name { get; private set; }

        const string EmailPattern = @"[\w-]+@([\w-]+\.)+[\w-]+";
        const string LinkPattern = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";

        private Search(string text, string pattern, string name, int length)
        {
            Text = text;
            Pattern = new Regex(pattern, RegexOptions.Compiled);
            Name = name;
            Length = length;
        }

        public static Search Email(string text)
        {
            return new Search(text, EmailPattern, "Email", 0);
        }

        public static Search Link(string text)
        {
            return new Search(text, LinkPattern, "Link", 0);
        }

        public void SearchObj(object obj)
        {
            Search serch = (Search)obj;
            Match matches = Pattern.Match(serch.Text);
            if (matches.Length != 0)
            {
                serch.Length++;
                while (matches.Success)
                {
                    var args = new CustomEventArgs(serch.Name, matches.Value);
                    NewResult?.Invoke(this, args);
                    matches = matches.NextMatch();
                }
            }
        }
    }

    public class CustomEventArgs : EventArgs
    {
        public string Nameev { get; private set; }
        public string Valueev { get; private set; }

        public CustomEventArgs(string name, string value)
        {
            Nameev = name;
            Valueev = value;
        }
    }
}
