using System;
using FinancePlanner.Shared.Models.Common;
using FinancePlanner.Shared.Models.Exceptions;
using FinancePlanner.Shared.Models.WageServices;

namespace FinancePlanner.WageServices.Services.Services;

public class PreTaxService : IPreTaxService
{
    public PreTaxDeductionResponse CalculateTaxableWages(PreTaxDeductionRequest request)
    {
        try
        {
            decimal totalGrossPay = 0;

            // Calculate total gross pay
            foreach (WeeklyHoursAndRateDto weeklyHour in request.WeeklyHoursAndRate)
            {
                decimal rate = weeklyHour.HourlyRate;
                decimal overTime = (weeklyHour.TotalHours - weeklyHour.TimeOffHours) > 40
                    ? (weeklyHour.TotalHours - weeklyHour.TimeOffHours - 40)
                    : 0;
                decimal regularHours = weeklyHour.TotalHours - weeklyHour.TimeOffHours - overTime;
                totalGrossPay += regularHours * rate + weeklyHour.TimeOffHours * rate + overTime * (rate + rate / 2);
            }

            decimal totalPreTaxDeductions = request.PreTaxDeduction.Dental +
                                            request.PreTaxDeduction.HealthSavingAccountAmount +
                                            request.PreTaxDeduction.Medical +
                                            request.PreTaxDeduction.Vision +
                                            request.PreTaxDeduction.MiscellaneousAmount +
                                            request.PreTaxDeduction.Traditional401KPercentage / 100 * totalGrossPay;

            TaxableWageInformationDto taxableWageInformation = new()
            {
                StateAndFederalTaxableWages = totalGrossPay - totalPreTaxDeductions,
                SocialAndMedicareTaxableWages = totalGrossPay - totalPreTaxDeductions +
                                                request.PreTaxDeduction.Traditional401KPercentage / 100 * totalGrossPay
            };

            return new PreTaxDeductionResponse()
            {
                GrossPay = totalGrossPay,
                TotalPreTaxDeductionAmount = totalPreTaxDeductions,
                TaxableWageInformation = taxableWageInformation
            };
        }
        catch (Exception ex)
        {
            throw new InternalServerErrorException(ex.Message, ex);
        }
    }
}