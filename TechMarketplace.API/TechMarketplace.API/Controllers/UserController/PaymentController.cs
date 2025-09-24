using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketplace.API.Data;
using TechMarketplace.API.Models.Payments;
using TechMarketplace.API.Services;

namespace TechMarketplace.API.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PayPalService _payPalService;
        private readonly DataContext _context;

        public PaymentController(PayPalService payPalService, DataContext context)
        {
            _payPalService = payPalService;
            _context = context;
        }

        // PayPal კავშირის ტესტი
        [HttpGet("test")]
        public async Task<IActionResult> TestPayPal()
        {
            try
            {
                var token = await _payPalService.GetAccessTokenAsync();
                return Ok(new
                {
                    success = true,
                    message = "PayPal კავშირი წარმატებულია!",
                    hasToken = !string.IsNullOrEmpty(token)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "PayPal კავშირი ვერ მოხერხდა",
                    error = ex.Message
                });
            }
        }

        // Payment შექმნა Order-ისთვის
        [HttpPost("create-payment/{orderId}")]
        public async Task<IActionResult> CreatePayment(int orderId)
        {
            try
            {
                // Order მოძებნა
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                    return NotFound(new { message = "Order not found" });

                // უკვე Payment არსებობს?
                var existingPayment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.OrderId == orderId && p.Status == PaymentStatus.Completed);
                if (existingPayment != null)
                    return BadRequest(new { message = "Order already paid" });

                // PayPal Order შექმნა
                var paypalResult = await _payPalService.CreateOrderAsync(order.TotalAmount);

                // Debug ინფორმაცია
                Console.WriteLine($"PayPal Order ID: {paypalResult.OrderId}");
                Console.WriteLine($"PayPal Status: {paypalResult.Status}");
                Console.WriteLine($"PayPal Approval URL: {paypalResult.ApprovalUrl}");

                // Payment Database-ში შენახვა
                var payment = new Payment
                {
                    OrderId = orderId,
                    Amount = order.TotalAmount,
                    PaymentMethod = "PayPal",
                    Status = PaymentStatus.Pending,
                    TransactionId = paypalResult.OrderId, // PayPal-ის ნამდვილი ID
                    CreatedAt = DateTime.UtcNow
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Database-ში შენახული TransactionId: {payment.TransactionId}");

                return Ok(new
                {
                    success = true,
                    message = "Payment created successfully",
                    paymentId = payment.Id,
                    paypalOrderId = paypalResult.OrderId, // ეს გამოიყენეთ Capture-ისთვის
                    paymentUrl = paypalResult.ApprovalUrl,
                    amount = payment.Amount,
                    status = payment.Status.ToString(),
                    instructions = "გადადით paymentUrl-ზე გადახდისთვის, შემდეგ გამოიყენეთ paypalOrderId Capture-ისთვის"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Payment Creation Error: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Payment creation failed",
                    error = ex.Message
                });
            }
        }

        // PayPal Payment დადასტურება
        [HttpPost("capture-payment/{paypalOrderId}")]
        public async Task<IActionResult> CapturePayment(string paypalOrderId)
        {
            try
            {
                Console.WriteLine($"Capture მცდელობა PayPal Order ID: {paypalOrderId}");

                // Database-ში Payment მოძებნა PayPal Order ID-ის მიხედვით
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.TransactionId == paypalOrderId);

                if (payment == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Payment with PayPal Order ID '{paypalOrderId}' not found in database"
                    });
                }

                Console.WriteLine($"Database-ში Payment ნაპოვნია: PaymentId = {payment.Id}, Status = {payment.Status}");

                // PayPal-ზე Payment Capture
                var captureResult = await _payPalService.CaptureOrderAsync(paypalOrderId);

                if (captureResult.IsSuccess)
                {
                    // Payment Status განახლება
                    payment.Status = PaymentStatus.Completed;
                    payment.PaidAt = DateTime.UtcNow;
                    _context.Payments.Update(payment);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Payment წარმატებით დასრულდა: PaymentId = {payment.Id}");

                    return Ok(new
                    {
                        success = true,
                        message = "Payment completed successfully!",
                        paymentId = payment.Id,
                        orderId = payment.OrderId,
                        amount = payment.Amount,
                        paidAt = payment.PaidAt,
                        paypalStatus = captureResult.Status,
                        captureId = captureResult.CaptureId
                    });
                }
                else
                {
                    // Payment Failed-ად მონიშვნა
                    payment.Status = PaymentStatus.Failed;
                    _context.Payments.Update(payment);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Payment Capture ვერ მოხერხდა: {captureResult.ErrorMessage}");

                    return BadRequest(new
                    {
                        success = false,
                        message = "Payment capture failed",
                        paypalStatus = captureResult.Status,
                        error = captureResult.ErrorMessage,
                        debugInfo = captureResult.RawResponse
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Payment Capture Exception: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Payment capture error",
                    error = ex.Message
                });
            }
        }

        // Payment Status შემოწმება
        [HttpGet("payment-status/{orderId}")]
        public async Task<IActionResult> GetPaymentStatus(int orderId)
        {
            try
            {
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.OrderId == orderId);

                if (payment == null)
                    return NotFound(new { message = "Payment not found" });

                return Ok(new
                {
                    orderId = payment.OrderId,
                    paymentId = payment.Id,
                    status = payment.Status.ToString(),
                    amount = payment.Amount,
                    paymentMethod = payment.PaymentMethod,
                    transactionId = payment.TransactionId,
                    createdAt = payment.CreatedAt,
                    paidAt = payment.PaidAt,
                    isPaid = payment.Status == PaymentStatus.Completed
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error getting payment status", error = ex.Message });
            }
        }

        // All Payments (Debug-ისთვის)
        [HttpGet("all-payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _context.Payments
                    .Select(p => new
                    {
                        paymentId = p.Id,
                        orderId = p.OrderId,
                        amount = p.Amount,
                        status = p.Status.ToString(),
                        transactionId = p.TransactionId,
                        paymentMethod = p.PaymentMethod,
                        createdAt = p.CreatedAt,
                        paidAt = p.PaidAt
                    })
                    .ToListAsync();

                return Ok(new { payments = payments, count = payments.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Debug PayPal Response
        [HttpGet("debug-paypal/{paypalOrderId}")]
        public async Task<IActionResult> DebugPayPal(string paypalOrderId)
        {
            try
            {
                Console.WriteLine($"Debug PayPal Order ID: {paypalOrderId}");

                var captureResult = await _payPalService.CaptureOrderAsync(paypalOrderId);

                return Ok(new
                {
                    paypalOrderId = paypalOrderId,
                    isSuccess = captureResult.IsSuccess,
                    status = captureResult.Status,
                    captureId = captureResult.CaptureId,
                    errorMessage = captureResult.ErrorMessage,
                    rawResponse = captureResult.RawResponse
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

}

