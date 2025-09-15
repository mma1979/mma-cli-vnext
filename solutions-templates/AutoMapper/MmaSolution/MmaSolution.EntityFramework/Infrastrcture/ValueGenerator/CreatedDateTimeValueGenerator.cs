namespace MmaSolution.EntityFramework.Infrastrcture.ValueGenerator;

public class CreatedDateTimeValueGenerator : ValueGenerator<DateTime>
{
    public override DateTime Next(EntityEntry entry)
    {
        return DateTime.UtcNow;
    }

    public override bool GeneratesTemporaryValues => false;
}
