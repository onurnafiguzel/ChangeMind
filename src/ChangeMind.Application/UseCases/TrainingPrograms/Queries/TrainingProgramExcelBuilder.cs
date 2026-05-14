namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using System.Text.RegularExpressions;
using ClosedXML.Excel;
using ChangeMind.Application.DTOs;

internal static class TrainingProgramExcelBuilder
{
    private static readonly Regex DayKeyRegex = new(@"^Day-(\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static byte[] Build(
        ActiveProgramDetailDto program,
        IReadOnlyList<WorkoutSessionDto> sessions,
        IReadOnlyDictionary<Guid, string> exerciseNames)
    {
        using var workbook = new XLWorkbook();

        BuildSummarySheet(workbook, program);

        var dailyExercises = program.DailyExercises ?? new Dictionary<string, List<ProgramExerciseDetail>>();
        var orderedKeys = dailyExercises.Keys
            .OrderBy(ExtractDayNumber)
            .ThenBy(k => k, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var usedSheetNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Özet" };

        foreach (var dayKey in orderedKeys)
        {
            var sheetName = UniqueSheetName(ToDisplayDayName(dayKey), usedSheetNames);
            var ws = workbook.Worksheets.Add(sheetName);

            var daySessions = sessions
                .Where(s => string.Equals(s.DayKey, dayKey, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.RecordDate)
                .ThenBy(s => s.CreatedAt)
                .ToList();

            BuildDaySheet(ws, dayKey, dailyExercises[dayKey], daySessions, exerciseNames);
        }

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        return ms.ToArray();
    }

    // ─────────────────────────────────────────────────────────────────
    // Summary sheet
    // ─────────────────────────────────────────────────────────────────
    private static void BuildSummarySheet(XLWorkbook workbook, ActiveProgramDetailDto program)
    {
        var ws = workbook.Worksheets.Add("Özet");

        ws.Cell(1, 1).Value = "Antrenman Programı";
        ws.Range(1, 1, 1, 2).Merge();
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;
        ws.Cell(1, 1).Style.Font.FontColor = XLColor.White;
        ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
        ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        var rows = new (string Label, object? Value)[]
        {
            ("Program Adı", program.Name),
            ("Açıklama", program.Description),
            ("Koç", program.CoachName),
            ("Süre (Hafta)", program.DurationWeeks),
            ("Zorluk", program.Difficulty?.ToString()),
            ("Durum", program.Status),
            ("Başlangıç", program.StartDate?.ToString("yyyy-MM-dd")),
            ("Bitiş", program.EndDate?.ToString("yyyy-MM-dd")),
            ("Kullanıcı Yaş", program.UserAge),
            ("Kullanıcı Boy", program.UserHeight),
            ("Kullanıcı Kilo", program.UserWeight),
            ("Kullanıcı Cinsiyet", program.UserGender?.ToString())
        };

        var row = 3;
        foreach (var (label, value) in rows)
        {
            ws.Cell(row, 1).Value = label;
            ws.Cell(row, 1).Style.Font.Bold = true;
            ws.Cell(row, 1).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            ws.Cell(row, 2).Value = value switch
            {
                null => string.Empty,
                int i => i,
                decimal d => d,
                _ => value.ToString() ?? string.Empty
            };
            row++;
        }

        var dataRange = ws.Range(3, 1, row - 1, 2);
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        ws.Columns().AdjustToContents();
    }

    // ─────────────────────────────────────────────────────────────────
    // Day sheet — exercise × date matrix
    // ─────────────────────────────────────────────────────────────────
    private static void BuildDaySheet(
        IXLWorksheet ws,
        string dayKey,
        IReadOnlyList<ProgramExerciseDetail> plan,
        IReadOnlyList<WorkoutSessionDto> daySessions,
        IReadOnlyDictionary<Guid, string> exerciseNames)
    {
        var orderedExerciseIds = EnumerateExerciseOrder(plan, daySessions).ToList();
        var planByExercise = plan
            .GroupBy(p => p.ExerciseId)
            .ToDictionary(g => g.Key, g => g.First());

        // Build column layout: column 1 = exercise name, then for each session 2 cols.
        var hasSessions = daySessions.Count > 0;
        var dateColumns = hasSessions ? daySessions.Count : 1; // at least the plan column
        var totalCols = 1 + dateColumns * 2;

        // Row 1: top header (dates) ────────────────────────────────────
        ws.Cell(1, 1).Value = $"Program – {ToDisplayDayName(dayKey)}";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontColor = XLColor.White;
        ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
        ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        if (hasSessions)
        {
            for (var i = 0; i < daySessions.Count; i++)
            {
                var col = 2 + i * 2;
                var range = ws.Range(1, col, 1, col + 1);
                range.Merge();
                range.Value = daySessions[i].RecordDate.ToString("yyyy-MM-dd");
                range.Style.Font.Bold = true;
                range.Style.Font.FontColor = XLColor.White;
                range.Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
                range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
        }
        else
        {
            var range = ws.Range(1, 2, 1, 3);
            range.Merge();
            range.Value = "Plan";
            range.Style.Font.Bold = true;
            range.Style.Font.FontColor = XLColor.White;
            range.Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        // Row 2: plan sub-header "Sets | Reps" repeating ─────────────
        ws.Cell(2, 1).Value = "Egzersiz";
        StylePlanSubHeader(ws.Range(2, 1, 2, 1));
        for (var i = 0; i < dateColumns; i++)
        {
            var col = 2 + i * 2;
            ws.Cell(2, col).Value = "Sets";
            ws.Cell(2, col + 1).Value = "Reps";
            StylePlanSubHeader(ws.Range(2, col, 2, col + 1));
        }

        // Exercise blocks ─────────────────────────────────────────────
        var currentRow = 3;
        foreach (var exerciseId in orderedExerciseIds)
        {
            var name = ResolveName(exerciseNames, exerciseId);
            var maxSets = MaxSetCount(exerciseId, daySessions);
            var hasActuals = hasSessions && maxSets > 0;

            var planRow = currentRow;
            var actualHeaderRow = currentRow + 1;
            var firstActualRow = currentRow + 2;
            var lastActualRow = firstActualRow + Math.Max(0, maxSets - 1);
            var blockLastRow = hasActuals ? lastActualRow : actualHeaderRow;

            // Plan row
            ws.Cell(planRow, 1).Value = name;
            if (planByExercise.TryGetValue(exerciseId, out var planDetail))
            {
                for (var i = 0; i < dateColumns; i++)
                {
                    var col = 2 + i * 2;
                    ws.Cell(planRow, col).Value = planDetail.Sets;
                    ws.Cell(planRow, col + 1).Value = planDetail.Reps;
                }
            }
            StylePlanRow(ws.Range(planRow, 2, planRow, totalCols));

            // Actual sub-header row: "Load | Reps" per date
            if (hasSessions)
            {
                for (var i = 0; i < daySessions.Count; i++)
                {
                    var col = 2 + i * 2;
                    ws.Cell(actualHeaderRow, col).Value = "Load";
                    ws.Cell(actualHeaderRow, col + 1).Value = "Reps";
                }
                StyleActualSubHeader(ws.Range(actualHeaderRow, 2, actualHeaderRow, totalCols));
            }

            // Set rows
            if (hasActuals)
            {
                for (var setIndex = 1; setIndex <= maxSets; setIndex++)
                {
                    var setRow = firstActualRow + (setIndex - 1);
                    ws.Cell(setRow, 1).Value = $"Set {setIndex}";
                    ws.Cell(setRow, 1).Style.Font.Italic = true;
                    ws.Cell(setRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    for (var s = 0; s < daySessions.Count; s++)
                    {
                        var session = daySessions[s];
                        var col = 2 + s * 2;
                        var exercise = session.Exercises.FirstOrDefault(e => e.ExerciseId == exerciseId);
                        var set = exercise?.Sets.FirstOrDefault(x => x.SetNumber == setIndex);
                        if (set is not null)
                        {
                            ws.Cell(setRow, col).Value = set.Weight;
                            ws.Cell(setRow, col + 1).Value = set.Reps;
                        }
                    }
                }
                ws.Range(firstActualRow, 2, lastActualRow, totalCols)
                  .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Merge exercise name cell vertically across the whole block
            if (blockLastRow > planRow)
            {
                var nameRange = ws.Range(planRow, 1, blockLastRow, 1);
                nameRange.Merge();
                nameRange.Style.Font.Bold = true;
                nameRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                nameRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                nameRange.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
                nameRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
            else
            {
                ws.Cell(planRow, 1).Style.Font.Bold = true;
                ws.Cell(planRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(planRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(planRow, 1).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
                ws.Cell(planRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            // Block outer border
            ws.Range(planRow, 1, blockLastRow, totalCols)
              .Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            ws.Range(planRow, 2, blockLastRow, totalCols)
              .Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            currentRow = blockLastRow + 2; // 1 separator row
        }

        if (!hasSessions)
        {
            ws.Cell(currentRow, 1).Value = "Henüz kayıt yok.";
            ws.Cell(currentRow, 1).Style.Font.Italic = true;
        }

        ws.SheetView.FreezeRows(2);
        ws.SheetView.FreezeColumns(1);
        ws.Columns().AdjustToContents();
        ws.Column(1).Width = Math.Max(ws.Column(1).Width, 22);
    }

    // ─────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────
    private static IEnumerable<Guid> EnumerateExerciseOrder(
        IReadOnlyList<ProgramExerciseDetail> plan,
        IReadOnlyList<WorkoutSessionDto> sessions)
    {
        var seen = new HashSet<Guid>();
        foreach (var p in plan)
            if (seen.Add(p.ExerciseId)) yield return p.ExerciseId;
        foreach (var s in sessions)
            foreach (var e in s.Exercises)
                if (seen.Add(e.ExerciseId)) yield return e.ExerciseId;
    }

    private static int MaxSetCount(Guid exerciseId, IReadOnlyList<WorkoutSessionDto> sessions)
    {
        var max = 0;
        foreach (var s in sessions)
        {
            var ex = s.Exercises.FirstOrDefault(e => e.ExerciseId == exerciseId);
            if (ex is null) continue;
            foreach (var set in ex.Sets)
                if (set.SetNumber > max) max = set.SetNumber;
        }
        return max;
    }

    private static void StylePlanSubHeader(IXLRange range)
    {
        range.Style.Font.Bold = true;
        range.Style.Fill.BackgroundColor = XLColor.SteelBlue;
        range.Style.Font.FontColor = XLColor.White;
        range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    }

    private static void StyleActualSubHeader(IXLRange range)
    {
        range.Style.Font.Bold = true;
        range.Style.Fill.BackgroundColor = XLColor.LightGray;
        range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    }

    private static void StylePlanRow(IXLRange range)
    {
        range.Style.Fill.BackgroundColor = XLColor.AliceBlue;
        range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    }

    private static string ResolveName(IReadOnlyDictionary<Guid, string> names, Guid id)
        => names.TryGetValue(id, out var n) ? n : id.ToString();

    private static int ExtractDayNumber(string dayKey)
    {
        var m = DayKeyRegex.Match(dayKey);
        return m.Success && int.TryParse(m.Groups[1].Value, out var n) ? n : int.MaxValue;
    }

    private static string ToDisplayDayName(string dayKey)
    {
        var m = DayKeyRegex.Match(dayKey);
        return m.Success ? $"Gün {m.Groups[1].Value}" : dayKey;
    }

    private static string UniqueSheetName(string name, HashSet<string> used)
    {
        var safe = name;
        foreach (var ch in new[] { ':', '\\', '/', '?', '*', '[', ']' })
            safe = safe.Replace(ch, '_');
        if (safe.Length > 31) safe = safe[..31];
        if (string.IsNullOrWhiteSpace(safe)) safe = "Sheet";

        var candidate = safe;
        var i = 2;
        while (!used.Add(candidate))
        {
            var suffix = $" ({i++})";
            var maxBase = 31 - suffix.Length;
            candidate = (safe.Length > maxBase ? safe[..maxBase] : safe) + suffix;
        }
        return candidate;
    }
}
