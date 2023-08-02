using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using Xunit;

namespace MySpot.E2E.Reservations;

public class ReservationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ReservationTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Fact]
    public async Task TestReservationsEndpoint()
    {
        // Create a client
        var client = _factory.CreateClient();

        // First GET request
        var initialResponse = await client.GetAsync("/reservations");
        Assert.True(initialResponse.IsSuccessStatusCode);
        var initialResponseContent = await initialResponse.Content.ReadAsStringAsync();
        var initialReservations = JsonSerializer.Deserialize<object[]>(initialResponseContent);
        Assert.Empty(initialReservations); // Expects an empty list

        // POST request
        var reservation = new
        {
            EmployeeName = "John Doe",
            ParkingSpotName = "P1",
            LicensePlate = "XYZ123",
            Date = "2023-08-03"
        };
        var postResponse = await client.PostAsJsonAsync("/reservations", reservation);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        // Parse Location header
        var locationUrl = postResponse.Headers.Location.ToString();

        // Another GET request
        var getResponse = await client.GetAsync(locationUrl);
        Assert.True(getResponse.IsSuccessStatusCode);
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        var returnedReservation = JsonSerializer.Deserialize<dynamic>(getResponseContent);

        // Validate response
        Assert.Equal("John Doe", returnedReservation.GetProperty("employeeName").GetString());
        Assert.Equal("P1", returnedReservation.GetProperty("parkingSpotName").GetString());
        Assert.Equal("XYZ123", returnedReservation.GetProperty("licensePlate").GetString());
        Assert.Equal("2023-08-03T00:00:00Z", returnedReservation.GetProperty("date").GetString());
    }
}