using System.Net.Http.Headers;

namespace VMCS.Core.Domains.CodeSharing.Models;

public class TextFile
{
    public int Id { get; set; }
    public string Name { get; set; }
    private string originText { get; }
    private List<Change> changes { get; set; }
    public string Text => ApplyAllChanges();
    public int VersionId  { get; set;}
    public bool IsDeleted { get; set; }

    public TextFile(string name, string originText)
    {
        Name = name;
        this.originText = originText;
        changes = new List<Change>();
    }

    private Change CorrectChange(Change change)
    {
        if (change.VersionId > changes.Count || change.VersionId < 0)
            throw new ArgumentException($"Wrong version id for file {Name}");

        foreach(var ch in changes.Skip(change.VersionId + 1))
        {
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

    public void ApplyChange(Change change)
    {
        changes.Add(CorrectChange(change));
        VersionId++;
    }

    private string ApplyAllChanges()
    {
        IEnumerable<char> res = originText.ToCharArray();

        foreach(var ch in changes)
        {
            if (ch.Action == 0)
            {
                res = res.Take(ch.Position)
                    .Concat(res.Skip(ch.Position + ch.CharsDeleted));
            }
            else
            {
                res = res.Take(ch.Position)
                    .Concat(ch.InsertedString)
                    .Concat(res.Skip(ch.Position));
            }
        }

        return new string(res.ToArray());
    }
}