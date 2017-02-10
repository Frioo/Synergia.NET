using NodaTime;

namespace Synergia.NET.Models
{
    public class LuckyNumber
    {
        public string Number { get; }
        public LocalDate LuckyNumberDay { get; }

        public LuckyNumber(string Number, LocalDate LuckyNumberDay)
        {
            this.Number = Number;
            this.LuckyNumberDay = LuckyNumberDay;
        }
    }
}
