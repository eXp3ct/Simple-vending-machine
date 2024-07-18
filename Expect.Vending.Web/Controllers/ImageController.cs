using AutoMapper;
using Expect.Vending.Data.Interfaces;
using Expect.Vending.Domain.Dtos;
using Expect.Vending.Domain.Models;
using Expect.Vending.Infrastructure.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Expect.Vending.Web.Controllers
{
    /// <summary>
    /// Контроллер картинок
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileManager _imageManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="imageManager"></param>
        public ImageController(IUnitOfWork unitOfWork, IMapper mapper, IFileManager imageManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageManager = imageManager;
        }

        /// <summary>
        /// Получение всех картинок из бд
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список всех картинок</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Image>();

            var images = await repo.GetAll(cancellationToken);

            return Ok(images);
        }

        /// <summary>
        /// Создание картинки в бд
        /// </summary>
        /// <param name="dto">Информация о картинке</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Созданная картинка</returns>
        [HttpPost] 
        public async Task<IActionResult> CreateImage([FromForm] ImageDto dto, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Image>();
            var image = _mapper.Map<Image>(dto);

            if (!await _imageManager.SaveImage(image.Id, dto.ImageFile, cancellationToken))
                return BadRequest();

            var created = await repo.Add(image, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(created);
        }

        /// <summary>
        /// Удаление картинки из бд
        /// </summary>
        /// <param name="id">Id картинки</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Удаленная картинка</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteImage([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Image>();

            if(!await _imageManager.DeleteImage(id, cancellationToken))
                return NotFound();

            var deleted = await repo.Delete(id, cancellationToken);

            if (deleted is null)
                return NotFound();

            if (!await UnlinkImageFromDrink(id, cancellationToken))
                return NotFound();


            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleted);
        }

        /// <summary>
        /// Обновление картинки в бд
        /// </summary>
        /// <param name="id">Id картинки</param>
        /// <param name="newFile">Новая картинка</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленная картинка</returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateImage([FromRoute] Guid id, IFormFile newFile, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Image>();
            var newImage = new Image
            {
                Id = id,
            };

            if(!await _imageManager.SaveImage(id, newFile, cancellationToken)) return BadRequest();

            var updated = await repo.Update(id, newImage, cancellationToken);

            if (updated is null)
                return NotFound();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updated);
        }

        /// <summary>
        /// Получение картинки
        /// </summary>
        /// <param name="id">Id картинки</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Картинка</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetImage([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var contentType = "image/jpeg";
            var imageBytes = await _imageManager.GetImage(id, cancellationToken);

            if (imageBytes is null) return NoContent();

            return File(imageBytes, contentType);
        }

        private async Task<bool> UnlinkImageFromDrink(Guid imageId, CancellationToken cancellationToken)
        {
            var drinks = _unitOfWork.Repository<Drink>();
            var drink = await GetDrinkByImageId(imageId, cancellationToken);

            if (drink is null)
                return false;

            drink.ImageId = null;

            var updated = await drinks.Update(drink.Id, drink, cancellationToken);

            if (updated is null)
                return false;

            return true;
        }

        private async Task<Drink?> GetDrinkByImageId(Guid imageId, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Drink>();
            var drinks = await repo.GetAll(cancellationToken);

            var drink = drinks.FirstOrDefault(x => x.ImageId == imageId);

            if (drink is null) return null;

            return drink;
        }

    }
}
