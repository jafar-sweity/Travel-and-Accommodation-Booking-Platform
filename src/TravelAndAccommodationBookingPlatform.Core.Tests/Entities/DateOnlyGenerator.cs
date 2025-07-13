using AutoFixture;
using AutoFixture.Kernel;

public class DateOnlyGenerator : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type t && t == typeof(DateOnly))
        {
            var dateTime = context.Create<DateTime>();
            return DateOnly.FromDateTime(dateTime);
        }

        return new NoSpecimen();
    }
}
