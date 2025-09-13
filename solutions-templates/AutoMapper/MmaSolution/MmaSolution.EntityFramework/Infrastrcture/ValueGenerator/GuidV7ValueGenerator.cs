using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace MmaSolution.EntityFramework.Infrastrcture.ValueGenerator;
public class GuidV7ValueGenerator : ValueGenerator<Guid>
{
    public override Guid Next(EntityEntry entry)
    {
        return Ulid.NewUlid().ToGuid();
    }

    public override bool GeneratesTemporaryValues => false;
}