using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MilkStore.API.Models.ProductItemModel;
using MilkStore.Repo.Entities;
using MilkStore.Repo.UnitOfWork;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MilkStore.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/productitems")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductItemController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("topsold")]
        public IActionResult GetTopSoldProductItems()
        {
            var productItems = _unitOfWork.ProductItemRepository
                .Get(
                    filter: p => p.Delete == 1,
                    orderBy: query => query.OrderByDescending(p => p.SoldQuantity)
                )
                .Take(5)
                .Select(productItem => new ResponseProductItemModel
                {
                    ProductItemID = productItem.ProductItemID,
                    ItemName = productItem.ItemName,
                    Price = productItem.Price,
                    Benefit = productItem.Benefit,
                    Description = productItem.Description,
                    Image1 = productItem.Image1,
                    Image2 = productItem.Image2,
                    Image3 = productItem.Image3,
                    Weight = productItem.Weight,
                    BrandMilkID = productItem.BrandMilkID,
                    Baby = productItem.Baby,
                    Mama = productItem.Mama,
                    BrandName = productItem.BrandName,
                    CountryName = productItem.CountryName,
                    CompanyName = productItem.CompanyName,
                    Discount = productItem.Discount,
                    SoldQuantity = productItem.SoldQuantity,
                    StockQuantity = productItem.StockQuantity,
                    Delete = productItem.Delete
                })
                .ToList();

            return Ok(productItems);
        }


        [HttpGet]
        public IActionResult GetAllProductItems()
        {
            var productItems = _unitOfWork.ProductItemRepository
                .Get(filter: p => p.Delete == 1)
                .ToList();

            var responseProductItems = productItems.Select(productItem => new ResponseProductItemModel
            {
                ProductItemID = productItem.ProductItemID,
                ItemName = productItem.ItemName,
                Price = productItem.Price,
                Benefit = productItem.Benefit,
                Description = productItem.Description,
                Image1 = productItem.Image1,
                Image2 = productItem.Image2,
                Image3 = productItem.Image3,
                Weight = productItem.Weight,
                BrandMilkID = productItem.BrandMilkID,
                Baby = productItem.Baby,
                Mama = productItem.Mama,
                BrandName = productItem.BrandName,
                CountryName = productItem.CountryName,
                CompanyName = productItem.CompanyName,
                Discount = productItem.Discount,
                SoldQuantity = productItem.SoldQuantity,
                StockQuantity = productItem.StockQuantity,
                Delete = productItem.Delete
            }).ToList();

            return Ok(responseProductItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductItemById(int id)
        {
            var productItem = _unitOfWork.ProductItemRepository.GetByID(id);

            if (productItem == null || productItem.Delete != 1)
            {
                return NotFound();
            }

            var responseProductItem = new ResponseProductItemModel
            {
                ProductItemID = productItem.ProductItemID,
                ItemName = productItem.ItemName,
                Price = productItem.Price,
                Benefit = productItem.Benefit,
                Description = productItem.Description,
                Image1 = productItem.Image1,
                Image2 = productItem.Image2,
                Image3 = productItem.Image3,
                Weight = productItem.Weight,
                BrandMilkID = productItem.BrandMilkID,
                Baby = productItem.Baby,
                Mama = productItem.Mama,
                BrandName = productItem.BrandName,
                CountryName = productItem.CountryName,
                CompanyName = productItem.CompanyName,
                Discount = productItem.Discount,
                SoldQuantity = productItem.SoldQuantity,
                StockQuantity = productItem.StockQuantity,
                Delete = productItem.Delete
            };

            return Ok(responseProductItem);
        }

        [HttpGet("search")]
        public IActionResult SearchProductItem([FromQuery] RequestSearchProductItemModel requestSearchProductItemModel)
        {
            Expression<Func<ProductItem, bool>> filter = x =>
                (string.IsNullOrEmpty(requestSearchProductItemModel.ItemName) || x.ItemName.Contains(requestSearchProductItemModel.ItemName)) &&
                x.Price >= requestSearchProductItemModel.FromPrice &&
                (requestSearchProductItemModel.ToPrice == null || x.Price <= requestSearchProductItemModel.ToPrice) &&
                (string.IsNullOrEmpty(requestSearchProductItemModel.CountryName) || x.CountryName.Contains(requestSearchProductItemModel.CountryName)) &&
                (string.IsNullOrEmpty(requestSearchProductItemModel.Mama) || x.Mama.Contains(requestSearchProductItemModel.Mama)) &&
                (string.IsNullOrEmpty(requestSearchProductItemModel.Baby) || x.Baby.Contains(requestSearchProductItemModel.Baby)) &&
                (string.IsNullOrEmpty(requestSearchProductItemModel.BrandName) || x.BrandName.Contains(requestSearchProductItemModel.BrandName)) &&
                (string.IsNullOrEmpty(requestSearchProductItemModel.CompanyName) || x.CompanyName.Contains(requestSearchProductItemModel.CompanyName)) &&
                x.Delete == 1; // Add condition for soft delete

            Func<IQueryable<ProductItem>, IOrderedQueryable<ProductItem>> orderBy = query =>
                query.OrderByDescending(p => p.SoldQuantity); // Default order by SoldQuantity descending

            if (requestSearchProductItemModel.SortContent != null && requestSearchProductItemModel.SortContent.SortPrice.HasValue)
            {
                var sortType = requestSearchProductItemModel.SortContent.SortPrice.Value;

                switch (sortType)
                {
                    case SortProductItemTypeEnum.Ascending:
                        orderBy = query => query.OrderBy(p => p.Price);
                        break;
                    case SortProductItemTypeEnum.Descending:
                        orderBy = query => query.OrderByDescending(p => p.Price);
                        break;
                    // Handle additional sorting criteria if needed
                    default:
                        orderBy = query => query.OrderByDescending(p => p.SoldQuantity); // Default to descending by SoldQuantity
                        break;
                }
            }

            var productItems = _unitOfWork.ProductItemRepository.Search(
                searchExpression: filter,
                includeProperties: "",
                orderBy: orderBy,
                pageIndex: requestSearchProductItemModel.PageIndex,
                pageSize: requestSearchProductItemModel.PageSize
            );

            var responseProductItems = productItems.Select(productItem => new ResponseProductItemModel
            {
                ProductItemID = productItem.ProductItemID,
                ItemName = productItem.ItemName,
                Price = productItem.Price,
                Benefit = productItem.Benefit,
                Description = productItem.Description,
                Image1 = productItem.Image1,
                Image2 = productItem.Image2,
                Image3 = productItem.Image3,
                Weight = productItem.Weight,
                BrandMilkID = productItem.BrandMilkID,
                Baby = productItem.Baby,
                Mama = productItem.Mama,
                BrandName = productItem.BrandName,
                CountryName = productItem.CountryName,
                CompanyName = productItem.CompanyName,
                Discount = productItem.Discount,
                SoldQuantity = productItem.SoldQuantity,
                StockQuantity = productItem.StockQuantity
            }).ToList();

            return Ok(responseProductItems);
        }


        [HttpPost]
        public IActionResult CreateProductItem(RequestCreateProductItemModel requestCreateProductItemModel)
        {
            var productItem = new ProductItem
            {
                ProductItemID = requestCreateProductItemModel.ProductItemID,
                ItemName = requestCreateProductItemModel.ItemName,
                Price = requestCreateProductItemModel.Price,
                Benefit = requestCreateProductItemModel.Benefit,
                Description = requestCreateProductItemModel.Description,
                Image1 = requestCreateProductItemModel.Image1,
                Image2 = requestCreateProductItemModel.Image2,
                Image3 = requestCreateProductItemModel.Image3,
                Weight = requestCreateProductItemModel.Weight,
                BrandMilkID = requestCreateProductItemModel.BrandMilkID,
                Baby = requestCreateProductItemModel.Baby,
                Mama = requestCreateProductItemModel.Mama,
                BrandName = requestCreateProductItemModel.BrandName,
                CountryName = requestCreateProductItemModel.CountryName,
                CompanyName = requestCreateProductItemModel.CompanyName,
                Discount = requestCreateProductItemModel.Discount,
                SoldQuantity = requestCreateProductItemModel.SoldQuantity,
                StockQuantity = requestCreateProductItemModel.StockQuantity,
                Delete = requestCreateProductItemModel.Delete
            };

            _unitOfWork.ProductItemRepository.Insert(productItem);
            _unitOfWork.Save();

            return Ok(new { message = "ProductItem created successfully." });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProductItem(int id, RequestUpdateProductItemModel requestUpdateProductItemModel)
        {
            var existedProductItem = _unitOfWork.ProductItemRepository.GetByID(id);
            if (existedProductItem != null)
            {
                existedProductItem.ItemName = requestUpdateProductItemModel.ItemName;
                existedProductItem.Price = requestUpdateProductItemModel.Price;
                existedProductItem.Benefit = requestUpdateProductItemModel.Benefit;
                existedProductItem.Description = requestUpdateProductItemModel.Description;
                existedProductItem.Image1 = requestUpdateProductItemModel.Image1;
                existedProductItem.Image2 = requestUpdateProductItemModel.Image2;
                existedProductItem.Image3 = requestUpdateProductItemModel.Image3;
                existedProductItem.Weight = requestUpdateProductItemModel.Weight;
                existedProductItem.BrandMilkID = requestUpdateProductItemModel.BrandMilkID;
                existedProductItem.Baby = requestUpdateProductItemModel.Baby;
                existedProductItem.Mama = requestUpdateProductItemModel.Mama;
                existedProductItem.BrandName = requestUpdateProductItemModel.BrandName;
                existedProductItem.CountryName = requestUpdateProductItemModel.CountryName;
                existedProductItem.CompanyName = requestUpdateProductItemModel.CompanyName;
                existedProductItem.Discount = requestUpdateProductItemModel.Discount;
                existedProductItem.SoldQuantity = requestUpdateProductItemModel.SoldQuantity;
                existedProductItem.StockQuantity = requestUpdateProductItemModel.StockQuantity;
                existedProductItem.Delete = requestUpdateProductItemModel.Delete;

                _unitOfWork.ProductItemRepository.Update(existedProductItem);
                _unitOfWork.Save();

                return Ok(new { message = "ProductItem updated successfully." });
            }

            return NotFound();
        }

        [HttpDelete("softdelete/{id}")]
        public IActionResult SoftDeleteProductItem(int id)
        {
            var existedProductItem = _unitOfWork.ProductItemRepository.GetByID(id);
            if (existedProductItem == null || existedProductItem.Delete != 1)
            {
                return NotFound();
            }

            // Check if the entity supports soft delete (has a 'Delete' property of type int)
            var deletePropertyInfo = existedProductItem.GetType().GetProperty("Delete");
            if (deletePropertyInfo != null && deletePropertyInfo.PropertyType == typeof(int))
            {
                // Soft delete
                existedProductItem.Delete = 0; // Set Delete to 0 to indicate soft delete
                _unitOfWork.ProductItemRepository.Update(existedProductItem);
                _unitOfWork.Save();

                return Ok(new { message = "ProductItem deleted (soft) successfully." });
            }

            return BadRequest("Soft delete is not supported for this entity.");
        }
        [HttpDelete("harddelete/{id}")]
        public IActionResult HardDeleteProductItem(int id)
        {
            var existedProductItem = _unitOfWork.ProductItemRepository.GetByID(id);
            if (existedProductItem == null || existedProductItem.Delete != 1)
            {
                return NotFound();
            }

            _unitOfWork.ProductItemRepository.Delete(existedProductItem);
            _unitOfWork.Save();

            return Ok(new { message = "ProductItem deleted (hard) successfully." });
        }

    }
}
