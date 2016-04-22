using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCellsNormalizer
{
    public class CellsNormalizer : ICellsNormalizer
    {
        public string Normalize(string s)
        {
            s = s.Replace("\\", "/");
            if (s.Contains("-"))
            {
                s = s.ToUpper().Replace(".", "").Replace("№", "").Replace(" ", "").Replace("/", " / ").Trim();
                return s;
            }
            if (s.Contains("/"))
            {
                s = s.ToUpper().Replace(".", "").Replace("№", "").Replace(" ", "").Replace("/", " / ").Trim();
                return s;
            }
            s = s.ToUpper().Replace(".", " ").Replace("№", "").Replace("/", " / ").Trim();

            var t = s
                .Split(' ')
                .Where(w => !string.IsNullOrWhiteSpace(w));
            s = string.Join(" ", t);

            var c = s.ToCharArray();

            for (int i = 1; i < c.Length; i++)
            {
                if (i > 1 && char.IsDigit(c[i]))
                {
                    if (c[i - 1] == char.Parse(" ") && !char.IsDigit(c[i - 2]))
                        c[i - 1] = char.Parse(".");
                }
            }

            string tmp = "";
            for (int i = 0; i < c.Length; i++)
            {
                if (i > 0 && char.IsDigit(c[i]))
                {
                    if (i < c.Length - 1 && char.IsLetter(c[i + 1]))
                    {
                        tmp += c[i] + " ";
                        continue;
                    }
                }
                tmp += c[i];
            }

            s = tmp;
            s = s.Replace(".", "");

            return s;
        }
    }
}
