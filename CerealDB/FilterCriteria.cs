namespace CerealDB
{
    public class FilterCriteria
    {
        private string? _OrderBy = "id";
        public string Parameter { get; set; }
        public string Op { get; set; }

        public float Value { get; set; }
        public string? Orderby
        {
            get { return _OrderBy ?? "id"; }
            set { _OrderBy = value; }
        }
    }
}
