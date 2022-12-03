using FinancePlanner.Shared.Models.WageServices;

namespace FinancePlanner.WageServices.Services.Services;

public interface IPreTaxService
{
    PreTaxDeductionResponse CalculateTaxableWages(PreTaxDeductionRequest request);
}