namespace GxShared.Others
{
    /* ===========================
        MODELS
        =========================== */
    public class MenuItem
    {
        public int Id { get; set; }
        public string Label { get; set; } = "";
        public string Icon { get; set; } = "";
    }

    public class GroupItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
