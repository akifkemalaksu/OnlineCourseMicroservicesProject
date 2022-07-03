namespace FreeCourse.Services.Discount.Models
{
    public class EntityBase<T>
        where T : struct
    {
        [Dapper.Contrib.Extensions.Key]
        public T id { get; set; }
    }
}