﻿using Kiwify.API.Services;
using Kiwify.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Kiwify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KiwifyController : ControllerBase
    {
        private readonly KiwifyPaymentHandler _kiwifyPayment;
        private readonly IConfiguration _configuration;

        public KiwifyController(KiwifyPaymentHandler kiwifyPayment, IConfiguration configuration)
        {
            _kiwifyPayment = kiwifyPayment;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post([FromBody] string payload, [FromQuery(Name = "signature")] string signature)
        {
            if (string.IsNullOrEmpty(payload) || string.IsNullOrEmpty(signature))
                return BadRequest(new { error = "Object body cannot be null or empty" });

            if (!IsValidSignature(payload, signature))
                return BadRequest(new { error = "Incorrect signature" });

            var order = JsonSerializer.Deserialize<KiwifyOrder>(payload);
            if (order == null)
                return BadRequest(new { error = "Invalid json format" });

            _kiwifyPayment.Handler(order);
            return Ok();
        }

        public bool IsValidSignature(string orderJson, string signature)
        {
            var secretToken = Environment.GetEnvironmentVariable("SecretToken");
            var kiwifyConfig = _configuration.GetSection("KiwifyConfiguration").Get<KiwifyConfiguration>();

            // calculate signature
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretToken ?? kiwifyConfig.SecretToken);
            byte[] orderBytes = Encoding.UTF8.GetBytes(orderJson);

            using (var hmac = new HMACSHA1(secretKeyBytes))
            {
                byte[] hash = hmac.ComputeHash(orderBytes);
                string calculatedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();

                Console.WriteLine($"signature: {signature}");
                Console.WriteLine($"calculatedSignature: {calculatedSignature}");
                return signature.Equals(calculatedSignature);
            }
        }
    }
}