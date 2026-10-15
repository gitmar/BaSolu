namespace GxPilo.Services.Cols
{
    //public sealed class ColumnSyncPreviewDto
    //{
    //    public List<string> MissingColumns { get; init; } = [];
    //    public List<string> StaleColumns { get; init; } = [];
    //}

    public sealed class ColumnSyncResultDto
    {
        public int AddedCount { get; init; }
        public int MarkedStaleCount { get; init; }
    }
}
