using System;
using FinancePlanner.Shared.Models.Common;
using FinancePlanner.Shared.Models.Exceptions;
using FinancePlanner.Shared.Models.WageServices;

namespace FinancePlanner.WageServices.Services.Services;

public class PostTaxService : IPostTaxService
{
    public PostTaxDeductionResponse CalculatePostTaxDeductions(PostTaxDeductionRequest request)
    {
        try
        {
            PostTaxDeductionDto postTaxDeductions = new()
            {
                AccidentInsuranceAmount = request.PostTaxDeduction.AccidentInsuranceAmount,
                EmployeeStockPlanAmount = request.PostTaxDeduction.EmployeeStockPlanAmount,
                LifeInsuranceAmount = request.PostTaxDeduction.LifeInsuranceAmount,
                MiscellaneousAmount = request.PostTaxDeduction.MiscellaneousAmount,
                Roth401KPercentage = request.PostTaxDeduction.Roth401KPercentage
            };

            decimal totalDeductions = postTaxDeductions.EmployeeStockPlanAmount 
                                      + postTaxDeductions.Roth401KPercentage/100 * request.TotalGrossPay
                                      + postTaxDeductions.AccidentInsuranceAmount
                                      + postTaxDeductions.LifeInsuranceAmount
                                      + postTaxDeductions.MiscellaneousAmount;

            return new PostTaxDeductionResponse()
            {
                TotalGrossPay = request.TotalGrossPay,
                TotalPostTaxDeductionAmount = totalDeductions,
                PostTaxDeduction = postTaxDeductions
            };

        }
        catch (Exception ex)
        {
            throw new InternalServerErrorException(ex.Message, ex);
        }
    }
}