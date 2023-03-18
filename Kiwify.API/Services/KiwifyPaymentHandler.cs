using Kiwify.Core.Data.Entities;
using Kiwify.Core.Exceptions;
using Kiwify.Core.Models;
using Kiwify.Core.Repository;
using Serilog;

namespace Kiwify.API.Services
{
    public abstract class PaymentHandler<T>
    {
        public abstract void Handler(T order);
    }

    public class KiwifyPaymentHandler : PaymentHandler<KiwifyOrder>
    {
        private readonly IOrderRepository _repository;

        public KiwifyPaymentHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public override async void Handler(KiwifyOrder order)
        {
            try
            {
                var orderId = order.OrderId;
                var email = order.Customer?.Email?.ToLower();
                if (orderId == null || email == null)
                    throw new KiwifyException($"order_id ou customer_email não pode ser nulo");

                switch (order.OrderStatus)
                {
                    case "paid":
                        await CreateOrUpdateOrder(order);
                        break;
                    case "waiting_payment":
                        await CreateOrUpdateOrder(order);
                        break;
                    case "refused":
                    case "refunded":
                    case "chargedback":
                        await CreateOrUpdateOrder(order);
                        break;
                    default:
                        Log.Warning($"Handler order_status '{order.OrderStatus}' not implemented");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Handler Error: {ex}");
            }
        }

        private async Task CreateOrUpdateOrder(KiwifyOrder kiwifyOrder)
        {
            var existingOrder = await _repository.GetOrderByOrderIdAndEmail(kiwifyOrder.OrderId, kiwifyOrder.Customer.Email);
            if (existingOrder == null)
            {
                var newOrder = new Order
                {
                    OrderId = kiwifyOrder.OrderId!,
                    Platform = "Kiwify",
                    ProductName = kiwifyOrder.Product.ProductName,
                    BuyerName = kiwifyOrder.Customer.FullName,
                    BuyerEmail = kiwifyOrder.Customer.Email.ToLower(),
                    BuyerMobile = kiwifyOrder.Customer?.Mobile ?? "BuyerMobile_NOT_FOUND",
                    BuyerCPF = kiwifyOrder.Customer?.CPF ?? "CPF_NOT_FOUND",
                    Status = kiwifyOrder.OrderStatus,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    SubscriberId = -1,
                    IsActivated = false,
                };

                var createdOrder = await _repository.CreateOrderAsync(newOrder);
                Log.Debug($"Pedido '{createdOrder.OrderId}' criado no banco de dados - status: {createdOrder.Status}");
            }
            else
            {
                existingOrder.Status = kiwifyOrder.OrderStatus;
                existingOrder.UpdatedAt = DateTime.UtcNow;
                var updatedOrder = await _repository.UpdateOrderAsync(existingOrder);
                Log.Debug($"Pedido '{updatedOrder?.OrderId}' atualizado no banco de dados - status: {updatedOrder?.Status}");
            }
        }
    }
}
