namespace WebApiPractice.Services
{
    public interface IPriceRecalculationService
    {
        double CalculateTotalPrice(Guid orderId);
    }
}
