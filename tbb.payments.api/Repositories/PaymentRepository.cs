using System.Threading.Tasks;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Models;
using Dapper;
using System.Data.SqlClient;

namespace tbb.payments.api.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> AddRefundRecordAsync(RefundDetails refundDetails)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Refunds (TransactionId, Amount, UserId) VALUES (@TransactionId, @Amount, @UserId)";
                var result = await connection.ExecuteAsync(sql, refundDetails);
                return result > 0;
            }
        }

        public async Task<string> GetUserEmailByIdAsync(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT Email FROM Users WHERE UserId = @UserId";
                return await connection.QuerySingleOrDefaultAsync<string>(sql, new { UserId = userId });
            }
        }
    }
}
