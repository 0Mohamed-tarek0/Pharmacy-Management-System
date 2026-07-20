using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBL.DTOs.Returns;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Enums;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class ReturnService : IReturnService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReturnService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReturnRecordDto>> GetAllAsync()
        {
            var returns = (await _unitOfWork.StockTransactions.GetReturnsWithDetailsAsync()).ToList();

            // Resolve the OrderNumber / InvoiceNumber for every referenced document in one
            // batch each, rather than querying per row.
            var orderIds = returns
                .Where(t => t.ReferenceType == "Order" && t.ReferenceId.HasValue)
                .Select(t => t.ReferenceId!.Value)
                .Distinct()
                .ToList();

            var saleIds = returns
                .Where(t => t.ReferenceType == "Sale" && t.ReferenceId.HasValue)
                .Select(t => t.ReferenceId!.Value)
                .Distinct()
                .ToList();

            var orderNumbersById = orderIds.Count == 0
                ? new Dictionary<int, string>()
                : (await _unitOfWork.Orders.FindAsync(o => orderIds.Contains(o.Id)))
                    .ToDictionary(o => o.Id, o => o.OrderNumber);

            var invoiceNumbersById = saleIds.Count == 0
                ? new Dictionary<int, string>()
                : (await _unitOfWork.Sales.FindAsync(s => saleIds.Contains(s.Id)))
                    .ToDictionary(s => s.Id, s => s.InvoiceNumber);

            return returns.Select(t =>
            {
                var isPurchaseReturn = t.Type == StockTransactionType.PurchaseReturn;
                var documentId = t.ReferenceId ?? 0;

                string documentNumber = "-";
                if (isPurchaseReturn && orderNumbersById.TryGetValue(documentId, out var orderNumber))
                    documentNumber = orderNumber;
                else if (!isPurchaseReturn && invoiceNumbersById.TryGetValue(documentId, out var invoiceNumber))
                    documentNumber = invoiceNumber;

                return new ReturnRecordDto
                {
                    Id = t.Id,
                    TransactionDate = t.TransactionDate,
                    ReturnType = isPurchaseReturn ? "Purchase" : "Sale",
                    DocumentNumber = documentNumber,
                    DocumentId = documentId,
                    MedicineName = t.Medicine?.Name ?? "Unknown",
                    BatchNumber = t.MedicineBatch?.BatchNumber ?? "-",
                    Quantity = System.Math.Abs(t.Quantity),
                    Notes = t.Notes,
                    PerformedBy = ResolvePerformedBy(t.ApplicationUser)
                };
            })
            .OrderByDescending(r => r.TransactionDate)
            .ToList();
        }

        /// <summary>Prefers FullName, falls back to username, then email, so a return
        /// is never shown as "Unknown" just because FullName wasn't filled in for that account.</summary>
        private static string ResolvePerformedBy(ApplicationUser? user)
        {
            if (user == null)
                return "Unknown";

            if (!string.IsNullOrWhiteSpace(user.FullName))
                return user.FullName;

            if (!string.IsNullOrWhiteSpace(user.UserName))
                return user.UserName;

            if (!string.IsNullOrWhiteSpace(user.Email))
                return user.Email;

            return "Unknown";
        }
    }
}
