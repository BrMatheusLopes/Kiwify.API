using System.Text.Json.Serialization;

#nullable disable
namespace PerfectPay.Library.Models
{
    public class PerfectPayOrder
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("sale_status_enum")]
        public EOrderStatus SaleStatusEnum { get; set; }

        [JsonPropertyName("sale_status_detail")]
        public string SaleStatusDetail { get; set; }

        [JsonPropertyName("date_created")]
        public string DateCreated { get; set; }

        [JsonPropertyName("date_approved")]
        public string DateApproved { get; set; }

        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("plan")]
        public Plan Plan { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("webhook_owner")]
        public string WebhookOwner { get; set; }
    }

    public enum EOrderStatus
    {
        None = 0,
        Pending = 1, // boleto pendente
        Approved = 2, // venda aprovada boleto ou cartão
        InProcess = 3, // em revisão manual
        InMediation = 4, // em moderação
        Rejected = 5, // rejeitado
        Cancelled = 6, // cancelado do cartão
        Refunded = 7, // devolvido
        Authorized = 8, // autorizada
        ChargedBack = 9, // solicitado charge back
        Completed = 10, // 30 dias após a venda aprovada
        CheckoutError = 11, // erro durante checkout
        Precheckout = 12, // abandono
        Expired = 13, // boleto expirado
        InReview = 16 // em análise
    }

    public class Product
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("external_reference")]
        public string ExternalReference { get; set; }

        [JsonPropertyName("guarantee")]
        public int Guarantee { get; set; }
    }

    public class Plan
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class Customer
    {
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("identification_type")]
        public string IdentificationType { get; set; }

        [JsonPropertyName("identification_number")]
        public string IdentificationNumber { get; set; }

        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }

        [JsonPropertyName("phone_area_code")]
        public string PhoneAreaCode { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("street_name")]
        public string StreetName { get; set; }

        [JsonPropertyName("street_number")]
        public string StreetNumber { get; set; }

        [JsonPropertyName("district")]
        public string District { get; set; }

        [JsonPropertyName("complement")]
        public string Complement { get; set; }

        [JsonPropertyName("zip_code")]
        public string ZipCode { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
    }
}