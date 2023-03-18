using System.ComponentModel.DataAnnotations;

namespace Kiwify.Core.Data.Entities
{
    public class Order : BaseEntity
    {
        /// <summary>
        /// Identificador do pedido/assinatura ou ID da transação.
        /// </summary>
        [MaxLength(64), Required]
        public string OrderId { get; set; }

        /// <summary>
        /// Plataforma onde foi feita a compra/assinatura
        /// </summary>
        [MaxLength(32), Required]
        public string Platform { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        [MaxLength(128), Required]
        public string ProductName { get; set; }

        /// <summary>
        /// Nome do comprador.
        /// </summary>
        [MaxLength(128), Required]
        public string BuyerName { get; set; }

        /// <summary>
        /// Endereço de e-mail do comprador.
        /// </summary>
        [MaxLength(128), Required]
        public string BuyerEmail { get; set; }

        /// <summary>
        /// Telefone do comprador
        /// </summary>
        [MaxLength(64)]
        public string BuyerMobile { get; set; }

        /// <summary>
        /// CPF do comprador
        /// </summary>
        [MaxLength(64)]
        public string BuyerCPF { get; set; }

        /// <summary>
        /// Status da compra/assinatura
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Data da criação da compra/assinatura.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data da última atualização.
        /// </summary>
        [Required]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Id do Subscriber após a ativação
        /// </summary>
        public long SubscriberId { get; set; }

        /// <summary>
        /// Se a assinatura foi ativada pelo usuário
        /// </summary>
        public bool IsActivated { get; set; }
    }

    public enum EOrderStatus
    {
        WAITING,
        APPROVED,
        CANCELED,
        EXPIRED,
        REFUNDED,
        CHARGEBACK
    }
}
