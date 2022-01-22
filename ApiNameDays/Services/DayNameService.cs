#region Info
// FileName:    DayNameService.cs
// Author:      Ladislav Filip
// Created:     22.01.2022
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ApiNameDays.Services
{
    public class DayNameService
    {
        private readonly string _filePath;
        private readonly ILogger _logger;
        private readonly List<DayNameItem> _dayNames = new();

        public IReadOnlyCollection<DayNameItem> DayNames => _dayNames;

        public record DayNameItem(string Day, List<string> Names);

        public DayNameService(string filePath, ILogger logger)
        {
            _filePath = filePath;
            _logger = logger;
            Init();
        }

        private void Init()
        {
            if (!File.Exists(_filePath))
            {
                _logger.LogError("File not found {FilePath}", _filePath);
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
                    _logger.LogWarning("Unsupported format {Line}", line);
                    continue;
                }
                var day = tmp[0];
                var names = tmp[1].Split(',').Select(p => p.Trim()).ToList();
                var item = new DayNameItem(day, names);
                _dayNames.Add(item);
            }
        }

        public Dictionary<string, string[]> Search(string startLetters)
        {
            var result = new Dictionary<string, string[]>();
            foreach (var (day, list) in DayNames)
            {
                var names = list.Where(p => p.StartsWith(startLetters, StringComparison.OrdinalIgnoreCase)).ToList();
                if (names.Any())
                {
                    result.Add(day, names.ToArray());
                }
            }

            return result;
        }
    }
}