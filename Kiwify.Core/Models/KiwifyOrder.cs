using System.Text.Json.Serialization;

namespace Kiwify.Core.Models
{
    public class KiwifyOrder
    {
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("order_ref")]
        public string OrderRef { get; set; }

        [JsonPropertyName("order_status")]
        public string OrderStatus { get; set; }

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Data da aprovação do pagamento
        /// </summary>
        [JsonPropertyName("approved_date")]
        public DateTime ApprovedDate { get; set; }

        [JsonPropertyName("Product")]
        public Product Product { get; set; }

        [JsonPropertyName("Customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("Subscription")]
        public Subscription? Subscription { get; set; }

        [JsonPropertyName("subscription_id")]
        public string? SubscriptionId { get; set; }

        [JsonPropertyName("access_url")]
        public string? AccessUrl { get; set; }

        public OrderStatus GetOrderStatus()
        {
            return Enum.TryParse<OrderStatus>(OrderStatus, out var result) ? result : throw new Exception();
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Paid,
        WaitingPayment,
        Refused,
        Refunded,
        Chargedback
    }

    public class Product
    {
        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
    }

    public class Customer
    {
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("CPF")]
        public string CPF { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }
    }

    public class Subscription
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("next_payment")]
        public DateTime NextPayment { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("plan")]
        public Plan? Plan { get; set; }
    }

    public class Plan
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("frequency")]
        public string? Frequency { get; set; }
    }
}
