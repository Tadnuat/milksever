using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MilkStore.API.Models.StorageModel;
using MilkStore.Repo.Entities;
using MilkStore.Repo.UnitOfWork;
using System.Linq;
using System.Web;

namespace MilkStore.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/storage")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class StorageController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public StorageController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var storages = _unitOfWork.StorageRepository.Get(s => s.Delete == 1)
                                    .Select(storage => new ResponseStorageModel
                                    {
                                        StorageID = storage.StorageID,
                                        StorageName = storage.StorageName,
                                        Delete = storage.Delete
                                    })
                                    .ToList();

            return Ok(storages);
        }

        [HttpGet("{id}")]
        public IActionResult GetStorageById(int id)
        {
            var storage = _unitOfWork.StorageRepository.GetByID(id);

            if (storage == null || storage.Delete != 1)
            {
                return NotFound();
            }

            var responseStorage = new ResponseStorageModel
            {
                StorageID = storage.StorageID,
                StorageName = storage.StorageName,
                Delete = storage.Delete
            };

            return Ok(responseStorage);
        }

        [HttpPost]
        public IActionResult CreateOrderDetail(RequestCreateStorageModel requestCreateStorageModel)
        {
            var storage = new Storage
            {
                StorageID = requestCreateStorageModel.StorageID,
                StorageName = requestCreateStorageModel.StorageName,
                Delete = requestCreateStorageModel.Delete
            };
            _unitOfWork.StorageRepository.Insert(storage);
            _unitOfWork.Save();
            return Ok(new { message = "Storage created successfully." });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStorage(int id, RequestUpdateStorageModel requestUpdateStorageModel)
        {
            var existedStorage = _unitOfWork.StorageRepository.GetByID(id);
            if (existedStorage == null)
            {
                return NotFound();
            }

            existedStorage.StorageName = requestUpdateStorageModel.StorageName;
            existedStorage.Delete = requestUpdateStorageModel.Delete;

            _unitOfWork.StorageRepository.Update(existedStorage);
            _unitOfWork.Save();
            return Ok(new { message = "Storage updated successfully." });
        }

        [HttpDelete("softdelete/{id}")]
        public IActionResult DeleteStorage(int id)
        {
            var existedStorage = _unitOfWork.StorageRepository.GetByID(id);
            if (existedStorage == null || existedStorage.Delete != 1)
            {
                return NotFound();
            }

            _unitOfWork.StorageRepository.SoftDelete(existedStorage); // Using SoftDelete method
            _unitOfWork.Save();
            return Ok(new { message = "Storage deleted (soft) successfully." });
        }

        [HttpDelete("harddelete/{id}")]
        public IActionResult HardDeleteStorage(int id)
        {
            var existedStorage = _unitOfWork.StorageRepository.GetByID(id);
            if (existedStorage == null || existedStorage.Delete != 1)
            {
                return NotFound();
            }

            _unitOfWork.StorageRepository.Delete(existedStorage); // Using HardDelete method
            _unitOfWork.Save();
            return Ok(new { message = "Storage deleted (hard) successfully." });
        }

        [HttpGet("searchstorage")]
        public IActionResult SearchStorage([FromQuery] RequestSearchStorageModel requestSearchStorageModel)
        {
            var decodedStorageName = HttpUtility.UrlDecode(requestSearchStorageModel.StorageName);

            var sortType = requestSearchStorageModel.SortContent?.SortStorageType;

            Func<Storage, bool> filter = x =>
                (string.IsNullOrEmpty(decodedStorageName) || RemoveVietnameseAccents(x.StorageName).Contains(RemoveVietnameseAccents(decodedStorageName)))
                && x.Delete == 1; // Thêm điều kiện x.Delete == 1 vào filter

            Func<IQueryable<Storage>, IOrderedQueryable<Storage>> orderBy = query =>
            {
                if (sortType == SortStorageTypeEnum.Ascending)
                {
                    return query.OrderBy(p => p.StorageName);
                }
                else
                {
                    return query.OrderByDescending(p => p.StorageName);
                }
            };

            var storages = _unitOfWork.StorageRepository.Get(
                orderBy: orderBy,
                pageIndex: requestSearchStorageModel.PageIndex,
                pageSize: requestSearchStorageModel.PageSize
            ).AsEnumerable().Where(filter).ToList();

            var response = storages.Select(storage => new ResponseStorageModel
            {
                StorageID = storage.StorageID,
                StorageName = storage.StorageName,
                Delete = storage.Delete
            }).ToList();

            return Ok(response);
        }
        public static string RemoveVietnameseAccents(string text)
        {
            string[] VietnameseSigns = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    text = text.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return text;
        }
    }
}
