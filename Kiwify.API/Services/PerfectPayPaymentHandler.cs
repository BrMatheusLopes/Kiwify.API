using Kiwify.Core.Repository;
using PerfectPay.Library.Models;
using Serilog;

namespace Kiwify.API.Services
{
    public class PerfectPayPaymentHandler : PaymentHandler<PerfectPayOrder>
    {
        private readonly IOrderRepository _repository;
        private static readonly IList<Customer> _customers = new List<Customer>();

        public PerfectPayPaymentHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public IList<Customer> Customers => _customers;

        public override async Task Handler(PerfectPayOrder order)
        {
            try
            {
                switch (order.SaleStatusEnum)
                {
                    case EOrderStatus.None:
                        break;
                    case EOrderStatus.Pending:
                    case EOrderStatus.Approved:
                    case EOrderStatus.InProcess:
                    case EOrderStatus.InMediation:
                    case EOrderStatus.Rejected:
                    case EOrderStatus.Cancelled:
                    case EOrderStatus.Refunded:
                    case EOrderStatus.Authorized:
                    case EOrderStatus.ChargedBack:
                    case EOrderStatus.Completed:
                    case EOrderStatus.CheckoutError:
                    case EOrderStatus.Precheckout:
                    case EOrderStatus.Expired:
                    case EOrderStatus.InReview:
                        await CreateOrUpdateOrder(order);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Handler Error: {ex}");
            }
        }

        private async Task CreateOrUpdateOrder(PerfectPayOrder order)
        {
            var existingOrder = await _repository.GetOrderByOrderIdAndEmail(order.Code, order.Customer.Email);
            if (existingOrder == null)
            {
                var newOrder = new Core.Data.Entities.Order
                {
                    OrderId = order.Code,
                    Platform = "PerfectPay",
                    ProductName = order.Product.Name,
                    BuyerName = order.Customer.FullName,
                    BuyerEmail = order.Customer.Email.ToLower(),
                    BuyerMobile = order.Customer?.PhoneNumber ?? "BuyerMobile_NOT_FOUND",
                    BuyerCPF = order.Customer?.IdentificationNumber ?? "CPF_NOT_FOUND",
                    Status = order.SaleStatusEnum.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    SubscriberId = -1,
                    IsActivated = false,
                };

                var createdOrder = await _repository.CreateOrderAsync(newOrder);
                Log.Information($"Pedido '{createdOrder.OrderId}' criado no banco de dados - status: {createdOrder.Status}");

                if (order.Customer != null)
                {
                    if (_customers.FirstOrDefault(x => x.Email == order.Customer.Email) == null)
                    {
                        _customers.Add(order.Customer);
                    }
                }
            }
            else
            {
                existingOrder.Status = order.SaleStatusEnum.ToString();
                existingOrder.UpdatedAt = DateTime.UtcNow;
                var updatedOrder = await _repository.UpdateOrderAsync(existingOrder);
                Log.Information($"Pedido '{updatedOrder?.OrderId}' atualizado no banco de dados - status: {updatedOrder?.Status}");
            }
        }
    }
}
