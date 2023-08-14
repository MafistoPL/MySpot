﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Configurations;

internal sealed class WeeklyParkingSpotConfiguration : IEntityTypeConfiguration<WeeklyParkingSpot>
{
    public void Configure(EntityTypeBuilder<WeeklyParkingSpot> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value,
                x => new ParkingSpotId(x));
        builder.Property(x => x.Name)
            .HasConversion(x => x.Value,
                x => new ParkingSpotName(x));
        builder.Property(x => x.Week)
            .HasConversion(x => x.To.Value.ToUniversalTime(),
                x => new Week(x));
    }
}