using GxFormula.Forasource;

using GxShared.GxDtos;

namespace GxTie.Services.Calculation
{
    public interface IProgramLineSource
    {
        string? GetSourceText();
        IReadOnlyCollection<ProgramLineContext> GetContexts();
    }
    public interface IProgramLineParser
    {
        IReadOnlyList<ProgramLine> Parse(IProgramLineSource source);
        List<ProgramLine> ParseProgramLines(string? sourceText);
    }

    public sealed class ProgramLineParser : IProgramLineParser
    {
        public IReadOnlyList<ProgramLine> Parse(IProgramLineSource source)
        {
            var lines = ParseProgramLines(source.GetSourceText());
            return EnrichProgramLines(lines, source.GetContexts());
        }

        public List<ProgramLine> ParseProgramLines(string? sourceText)
        {
            var lines = new List<ProgramLine>();

            if (string.IsNullOrWhiteSpace(sourceText))
                return lines;

            var rawLines = sourceText
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var raw in rawLines)
            {
                var parsed = ParseProgramLine(raw);
                if (parsed != null)
                    lines.Add(parsed);
            }

            return lines;
        }

        private ProgramLine? ParseProgramLine(string raw)
        {
            var line = raw.Trim();
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var atIndex = line.IndexOf('@');
            if (atIndex < 0)
                return null;

            line = line[(atIndex + 1)..].Trim();

            var colonIndex = line.IndexOf(':');
            if (colonIndex <= 0)
                return null;

            if (!int.TryParse(line[..colonIndex].Trim(), out var lineNumber))
                return null;

            var exprPart = line[(colonIndex + 1)..].Trim();

            string? meta = null;
            var metaStart = exprPart.LastIndexOf('[');
            var metaEnd = exprPart.LastIndexOf(']');

            if (metaStart >= 0 && metaEnd > metaStart)
            {
                meta = exprPart[(metaStart + 1)..metaEnd].Trim();
                exprPart = exprPart[..metaStart].Trim();
            }

            if (exprPart.EndsWith(";"))
                exprPart = exprPart[..^1].TrimEnd();

            if (string.IsNullOrWhiteSpace(exprPart))
                return null;

            return new ProgramLine
            {
                LineNumber = lineNumber,
                Formula = exprPart,
                Meta = meta
            };
        }

        private List<ProgramLine> EnrichProgramLines(
            List<ProgramLine> lines,
            IReadOnlyCollection<ProgramLineContext> contexts)
        {
            var byLine = contexts
                .Where(x => x.LineNumber.HasValue)
                .GroupBy(x => x.LineNumber!.Value)
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var line in lines)
            {
                byLine.TryGetValue(line.LineNumber ?? 0, out var ctx);
                line.Irub = ctx?.Irub;
                line.Ifmt = ctx?.Ifmt;
                line.Liba = ctx?.Liba;
                line.Type = ProgramLineTypeMapper.MapType(line.Meta, ctx?.Rubvar, ctx?.Rubfmt);
                line.SaveDetail = ProgramLineTypeMapper.ShouldSaveDetail(line.Meta, ctx?.Rubvar, ctx?.Rubfmt);
            }

