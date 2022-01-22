#region Info
// FileName:    DayNameStorage.cs
// Author:      Ladislav Filip
// Created:     22.01.2022
#endregion

namespace NameDayWorker;

public class DayNameStorage
{
    private readonly string _filePath;
    private readonly List<DayNameItem> _dayNames = new();

    public IReadOnlyCollection<DayNameItem> DayNames => _dayNames;

    public record DayNameItem(string Day, List<string> Names);

    public DayNameStorage(string filePath)
    {
        _filePath = filePath;
        Init();
    }

    private void Init()
    {
        if (!File.Exists(_filePath))
        {
            Helper.Log.Error("File not found {FilePath}", _filePath);
            throw new FileNotFoundException();
        }
        var lines = File.ReadAllLines(_filePath);
        Console.WriteLine(lines.Length);
        Parse(lines);
    }

    private void Parse(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var tmp = line.Split('\t');
            if (tmp.Length != 2)
            {
                Helper.Log.Warning("Unsupported format {Line}", line);
                continue;
            }
            var day = tmp[0];
            var names = tmp[1].Split(',').Select(p => p.Trim()).ToList();
            var item = new DayNameItem(day, names);
            _dayNames.Add(item);
        }
    }
}