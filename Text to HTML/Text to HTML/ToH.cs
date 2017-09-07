using System;
using System.Text;
using System.Web;

namespace Text_to_HTML
{
  public static class StringMethodExtensions
        {
            private static string _paraBreak = "\r\n\r\n";
            private static string _link = "<a href=\"{0}\">{1}</a>";
            private static string _linkNoFollow = "<a href=\"{0}\" rel=\"nofollow\">{1}</a>";

            public static string ToHtml(this string s)
            {
                return ToHtml(s, false);
            }

            public static string ToHtml(this string s, bool nofollow)
            {
                StringBuilder sb = new StringBuilder();

                int pos = 0;
                while (pos < s.Length)
                {
                    int start = pos;
                    pos = s.IndexOf(_paraBreak, start);

                    if (pos < 0)
                    {
                        pos = s.Length;
                    }

                    string para = s.Substring(start, pos - start).Trim();


                    if (para.Length > 0)
                        EncodeParagraph(para, sb, nofollow);

                    pos += _paraBreak.Length;
                }
                return sb.ToString();
            }

            private static void EncodeParagraph(string s, StringBuilder sb, bool nofollow)
            {
                sb.AppendLine("<p>");

                s = HttpUtility.HtmlEncode(s);

                s = s.Replace(Environment.NewLine, "<br />\r\n");
                EncodeLinks(s, sb, nofollow);

                sb.AppendLine("\r\n</p>");
            }

            private static void EncodeLinks(string s, StringBuilder sb, bool nofollow)
            {
                int pos = 0;
                while (pos < s.Length)
                {
                    int start = pos;
                    pos = s.IndexOf("[[", pos);
                    if (pos < 0)
                        pos = s.Length;
                    sb.Append(s.Substring(start, pos - start));
                    if (pos < s.Length)
                    {
                        string label, link;

                        start = pos + 2;
                        pos = s.IndexOf("]]", start);
                        if (pos < 0)
                            pos = s.Length;
                        label = s.Substring(start, pos - start);
                        int i = label.IndexOf("][");
                        if (i >= 0)
                        {
                            link = label.Substring(i + 2);
                            label = label.Substring(0, i);
                        }
                        else
                        {
                            link = label;
                        }
                        sb.Append(String.Format(nofollow ? _linkNoFollow : _link, link, label));

                        pos += 2;
                    }
                }
            }
        }
    }
