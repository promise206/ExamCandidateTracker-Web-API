namespace Exam.Domain.Interface
{
    public interface IAuditable
    {
        public DateTimeOffset CheckedInDateTime { get; set; }
    }
}
