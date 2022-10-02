using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VacationRental.Domain.Extensions.Common;
using VacationRental.Domain.VacationRental.Interfaces;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Test
{
    [TestClass]
    public class BookingTests
    {
        private readonly IRentalsService _rentalsService;
        private readonly IBookingService _bookingService;

        public BookingTests(IRentalsService paramRentalsService, IBookingService paramBookingService)
        {
            _rentalsService = paramRentalsService;
            _bookingService = paramBookingService;
        }

        [TestMethod]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4,
                PreparationTimeInDays = 2
            };

            var postRentalResponse = await _rentalsService.Post(postRentalRequest);
            {
                Assert.IsTrue(postRentalResponse is not null);
            }

            var postBookingRequest = new BookingBindingModel
            {
                RentalId = postRentalResponse.Id,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };

            var postBookingResponse = await _bookingService.Post(postBookingRequest);
            {
                Assert.IsTrue(postBookingResponse is not null);
            }

            var getBookingResponse = await _bookingService.Get(postBookingResponse.Id);
            {
                Assert.IsTrue(getBookingResponse is not null);

                Assert.AreEqual(postBookingRequest.RentalId, getBookingResponse.RentalId);
                Assert.AreEqual(postBookingRequest.Nights, getBookingResponse.Nights);
                Assert.AreEqual(postBookingRequest.Start, getBookingResponse.Start);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ConflictException))]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            var postRentalResponse = await _rentalsService.Post(postRentalRequest);
            {
                Assert.IsTrue(postRentalResponse is not null);
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResponse.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            var postBooking1Response = await _bookingService.Post(postBooking1Request);
            {
                Assert.IsTrue(postBooking1Response is not null);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResponse.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            await Assert.ThrowsExceptionAsync<ConflictException>(async () =>
            {
                await _bookingService.Post(postBooking2Request);
            });
        }
    }
}