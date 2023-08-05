using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using FluentAssertions;
using MySpot.Api.Models;
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
        const string parkingSpotId = "00000000-0000-0000-0000-000000000001";
        
        // Create a client
        var client = _factory.CreateClient();

        // First GET request
        var initialResponse = await client.GetAsync("/reservations");
        initialResponse.IsSuccessStatusCode.Should().BeTrue();
        var initialResponseContent = await initialResponse.Content.ReadAsStringAsync();
        var initialReservations = JsonSerializer.Deserialize<object[]>(initialResponseContent);
        initialReservations.Should().BeEmpty(); // Expects an empty list
        
        // PUT with wrong ID
        var putBody = new
        {
            LicensePlate = "ABC987",
        };
        var wrongId = "00000000-0000-0000-0000-000000000000";
        var putResponse = await client.PutAsJsonAsync($"/reservations/{wrongId}", putBody);
        putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        // Delete with wrong Id
        var deleteResponse = await client.DeleteAsync($"/reservations/{wrongId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // POST request
        var reservationDate = DateTime.UtcNow.Date.AddDays(1);
        var reservation = new
        {
            EmployeeName = "John Doe",
            ParkingSpotId = parkingSpotId,
            LicensePlate = "XYZ123",
            Date = reservationDate.ToString("yyyy-MM-dd")
        };
        var postResponse = await client.PostAsJsonAsync("/reservations", reservation);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // GET all reservations should return 1 item
        var getAllResponse = await client.GetAsync("/reservations");
        getAllResponse.IsSuccessStatusCode.Should().BeTrue();
        var getAllResponseContent = await getAllResponse.Content.ReadAsStringAsync();
        var getAllResponseObj = JsonSerializer.Deserialize<object[]>(getAllResponseContent);
        getAllResponseObj.Should().HaveCount(1); // IT SHOULD CONTAIN ONE OBJECT

        // You cannot add two reservations to one parking spot
        // for now server return 500, but it will return 400 after global exception handling will be added
        //var postResponse2 = await client.PostAsJsonAsync("/reservations", reservation);
        //postResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Parse Location header
        var locationUrl = postResponse.Headers.Location.ToString();
        var id = locationUrl.Split('/')[^1];

        // Another GET request
        var getResponse = await client.GetAsync(locationUrl);
        getResponse.IsSuccessStatusCode.Should().BeTrue();
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        var returnedReservation = JsonSerializer.Deserialize<dynamic>(getResponseContent);

        // Validate response
        // Business requirement changed now name should not be present in response, and parking spot name as well
        Assert.Equal("XYZ123", returnedReservation.GetProperty("licensePlate").GetString());
        Assert.Equal(reservationDate.ToString("yyyy-MM-ddTHH:mm:ss"), returnedReservation.GetProperty("date").GetString());

        // Change License Plate
        putResponse = await client.PutAsJsonAsync($"/reservations/{id}", putBody);
        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Yet another GET to verify if license plate changed
        getResponse = await client.GetAsync(locationUrl);
        getResponseContent = await getResponse.Content.ReadAsStringAsync();
        returnedReservation = JsonSerializer.Deserialize<dynamic>(getResponseContent);
        
        Assert.Equal(putBody.LicensePlate, returnedReservation.GetProperty("licensePlate").GetString());
        Assert.Equal(reservationDate.ToString("yyyy-MM-ddTHH:mm:ss"), returnedReservation.GetProperty("date").GetString());

        // // Remove reservation with wrong Id
        // deleteResponse = await client.DeleteAsync($"/reservations/{wrongId}");
        // deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        // Remove reservation with proper Id
        deleteResponse = await client.DeleteAsync($"/reservations/{id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // GET all reservations should return 0 items
        getAllResponse = await client.GetAsync("/reservations");
        getAllResponse.IsSuccessStatusCode.Should().BeTrue();
        getAllResponseContent = await getAllResponse.Content.ReadAsStringAsync();
        getAllResponseObj = JsonSerializer.Deserialize<object[]>(getAllResponseContent);
        getAllResponseObj.Should().HaveCount(0);
    }
}