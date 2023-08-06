﻿using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects;

public sealed record ParkingSpotId
{
    public Guid Value { get; }

    public ParkingSpotId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException(value);
        }
        
        Value = value;
    }

    public static ParkingSpotId Create()
        => new ParkingSpotId(Guid.NewGuid());

    public static implicit operator Guid(ParkingSpotId parkingSpotId) 
        => parkingSpotId.Value;

    public static implicit operator ParkingSpotId(Guid id) 
        => new ParkingSpotId(id);

    public override string ToString() => Value.ToString("N");
}