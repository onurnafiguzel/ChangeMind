namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using ClosedXML.Excel;
using ChangeMind.Application.DTOs;
using ChangeMind.Domain.Enums;

internal static class NutritionPlanExcelBuilder
{
    private const int ColumnCount = 6;

    public static byte[] Build(NutritionPlanDetailDto plan)
    {
        using var workbook = new XLWorkbook();

        var usedSheetNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var day in plan.Days)
        {
            var sheetName = UniqueSheetName(DayTypeToDisplayName(day.DayType), usedSheetNames);
            var ws = workbook.Worksheets.Add(sheetName);
            BuildDaySheet(ws, plan, day);
        }

        if (plan.Days.Count == 0)
        {
            var ws = workbook.Worksheets.Add("Plan");
            ws.Cell(1, 1).Value = "Bu beslenme programında gün tanımı yok.";
            ws.Cell(1, 1).Style.Font.Italic = true;
        }

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        return ms.ToArray();
    }

    private static void BuildDaySheet(IXLWorksheet ws, NutritionPlanDetailDto plan, NutritionDayDto day)
    {
        var row = 1;

        // Day header
        var headerRange = ws.Range(row, 1, row, ColumnCount);
        headerRange.Merge();
        headerRange.Value = DayTypeToDisplayName(day.DayType);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Font.FontColor = XLColor.White;
        headerRange.Style.Font.FontSize = 14;
        headerRange.Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
        row++;

        // Meta line
        var metaRange = ws.Range(row, 1, row, ColumnCount);
        metaRange.Merge();
        metaRange.Value = $"{plan.Title} · Koç: {plan.CoachName} · Versiyon: {plan.VersionNumber}";
        metaRange.Style.Font.Italic = true;
        metaRange.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
        metaRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        metaRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        row += 2;

        // Meals
        for (var i = 0; i < day.Meals.Count; i++)
        {
            var meal = day.Meals[i];
            row = BuildMealBlock(ws, row, i + 1, meal);
            row++; // 1 row separator
        }

        // Day total row
        var dayTotalLabel = ws.Range(row, 1, row, 2);
        dayTotalLabel.Merge();
        dayTotalLabel.Value = "GÜN TOPLAMI";
        dayTotalLabel.Style.Font.Bold = true;
        dayTotalLabel.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ws.Cell(row, 3).Value = day.TotalCalories;
        ws.Cell(row, 4).Value = day.TotalProtein;
        ws.Cell(row, 5).Value = day.TotalCarbs;
        ws.Cell(row, 6).Value = day.TotalFat;

        var dayTotalRange = ws.Range(row, 1, row, ColumnCount);
        dayTotalRange.Style.Font.Bold = true;
        dayTotalRange.Style.Font.FontSize = 12;
        dayTotalRange.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
        dayTotalRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        dayTotalRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
        dayTotalRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        ws.Range(row, 3, row, ColumnCount).Style.NumberFormat.Format = "0.0";

        ws.SheetView.FreezeRows(1);
        ws.Columns().AdjustToContents();
        ws.Column(1).Width = Math.Max(ws.Column(1).Width, 24);
        for (var c = 2; c <= ColumnCount; c++)
            ws.Column(c).Width = Math.Max(ws.Column(c).Width, 14);
    }

    private static int BuildMealBlock(IXLWorksheet ws, int startRow, int mealNumber, MealDto meal)
    {
        var row = startRow;

        // Meal title — "ÖĞÜN N"
        var titleRange = ws.Range(row, 1, row, ColumnCount);
        titleRange.Merge();
        titleRange.Value = $"ÖĞÜN {mealNumber}";
        titleRange.Style.Font.Bold = true;
        titleRange.Style.Font.FontColor = XLColor.White;
        titleRange.Style.Font.FontSize = 12;
        titleRange.Style.Fill.BackgroundColor = XLColor.DarkSlateGray;
        titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        row++;

        // Column headers
        ws.Cell(row, 1).Value = "Besin";
        ws.Cell(row, 2).Value = "Miktar";
        ws.Cell(row, 3).Value = "Kalori";
        ws.Cell(row, 4).Value = "Protein";
        ws.Cell(row, 5).Value = "Karbonhidrat";
        ws.Cell(row, 6).Value = "Yağ";
        var headerRange = ws.Range(row, 1, row, ColumnCount);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Font.FontColor = XLColor.White;
        headerRange.Style.Fill.BackgroundColor = XLColor.SteelBlue;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        row++;

        var firstDataRow = row;

        if (meal.Items.Count == 0)
        {
            var emptyRange = ws.Range(row, 1, row, ColumnCount);
            emptyRange.Merge();
            emptyRange.Value = "(öğünde besin yok)";
            emptyRange.Style.Font.Italic = true;
            emptyRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            row++;
        }
        else
        {
            foreach (var item in meal.Items)
            {
                ws.Cell(row, 1).Value = item.FoodName;
                ws.Cell(row, 2).Value = $"{item.Quantity:0.##} {item.QuantityUnit}";
                ws.Cell(row, 3).Value = item.Calories;
                ws.Cell(row, 4).Value = item.Protein;
                ws.Cell(row, 5).Value = item.Carbs;
                ws.Cell(row, 6).Value = item.Fat;
                row++;
            }
        }

        var lastDataRow = row - 1;

        // Totals row
        var totalsLabel = ws.Range(row, 1, row, 2);
        totalsLabel.Merge();
        totalsLabel.Value = "Toplamlar:";
        totalsLabel.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        ws.Cell(row, 3).Value = meal.TotalCalories;
        ws.Cell(row, 4).Value = meal.TotalProtein;
        ws.Cell(row, 5).Value = meal.TotalCarbs;
        ws.Cell(row, 6).Value = meal.TotalFat;
        var totalsRange = ws.Range(row, 1, row, ColumnCount);
        totalsRange.Style.Font.Bold = true;
        totalsRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        totalsRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        ws.Range(row, 3, row, ColumnCount).Style.NumberFormat.Format = "0.0";

        // Block-level formatting: number format & borders for data
        if (lastDataRow >= firstDataRow)
        {
            var dataRange = ws.Range(firstDataRow, 1, lastDataRow, ColumnCount);
            dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws.Range(firstDataRow, 3, lastDataRow, ColumnCount).Style.NumberFormat.Format = "0.0";
            ws.Range(firstDataRow, 1, lastDataRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        }

        // Outer border for the whole meal block (title → totals)
        ws.Range(startRow, 1, row, ColumnCount).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

        return row;
    }

    private static string DayTypeToDisplayName(NutritionDayType dayType) => dayType switch
    {
        NutritionDayType.WorkoutDay => "Antrenman Günü",
        NutritionDayType.OffDay     => "Dinlenme Günü",
        _                            => dayType.ToString()
    };

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