            return lines;
        }
    }
    /// <summary>
    /// 
    /// </summary>

    //public sealed class ProgramLineParser
    //{
    //    public List<ProgramLine> Parse(IProgramLineSource source)
    //    {
    //        var lines = ParseProgramLines(source.GetSourceText());
    //        return EnrichProgramLines(lines, source.GetContexts());
    //    }

    //    public List<ProgramLine> ParseProgramLines(string? sourceText)
    //    {
    //        var lines = new List<ProgramLine>();

    //        if (string.IsNullOrWhiteSpace(sourceText))
    //            return lines;

    //        var rawLines = sourceText
    //            .Replace("\r\n", "\n")
    //            .Replace('\r', '\n')
    //            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    //        foreach (var raw in rawLines)
    //        {
    //            var parsed = ParseProgramLine(raw);
    //            if (parsed != null)
    //                lines.Add(parsed);
    //        }

    //        return lines;
    //    }

    //    private ProgramLine? ParseProgramLine(string raw)
    //    {
    //        var line = raw.Trim();
    //        if (string.IsNullOrWhiteSpace(line))
    //            return null;

    //        var atIndex = line.IndexOf('@');
    //        if (atIndex < 0)
    //            return null;

    //        line = line[(atIndex + 1)..].Trim();

    //        var colonIndex = line.IndexOf(':');
    //        if (colonIndex <= 0)
    //            return null;

    //        if (!int.TryParse(line[..colonIndex].Trim(), out var lineNumber))
    //            return null;

    //        var exprPart = line[(colonIndex + 1)..].Trim();

    //        string? meta = null;
    //        var metaStart = exprPart.LastIndexOf('[');
    //        var metaEnd = exprPart.LastIndexOf(']');

    //        if (metaStart >= 0 && metaEnd > metaStart)
    //        {
    //            meta = exprPart[(metaStart + 1)..metaEnd].Trim();
    //            exprPart = exprPart[..metaStart].Trim();
    //        }

    //        if (exprPart.EndsWith(";"))
    //            exprPart = exprPart[..^1].TrimEnd();

    //        if (string.IsNullOrWhiteSpace(exprPart))
    //            return null;

    //        return new ProgramLine
    //        {
    //            LineNumber = lineNumber,
    //            Formula = exprPart,
    //            Meta = meta
    //        };
    //    }
    //    private List<ProgramLine> EnrichProgramLines(
    //        List<ProgramLine> lines,
    //        IReadOnlyCollection<ProgramLineContext> contexts)
    //    {
    //        var byLine = contexts
    //            .Where(x => x.LineNumber.HasValue)
    //            .GroupBy(x => x.LineNumber!.Value)
    //            .ToDictionary(g => g.Key, g => g.First());

    //        foreach (var line in lines)
    //        {
    //            byLine.TryGetValue(line.LineNumber ?? 0, out var ctx);
    //            line.Irub = ctx?.Irub;
    //            line.Ifmt = ctx?.Ifmt;
    //            line.Liba = ctx?.Liba;
    //            line.Type = ProgramLineTypeMapper.MapType(line.Meta, ctx?.Rubvar, ctx?.Rubfmt);
    //            line.SaveDetail = ProgramLineTypeMapper.ShouldSaveDetail(line.Meta, ctx?.Rubvar, ctx?.Rubfmt);
    //        }

    //        return lines;
    //    }
    //}
    public sealed class PlngenLineSource : IProgramLineSource
    {
        private readonly PlngenDto _program;

        public PlngenLineSource(PlngenDto program)
        {
            _program = program;
        }

        public string? GetSourceText() => _program?.Fpsrc;

        public IReadOnlyCollection<ProgramLineContext> GetContexts()
        {
            var items = new List<ProgramLineContext>();

            foreach (var rubvar in _program.Rubvars)
            {
                items.Add(new ProgramLineContext
                {
                    LineNumber = TryParseLine(rubvar.Scdrub),
                    Irub = rubvar.Id,
                    Liba = rubvar.Liba,
                    Rubvar = rubvar
                });

                foreach (var rubfmt in rubvar.Rubfmts)
                {
                    items.Add(new ProgramLineContext
                    {
                        LineNumber = TryParseLine(rubfmt.Zcdrub),
                        Irub = rubvar.Id,
                        Ifmt = rubfmt.Id,
                        Liba = rubfmt.Liba,
                        Rubvar = rubvar,
                        Rubfmt = rubfmt
                    });
                }
            }

            return items;
        }

        private static int? TryParseLine(string? value)
            => int.TryParse(value, out var n) ? n : null;
    }

    public sealed class RubvarLineSource : IProgramLineSource
    {
        private readonly RubvarDto _rubvar;
        private readonly IReadOnlyCollection<RubfmtDto> _rubfmts;

        public RubvarLineSource(RubvarDto rubvar, IReadOnlyCollection<RubfmtDto> rubfmts)
        {
            _rubvar = rubvar;
            _rubfmts = rubfmts;
        }

        public string? GetSourceText() => _rubvar?.Frsrc;

        public IReadOnlyCollection<ProgramLineContext> GetContexts()
        {
            var contexts = new List<ProgramLineContext>
        {
            new() { Irub = _rubvar.Id, Liba = _rubvar.Liba }
        };

            foreach (var fmt in _rubfmts)
            {
                contexts.Add(new ProgramLineContext
                {
                    LineNumber = int.TryParse(fmt.Zcdrub, out var n) ? n : null,
                    Irub = _rubvar.Id,
                    Ifmt = fmt.Id,
                    Liba = fmt.Liba
                });
            }

            return contexts;
        }
    }

    public sealed class RubvarRowLineSource : IProgramLineSource
    {
        private readonly RubVarRow _row;
        private readonly IReadOnlyCollection<RubFmtRow> _details;

        public RubvarRowLineSource(RubVarRow row, IReadOnlyCollection<RubFmtRow> details)
        {
            _row = row;
            _details = details;
        }

        public string? GetSourceText() => _row?.Frscr;

        public IReadOnlyCollection<ProgramLineContext> GetContexts()
        {
            var contexts = new List<ProgramLineContext>
        {
            new() { Irub = _row.Irub, Liba = _row.Liba }
        };

            foreach (var detail in _details)
            {
                contexts.Add(new ProgramLineContext
                {
                    LineNumber = int.TryParse(detail.Scdfmt, out var n) ? n : null,
                    Irub = detail.Irub,
                    Ifmt = detail.Ifmt,
                    Liba = detail.Liba
                });
            }

            return contexts;
        }
    }
    public static class ProgramLineTypeMapper
    {
        public static LineType MapType(string? meta, RubvarDto? rubvar, RubfmtDto? rubfmt)
        {
            if (rubvar?.Atyp != null)
            {
                return rubvar.Atyp.Value switch
                {
                    1 => LineType.Int,
                    2 => LineType.Decimal,
                    3 => LineType.Date,
                    4 => LineType.Boolean,
                    _ => LineType.Decimal
                };
            }

            return meta?.ToLowerInvariant() switch
            {
                "i" => LineType.Int,
                "d" => LineType.Decimal,
                "w" => LineType.Decimal,
                _ => LineType.Decimal
            };
        }

        public static bool ShouldSaveDetail(string? meta, RubvarDto? rubvar, RubfmtDto? rubfmt)
        {
            if (!string.IsNullOrWhiteSpace(meta))
                return true;

            return rubfmt != null || rubvar != null;
        }
    }
    //public static class ProgramLineTypeMapper
    //{
    //    public static LineType MapType(string? meta, RubvarDto? rubvar, RubfmtDto? rubfmt)
    //    {
    //        if (rubvar?.Atyp != null)
    //        {
    //            return rubvar.Atyp.Value switch
    //            {
    //                1 => LineType.Int,
    //                2 => LineType.Decimal,
    //                3 => LineType.Date,
    //                4 => LineType.Boolean,
    //                _ => LineType.Decimal
    //            };
    //        }

    //        return meta?.ToLowerInvariant() switch
    //        {
    //            "i" => LineType.Int,
    //            "d" => LineType.Decimal,
    //            "w" => LineType.Decimal,
    //            _ => LineType.Decimal
    //        };
    //    }
    //    public static bool ShouldSaveDetail(string? meta, RubvarDto? rubvar, RubfmtDto? rubfmt)
    //    {
    //        if (!string.IsNullOrWhiteSpace(meta))
    //            return true;

    //        return rubfmt != null || rubvar != null;
    //    }
    //}
    //public static class ProgramLineDetailPolicy
    //{
    //    public static bool ShouldSaveDetail(string? meta, RubvarDto? rubvar, RubfmtDto? rubfmt)
    //    {
    //        if (!string.IsNullOrWhiteSpace(meta))
    //            return true;

    //        return rubfmt != null || rubvar != null;
    //    }
    //}
    //public static class ProgramLineRules
    //{
    //    public static LineType MapType(string? meta, RubvarDto? rubvar, RubfmtDto? rubfmt)
    //    {
    //        if (rubvar?.Atyp != null)
    //        {
    //            return rubvar.Atyp.Value switch
    //            {
    //                1 => LineType.Int,
    //                2 => LineType.Decimal,
    //                3 => LineType.Date,
    //                4 => LineType.Boolean,
    //                _ => LineType.Decimal
    //            };
    //        }

    //        return meta?.ToLowerInvariant() switch
    //        {
    //            "i" => LineType.Int,
    //            "d" => LineType.Decimal,
    //            "w" => LineType.Decimal,
    //            _ => LineType.Decimal
    //        };
    //    }

    //    public static bool ShouldSaveDetail(string? meta, RubvarDto? rubvar, RubfmtDto? rubfmt)
    //    {
    //        if (!string.IsNullOrWhiteSpace(meta))
    //            return true;

    //        return rubfmt != null || rubvar != null;
    //    }
    //}
}
//public sealed class RubvarRowLineSource : IProgramLineSource
//{
//    private readonly RubVarRow _row;
//    private readonly IReadOnlyCollection<RubFmtRow> _details;

//    public RubvarRowLineSource(RubVarRow row, IReadOnlyCollection<RubFmtRow> details)
//    {
//        _row = row;
//        _details = details;
//    }

//    public string? GetSourceText() => _row?.Frscr;

//    public IReadOnlyCollection<ProgramLineContext> GetContexts()
//    {
//        var contexts = new List<ProgramLineContext>
//    {
//        new ProgramLineContext { Irub = _row.Irub, Liba = _row.Liba }
//    };

//        foreach (var detail in _details)
//        {
//            contexts.Add(new ProgramLineContext
//            {
//                LineNumber = int.TryParse(detail.Scdfmt, out var n) ? n : null,
//                Irub = detail.Irub,
//                Ifmt = detail.Ifmt,
//                Liba = detail.Liba
//            });
//        }

//        return contexts;
//    }
//}