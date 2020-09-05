using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel.Charts;

using Lab1_1.DTO;

namespace Lab1_1
{
    public class ExcelHelper
    {
        private readonly XSSFWorkbook _workbook = new XSSFWorkbook();

        private ISheet _sheet;
        private readonly IFont _font;
        private readonly IFont _boldFont;
        private readonly ICellStyle _borderedCellStyle;
        private readonly ICellStyle _boldCellStyle;
        private readonly ICellStyle _percentageCellStyle;

        private const int OffsetStep = 7;
        private const int TitleColumnNum = 2;
        private const int ResultColumnNum = 4;
        private const int TableWidth = 5;
        private int _offset;

        private readonly List<string> _errorList = new List<string>();
        private readonly List<double> _dichotomyIterations = new List<double>();
        private readonly List<double> _goldenRatioIterations = new List<double>();
        private readonly List<double> _fibonacciIterations = new List<double>();

        public ExcelHelper()
        {
            _font = GetDefaultFont();
            _boldFont = GetDefaultFont();
            _boldFont.IsBold = true;

            _borderedCellStyle = GetDefaultCellStyle();
            _boldCellStyle = GetBoldCellStyle();
            _percentageCellStyle = GetDefaultCellStyle();
            _percentageCellStyle.DataFormat = _workbook.CreateDataFormat().GetFormat("0.00000000%");
        }

        private IFont GetDefaultFont()
        {
            var font = _workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "Tahoma";

            return font;
        }
        private ICellStyle GetDefaultCellStyle()
        {
            var style = _workbook.CreateCellStyle();
            style.SetFont(_font);

            style.BorderLeft = BorderStyle.Medium;
            style.BorderTop = BorderStyle.Medium;
            style.BorderRight = BorderStyle.Medium;
            style.BorderBottom = BorderStyle.Medium;

            style.VerticalAlignment = VerticalAlignment.Center;
            style.Alignment = HorizontalAlignment.Center;

            return style;
        }
        private ICellStyle GetBoldCellStyle()
        {
            var style = _workbook.CreateCellStyle();
            style.SetFont(_boldFont);

            style.BorderLeft = BorderStyle.Thick;
            style.BorderTop = BorderStyle.Thick;
            style.BorderRight = BorderStyle.Thick;
            style.BorderBottom = BorderStyle.Thick;

            style.VerticalAlignment = VerticalAlignment.Center;
            style.Alignment = HorizontalAlignment.Center;

            return style;
        }

        public void ProcessSheet(FinalResult[] results, double error)
        {
            _sheet = _workbook.CreateSheet($"ε = {error:G}");
            _errorList.Add($"{error:G}");
            _offset = 0;

            var rowCount = results.OrderByDescending(res => res.IterationCount).First().IterationCount + 3;
            for (int i = 0; i < rowCount; i++)
                _sheet.CreateRow(i);

            foreach (var res in results)
            {
                FillSheet(res);
                switch (res.Method)
                {
                    case Methods.Dichotomy:
                        _dichotomyIterations.Add(res.IterationCount);
                        break;
                    case Methods.GoldenRatio:
                        _goldenRatioIterations.Add(res.IterationCount);
                        break;
                    case Methods.Fibonacci:
                        _fibonacciIterations.Add(res.IterationCount);
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(res.Method), "Method is not supported");
                }
            }

            for (int i = 0; i < TableWidth + _offset; i++)
                _sheet.AutoSizeColumn(i);
        }
        public void ProcessChart()
        {
            var sheet = _workbook.CreateSheet("Chart");
            var drawing = sheet.CreateDrawingPatriarch();
            var anchor = drawing.CreateAnchor(0, 0, 0, 0, 0, 5, 21, 35);

            AddChart(drawing, sheet, anchor);
        }
        public void SaveDoc(string path)
        {
            using var fileData = new FileStream(path, FileMode.Create);
            _workbook.Write(fileData);
        }

        private void FillSheet(FinalResult result)
        {
            AddTitle(result);
            AddHeader();
            AddData(result);

            _offset += OffsetStep;
        }

