using Microsoft.AspNetCore.Mvc;
using MilkStore.API.Models.OrderDetailModel;
using MilkStore.Repo.UnitOfWork;
using MilkStore.Repo.Entities;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace MilkStore.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/orderdetails")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class OrderDetailController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public OrderDetailController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orderDetails = _unitOfWork.OrderDetailRepository.Get(od => od.Delete == 1)
                                    .Select(orderDetail => new ResponseOrderDetailModel
                                    {
                                        OrderDetailID = orderDetail.OrderDetailID,
                                        OrderID = orderDetail.OrderID,
                                        CustomerID = orderDetail.CustomerID,
                                        ProductItemID = orderDetail.ProductItemID,
                                        Quantity = orderDetail.Quantity,
                                        Price = orderDetail.Price,
                                        ItemName = orderDetail.ItemName,
                                        Image = orderDetail.Image,
                                        OrderStatus = orderDetail.OrderStatus,
                                        Discount = orderDetail.Discount,
                                        Total = CalculateTotal(orderDetail.Price, orderDetail.Quantity, orderDetail.Discount),
                                        Delete = orderDetail.Delete
                                    })
                                    .ToList();

            return Ok(orderDetails);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderDetailByID(int id)
        {
            var orderDetail = _unitOfWork.OrderDetailRepository.GetByID(id);

            if (orderDetail == null || orderDetail.Delete != 1)
            {
                return NotFound();
            }

            var responseOrderDetail = new ResponseOrderDetailModel
            {
                OrderDetailID = orderDetail.OrderDetailID,
                OrderID = orderDetail.OrderID,
                CustomerID = orderDetail.CustomerID,
                ProductItemID = orderDetail.ProductItemID,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price,
                ItemName = orderDetail.ItemName,
                Image = orderDetail.Image,
                OrderStatus = orderDetail.OrderStatus,
                Discount = orderDetail.Discount,
                Total = CalculateTotal(orderDetail.Price, orderDetail.Quantity, orderDetail.Discount),
                Delete = orderDetail.Delete
            };

            return Ok(responseOrderDetail);
        }

        [HttpGet("cart")]
        public IActionResult GetCart([FromQuery] int customerID)
        {
            var orderDetailsQuery = _unitOfWork.OrderDetailRepository.Get(od => od.OrderStatus == 1 && od.Delete == 1 && od.CustomerID == customerID);

            var orderDetails = orderDetailsQuery.Select(orderDetail => new ResponseOrderDetailModel
            {
                OrderDetailID = orderDetail.OrderDetailID,
                OrderID = orderDetail.OrderID,
                CustomerID = orderDetail.CustomerID,
                ProductItemID = orderDetail.ProductItemID,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price,
                ItemName = orderDetail.ItemName,
                Image = orderDetail.Image,
                OrderStatus = orderDetail.OrderStatus,
                Discount = orderDetail.Discount,
                Total = CalculateTotal(orderDetail.Price, orderDetail.Quantity, orderDetail.Discount),
                Delete = orderDetail.Delete
            })
            .ToList();

            return Ok(orderDetails);
        }
        [HttpGet("order")]
        public IActionResult GetOrder([FromQuery] int customerID)
        {
            var orderDetailsQuery = _unitOfWork.OrderDetailRepository.Get(od => od.OrderStatus == 2 && od.Delete == 1 && od.CustomerID == customerID);

            var orderDetails = orderDetailsQuery.Select(orderDetail => new ResponseOrderDetailModel
            {
                OrderDetailID = orderDetail.OrderDetailID,
                OrderID = orderDetail.OrderID,
                CustomerID = orderDetail.CustomerID,
                ProductItemID = orderDetail.ProductItemID,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price,
                ItemName = orderDetail.ItemName,
                Image = orderDetail.Image,
                OrderStatus = orderDetail.OrderStatus,
                Discount = orderDetail.Discount,
                Total = CalculateTotal(orderDetail.Price, orderDetail.Quantity, orderDetail.Discount),
                Delete = orderDetail.Delete
            })
            .ToList();

            return Ok(orderDetails);
        }


        [HttpPost]
        public IActionResult CreateOrderDetail([FromBody] RequestCreateOrderDetailModel requestCreateOrderDetailModel)
        {
            if (requestCreateOrderDetailModel == null)
            {
                return BadRequest(new { message = "Request model is null." });
            }

            var orderDetail = new OrderDetail
            {
                OrderDetailID = requestCreateOrderDetailModel.OrderDetailID,
                OrderID = requestCreateOrderDetailModel.OrderID, // OrderID được phép null
                CustomerID = requestCreateOrderDetailModel.CustomerID,
                ProductItemID = requestCreateOrderDetailModel.ProductItemID,
                Quantity = requestCreateOrderDetailModel.Quantity,
                Price = requestCreateOrderDetailModel.Price,
                ItemName = requestCreateOrderDetailModel.ItemName,
                Image = requestCreateOrderDetailModel.Image,
                OrderStatus = requestCreateOrderDetailModel.OrderStatus,
                Discount = requestCreateOrderDetailModel.Discount,
                Delete = requestCreateOrderDetailModel.Delete
            };

            _unitOfWork.OrderDetailRepository.Insert(orderDetail);
            _unitOfWork.Save();

            return Ok(new { message = "OrderDetail created successfully." });
        }



        [HttpPut("{id}")]
        public IActionResult UpdateOrderDetail(int id, RequestUpdateOrderDetailModel requestUpdateOrderDetailModel)
        {
            var existedOrderDetail = _unitOfWork.OrderDetailRepository.GetByID(id);
            if (existedOrderDetail == null)
            {
                return NotFound();
            }

            existedOrderDetail.OrderID = requestUpdateOrderDetailModel.OrderID;
            existedOrderDetail.CustomerID = requestUpdateOrderDetailModel.CustomerID;
            existedOrderDetail.ProductItemID = requestUpdateOrderDetailModel.ProductItemID;
            existedOrderDetail.Quantity = requestUpdateOrderDetailModel.Quantity;
            existedOrderDetail.Price = requestUpdateOrderDetailModel.Price;
            existedOrderDetail.ItemName = requestUpdateOrderDetailModel.ItemName;
            existedOrderDetail.Image = requestUpdateOrderDetailModel.Image;
            existedOrderDetail.OrderStatus = requestUpdateOrderDetailModel.OrderStatus;
            existedOrderDetail.Discount = requestUpdateOrderDetailModel.Discount;
            existedOrderDetail.Delete = requestUpdateOrderDetailModel.Delete;

            _unitOfWork.OrderDetailRepository.Update(existedOrderDetail);
            _unitOfWork.Save();

            return Ok(new { message = "OrderDetail updated successfully." });
        }

        [HttpDelete("harddelete/{id}")]
        public IActionResult HardDeleteOrderDetail(int id)
        {
            var existedOrderDetail = _unitOfWork.OrderDetailRepository.GetByID(id);
            if (existedOrderDetail == null || existedOrderDetail.Delete != 1)
            {
                return NotFound();
            }

            _unitOfWork.OrderDetailRepository.Delete(existedOrderDetail); // Sử dụng phương thức Delete cứng
            _unitOfWork.Save();

            return Ok(new { message = "OrderDetail hard deleted successfully." });
        }


        [HttpDelete("softdelete{id}")]
        public IActionResult DeleteOrderDetail(int id)
        {
            var existedOrderDetail = _unitOfWork.OrderDetailRepository.GetByID(id);
            if (existedOrderDetail == null || existedOrderDetail.Delete != 1)
            {
                return NotFound();
            }

            // Sử dụng phương thức SoftDelete
            var propertyInfo = existedOrderDetail.GetType().GetProperty("Delete");
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(int))
            {
                propertyInfo.SetValue(existedOrderDetail, 0);
                _unitOfWork.OrderDetailRepository.Update(existedOrderDetail); // Cập nhật thực thể
                _unitOfWork.Save();

                return Ok(new { message = "OrderDetail deleted successfully." });
            }
            else
            {
                return BadRequest(new { message = "OrderDetail does not have a 'Delete' property." });
            }
        }


        private decimal CalculateTotal(decimal? price, int? quantity, decimal? discount)
        {
            if (price.HasValue && quantity.HasValue && discount.HasValue)
            {
                return price.Value * quantity.Value * (1 - discount.Value / 100);
            }
            return 0;
        }
    }
}
