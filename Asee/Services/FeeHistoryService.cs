using Asee.Data;
using Asee.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Asee.Services
{
    public class FeeHistoryService
    {
        private readonly FeeDbContext _context;

        public FeeHistoryService(FeeDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(object input, object output, string transactionId)
        {
            var entry = new FeeCalculationHistory
            {
                Id = Guid.NewGuid(),
                TransactionId = transactionId,
                InputJson = JsonSerializer.Serialize(input),
                OutputJson = JsonSerializer.Serialize(output),
                Timestamp = DateTime.UtcNow
            };

            _context.FeeHistories.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FeeCalculationHistory>> GetAllAsync()
        {
            return await _context.FeeHistories.OrderByDescending(x => x.Timestamp).ToListAsync();
        }
    }
}
