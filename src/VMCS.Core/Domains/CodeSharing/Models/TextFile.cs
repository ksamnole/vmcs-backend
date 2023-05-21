using Microsoft.Extensions.Logging;

namespace VMCS.Core.Domains.CodeSharing.Models;

public class TextFile
{
    private readonly string _lockObject = "";
    public string Text = "";

    public TextFile(string name, string originText)
    {
        Name = name;
        this.originText = originText;
        changes = new List<Change>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    private string originText { get; }
    private List<Change> changes { get; }
    public int VersionId { get; set; }
    public bool IsDeleted { get; set; }

    private Change CorrectChange(Change change)
    {
        if (change.VersionId > changes.Count || change.VersionId < 0)
            throw new ArgumentException($"Wrong version id for file {Name}");

        foreach (var ch in changes.Skip(change.VersionId + 1))
        {
            if (ch.ConnectionId == change.ConnectionId)
                continue;
            if (ch.Position <= change.Position)
            {
                if (ch.Action == 0)
                    change.Position -= ch.CharsDeleted;
                else
                    change.Position += ch.InsertedString.Length;
            }
        }

        change.VersionId = changes.Count + 1;

        return change;
    }

    public void ApplyChange(Change change, ILogger logger)
    {
        lock (_lockObject)
        {
            var oldText = Text;
            changes.Add(CorrectChange(change));
            VersionId++;
            //logger.LogInformation("\n File ============" +
            //    "\n OldText: \n" + oldText + "\n NewText: \n" + Text
            //    + "\n Changes \n" + "\n VersionId: " + VersionId);
        }
    }

    private string ApplyAllChanges()
    {
        IEnumerable<char> res = originText.ToCharArray();

        foreach (var ch in changes)
            if (ch.Action == 0)
                res = res.Take(ch.Position)
                    .Concat(res.Skip(ch.Position + ch.CharsDeleted));
            else
                res = res.Take(ch.Position)
                    .Concat(ch.InsertedString)
                    .Concat(res.Skip(ch.Position));

        return new string(res.ToArray());
    }
}