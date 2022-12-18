using AngleSharp.Text;

namespace GRSMU.Bot.Core.Comparers;

public class GroupComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x == null) return 1;
        if (y == null) return -1;

        var xNo = GetGroupNo(x);
        var yNo = GetGroupNo(x);

        return xNo.CompareTo(yNo);
    }

    private int GetGroupNo(string x)
    {
        return int.Parse(string.Concat(x.TakeWhile(x => x.IsDigit())));
    }
}