        private void AddTitle(FinalResult result)
        {
            var currentRow = _sheet.GetRow(0);

            InsertCell(currentRow, _offset, (double) result.IterationCount, _boldCellStyle, CellType.Numeric);
            InsertCell(currentRow, TitleColumnNum + _offset, result.Method.ToString(), _boldCellStyle, CellType.String);
            InsertCell(currentRow, ResultColumnNum + _offset, result.Res, _boldCellStyle, CellType.Numeric);
        }
        private void AddHeader()
        {
            var currentRow = _sheet.GetRow(1);

            InsertCell(currentRow, _offset, "Length Ratio", _boldCellStyle, CellType.String);
            InsertCell(currentRow, _offset + 1, "From", _boldCellStyle, CellType.String);
            InsertCell(currentRow, _offset + 2, "To", _boldCellStyle, CellType.String);
            InsertCell(currentRow, _offset + 3, "Res1", _boldCellStyle, CellType.String);
            InsertCell(currentRow, _offset + 4, "Res2", _boldCellStyle, CellType.String);
        }
        private void AddData(FinalResult result)
        {
            const int precision = 10;
            int rowNum = 2;

            foreach (var iterationResult in result.Results)
            {
                var currentRow = _sheet.GetRow(rowNum);
                
                InsertCell(currentRow, _offset, Math.Round(iterationResult.LengthRatio, precision),
                    _percentageCellStyle, CellType.Numeric);
                InsertCell(currentRow, _offset + 1, Math.Round(iterationResult.From, precision),
                    _borderedCellStyle, CellType.Numeric);
                InsertCell(currentRow, _offset + 2, Math.Round(iterationResult.To, precision),
                    _borderedCellStyle, CellType.Numeric);
                InsertCell(currentRow, _offset + 3, Math.Round(iterationResult.Res1, precision),
                    _borderedCellStyle, CellType.Numeric);
                InsertCell(currentRow, _offset + 4, Math.Round(iterationResult.Res2, precision),
                    _borderedCellStyle, CellType.Numeric);

                rowNum++;
            }
        }
        private void AddChart(IDrawing drawing, ISheet sheet, IClientAnchor anchor)
        {
            var chart = drawing.CreateChart(anchor);
            var legend = chart.GetOrCreateLegend();
            legend.Position = LegendPosition.Bottom;

            var data = chart.ChartDataFactory.CreateLineChartData<string, double>();

            var bottomAxis = chart.ChartAxisFactory.CreateCategoryAxis(AxisPosition.Bottom);
            var leftAxis = chart.ChartAxisFactory.CreateValueAxis(AxisPosition.Left);
            leftAxis.Crosses = AxisCrosses.AutoZero;

            var row = sheet.CreateRow(0);
            for (int i = 0; i < _errorList.Count; i++)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(_errorList[i]);
            }
            var xAxis = DataSources.FromStringCellRange(sheet, new CellRangeAddress(0, 0, 0, _errorList.Count - 1));

            FillChartDataCells(sheet, 1, _dichotomyIterations, xAxis, data);
            FillChartDataCells(sheet, 2, _goldenRatioIterations, xAxis, data);
            FillChartDataCells(sheet, 3, _fibonacciIterations, xAxis, data);

            chart.Plot(data, bottomAxis, leftAxis);
        }

        private static void InsertCell(IRow currentRow, int cellIndex, object value, ICellStyle style, CellType type)
        {
            var cell = currentRow.CreateCell(cellIndex);
            cell.SetCellType(type);

            if (type == CellType.String)
                cell.SetCellValue(value as string);
            else
                cell.SetCellValue((double) value);

            cell.CellStyle = style;
            cell.SetCellType(type);
        }
        private static void FillChartDataCells(ISheet sheet, int rowNum, IReadOnlyList<double> iterationsList, IChartDataSource<string> xAxis, ILineChartData<string, double> data)
        {
            var row = sheet.CreateRow(rowNum);
            for (int i = 0; i < iterationsList.Count; i++)
                row.CreateCell(i).SetCellValue(iterationsList[i]);

            var yAxis = DataSources.FromNumericCellRange(sheet,
                new CellRangeAddress(rowNum, rowNum, 0, iterationsList.Count - 1));

            data.AddSeries(xAxis, yAxis).SetTitle(((Methods)(rowNum - 1)).ToString());
        }
    }
}
