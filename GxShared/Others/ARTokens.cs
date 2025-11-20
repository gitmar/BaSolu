namespace GxShared.Others
{
    public class ARtokens
    {       
        public int Id { get; set; } //primary key
        public string Atoken { get; set; } = string.Empty;
        public string Rtoken { get; set; } = string.Empty;
        public string Etoken { get; set; } = string.Empty;
        public DateTime? Rdexp { get; set; }
        public DateTime? Adexp { get; set; }
        public DateTime? Sdexp { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime? Rexpdate { get; set; }
    }
}
