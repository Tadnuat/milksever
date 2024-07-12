using Microsoft.AspNetCore.Mvc;
using MilkStore.API.Models.PaymentModel;
using MilkStore.Repo.UnitOfWork;
using MilkStore.Repo.Entities;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace MilkStore.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/payments")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class PaymentController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public PaymentController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var payments = _unitOfWork.PaymentRepository.Get(p => p.Delete == 1)
                                    .Select(payment => new ResponsePaymentModel
                                    {
                                        PaymentID = payment.PaymentID,
                                        Amount = payment.Amount,
                                        PaymentMethod = payment.PaymentMethod,
                                        OrderID = payment.OrderID,
                                        Delete = payment.Delete
                                    })
                                    .ToList();

            return Ok(payments);
        }

        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = _unitOfWork.PaymentRepository.GetByID(id);

            if (payment == null || payment.Delete != 1)
            {
                return NotFound();
            }

            var responsePayment = new ResponsePaymentModel
            {
                PaymentID = payment.PaymentID,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                OrderID = payment.OrderID,
                Delete = payment.Delete
            };

            return Ok(responsePayment);
        }

        [HttpPost]
        public IActionResult CreatePayment(RequestCreatePaymentlModel requestCreatePaymentModel)
        {
            var payment = new Payment
            {
                PaymentID = requestCreatePaymentModel.PaymentID,
                Amount = requestCreatePaymentModel.Amount,
                PaymentMethod = requestCreatePaymentModel.PaymentMethod,
                OrderID = requestCreatePaymentModel.OrderID,
                Delete = requestCreatePaymentModel.Delete
            };

            _unitOfWork.PaymentRepository.Insert(payment);
            _unitOfWork.Save();
            return Ok(new { message = "Payment created successfully." });
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, RequestUpdatePaymentlModel requestUpdatePaymentModel)
        {
            var existedPayment = _unitOfWork.PaymentRepository.GetByID(id);
            if (existedPayment == null)
            {
                return NotFound();
            }

            existedPayment.Amount = requestUpdatePaymentModel.Amount;
            existedPayment.PaymentMethod = requestUpdatePaymentModel.PaymentMethod;
            existedPayment.OrderID = requestUpdatePaymentModel.OrderID;
            existedPayment.Delete = requestUpdatePaymentModel.Delete;

            _unitOfWork.PaymentRepository.Update(existedPayment);
            _unitOfWork.Save();
            return Ok(new { message = "Payment updated successfully." });
        }

        [HttpDelete("softdelete/{id}")]
        public IActionResult SoftDeletePayment(int id)
        {
            var existedPayment = _unitOfWork.PaymentRepository.GetByID(id);
            if (existedPayment == null || existedPayment.Delete != 1)
            {
                return NotFound();
            }

            _unitOfWork.PaymentRepository.SoftDelete(existedPayment); // Using SoftDelete method
            _unitOfWork.Save();
            return Ok(new { message = "Payment deleted (soft) successfully." });
        }

        [HttpDelete("harddelete/{id}")]
        public IActionResult HardDeletePayment(int id)
        {
            var existedPayment = _unitOfWork.PaymentRepository.GetByID(id);
            if (existedPayment == null || existedPayment.Delete != 1)
            {
                return NotFound();
            }

            _unitOfWork.PaymentRepository.Delete(existedPayment); // Using HardDelete method
            _unitOfWork.Save();
            return Ok(new { message = "Payment deleted (hard) successfully." });
        }
    }
}
