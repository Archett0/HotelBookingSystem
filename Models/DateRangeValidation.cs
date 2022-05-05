using System.ComponentModel.DataAnnotations;
using HotelBookingSystem.ViewModels;

namespace HotelBookingSystem.Models
{
    public class DateRangeValidation: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customerReservation = (CustomerReservationViewModel)validationContext.ObjectInstance;

            // We can not let CheckOutTime to be earlier than CheckInTime
            if (customerReservation.CheckOutTime.CompareTo(customerReservation.CheckInTime) < 0)
            {
                return new ValidationResult("退房日期不可早于入住日期");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
