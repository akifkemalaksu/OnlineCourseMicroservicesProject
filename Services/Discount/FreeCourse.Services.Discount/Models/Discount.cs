namespace FreeCourse.Services.Discount.Models
{
    [Dapper.Contrib.Extensions.Table("discounts")]
    public class Discount : EntityBase<int>
    {
        public string userid { get; set; }
        public int rate { get; set; }
        public string code { get; set; }
        public DateTime createddate { get; set; }
    }
}