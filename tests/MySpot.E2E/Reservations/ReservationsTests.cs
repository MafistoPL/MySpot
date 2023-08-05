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
        
        // PUT with wrong ID
        var putBody = new
        {
            LicensePlate = "ABC987",
        };
        var putId = 1;
        var putResponse = await client.PutAsJsonAsync($"/reservations/{putId}", putBody);
        Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);

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
        
        // You cannot add two reservations to one parking spot
        var postResponse2 = await client.PostAsJsonAsync("/reservations", reservation);
        Assert.Equal(HttpStatusCode.BadRequest, postResponse2.StatusCode);

        // Parse Location header
        var locationUrl = postResponse.Headers.Location.ToString();
        var id = locationUrl.Split('/')[^1];

        // Another GET request
        var getResponse = await client.GetAsync(locationUrl);
        Assert.True(getResponse.IsSuccessStatusCode);
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        var returnedReservation = JsonSerializer.Deserialize<dynamic>(getResponseContent);

        // Validate response
        Assert.Equal("John Doe", returnedReservation.GetProperty("employeeName").GetString());
        Assert.Equal("P1", returnedReservation.GetProperty("parkingSpotName").GetString());
        Assert.Equal("XYZ123", returnedReservation.GetProperty("licensePlate").GetString());

        Assert.Equal(DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-ddTHH:mm:ssZ"), returnedReservation.GetProperty("date").GetString());
        
        // Change License Plate
        putResponse = await client.PutAsJsonAsync($"/reservations/{id}", putBody);
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

        // Yet another GET to verify if license plate chaned
        getResponse = await client.GetAsync(locationUrl);
        getResponseContent = await getResponse.Content.ReadAsStringAsync();
        returnedReservation = JsonSerializer.Deserialize<dynamic>(getResponseContent);
        Assert.Equal("John Doe", returnedReservation.GetProperty("employeeName").GetString());
        Assert.Equal("P1", returnedReservation.GetProperty("parkingSpotName").GetString());
        Assert.Equal(putBody.LicensePlate, returnedReservation.GetProperty("licensePlate").GetString());
    }
}