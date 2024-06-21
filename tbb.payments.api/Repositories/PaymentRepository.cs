using System;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Models;

namespace tbb.payments.api.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task SavePaymentAsync(Payment payment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Payments (Id, Amount, Status, Email, TicketDetails) VALUES (@Id, @Amount, @Status, @Email, @TicketDetails)";
                await connection.ExecuteAsync(query, payment);
            }
        }

        public async Task<Payment> GetPaymentAsync(Guid paymentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Payments WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Payment>(query, new { Id = paymentId });
            }
        }

        public async Task SaveRefundAsync(Refund refund)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Refunds (Id, PaymentId, Amount, Reason) VALUES (@Id, @PaymentId, @Amount, @Reason)";
                await connection.ExecuteAsync(query, refund);
            }
        }

        public async Task AddRefundRecordAsync(RefundDetails refundDetails)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Refunds (PaymentId, Amount, Reason) VALUES (@PaymentId, @Amount, @Reason)";
                await connection.ExecuteAsync(query, refundDetails);
            }
        }
    }
}
