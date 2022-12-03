using FinancePlanner.Shared.Models.WageServices;

namespace FinancePlanner.WageServices.Services.Services;

public interface IPostTaxService
{
    PostTaxDeductionResponse CalculatePostTaxDeductions(PostTaxDeductionRequest request);
}